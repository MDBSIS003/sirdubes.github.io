# Build and test evidence — LocalDB migration

Validation date: 18 October 2026. Host: Linux x64, .NET SDK 10.0.402. This is source/build/portable evidence, not an interactive Windows or live LocalDB readiness claim.

## Before migration

The supplied reference was inspected and tested before editing. Its original SQLite suite executed **58 passed, 0 failed, 0 skipped** against real temporary SQLite databases. That baseline is retained under `validation/reference/reference-baseline.trx`. The original supplied TRX/hash list are separately labelled historical evidence. These results do not validate the LocalDB implementation.

## Current LocalDB edition

| Check | Actual result |
|---|---|
| Locked restore of full FuturePath.Development.sln | Passed |
| Full Release build, all five original projects | Passed; 0 warnings, 0 errors |
| App-only FuturePath.sln build | Passed; 0 warnings, 0 errors |
| Default solution restored/built from a fresh ZIP extraction | Passed; 0 warnings, 0 errors |
| Portable test project on Linux | **36 passed, 0 failed, 48 skipped** LocalDB cases/theories |
| Five required named learner outcomes | Passed using the real engine and exact reference inputs |
| Gap thresholds/OR/average/interest/formula/alternatives | Passed in the portable suite |
| Shared grade/profile/mark/question validation | Passed in the portable suite |
| Report sections, HTML encoding and stored-reason gap totals | Passed in the portable suite |
| Salted password hashing and guest entry restrictions | Passed in the portable suite; live account/login tests require LocalDB |
| SQL Server grammar parsing |30 actual scripts/complete query literals parsed;0 errors. Grammar only, not execution |
| Package vulnerability audit, including transitives | No vulnerable packages reported by the configured NuGet source |
| Source/provider inspection | Runtime SqliteStore/Microsoft.Data.Sqlite removed; only LocalDB store is wired into Program |
| In-app first-run setup and explicit SQL helpers | Compiled with embedded canonical scripts; two new Windows LocalDB tests cover missing database, repeat preservation and refusal of unrelated tables; Windows execution not performed |
| Live LocalDB setup/repeat/CRUD/reopen/constraints/lockout/backup/restore | **Not executed**; Windows required |
| Windows Forms tests: four existing cases plus three overview resize cases | Compiled; **not executed** on Linux |
| Manual layout 100/125/150/200% DPI, keyboard, print/PDF/offline/file errors | **Not executed**; Windows demonstration machine required |

The 48 skipped entries include conditionally skipped theories that will expand into their data cases on Windows. A skipped case is not a pass. Portable fixtures supply exact input data to real rules/validation/report composition; they are not a substitute SQL/SQLite store and do not prove persistence. The complete ported integration tests use uniquely named disposable LocalDB databases on Windows. They do not target/drop the application database.

## Reproduce

From the solution root on Linux:

```bash
dotnet restore FuturePath.Development.sln --locked-mode
dotnet build FuturePath.Development.sln -c Release --no-restore
dotnet test tests/FuturePath.Tests -c Release --no-build --logger trx --results-directory TestResults
```

On Windows with SQL Server Express LocalDB installed:

```powershell
dotnet restore FuturePath.Development.sln --locked-mode
dotnet build FuturePath.Development.sln -c Release --no-restore
dotnet test FuturePath.Development.sln -c Release --no-build --logger trx --results-directory TestResults
```

Then open FuturePath.sln/F5 or RUN-FUTUREPATH.cmd and complete TC01–TC37, particularly real setup/repeat, Back/Next retention, no premature writes, embedded summaries/editors, persistence across restart, deactivation, backup/restore, export/printing and DPI. Record actual tester/date/results and resolve runtime findings before presentation.

Current logs/TRX are under `validation/`. Complete source/hash list accompanies the final package. NuGet restore/signature/checksum/TLS verification was not disabled and warnings/tests were not suppressed to obtain a pass. The PKCS pin updates the SQL provider's already needed dependency; no unrelated runtime feature package was introduced.

## Visual Studio startup correction

`FuturePath.sln` now contains only Desktop, Business and Data. Desktop is the first and only runnable project and remains a Windows `WinExe`. The complete test solution is `FuturePath.Development.sln`. Program opens MainForm before any database connection; the window checks LocalDB asynchronously and offers explicit in-app setup/retry on failure. Setup uses embedded Database/01–03, refuses unversioned tables and skips all seed inserts on nonempty databases. No PowerShell or terminal is required for the Visual Studio/F5 workflow. The ZIP excludes Git metadata, GitHub workflows, local IDE state and build outputs.

Verified in this workspace: both solution builds (0 warnings/errors), locked restore, 36 portable tests passed, 48 LocalDB tests/theories skipped, 30 SQL grammar checks passed. Opening Visual Studio, F5, LocalDB and the first-run button have not been executed on Windows here; TC27, TC28, TC33, TC35 and TC36 remain required Windows acceptance checks.

## Display layout correction (1.0.1)

The user supplied screenshots from two Windows installations. One showed sidebar words split across extra lines and clipped navigation, while the other retained the intended proportions. The user confirmed 200% display scaling on the Windows 10 installation; the Windows 11 scale was not supplied. Source inspection found fixed pixel sidebar/card dimensions and AutoScaleMode.Dpi without declared AutoScaleDimensions.

AppForm/AppPage now declare 96-DPI dimensions and complete construction before layout resumes. Learner step hosts and other dynamic pages use the same DPI mode. Sidebar sizing uses current DeviceDpi, header/footer height follows content, and PageStack/ContentCard adapt to the viewport. The window is constrained to the current monitor working area. Package version is 1.0.1. No database schema, rules or records were changed for this correction.

The complete solution and the default solution from a fresh ZIP extraction build with 0 warnings/errors, including three new Windows overview resize cases at 960/1280/1600 logical pixels. Those cases require Windows, use the real controls at the host DPI and do not need LocalDB. They have not been executed in this Linux workspace. They are not a substitute for the actual 100/125/150/200% DPI and monitor-movement checks in TC37. The existing 36 portable passes/48 LocalDB skips remain the prior verified business/data result; that code was not changed for this display correction.
