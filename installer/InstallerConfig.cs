using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;
using System.IO;
namespace Spark.Installer
{

    public class InstallerFile
    {
        public string InstallerPath;
        public string LocalPath;
        public InstallerFile() { }
        public InstallerFile(string local, string install)
        {
            InstallerPath = install;
            LocalPath = local;
        }
    }
    public class InstallerConfig
    {
        private List<InstallerFile> _lstFiles = new List<InstallerFile>();// All files. This is the final list of files the installer will package.

        private List<InstallerFile> _lstIncludeFiles = new List<InstallerFile>();
        private List<InstallerFile> _lstExcludeFiles = new List<InstallerFile>();
        private List<InstallerFile> _lstIncludeDirs = new List<InstallerFile>();
        private List<InstallerFile> _lstExcludeDirs = new List<InstallerFile>();

        private InstallOptions _objInstallOptions = new InstallOptions();

        private List<string> _lstExcludeExtensions = new List<string>();
        private string _strConfigPath = string.Empty;

        public int NumExcludedFiles = 0;
        public int NumExcludedDirectories = 0;

        public InstallOptions Options { get { return _objInstallOptions; } private set { _objInstallOptions = value;  } }

        public List<InstallerFile> GetFiles() { return _lstFiles; }
        public InstallerConfig(string path)
        {
            //Make sure the path is rooted
            _strConfigPath = path;
            if (!System.IO.Path.IsPathRooted(_strConfigPath))
                _strConfigPath = System.IO.Path.GetFullPath(_strConfigPath);
        }
        public byte[] Serialize()
        {
            return _objInstallOptions.Serialize();
        }
        private bool IsDirectory(string path)
        {
            FileAttributes attr;
            attr = FileAttributes.Normal;

            // Append location of config file to path.
            if (!System.IO.Path.IsPathRooted(path))
                path = System.IO.Path.Combine(_strConfigPath, path);

            try
            {
                attr = File.GetAttributes(path);
            }
            catch (DirectoryNotFoundException)
            {
                Globals.Throw("Directory " + path + " doesn't exist. While compiling file list. (01875912)");
            }
            catch (FileNotFoundException)
            {
                Globals.Throw("File " + path + " doesn't exist. While compiling file list. (01899625)");
            }

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            return false;
        }
        private void ThrowInvalidNumberOfParameters(string pname, int line)
        {
            Globals.Throw("Invalid number of parameters to argument " + pname + " at line " + line.ToString());
        }

        #region Config
        public void Load()
        {
            string[] lines = System.IO.File.ReadAllLines(_strConfigPath);
            for (int iLine = 0; iLine < lines.Length; iLine++)
            {
                ParseCommand(lines[iLine], iLine);
            }
        }
        private bool IsComment(string line)
        {
            string st = line.TrimStart();
            if (st[0] == '#')
                return true;
            return false;
        }
        private string[] ParseLine(string line)
        {
            List<string> tokens = new List<string>();
            string curToken = string.Empty;
            bool instr = false;
            foreach (char c in line.ToCharArray())
            {
                if ((c == ' ' || c == '\t' || c == '\r' || c == '\n') && !instr && curToken.Length > 0)
                {
                    //Whitespace
                    tokens.Add(curToken);
                    curToken = string.Empty;
                }
                else if (c == '"')
                {
                    //Quoted string
                    curToken += c;
                    instr = !instr;
                }
                else if ((c == '#') && !instr)
                {
                    //Line comment
                    break;
                }
                else if (c == '\n')
                {
                    //End of line
                    break;
                }
                else
                {
                    curToken += c;
                }

            }
            if (curToken.Length > 0)
                tokens.Add(curToken);
            return tokens.ToArray();
            //line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private void ParseCommand(string line, int iLine)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
                return;
            if (IsComment(line))
                return;

            string[] args = ParseLine(line);

            if (args.Length == 0)
                return;

            for (int i = 0; i < args.Length; i++)
                args[i] = args[i].Trim();

            List<string> lstInstallOptionKeys = new List<string> {
                InstallOption.DefaultDirectory      ,
                InstallOption.DisplayName           ,
                InstallOption.CreateDesktopShortcut ,
                InstallOption.CreateStartMenuFolder ,
                InstallOption.InstallLocation       ,
                InstallOption.Publisher             ,
                InstallOption.DisplayIcon           ,
                InstallOption.ApplicationVersion    ,
                InstallOption.DisplayVersion        ,
                InstallOption.URLInfoAbout          ,
                InstallOption.Contact               ,
                InstallOption.AppGuid               ,
           };


            if (args[0].Equals("BuildSection"))
            {
            }
            else if (args[0].Equals("InstallSection"))
            {
            }
            else if (args[0].Equals("Include"))
            {
                if (args.Length != 3)
                    ThrowInvalidNumberOfParameters(args[0], iLine);
                string local = StringUtils.Dequote(args[1]);
                string install = StringUtils.Dequote(args[2]);

                InstallerFile newFile = new InstallerFile(local, install);

                if (IsDirectory(newFile.LocalPath))
                    _lstIncludeDirs.Add(newFile);
                else
                    _lstIncludeFiles.Add(newFile);
            }
            else if (args[0].Equals("Exclude"))
            {
                if (args.Length != 2)
                    ThrowInvalidNumberOfParameters(args[0], iLine);
                string local = StringUtils.Dequote(args[1]);

                InstallerFile newFile = new InstallerFile(local, "");

                if (IsDirectory(newFile.LocalPath))
                    _lstExcludeDirs.Add(newFile);
                else
                    _lstExcludeFiles.Add(newFile);
            }
            else if (args[0].Equals("ExcludeExt"))
            {
                if (args.Length != 2)
                    ThrowInvalidNumberOfParameters(args[0], iLine);
                string ext = StringUtils.Dequote(args[1]);
                ext = ext.ToLower();
                _lstExcludeExtensions.Add(ext);
            }
            //Option Directives
            else if (lstInstallOptionKeys.Contains(args[0]))
            {
                if (args.Length != 2)
                    ThrowInvalidNumberOfParameters(args[0], iLine);
                _objInstallOptions.Options.Add(new InstallOption(args[0], StringUtils.Dequote(args[1])));
            }
            else
            {
                Globals.Logger.LogWarn("Unrecognized installer config command " + args[0]);
            }
        }
        #endregion

        public void BuildFileList()
        {
            if (_lstIncludeFiles.Count == 0 && _lstIncludeDirs.Count == 0)
                Globals.Throw("There were no files or directories to include.  Please check your config file: " + _strConfigPath);

            foreach (InstallerFile instDir in _lstIncludeDirs)
            {
                ExpandDirectory(instDir.LocalPath, instDir);
            }
            foreach (InstallerFile instFile in _lstIncludeFiles)
            {
                if (!FileIsExcluded(instFile.LocalPath))
                    _lstFiles.Add(instFile);
            }
        }
        private bool DirectoryIsExcluded(string path)
        {
            int count;
            if (_lstExcludeDirs == null)
                return false;

            count = _lstExcludeDirs.Where(x => x.LocalPath.Equals(path)).Count();
            if (count > 0)
                return true;

            return false;
        }
        private bool FileIsExcluded(string path)
        {
            int count;
            if (_lstExcludeFiles == null)
                return false;

            count = _lstExcludeFiles.Where(x => x.LocalPath.Equals(path)).Count();
            if (count > 0)
                return true;

            string ext = System.IO.Path.GetExtension(path).ToLower();
            count = _lstExcludeExtensions.Where(x => x.Equals(ext)).Count();
            if (count > 0)
                return true;

            return false;
        }
        private void ExpandDirectory(string curDir, InstallerFile rootDir)
        {
            string baseDir = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(curDir);
            string[] files = System.IO.Directory.GetFiles(curDir);

            foreach (string file in files)
            {
                //Files must be absolute paths.
                if (!FileIsExcluded(file))
                {
                    string relPath;

                    relPath = file.Substring(rootDir.LocalPath.Length, file.Length - rootDir.LocalPath.Length);
                    relPath = StringUtils.CombinePath(rootDir.InstallerPath, relPath);
                    _lstFiles.Add(new InstallerFile(file, relPath));
                }
                else
                    NumExcludedFiles++;
            }

            string[] dirs = System.IO.Directory.GetDirectories(curDir);
            foreach (string dir in dirs)
            {
                if (!DirectoryIsExcluded(dir))
                    ExpandDirectory(dir, rootDir);
                else
                    NumExcludedDirectories++;
            }

            System.IO.Directory.SetCurrentDirectory(baseDir);
        }

    }
}
