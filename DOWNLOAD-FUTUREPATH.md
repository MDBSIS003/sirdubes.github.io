# Future Path — Visual Studio project, display correction 1.0.1

[Download the corrected Visual Studio ZIP](https://github.com/MDBSIS003/sirdubes.github.io/raw/refs/heads/integration/localdb-single-window/FuturePath_VisualStudio_DisplayFix.zip)

Extract All into a new folder, open **FuturePath.sln**, then choose **Debug → Start Debugging** or press F5. The project requires Windows, Visual Studio 2026 with .NET desktop development, .NET 10 SDK and SQL Server Express LocalDB.

This version addresses the layout seen at 200% display scaling: forms/pages share a DPI baseline, sidebar text/buttons fit their container, and cards resize with the viewport. Keep your preferred Windows display scale. The source is also available in **FuturePath-LocalDB/**. The older ZIP filename now contains this same updated package.

The same Windows installation/account reuses its existing **FuturePathStudentAdvisingSystem** database at **(localdb)\MSSQLLocalDB**. No database scripts need to be repeated just for this layout correction. A different Windows installation has its own LocalDB instance.

Validation: the complete solution and a fresh ZIP extraction build with zero warnings/errors. Three Windows overview resize regression cases compile. Windows 10/11 visual DPI checks remain pending; they cannot run in the Linux build environment. Details: FuturePath-LocalDB/docs/Validation.md.
