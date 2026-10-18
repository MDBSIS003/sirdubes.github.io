# Build and test evidence — LocalDB migration

Validation date: 18 October 2026. Host: Linux x64, .NET SDK 10.0.402. This is source/build/portable evidence, not an interactive Windows or live LocalDB readiness claim.

## Before migration

The supplied reference was inspected and tested before editing. Its original SQLite suite executed **58 passed, 0 failed, 0 skipped** against real temporary SQLite databases. That baseline is retained under `validation/reference/reference-baseline.trx`. The original supplied TRX/hash list are separately labelled historical evidence. These results do not validate the LocalDB implementation.

## Current LocalDB edition

| Check | Actual result |
|---|---|
| Locked restore of full FuturePath.sln | Passed |
| Full Release build, all five original projects | Passed; 0 warnings, 0 errors |
| App-only FuturePath-App.sln build | Passed |
| Portable test project on Linux | **36 passed, 0 failed, 46 skipped** LocalDB cases/theories |
| Five required named learner outcomes | Passed using the real engine and exact reference inputs |
| Gap thresholds/OR/average/interest/formula/alternatives | Passed in the portable suite |
| Shared grade/profile/mark/question validation | Passed in the portable suite |
| Report sections, HTML encoding and stored-reason gap totals | Passed in the portable suite |
| Salted password hashing and guest entry restrictions | Passed in the portable suite; live account/login tests require LocalDB |
| SQL Server grammar parsing |30 actual scripts/complete query literals parsed;0 errors. Grammar only, not execution |
| Package vulnerability audit, including transitives | No vulnerable packages reported by the configured NuGet source |
| Source/provider inspection | Runtime SqliteStore/Microsoft.Data.Sqlite removed; only LocalDB store is wired into Program |
| Explicit create/schema/seed/verify helpers | Source/scripts supplied; Windows execution not performed |
| Live LocalDB setup/repeat/CRUD/reopen/constraints/lockout/backup/restore | **Not executed**; Windows required |
| Four Windows Forms control/workflow/one-window tests | Compiled; **not executed** on Linux |
| Manual layout100/125/150% DPI, keyboard, print/PDF/offline/file errors | **Not executed**; Windows demonstration machine required |
| GitHub Actions Windows workflow | Updated source supplied; no Actions run claimed |

The 46 skipped entries include conditionally skipped theories that will expand into their data cases on Windows. A skipped case is not a pass. Portable fixtures supply exact input data to real rules/validation/report composition; they are not a substitute SQL/SQLite store and do not prove persistence. The complete ported integration tests use uniquely named disposable LocalDB databases on Windows. They do not target/drop the application database.

## Reproduce

From the solution root on Linux:

```bash
dotnet restore FuturePath.sln --locked-mode
dotnet build FuturePath.sln -c Release --no-restore
dotnet test tests/FuturePath.Tests -c Release --no-build --logger trx --results-directory TestResults
```

On Windows with SQL Server Express LocalDB installed:

```powershell
.\SETUP-DEMO.cmd
dotnet restore FuturePath.sln --locked-mode
dotnet build FuturePath.sln -c Release --no-restore
dotnet test FuturePath.sln -c Release --no-build --logger trx --results-directory TestResults
```

Then open FuturePath-App.sln/F5 or RUN-FUTUREPATH.cmd and complete TC01–TC34, particularly real setup/repeat, Back/Next retention, no premature writes, embedded summaries/editors, persistence across restart, deactivation, backup/restore, export/printing and DPI. Record actual tester/date/results and resolve runtime findings before presentation.

Current logs/TRX are under `validation/`. Complete source/hash list accompanies the final package. NuGet restore/signature/checksum/TLS verification was not disabled and warnings/tests were not suppressed to obtain a pass. The PKCS pin updates the SQL provider's already needed dependency; no unrelated runtime feature package was introduced.
