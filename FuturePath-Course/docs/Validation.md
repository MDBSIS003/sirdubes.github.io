# Verification of Future Path 2.2

18 October 2026. Toolchain: .NET SDK 10.0.402 on Linux x64.

| Check | Result | Evidence under validation/ |
|---|---|---|
| Portable validation, authentication, reports, controllers and exports | 133 passed, zero failed | v2.2-signin-only.trx, v2.2-portable-tests.txt |
| Windows LocalDB cases | 63 skipped on Linux; not executed | Same TRX/output |
| Windows Forms test project | Compiled with zero warnings/errors; not executed | v2.2-windows-build.txt |
| Application ZIP | 127 files, including README, 109 C# files and five SQL scripts; source matches current application | v2.2-source-package.txt, Source-SHA256.txt |
| SQL grammar | All five scripts and 24 complete persistence/security statements parse; not executed against SQL Server | v2.2-source-package.txt, v2.2-sql-statements.txt |
| Freshly extracted 2.2 application | Locked restore and Release build passed; zero warnings/errors | v2.2-extracted-restore.txt, v2.2-extracted-build.txt |
| Original engine regression, earlier verification | 260 comparisons passed, including ten exact original inputs | fixes-engine-regression.txt |

Staff sign-in accepts an existing username and password. Public tester registration, email-code controls and their controller/storage methods have been removed. The shipped README identifies Adonias (Administrator) and SIRDUBES (Advisor). Portable tests read the shipped seed SQL and authenticate both exact requested credentials against the production password verifier; hashes in the seed and version 3 upgrade were also checked for equality and password validity. Passwords are stored as salted PBKDF2 hashes.

The version 3 upgrade adds either named staff account only when its username is missing. Existing learner data, assessments, account credentials and roles are retained. Version 1 and 2 databases upgrade automatically; version 3 does not run an account upgrade on each startup. Existing obsolete tester-email tables, if present in a previously used database, are left untouched and are no longer used by the program.

Staff Security still requires the current password to change username or password. Administrative account maintenance retains its authorization checks. Staff search, detailed reports, follow-up assessment handling, required learner fields, ten-digit contact validation, seven distinct subjects, complete questionnaires, transaction-based saving and wrapping funding/summary sections remain in place.

Portable tests use a recording implementation of the store interface. They establish validation, authorization, authentication, report behavior and storage contracts, not execution against SQL Server. Windows tests include sign-in after reopening the database, upgrades from versions 1 and 2, preservation of existing passwords and learner records, security changes, rollback, staff searches and reference maintenance. Windows Forms tests confirm the login form has only its Sign in button. These runtime cases remain unrun on Linux.

On Windows, run `dotnet test tests/FuturePath.Tests -c Release` and `dotnet test tests/FuturePath.Desktop.Tests -c Release` from this repository's FuturePath-Course directory. Use both README accounts, test the role restrictions and Security changes, finish a learner assessment, restart the program and search by its FP-reference. Check layouts at the target resolutions and display scales, including 200%, and print/export pagination. Compilation and portable test passes do not establish those Windows runtime results.

Earlier v2.1 evidence is retained as historical evidence for that version; its removed registration scenarios do not describe the current application.
