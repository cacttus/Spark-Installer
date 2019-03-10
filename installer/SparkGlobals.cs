using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;

namespace Spark.Installer
{
    public enum SparkProgramMode
    {
        Build,  // build an executable
        Install, // install the files.
        Uninstall // uninstall files.
        //Repair, //. ** Updat e/ reinstall
    }
    public class SparkFlags
    {
        //See SpartanGlobals.ParseArgs
        public const string Build = "/b";
        public const string ConfigPath = "/c";
        public const string OutputFile = "/o";
        public const string Uninstall = "/u";
    }
    public class SparkGlobals
    {

        public static string InstallerProgramName = "Spark Installer";
        public static SparkProgramMode ProgramMode = SparkProgramMode.Install;
        public static string BuildConfigPath = string.Empty;
        public static string OutputFile = string.Empty;
        public static bool GlobalsInitialized = false;

        public static void InitializeGlobals(List<string> args)
        {
            try
            {
                string tempPath = System.IO.Path.GetTempPath();
                tempPath = System.IO.Path.Combine(tempPath, "SparkInstaller");//Critical we have this folder so logger can move its files.
                string tempFile = System.IO.Path.Combine(tempPath, "SparkInstall.log");
                Globals.InitializeGlobals(tempFile);
                Globals.Logger.LogInfo("Setting Vc Environment Vars (for process only)");
            }
            catch (Exception ex)
            {
                BuildUtils.ShowErrorMessage("Could not initialize Installer:\r\n" + ex.ToString());
            }

            string errTxt = ParseArgs(args);

            if (string.IsNullOrEmpty(errTxt) == false)
                Globals.Logger.LogWarn(errTxt);

            Globals.ProgramName = SparkGlobals.InstallerProgramName;

            GlobalsInitialized = true;
        }
        public static string ParseArgs(List<string> args)
        {
            string temp = string.Empty;
            string tstr = string.Empty;
            string errorTxt = string.Empty; // can't log in this method as logger isn't created yet

            foreach (string str in args)
            {
                tstr = str.Trim();

                if (StringUtils.ParseCmdArg(tstr, SparkFlags.Build, ref temp))
                {
                    ProgramMode = SparkProgramMode.Build;
                    
                }
                else if (StringUtils.ParseCmdArg(tstr, SparkFlags.ConfigPath, ref temp))
                {
                    BuildConfigPath = temp;
                }
                else if (StringUtils.ParseCmdArg(tstr, SparkFlags.OutputFile, ref temp))
                {
                    OutputFile = temp;
                }
                else if (StringUtils.ParseCmdArg(tstr, SparkFlags.Uninstall, ref temp))
                {
                    ProgramMode = SparkProgramMode.Uninstall;
                }
                else
                {
                    errorTxt += "Possible Unrecognized argument " + str + " ";
                }
            }
            return errorTxt;
        }
    }

}
