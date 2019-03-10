#	Spark Simple Windows Installer
#	Derek Page (Fulmination Interactive)
#	10/18/2015
#
# How to Use
#	Run Spark.exe with /b /c:"./MyConfig.config" /o:"./MyInstaller.exe"
#	/c: configuration file name. (quotes required)
#	/o: output installer (quotes required)
#
# Required Options
#	The following options must be set in the Install section of the config file:
#		AppGuid - This must be set to the application you are installing.  To get your application guid right click on
#				  the project in Visual Studio solution explorer > Properties > Assembly information.
#		DisplayName - The name of the program.  This is visible to users in the installer window.
#
#
# For an example configuration see install_config.config
#
#
# Commands/Options
#	BuildSection - the section with all build directions.  (1 only)
#	InstallSection - The section with all install directions (1 only)
#	Include "(file path or directory path)" "(copied file path) - include all files in that directory and copy to "copied file path"
#	Exclude "file or directory" - exclude all files under directory, or a specific file from the installer.
#   ExcludeExt - exclude all files with extension.  You must include a dot before the extension.
#	DefaultDirectory - the default path that shows up when the installer runs.
#
#	**ALL PATHS MUST BE DOUBLE QUOTED**
#   **ALL RELATIVE PATHS ARE RELATIVE TO THE LOCATION OF THIS FILE***
#
# Install Location Variables
#  Variables are preceded by ?
#  ?PROGRAM_FILES = (windows path)\Program Files --**Note: if spark is 32 bit this will default to Program Files (x86) version
#  ?PROGRAM_FILES_x86 = (windows path)\Program Files (x86)
#  ?INSTALLDIR = The installed root directory the program is installed into.  This is selected by the user in the installer.
#  ?SYSTEMROOT  = drive that /Windows is installed on such as C:\Windows or D:\Windows
#  ?DESKTOP  = path to current user's desktop
#
#	*Wildcards/Regexs are not supported (*?+)


