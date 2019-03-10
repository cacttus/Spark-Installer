using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;

namespace Spark.Installer
{
    public class FileTableEntry
    {
        public Int64 Offset;  // offset of file relative to the beginning of our .exe
        public Int64 Size;    // size of file
        public string Path;  // file path, this may have $ wildcards
        public FileTableEntry(long off, long size, string path)
        {
            Offset = off;
            Size = size;
            Path = path;
        }
        public byte[] Serialize()
        {
            //off[4], size[4], pathlen[4], path[n]
            byte[] ret = new byte[0];
            ret = BufferUtils.Combine(ret, BitConverter.GetBytes(Offset));
            ret = BufferUtils.Combine(ret, BitConverter.GetBytes(Size));
            ret = BufferUtils.Combine(ret, StringUtils.SerializeUTF8Path(Path));
            return ret;
        }
        public static FileTableEntry Deserialize(InstallerBinary objBinary)
        {
            Int64 off = objBinary.ReadInt64();
            Int64 size = objBinary.ReadInt64();
            string path = objBinary.ReadUTF8Path();
            FileTableEntry ret = new FileTableEntry(off, size, path);
            return ret;
        }
    }
    public class InstallerFileTable
    {
        #region Private:Members
        InstallerConfig _objConfig = null;
        List<FileTableEntry> _lstEntries = new List<FileTableEntry>();
        InstallManager _objManager = null;
        #endregion
        
        #region Public:Members
     //   public List<string> InstalledFiles = new List<string>();
        #endregion

        #region Public:Methods

        public InstallerFileTable(InstallManager mgr)
        {
            _objManager = mgr;
        }
        public InstallerFileTable(InstallerConfig config)
        {
            _objConfig = config;
        }

        // - BaseOffset is the current offset of the program binary.  File table must start right after binary
        public byte[] Build(int baseOffset)
        {
            if (_objConfig == null)
                Globals.Throw("Installer config was null for build().  Please use the constructor tp pass the installer config into the InstallerFIleTable.");

            //Construct table entries
            foreach (InstallerFile file in _objConfig.GetFiles())
            {
                System.IO.FileInfo inf = new System.IO.FileInfo(file.LocalPath);
                inf.Refresh();
                _lstEntries.Add(new FileTableEntry(0, inf.Length, file.InstallerPath));
            }

            //Now serialize the class and add the length of this table to all file offsets.
            //Offsets will be invalid
            long tablen = Serialize().Length;

            long runningTotal = baseOffset; // Size of binary
            //runningTotal += toklen; // the "token" string length (bytes)
            runningTotal += tablen; // Length of the file table.

            foreach (FileTableEntry ent in _lstEntries)
            {
                ent.Offset = runningTotal;
                runningTotal += ent.Size;
            }

            //Serialize again returning the the correct offsets
            return Serialize();
        }
        public void Deserialize(InstallerBinary objBinary)
        {
            Int64 entries = objBinary.ReadInt64();
            for (Int64 iEntry = 0; iEntry < entries; iEntry++)
            {
                FileTableEntry ent = FileTableEntry.Deserialize(objBinary);
                _lstEntries.Add(ent);
            }
        }
        public byte[] PackFiles(BuildForm bf, double remainingProgress)
        {
            byte[] ret = new byte[0];
            byte[] tmp;
            double baseProgress = bf.CurrentProgress();

            int iFile=0;
            foreach (InstallerFile file in _objConfig.GetFiles())
            {

                if (!System.IO.File.Exists(file.LocalPath))
                    Globals.Throw("The file " + file.LocalPath + " does not seem to exist, or there is a lock preventing Spark from reading it.  Skip Failures is not set to true. Spark will now exit.");

                bf.DetailMessage("Packing: " + file.LocalPath);

                Globals.Logger.LogInfo("Packing " + file.LocalPath);
                tmp = System.IO.File.ReadAllBytes(file.LocalPath);
                ret = BufferUtils.Combine(ret, tmp);

                iFile++;

                double progress = baseProgress +
                    ((double)iFile / (double)_objConfig.GetFiles().Count) * (1.0 - baseProgress - remainingProgress);

                bf.Progress(progress);
            }
            return ret;
        }
        public void UnpackFiles(InstallerBinary objBinary, string strInstallRoot, 
            ref double dblProgress, System.Threading.CancellationToken token, ref string strCurrentFile)
        {
            if (!System.IO.Directory.Exists(strInstallRoot))
                System.IO.Directory.CreateDirectory(strInstallRoot);

            double dblInitialProgress = dblProgress;
            
            // Unpack all files.
            for (int iEntry = 0; iEntry < _lstEntries.Count; iEntry++)
            {
                FileTableEntry entry = _lstEntries[iEntry];

                UnpackFile(objBinary, entry, ref strCurrentFile);

                dblProgress = dblInitialProgress + ((double)iEntry / (double)_lstEntries.Count * (1.0 - dblInitialProgress));

                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
            }
        }
        public void RemoveInstalledFiles(
            string strInstallRoot, 
            ref double dblProgress, 
            ref string strCurrentFile, 
            double dblInitialProgress
            )
        {
            Globals.Logger.LogInfo("Rolling back installation.");

            for (int iEntry = 0; iEntry < _lstEntries.Count; iEntry++)
            {
                FileTableEntry entry = _lstEntries[iEntry];
                string strFilePath = entry.Path;
                try
                {
                    strFilePath = _objManager.ReplaceValueVariables(strFilePath);
                    strCurrentFile = strFilePath;

                    if (System.IO.File.Exists(strFilePath))
                        System.IO.File.Delete(strFilePath);
                }
                catch (Exception ex)
                {
                    Globals.Logger.LogError("Failed to delete file " + strFilePath + ".\r\n" + ex.ToString());
                }
                dblProgress = dblInitialProgress + ((double)(_lstEntries.Count - iEntry) / (double)_lstEntries.Count);
            }


           /* foreach (string file in InstalledFiles)
            {
                strCurrentFile = file;
                Globals.Logger.LogInfo("Deleting " + file);
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception ex)
                {
                    Globals.Logger.LogError("Failed to delete file " + file + ".\r\n" + ex.ToString());
                }
                // Show the progress bar "reversing"
                dblProgress = dblInitialProgress + ((double)(InstalledFiles.Count - iEntry) / (double)InstalledFiles.Count);
                iEntry++;
            }*/
            Globals.Logger.LogInfo("Current directory = " + System.IO.Directory.GetCurrentDirectory());

            RemoveDeleteDirectoryRecursive(strInstallRoot);
        }
        public void RemoveDeleteDirectoryRecursive(string strPath)
        {
            // Does not delete files the installer didn't put in.

            string strOriginalDir = System.IO.Directory.GetCurrentDirectory();
            System.IO.Directory.SetCurrentDirectory(strPath);

            string[] dirs = System.IO.Directory.GetDirectories(strPath);
            foreach (string dir in dirs)
            {
                RemoveDeleteDirectoryRecursive(dir);
            }

            //set this first because we can't delete the directory if we are inside it
            System.IO.Directory.SetCurrentDirectory(strOriginalDir);

            try
            {
                if (System.IO.Directory.Exists(strPath))
                {
                    if (System.IO.Directory.GetFiles(strPath).Length == 0)
                    {
                        Globals.Logger.LogInfo("Deleting directory " + strPath);
                        System.IO.Directory.Delete(strPath, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Logger.LogError("Rollback: Could not delete directory:\r\n" + ex.ToString());
            }

        }
        public string WriteUninstaller(InstallOptions objOptions, string strInstallRoot)
        {
            // ** Add the installer to the install directory so we can uninstall.
            string uninstallFileName = System.IO.Path.Combine(strInstallRoot, "Uninstall.exe");
            
            Globals.Logger.LogInfo("Writing File " + uninstallFileName);

            InstallerBuilder builder = new InstallerBuilder();
            builder.BuildUninstaller(this, objOptions, uninstallFileName);
            
            //InstalledFiles.Add(uninstallFileName);
            return uninstallFileName;
        }
        public byte[] Serialize()
        {
            byte[] ret = new byte[0];
            byte[] tmp;

            Int64 numEntries = _lstEntries.Count;
            ret = BufferUtils.Combine(ret, BitConverter.GetBytes(numEntries));
            foreach (FileTableEntry ent in _lstEntries)
            {
                tmp = ent.Serialize();
                ret = BufferUtils.Combine(ret, tmp);
            }

            return ret;
        }
        #endregion

        #region Private: Methods

        private void UnpackFile(InstallerBinary objBinary, FileTableEntry entry, ref string strCurrentFileName)
        {
            string strFilePath = entry.Path;

            strFilePath = _objManager.ReplaceValueVariables(strFilePath);

            strCurrentFileName = strFilePath;

            if (System.IO.File.Exists(strFilePath))
                Globals.Logger.LogInfo("Overwriting " + strFilePath);

            string dirname = System.IO.Path.GetDirectoryName(strFilePath);
            if (!System.IO.Directory.Exists(dirname))
            {
                Globals.Logger.LogInfo("Creating " + dirname);
                System.IO.Directory.CreateDirectory(dirname);
            }

            byte[] fileBytes = objBinary.ReadFileBytes(entry);

            Globals.Logger.LogInfo("Writing File " + strFilePath);

            FileUtils.WriteBytesViaStream(strFilePath, fileBytes);
     
           // System.IO.File.WriteAllBytes(strFilePath, fileBytes);

           // InstalledFiles.Add(strFilePath);
        }

        #endregion

    }
}
