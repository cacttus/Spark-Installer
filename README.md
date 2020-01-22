# Spark Installer

Spark is a lightweight installer GUI application for Microsoft Windows.  The installer GUI application guides users through application installation, similar to the many other popular installers.  The design GUI interface provides a fully configurable interface to the software distributor.  

For a C++ version of application packaging, see the exe-packer application.  That application provides a C++ interface to read contents from a packaged executable file.

## Technical Overview

The installer packages application contents into a single executable with the contents appended to the end of the executable, and the GUI application as the start of the executable.  This is the way many installers work.  

## Considerations

There is currently no encryption in this.  We could add encryption to increase application security.  Additionally if we like we could even archive the applciation contents with password-protection.
 
