using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;

namespace Spark.Installer
{
    public class InstallerBuilder
    {
        byte[] _final = new byte[0];
        InstallerConfig _objConfig;
        InstallerFileTable _objFileTable;
        /*
         * See InstallerManager for file format.
         * 
         */
        public void BuildUninstaller(InstallerFileTable ft, InstallOptions opt, string outputFileName)
        {
            //**Do not modify the filename.
            InstallerBinary objBinary = new InstallerBinary();
            objBinary.OpenStream();
            objBinary.CloseStream();

            byte[] temp;
            temp = objBinary.GetInstallerBinary();
            _final = BufferUtils.Combine(_final, temp);
            temp = InstallerBinary.GetTokenBytes();
            _final = BufferUtils.Combine(_final, temp);
            temp = opt.Serialize();
            _final = BufferUtils.Combine(_final, temp);
            temp = ft.Serialize(); // ** Ft is already built.  Do not call Build()
            _final = BufferUtils.Combine(_final, temp);

            FileUtils.WriteBytesViaStream(outputFileName, _final);
        }
        /*
         * See InstallerManager for file format.
         * 
         */
        public void BuildInstaller(BuildForm bf)
        {
            if (String.IsNullOrEmpty(SparkGlobals.BuildConfigPath))
                Globals.Throw("Build config path was not set.  Use the " + Spark.Installer.SparkFlags.ConfigPath + " flag to set this (" + Spark.Installer.SparkFlags.ConfigPath + ":\"path\\path\")");
            if (String.IsNullOrEmpty(SparkGlobals.OutputFile))
                Globals.Throw("Output file was not set.  Use the " + Spark.Installer.SparkFlags.OutputFile + " flag to set this (" + Spark.Installer.SparkFlags.OutputFile + ":\"file.exe\") ");

            LoadConfiguration(bf);
            BuildPostfixedBinary(bf);
            BuildConfig(bf);
            BuildFileTable(bf);
            BuildFiles(bf);
            WriteFile(bf);
        }
        private void LoadConfiguration(BuildForm bf)
        {
            _objConfig = new InstallerConfig(SparkGlobals.BuildConfigPath);
            _objConfig.Load();
            _objConfig.BuildFileList();

            if (_objConfig.Options.GetOption(InstallOption.AppGuid) == null)
                Globals.Throw("The install option 'AppGuid' is required. Please add this option to the input configuration file.");
            
            //Attempt to parse GUID - if it is invalid an exception will throw.
            ProgramInfo.TryParseGuidToUninstallGuid(_objConfig.Options.GetOption(InstallOption.AppGuid).Value);

            if (_objConfig.Options.GetOption(InstallOption.DisplayName) == null)
                Globals.Throw("The install option 'DisplayName' is required. Please add this option to the input configuration file.");

            Globals.Logger.LogInfo("Excluded " + _objConfig.NumExcludedFiles + " files.");
            Globals.Logger.LogInfo("Excluded " + _objConfig.NumExcludedDirectories + " directories.");
            Globals.Logger.LogInfo("Packaging " + _objConfig.GetFiles().Count + " total files.");

            _objFileTable = new InstallerFileTable(_objConfig);
            bf.Progress(0.01);
        }
        private void BuildPostfixedBinary(BuildForm bf)
        {
            byte[] temp;
            bf.DetailMessage("Building Binary.");
            temp = InstallerBinary.GetBytes();
            _final = BufferUtils.Combine(_final, temp);
            Globals.Logger.LogInfo("Binary:" + temp.Length.ToString() + "B Total:" + _final.Length.ToString() + "B");
            bf.Progress(0.03);
        }
        private void BuildConfig(BuildForm bf)
        {
            byte[] temp;
            bf.DetailMessage("Building Config.");
            temp = _objConfig.Serialize();
            _final = BufferUtils.Combine(_final, temp);
            Globals.Logger.LogInfo("Config:" + temp.Length.ToString() + "B Total:" + _final.Length.ToString() + "B");
            bf.Progress(0.05);
        }
        private void BuildFileTable(BuildForm bf)
        {
            byte[] temp;
            bf.DetailMessage("Building File Table.");
            temp = _objFileTable.Build(_final.Length);
            _final = BufferUtils.Combine(_final, temp);
            Globals.Logger.LogInfo("FileTable:" + temp.Length.ToString() + "B Total:" + _final.Length.ToString() + "B");
            bf.Progress(0.12);
        }
        private void BuildFiles(BuildForm bf)
        {
            double remainingProgress = 0.02;
            byte[] temp;
            bf.DetailMessage("Building Files.");
            temp = _objFileTable.PackFiles(bf, remainingProgress);
            _final = BufferUtils.Combine(_final, temp);
            Globals.Logger.LogInfo("Files:" + temp.Length.ToString() + "B Total:" + _final.Length.ToString() + "B");
            bf.Progress(1.0-remainingProgress);
        }
        private void WriteFile(BuildForm bf)
        {
            bf.DetailMessage("Saving " + SparkGlobals.OutputFile + ".");
            System.IO.File.WriteAllBytes(SparkGlobals.OutputFile, _final);
            bf.Progress(1.0);
        }

    }
}
