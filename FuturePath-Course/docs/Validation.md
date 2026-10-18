# Verification — course edition 2.0

Executed in Linux x64 on 18 October 2026 with .NET SDK 10.0.402. This host cannot run Windows Forms or SQL Server LocalDB. All application and both test projects target/build as declared; no replacement database was used.

| Check | Actual result | Evidence |
|---|---|---|
| Locked restore of FuturePath.Development.sln | Passed | validation/locked-restore.txt |
| Release build of all five projects, including Windows UI tests | Passed; 0 warnings, 0 errors | validation/build.txt |
| Portable rules/validation/report/security tests | **52 passed, 0 failed** | validation/course-portable.trx and portable-tests.txt |
| Windows LocalDB tests in that run | **53 skipped** because this host is Linux | Same TRX; skip reason identifies Windows LocalDB |
| Seven Windows Forms test cases | Compiled; **not executed** | Desktop test assembly in full build; tests/FuturePath.Desktop.Tests/DesktopTests.cs |
| Original engine comparison | **260 comparisons passed**: ten exact supplied scenarios and 250 deterministic variations | validation/engine-comparison.txt |
| T-SQL grammar | **30 scripts/complete query literals parsed; 0 errors** | validation/sql-grammar.txt; parsing is not SQL execution |
| Runtime dependency vulnerability query | No known vulnerable packages reported by current configured source | validation/dependency-audit.txt; point-in-time result |

The comparison checked every outcome, component/total score, explanation, ranked alternative and category score against the unchanged supplied engine assembly. Expected results for the ten original scenarios are stored in validation/reference/expected-engine-results.json and checked independently by the regression tests. The reference input JSON is test data, not an application database. Database/03_SeedData.sql is byte-for-byte unchanged from the input ZIP.

The package was re-extracted and checked before publication. Both the application solution and full development solution build with zero warnings/errors. The extracted portable test run also passed 52 and skipped 53 Windows LocalDB cases. Final compiled source/configuration equality was checked before packaging; see validation/extracted-package-build.txt, extracted-app-build.txt, extracted-package-tests.txt and package-structure.txt. Source hashes are in validation/Source-SHA256.txt and the download has a separate ZIP checksum. Older TRX files under validation/reference are explicitly historical, supplied/pre-migration evidence; they are not this edition's test result.

## Windows acceptance still required

Run the LocalDB and Desktop test projects from FuturePath.Development.sln on Windows with LocalDB installed. Then complete the 50-case workbook and Windows 10/11 × 100/125/150/175/200% display matrix. Check Designer opening, first run/repeat/refusal, all seven steps and Back retention, actual persistence and transaction failures, three reports/filters, roles/lockout/maintenance, backups, export failures, actual pagination/Print to PDF, keyboard use, offline use and duplicate-instance behavior.

Compiling UI tests does not prove the Designer/runtime layout on Windows. Passing portable checks does not prove LocalDB SQL execution. Unrun checks are not marked Pass. Any defects found on Windows need correction and a rerun before presentation. No mark, admission, funding or plagiarism percentage is guaranteed.

## Reproduce the checks

From the project folder, use the .NET 10 SDK:

```text
dotnet restore FuturePath.Development.sln --locked-mode
dotnet build FuturePath.Development.sln -c Release --no-restore
dotnet test tests/FuturePath.Tests -c Release --no-build
```

On Windows also run `dotnet test tests/FuturePath.Desktop.Tests -c Release --no-build`. For the prepared Linux cloud workflow, `bash scripts/setup-cloud.sh` creates its local runtime/cache directories and performs restore/build/portable tests. Normal application use is FuturePath.sln → F5.
