using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;

namespace Spark.Installer
{
    public class InstallerBinary
    {
        private const string _cOffsetToken = "@SPARK_FILE_DATA_BEGIN";
        private System.IO.FileStream _objFileStream;
        private System.IO.BinaryReader _objReader;
        private byte[] _binaryData;

        public byte[] GetInstallerBinary()
        {
            return _binaryData;
        }
        public string ReadUTF8Path()
        {
            Int64 pathLen = _objReader.ReadInt64();
            byte[] buffer = new byte[pathLen];
            _objReader.Read(buffer, 0, buffer.Length);
            string ret = System.Text.Encoding.UTF8.GetString(buffer);
            return ret;
        }
        public Int64 ReadInt64()
        {
            return _objReader.ReadInt64();
        }
        public byte[] ReadFileBytes(FileTableEntry entry)
        {
            //Note: Entry.path is not correct here.
            byte[] buffer = new byte[entry.Size];

            _objReader.BaseStream.Seek(entry.Offset, System.IO.SeekOrigin.Begin);
            _objReader.Read(buffer, 0, buffer.Length);
            
            return buffer;
        }
        public void CloseStream()
        {
            _objFileStream.Close();
            _objFileStream.Dispose();
            GC.Collect();
            _objFileStream = null;
        }
        public static byte[] GetTokenBytes()
        {
            return System.Text.Encoding.ASCII.GetBytes(_cOffsetToken);
        }
        public static byte[] GetBytes()
        {
            byte[] ret = ExeUtils.GetCurrentExeBytes();
            //Postfix binary with offset token.
            byte[] tokenBytes = GetTokenBytes();
            ret = BufferUtils.Combine(ret, tokenBytes);
            return ret;
        }
        public void OpenStream()
        {
            long exeSize = ExeUtils.GetCurrentExeVirtualSize();
            long fileSize = ExeUtils.GetCurrentExeDiskSize();
            
            // Validate size
            if ((exeSize + _cOffsetToken.Length) > fileSize)
                Globals.Throw("Installer exe file size was too short.  It appears the installer did not pack any data. (0094812).");

            byte[] buffer = new byte[_cOffsetToken.Length];
            string exePath = ExeUtils.GetCurrentExePath();

            // Create stream
            _objFileStream = new System.IO.FileStream(exePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            _objReader = new System.IO.BinaryReader(_objFileStream);
            
            //_objReader.BaseStream.Seek(exeSize, System.IO.SeekOrigin.Begin);
            //Read the binary.
            _binaryData = new byte[exeSize];
            _objReader.Read(_binaryData, 0, _binaryData.Length);

            //Read the token
            _objReader.Read(buffer, 0, buffer.Length);

            // Validate Token
            string strToken = System.Text.Encoding.ASCII.GetString(buffer);
            if (!strToken.Equals(_cOffsetToken))
            {
                //**This is no longer valid.  The uninstaller MUST have the file table and the config (just no files)
                //Globals.Logger.LogError("Installer data token was invalid.  The installer exe computed size may be wrong. OR the user has tried to run the uninstaller without the /u switdch.  App will now exit.", false, true);
                //Environment.Exit(1);

                //This will throw
                Globals.Logger.LogError("Error installing/Uninstalling, Invalid binary Token. (01903)", true, true);
            }
            
        }

    }
}
