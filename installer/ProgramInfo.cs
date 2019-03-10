using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proteus;

namespace Spark.Installer
{
    public class ProgramInfo
    {
        public string DisplayName           ;
        public string InstallLocation       ;
        public string Publisher             ;
        public string DisplayIcon           ;
        public string ApplicationVersion    ;
        public string DisplayVersion        ;
        public string URLInfoAbout          ;
        public string Contact               ;
        public string Comments              ; //Comments provided to the Add or Remove Programs control panel.
        public string AppGuid               ; // NOTE: this may NOT represent the correct Guid.

        public ProgramInfo(InstallOptions objOptions)
        {
            DisplayName         =  objOptions.GetOptionValueOrDefault( InstallOption.DisplayName        );
            InstallLocation     =  objOptions.GetOptionValueOrDefault( InstallOption.InstallLocation    );
            Publisher           =  objOptions.GetOptionValueOrDefault( InstallOption.Publisher          );
            DisplayIcon         =  objOptions.GetOptionValueOrDefault( InstallOption.DisplayIcon        );
            ApplicationVersion  =  objOptions.GetOptionValueOrDefault( InstallOption.ApplicationVersion );
            DisplayVersion      =  objOptions.GetOptionValueOrDefault( InstallOption.DisplayVersion     );
            URLInfoAbout        =  objOptions.GetOptionValueOrDefault( InstallOption.URLInfoAbout       );
            Contact             =  objOptions.GetOptionValueOrDefault( InstallOption.Contact            );
            AppGuid             =  objOptions.GetOptionValueOrDefault( InstallOption.AppGuid            );
        }
        public static string TryParseGuidToUninstallGuid(string strGuid)
        {
            Guid uninstallGuid;

            if (!Guid.TryParse(strGuid, out uninstallGuid))
                Globals.Throw("The supplied application GUID could not be parsed. The given string was invalid.  Please check the installer configuration.");

            return uninstallGuid.ToString("B");
        }
        public string GetUninstallerGuidAsString()
        {
            return TryParseGuidToUninstallGuid(AppGuid);
        }
    }
}
