# Verification of required-entry and staff fixes

18 October 2026. Toolchain: .NET SDK 10.0.402 on Linux x64.

| Check | Result | Evidence under validation/ |
|---|---|---|
| Freshly extracted application, locked restore and Release build | Passed; zero warnings or errors | fixes-extracted-restore.txt, fixes-extracted-build.txt |
| Portable validation, security, report, controller and export tests | 102 passed, zero failed | fixes-specification-fixes.trx, fixes-specification-test-output.txt |
| Windows LocalDB test cases | 55 skipped on Linux; not executed | Same TRX/output |
| Windows Forms test project | Compiled with zero warnings/errors; 11 cases not executed | fixes-windows-test-build.txt |
| Original engine regression | All 260 comparisons passed, including the ten exact original inputs | fixes-engine-regression.txt |
| Source package | 109 C# files; four SQL scripts parse; no code comments | fixes-package-source-checks.txt |
| Persistence SQL grammar | All 15 complete statements parse | fixes-persistence-sql-grammar.txt |

The specification acceptance tests exercise required profile fields, valid/invalid marks, subject count/duplicates, complete questionnaires, faculty-degree selection, staff credentials and roles, report details and file exports. The portable controller tests use a recording store at the existing interface: they demonstrate validation and data passed to storage, not SQL Server execution. The Windows tests cover actual database round trips, rollback, follow-ups, roles, funding filters and forms. They remain unrun on this host.

The exception report now includes faculty, stored interest alignment and alternative recommendations, grouped by outcome then learner name. New records require seven subjects and a complete questionnaire. Original historical scenarios remain in the database and in engine regression tests; their original academic data is not rewritten to fit the new entry rules.

On Windows, run `dotnet test tests/FuturePath.Tests -c Release` and `dotnet test tests/FuturePath.Desktop.Tests -c Release`. Verify all seven steps and the staff workspace at the target resolutions and display scales, including 200%. Confirm print pagination, backup/restore and database recovery using a separate test database. Compilation and portable passes do not establish those runtime results.
