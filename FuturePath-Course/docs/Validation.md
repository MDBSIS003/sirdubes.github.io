# Verification of Future Path 2.1

18 October 2026. Toolchain: .NET SDK 10.0.402 on Linux x64.

| Check | Result | Evidence under validation/ |
|---|---|---|
| Freshly extracted 2.1 application, locked restore and Release build | Passed; zero warnings or errors | v2.1-extracted-restore.txt, v2.1-extracted-build.txt |
| Portable validation, authentication, reports, controllers and export tests | 143 passed, zero failed | v2.1-security-summary.trx, v2.1-portable-tests.txt |
| Windows LocalDB cases | 64 skipped on Linux; not executed | Same TRX/output |
| Windows Forms test project | Compiled with zero warnings/errors; not executed | v2.1-windows-build.txt |
| Application-only source package | 128 files, including 111 C# files and five SQL scripts; code/SQL tokens preserved and scripts parse | v2.1-source-package.txt, Source-SHA256.txt |
| Account, persistence and schema SQL grammar | All 27 complete statements parse; not executed against SQL Server | v2.1-sql-statements.txt |
| Original engine regression, previous verification | 260 comparisons passed, including ten exact original inputs | fixes-engine-regression.txt |

The portable tests use a recording implementation of the store interface. They cover exact ten-digit contact validation, required learner profile fields, at least seven different subjects, completed questionnaires, authorization, saved report contents, username/password updates, tester registration expiry/cancellation, duplicate account rejection and storage failures. Registration tests confirm no account or sign-in before confirmation, hashed passwords, sign-in only after a successful save, and later sign-in using the username or registered email. These establish controller behavior and storage contracts, not SQL Server execution.

The LocalDB tests cover actual account persistence after reconnect, email login, duplicate-email rollback, security changes, audit rollback, additive schema upgrades, completed learner persistence and staff searches. The Windows Forms tests cover Designer construction, navigation, display-only wrapping sections, completed summaries, account editors and the offline tester form. Both sets require Windows and remain unrun on this host.

Tester registration is an explicitly simulated workflow. It sends no email and does not verify ownership of an email address. The requested tester button creates a new account; it cannot sign into an existing account or reset its password. Administrator and Advisor roles are available so a marker can exercise the staff features.

Existing version 1 databases receive the additive TesterRegistration table and a version 2 entry. Existing learner records and staff password hashes are retained. New account, email link and audit entry use one transaction. Completed learner data continues to use the existing assessment transaction. Step 7 retries report loading without creating another saved session if loading fails after a successful commit.

The learner summary omits internal scoring-version codes and raw question answers. Staff reports retain those details. Funding information uses a wrapping display label without an inner text scrollbar. A page can still scroll when its content exceeds the available window height; all content cannot be physically shown at every resolution and display scale.

On Windows, run `dotnet test tests/FuturePath.Tests -c Release` and `dotnet test tests/FuturePath.Desktop.Tests -c Release` from this repository's FuturePath-Course directory. Check all seven learner steps and the staff workspace at the target resolutions and display scales, including 200%. Test creating an account, closing and reopening the application, signing in by username and email, changing credentials in Security, and retrieving a completed learner by its FP-reference. Confirm print pagination, backup/restore and recovery using a separate test database. Compilation and portable passes do not establish those runtime results.
