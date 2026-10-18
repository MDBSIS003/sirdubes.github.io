# Open the Windows Forms app

1. Extract the whole ZIP to a writable Windows folder.
2. Install Visual Studio 2026 with **.NET desktop development**, .NET 10 SDK and **SQL Server Express LocalDB**.
3. Run **SETUP-DEMO.cmd** once. If PowerShell blocks it, use the four Database scripts in SSMS/SQL Server Object Explorer (Database-Setup.md).
4. Open **FuturePath-App.sln**. Build with **Ctrl+Shift+B**, then press **F5**.

The actual startup project is **FuturePath.Desktop**. The full `FuturePath.sln` also contains test projects; right-click Desktop → Set as Startup Project if needed. A terminal printing PASS is the test runner, not the application. `RUN-FUTUREPATH.cmd` is another way to build and start only the app.

The result should be one **Future Path** window with overview, sidebar and LocalDB database name. New advisory session uses retained panels with validated Next/Back. Reports and staff editors stay inside the shell. Code-built controls remain editable in C#; separate drag-and-drop Designer files are not part of the reference architecture.

Default server/database: `(localdb)\MSSQLLocalDB` / `FuturePathStudentAdvisingSystem`, Windows authentication. No SQLite file is used. Startup verifies setup; it does not reset data. Optional argument: `--database FuturePathDemo2026` after explicitly preparing that database name. A `.db` path is not accepted.

NuGet restore is necessary for Microsoft.Data.SqlClient and existing test packages. This project is not package-free. Read README.md, docs/Dependencies.md and docs/Validation.md for exact restore/build/tests and unverified Windows checks.
