using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spark.Installer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SparkGlobals.InitializeGlobals(args.ToList());
            string a = System.IO.Directory.GetCurrentDirectory();
            string dir = System.IO.Path.GetDirectoryName(ExeUtils.GetCurrentExePath());
            System.IO.Directory.SetCurrentDirectory(dir);
            string b = System.IO.Directory.GetCurrentDirectory();
            //
            // Args to build
            // 
            if (SparkGlobals.ProgramMode == SparkProgramMode.Build)
            {
                BuildForm bf = new BuildForm();
                try
                {

                    bf.Show();
                    InstallerBuilder ib = new InstallerBuilder();
                    ib.BuildInstaller(bf);
                    bf.Hide();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
                
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
