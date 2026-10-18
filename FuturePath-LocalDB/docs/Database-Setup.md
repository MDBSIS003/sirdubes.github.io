# SQL Server LocalDB setup

## Beginner route

1. In Visual Studio Installer choose Modify → .NET desktop development, then Individual components → SQL Server Express LocalDB. Keep .NET 10 installed.
2. Extract the whole project ZIP and double-click `FuturePath.sln`.
3. Press F5. The application window opens immediately. On first run choose **Prepare demonstration database** inside that window. The status bar names `FuturePathStudentAdvisingSystem`.

No PowerShell script is required for in-app setup. `SETUP-DEMO.cmd` remains an optional manual helper. PowerShell may block downloaded scripts under your existing security policy. Do not globally weaken the policy. Use the manual SQL route below if the helper is blocked. The helpers are source code for review; they were not executed on this Linux host.

## Manual route in Visual Studio / SSMS

Open View → SQL Server Object Explorer, add/connect to `(localdb)\MSSQLLocalDB` with Windows authentication. SSMS can connect to the same server. Open a query window and execute the complete contents of each file in order:

| File | Effect |
|---|---|
| `01_CreateDatabase.sql` | Create database only if missing; never drop it |
| `02_CreateTables.sql` | Create the 20 reference entities/tables, keys/checks/indexes and SchemaVersion; reject unknown unversioned tables |
| `03_SeedData.sql` | Optional explicit demo seed; transaction; skip all inserts if any domain records exist |
| `04_VerifyDatabase.sql` | Row counts, five named scenarios, missing-result/range checks and DBCC constraint checks |

Select database **FuturePathStudentAdvisingSystem** after script01. The `USE` statements also select it explicitly. Refresh Databases/Tables after running the scripts. All table and column names come from the supplied Visual Studio reference, not the smaller ZIP's incompatible earlier schema.

You should see10 learners,18 subjects,5 faculties,10 degrees,8 interest categories,8 questions,10 funding options and10 sessions. Stored recommendations include each selected result and up to five alternatives, computed by the C# rule engine when the seed file was generated.

Required primary outcomes: Gadija StrongMatch; Aadiel-Raboo NotCurrentlyEligible; Labeeqah PossibleMatch with Mathematics58 versus60; Siyabonga StrongMatch; Derek NotCurrentlyEligible with Mathematics and Physical Sciences absent. SQL stores enum identifiers; the GUI displays friendly spaced labels. Verify the explanations as well as the labels.

## Existing databases and persistence

Back up existing data before setup. Ordinary startup only checks SchemaVersion and opens short-lived connections. No automatic startup seed/reset occurs. If verification fails, the same main window offers an explicit demo-setup button. `DatabaseSetup.PrepareDemo` uses the embedded canonical SQL scripts; no terminal process is launched. Re-running the four scripts preserves existing data and credentials; the entire demo seed is skipped when the database is nonempty. Reference editing changes future calculations, while prior scores/explanations remain stored.

An unknown database with tables but no compatible SchemaVersion is deliberately refused by both in-app and manual setup. Choose a new name using `scripts/setup-database.ps1 -SeedDemo -DatabaseName FuturePathDemo2026`, then launch `RUN-FUTUREPATH.cmd --database FuturePathDemo2026`. Database names allow letters/numbers/underscores. Never delete an existing database to make setup pass.

No SQLite database file was supplied for data import. Existing `.db` files are left untouched. This edition does not maintain a parallel file database. The default LocalDB database also differs from the earlier integration's `FuturePathAdvisingSystem`, so that earlier data is not silently reused.

## Connection and errors

`src/FuturePath.Data/DatabaseConnection.cs` is the single connection helper. Default: `(localdb)\MSSQLLocalDB`, `FuturePathStudentAdvisingSystem`, integrated Windows security,15-second timeout. No SQL username/password. `TrustServerCertificate=True` is scoped to this local development SQL instance; downloads and package integrity checks remain enabled.

If unavailable, run `sqllocaldb info`, then `sqllocaldb start MSSQLLocalDB`; confirm your Windows account owns the instance and you ran setup under the same account. A connection error is shown inside the main window with Retry connection and Prepare demonstration database buttons, without resetting data. Empty lists generally mean the optional seed was not run or records are inactive; check the verification script and administrator reference records.

The SQL client is restored from NuGet by Visual Studio/.NET SDK. An installed .NET runtime alone cannot build source; install the SDK. The normal `FuturePath.sln` contains no test runner; its only application is FuturePath.Desktop.

## Backup and restore

Administrator → Administration → Back up database writes a new `.bak` using SQL Server `BACKUP DATABASE ... WITH COPY_ONLY,CHECKSUM`, followed by `RESTORE VERIFYONLY`. Existing files are refused. Choose a location writable by the LocalDB Windows account. This preserves staff backup capability without pretending that a SQL Server backup is a SQLite file.

Restore using SSMS against a **new destination database**, reviewing the file paths/MOVE options first. Protect the backup: it contains learner information. Do not overwrite a live or unrelated database. The LocalDB tests restore only to a uniquely named disposable test database and never target the application database.

## Windows tests

From the solution root run `dotnet test FuturePath.Development.sln -c Release`. Real integration tests create and remove only `FuturePathTest_<guid>` databases. Cleanup checks this exact prefix and GUID before deletion. If setup/restore/cleanup fails, the test fails; inspect the output rather than treating it as a pass. UI tests require Windows Forms and LocalDB. Neither was executed on Linux; see Validation.md.
