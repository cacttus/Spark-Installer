using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;

namespace Spark.Installer
{
    public class InstallerVariable
    {
        public const string ProgramFiles = "?PROGRAM_FILES";
        public const string ProgramFilesX86 = "?PROGRAM_FILES_X86";
        public const string InstallDir = "?INSTALLDIR";
        public const string Desktop = "?DESKTOP";
        public const string SystemRoot = "?SYSTEMROOT";
    }
    public class InstallOptionValue
    {
        public const string True = "True";
        public const string False = "True";
    }
    //key value pairs
    public class InstallOption
    {
        //Constants.
        public const string DefaultDirectory = "DefaultDirectory";
        public const string CreateDesktopShortcut = "CreateDesktopShortcut";
        public const string CreateStartMenuFolder = "CreateStartMenuFolder";
        //Program Info
        public const string DisplayName = "DisplayName";
        public const string InstallLocation = "InstallLocation";
        public const string Publisher = "Publisher";
        public const string DisplayIcon = "DisplayIcon";
        public const string ApplicationVersion = "ApplicationVersion";
        public const string DisplayVersion = "DisplayVersion";
        public const string URLInfoAbout = "URLInfoAbout";
        public const string Contact = "Contact";
        public const string UninstallFolderRoot_Uninstaller_Only = "UninstallFolderRoot_Uninstaller_Only"; //**DO NOT PACKAGE THIS WITH THE INSTALLER. This determines whether we are running from installer
        public const string AppGuid = "AppGuid";

        public string Key;
        public string Value;

        public InstallOption(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public byte[] Serialize()
        {
            byte[] ret = new byte[0];
            ret = BufferUtils.Combine(ret, StringUtils.SerializeUTF8Path(Key));
            ret = BufferUtils.Combine(ret, StringUtils.SerializeUTF8Path(Value));
            return ret;
        }
        public static InstallOption Deserialize(InstallerBinary binary)
        {
            string key = binary.ReadUTF8Path();
            string value = binary.ReadUTF8Path();
            return new InstallOption(key, value);
        }
    }

    public class InstallOptions
    {
        private List<InstallOption> _lstInstallOptions = new List<InstallOption>();
        public InstallOption GetOption(string key)
        {
            InstallOption opt = _lstInstallOptions.Where(x => x.Key.Equals(key)).FirstOrDefault();
            return opt;
        }
        public string GetOptionValueOrDefault(string key)
        {
            InstallOption opt = GetOption(key);
            if (opt == null)
                return string.Empty;
            return opt.Value;
        }
        
        public void AddOrReplaceOption(InstallOption opt)
        {
            InstallOption found = GetOption(opt.Key);
            if (found != null)
            {
                found.Value = opt.Value;
            }
            else
            {
                _lstInstallOptions.Add(opt);
            }
        }
        public List<InstallOption> Options
        {
            get
            {
                return _lstInstallOptions;
            }
            set
            {
                _lstInstallOptions = value;
            }
        }
        public void Deserialize(InstallerBinary objBinary)
        {
            Int64 count = objBinary.ReadInt64();
            for (Int64 iOption = 0; iOption < count; iOption++)
            {
                InstallOption opt = InstallOption.Deserialize(objBinary);
                _lstInstallOptions.Add(opt);
            }
        }
        public byte[] Serialize()
        {
            byte[] ret = new byte[0];
            byte[] tmp;

            Int64 numEntries = Options.Count;
            ret = BufferUtils.Combine(ret, BitConverter.GetBytes(numEntries));
            foreach (InstallOption ent in Options)
            {
                tmp = ent.Serialize();
                ret = BufferUtils.Combine(ret, tmp);
            }

            return ret;
        }
    }
}
