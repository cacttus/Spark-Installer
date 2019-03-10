using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;
using Microsoft.Win32;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
namespace Spark.Installer
{
    public enum InstallState
    {
        Pending,
        Installing, 
        Uninstalling,
        Rollingback,
        Canceled,
        Successful
    }
    /*
     * Installer File format
     * [exe data]
     * [Token string] [string]
     * [Options Table][int8 (count)][ InstallerOption...]
     * [File Table][int8(count)][InstallerFile...]
     * [Files...] [raw data]
     * 
     */
    public class InstallManager
    {
        #region Private:Members
        private InstallOptions _objOptions;
        private Object _objOptionsLockObject = new Object();
        private InstallerFileTable _objFileTable;
        private double _dblProgress = 0;//Primitives are atomic
        private CancellationTokenSource _installerCancellationToken;
        private Object _objInstallErrorsLockObject = new Object();
        private List<string> _lstInstallErrors = new List<string>();
        private string _strCurrentFile;
        private InstallState _enumInstallState = InstallState.Pending;
        #endregion

        #region Public:Properties
        public InstallOptions Options
        {
            get 
            { 
                lock(_objOptionsLockObject)
                    return _objOptions; 
            }
            private set 
            { 
                lock(_objOptionsLockObject)
                    _objOptions = value; 
            }
        }
        public double InstallProgress
        {
            get
            {
                return _dblProgress;
            }
            private set { }
        }
        public string CurrentFile
        {
            get
            {
                return _strCurrentFile;
            }
            private set { }
        }
        public List<string> InstallErrors
        {
            get
            {
                lock (_objInstallErrorsLockObject)
                    return _lstInstallErrors;
            }
            private set
            {
                lock (_objInstallErrorsLockObject)
                    _lstInstallErrors = value;
            }
        }
        public InstallState InstallState
        {
            get
            {
                return _enumInstallState;
            }
            private set
            {
                _enumInstallState = value;
            }
        }
        #endregion

        #region Public:Methods

        public string ReplaceValueVariables(string strValue)
        {
            if (strValue.Contains(InstallerVariable.ProgramFiles))
            {
                //**Also - Environment.GetEnvironmentVariable("ProgramFiles"); - returns x86 program files.
                string strProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                strValue = strValue.Replace(InstallerVariable.ProgramFiles, strProgramFiles);
            }
            if (strValue.Contains(InstallerVariable.ProgramFilesX86))
            {
                string strProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                strValue = strValue.Replace(InstallerVariable.ProgramFiles, strProgramFiles);
            }
            if (strValue.Contains(InstallerVariable.Desktop))
            {
                string strProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                strValue = strValue.Replace(InstallerVariable.ProgramFiles, strProgramFiles);
            }
            if (strValue.Contains(InstallerVariable.SystemRoot))
            {
                string strProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                strValue = strValue.Replace(InstallerVariable.ProgramFiles, strProgramFiles);
            }
            if (strValue.Contains(InstallerVariable.InstallDir))
            {
                InstallOption opt = GetOption(InstallOption.InstallLocation);
                if (opt == null)
                    Globals.Throw("Could not get install location.  Doesn't seem to have been set. (0101984)");
                strValue = strValue.Replace(InstallerVariable.InstallDir, opt.Value);
            }

            return strValue;
        }
        public InstallOption GetOption(string key)
        {
            return _objOptions.GetOption(key);
        }
        public InstallOption AddOrReplaceOption(string key, string value)
        {
            InstallOption opt = new InstallOption(key, value);
            _objOptions.AddOrReplaceOption(opt);
            return opt;
        }
        public void BeginInstallation()
        {
            Globals.Logger.LogInfo("Beginning install from main thread.");
            _installerCancellationToken = new CancellationTokenSource();
            CancellationToken ct = _installerCancellationToken.Token;

            Task.Factory.StartNew(new Action(ExecuteInstallation), _installerCancellationToken.Token);
        }
        public void BeginUninstall()
        {
            Globals.Logger.LogInfo("Beginning uninstall from main thread.");
            _installerCancellationToken = new CancellationTokenSource();
            CancellationToken ct = _installerCancellationToken.Token;

            Task.Factory.StartNew(new Action(ExecuteUninstall), _installerCancellationToken.Token);
        }
        public void CancelInstallation()
        {
            _installerCancellationToken.Cancel();
        }
        public void LoadOptions()
        {
            Globals.Logger.LogInfo("Extracting Install Options");

            //Use a dummy to get the options table.
            InstallerBinary dummy = new InstallerBinary();
            _objOptions = new InstallOptions();

            try
            {
                dummy.OpenStream();
                _objOptions.Deserialize(dummy);
            }
            catch (Exception ex)
            {
                Globals.Logger.LogInfo("Exception opening install options: " + ex.ToString());

                throw ex;
            }
            finally
            {
                dummy.CloseStream();
            }

            dummy = null;
        }
        #endregion

        #region Private:Methods

        private void ExecuteInstallation()
        {
            DisplayMsg("Beginning Installation");
            InstallState = InstallState.Installing;
            InstallerFileTable objTable = null;

            string strInstallRoot = CreateInstallRoot();
            if (string.IsNullOrEmpty(strInstallRoot))
                return; // error

            double dblProgressBeforeCopy = 0;

            try
            {
                DisplayMsg("Creating install dir " + strInstallRoot);
                if (!System.IO.Directory.Exists(strInstallRoot))
                    System.IO.Directory.CreateDirectory(strInstallRoot);

                // - Do work
                InstallerBinary objBinary;
                DisplayMsg("Creating binary");
                objBinary = new InstallerBinary();
                objBinary.OpenStream();

                //**Read past the install options section.  Note this
                // is NOT used
                DisplayMsg("Reading Dummy Options");
                InstallOptions dummy = new InstallOptions();
                dummy.Deserialize(objBinary);

                //Validate appliation GUID for uninstall.
                InstallOption guidOpt = GetOption(InstallOption.AppGuid);
                if (guidOpt == null)
                    Globals.Throw("Could not install.  Appliation GUID was not specified in install options.");
                else
                    ProgramInfo.TryParseGuidToUninstallGuid(guidOpt.Value);

                DisplayMsg("Creating Program Info");
                ProgramInfo objInfo = new ProgramInfo(_objOptions);

                DisplayMsg("Reading File Table");
                objTable = new InstallerFileTable(this);
                objTable.Deserialize(objBinary);

                _dblProgress = dblProgressBeforeCopy = 0.5;

                DisplayMsg("Unpacking Files");
                objTable.UnpackFiles(
                    objBinary,
                    strInstallRoot,
                    ref _dblProgress,
                    _installerCancellationToken.Token,
                    ref _strCurrentFile
                    );

                objBinary.CloseStream();

                CreateAppRegistryKeys(objInfo, strInstallRoot);
                CreateUninstaller(objInfo, objBinary, objTable, strInstallRoot);
            }
            catch (Exception ex)
            {
                InstallState = Installer.InstallState.Canceled;
                InstallErrors.Add("Error: " + ex.ToString());
                Globals.Logger.LogError(ex.ToString(), false, true);
                RollbackOrUninstall(objTable, strInstallRoot, dblProgressBeforeCopy);
            }
            finally
            {
                EndInstallation();
            }
        }
        private void ExecuteUninstall()
        {
            InstallerFileTable objTable = null;
            InstallerBinary objBinary = null;
            InstallOptions objOptions = null;

            InstallState = Installer.InstallState.Uninstalling;

            //TODO: uninstaller has to store options AND uninstall table.
            try
            {
                objBinary = new InstallerBinary();
                objBinary.OpenStream();
                objOptions = new InstallOptions();
                objOptions.Deserialize(objBinary);
                objTable = new InstallerFileTable(this);
                objTable.Deserialize(objBinary);
                objBinary.CloseStream();

                ProgramInfo objInfo = new ProgramInfo(objOptions);

                string strInstallRoot = objOptions.GetOptionValueOrDefault(InstallOption.UninstallFolderRoot_Uninstaller_Only);
                if (!System.IO.Directory.Exists(strInstallRoot))
                    Globals.Logger.LogError("Fatal Error: The original install directory '" 
                        + strInstallRoot 
                        + "' does not exist, or was not packed in the uninstaller binary.", true, true);

                RollbackOrUninstall(objTable, strInstallRoot, 0.0);

                RemoveRegistryKeys(objInfo);

                InstallState = Installer.InstallState.Successful;
            }
            catch (Exception ex)
            {
                InstallState = Installer.InstallState.Canceled;
                InstallErrors.Add("Error: " + ex.ToString());
                Globals.Logger.LogError(ex.ToString(), false, true);
            }
            finally
            {
                EndUninstall();
            }
        }
       
        
        private void DisplayMsg(string msg)
        {
            Globals.Logger.LogInfo(msg);
            _strCurrentFile = msg;
        }

        private void RollbackOrUninstall(InstallerFileTable objTable, string strInstallRoot, double initialProgress)
        {
            InstallState = InstallState.Rollingback;
            if (objTable != null)
                objTable.RemoveInstalledFiles(strInstallRoot, ref _dblProgress, ref _strCurrentFile, initialProgress);
        }

        public static bool ApplicationIsInstalled(ProgramInfo objProgramInfo)
        {
            bool blnValue = true;
            RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey(@"Software", true);
            if (softwareKey == null)
                blnValue = false;
            else
            {
                RegistryKey publisherKey;
                publisherKey = softwareKey.OpenSubKey(objProgramInfo.Publisher, true);
                if (publisherKey == null)
                    blnValue = false;
                else
                {
                    RegistryKey appKey;
                    appKey = publisherKey.OpenSubKey(objProgramInfo.DisplayName, true);
                    if (appKey == null)
                        blnValue = false;
                    else
                        appKey.Close();

                    publisherKey.Close();
                }
                softwareKey.Close();
            }
            return blnValue;
        }

        private string CreateInstallRoot()
        {
            string strInstallRoot = string.Empty;
            try
            {
                // - Checks
                Globals.Logger.LogInfo("Creating Root");
                InstallOption location = GetOption(InstallOption.InstallLocation);
                if (location == null)
                    Globals.Throw("Install location was not set.");

                strInstallRoot = ReplaceValueVariables(location.Value);

                Globals.Logger.LogInfo("Creating Root Dir..");
                if (!System.IO.Directory.Exists(strInstallRoot))
                    System.IO.Directory.CreateDirectory(strInstallRoot);
            }
            catch (Exception ex)
            {
                InstallErrors.Add("Error: " + ex.ToString());
                Globals.Logger.LogError(ex.ToString());
                return "";
            }
            return strInstallRoot;
        }
        private void EndInstallation()
        {
            if (InstallState == Installer.InstallState.Rollingback)
                InstallState = InstallState.Canceled;
            else if (InstallState == Installer.InstallState.Installing)
                InstallState = Installer.InstallState.Successful;
            else
                Globals.Throw("Install state was invalid for end installation.  The program may not have installed.");
        }
        private void EndUninstall()
        {
        }
        private void CreateUninstaller(ProgramInfo objProgramInfo, InstallerBinary objBinary, InstallerFileTable objTable, string strInstallRoot)
        {
            string strUninstallExePath;

            Globals.Logger.LogInfo("Creating Uninstaller");

            //Add the root to the options.
            _objOptions.AddOrReplaceOption(new InstallOption(InstallOption.UninstallFolderRoot_Uninstaller_Only, strInstallRoot));

            strUninstallExePath = objTable.WriteUninstaller(_objOptions, strInstallRoot);

            WriteUninstallerRegistryKeys(objProgramInfo, strUninstallExePath, strInstallRoot);
        }
        private void CreateAppRegistryKeys(ProgramInfo objProgramInfo, string strInstallRoot)
        {
            // Adds HKLM\Software\*Publisher*\*App*
            // Adds HKLM\Software\*Publisher*\*App*\InstallDir
            //**Note: registry keys in HKLM will fall under Wow6432Node in 32 bit apps. HKLM\Software\Wow6432Node\..

            DisplayMsg("Creating App Reg key");

            RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey(@"Software", true);

            if (!string.IsNullOrEmpty(objProgramInfo.Publisher))
            {
                RegistryKey publisherKey;
                publisherKey = softwareKey.OpenSubKey(objProgramInfo.Publisher, true);
                if (publisherKey == null)
                    publisherKey = softwareKey.CreateSubKey(objProgramInfo.Publisher);

                RegistryKey appKey;
                appKey = publisherKey.OpenSubKey(objProgramInfo.DisplayName, true);
                if (appKey == null)
                    appKey = publisherKey.CreateSubKey(objProgramInfo.DisplayName);

                appKey.SetValue("InstallDir", strInstallRoot);

                if (appKey != null)
                    appKey.Close();
                if (publisherKey != null)
                    publisherKey.Close();
            }
            else
                Globals.Logger.LogWarn("Program publisher was not set in the install config.  Cannot create /software registry key.");

            if (softwareKey != null)
                softwareKey.Close();
        }
        private RegistryKey GetUninstallerRegistryKey(ProgramInfo objProgramInfo, bool blnCreateIfNotFound = false)
        {
            RegistryKey appKey;
            RegistryKey softwareKey;

            softwareKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);

            if (softwareKey == null)
                Globals.Logger.LogError("HKLM\\Software registry key not found.", true, true);

            string guidText = objProgramInfo.GetUninstallerGuidAsString();
            appKey = softwareKey.OpenSubKey(guidText, true) ?? softwareKey.CreateSubKey(guidText);

            if (appKey == null)
                Globals.Throw(String.Format("Unable to create uninstaller {0}", guidText));

            if (softwareKey != null)
                softwareKey.Close();

            return appKey;
        }
        private void WriteUninstallerRegistryKeys(ProgramInfo objProgramInfo, string strUninstallExePath, string installLocation)
        {
            //https://msdn.microsoft.com/en-us/library/aa372105(v=vs.85).aspx
            RegistryKey key = GetUninstallerRegistryKey(objProgramInfo, true);

            WriteUninstallRegistryKeyValues(objProgramInfo, key, strUninstallExePath, installLocation);
            if (key != null)
                key.Close();
        }
        private void WriteUninstallRegistryKeyValues(ProgramInfo objProgramInfo, RegistryKey key, string strUninstallExePath, string installLocation)
        {
            //**Note: if you modify this to add keys make sure to modify the RemoveInstallerRegistryKeys
            if (!String.IsNullOrEmpty(objProgramInfo.DisplayName))
                key.SetValue("DisplayName", objProgramInfo.DisplayName);
            else
            {
                Globals.Logger.LogWarn("DisplayName not set in installer configuration.");
                key.SetValue("DisplayName", "Spark Program");
            }

            key.SetValue("InstallLocation", installLocation);
            key.SetValue("ApplicationVersion", objProgramInfo.ApplicationVersion);
            key.SetValue("DisplayVersion", objProgramInfo.DisplayVersion);

            if (!String.IsNullOrEmpty(objProgramInfo.Publisher))
                key.SetValue("Publisher", objProgramInfo.Publisher);
            //  Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            key.SetValue("DisplayIcon", strUninstallExePath);

            if (!string.IsNullOrEmpty(objProgramInfo.URLInfoAbout))
                key.SetValue("URLInfoAbout", objProgramInfo.URLInfoAbout);
            if (!string.IsNullOrEmpty(objProgramInfo.Contact))
                key.SetValue("Contact", objProgramInfo.Contact);
            if (!string.IsNullOrEmpty(objProgramInfo.Comments))
                key.SetValue("Comments", objProgramInfo.Comments);


            key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
            key.SetValue("UninstallString", strUninstallExePath + " " + SparkFlags.Uninstall);

        }
        private void RemoveRegistryKeys(ProgramInfo objProgramInfo)
        {
            RemoveAppRegistryKeys(objProgramInfo);
            RemoveInstallerRegistryKeys(objProgramInfo);
        }
        private void RemoveAppRegistryKeys(ProgramInfo objProgramInfo)
        {
            // ** This keeps the publisher key there. I tis possible to remove publisher key
            // if there are no further keys.. but we don't do that.
            RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey(@"Software", true);
            if (softwareKey!= null)
            {
                RegistryKey publisherKey;
                publisherKey = softwareKey.OpenSubKey(objProgramInfo.Publisher, true);
                if (publisherKey != null)
                {
                    publisherKey.DeleteSubKeyTree(objProgramInfo.DisplayName, false);
                    publisherKey.Close();
                }
                else
                    Globals.Logger.LogError("Could not find application subkey " + objProgramInfo.DisplayName + ".  The uninstall may not have succeeded.");
                softwareKey.Close();
            }
            else
                Globals.Logger.LogError("Could not find HKLM\\Software.  The uninstall may not have succeeded.");
        }
        private void RemoveInstallerRegistryKeys(ProgramInfo objProgramInfo)
        {
            RegistryKey softwareKey;

            string guidText = objProgramInfo.GetUninstallerGuidAsString(); 

            softwareKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);

            if (softwareKey == null)
            {
                Globals.Logger.LogError("HKLM\\Software registry key not found.");
                return;
            }

            softwareKey.DeleteSubKeyTree(guidText, false);

            if (softwareKey != null)
                softwareKey.Close();
        }
        #endregion

    }
}
