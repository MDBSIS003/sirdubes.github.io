# Verification of Future Path 2.3

18 October 2026. Toolchain: .NET SDK 10.0.402 on Linux x64.

| Check | Result | Evidence under validation/ |
|---|---|---|
| Portable authentication, required entries, controllers, reports and exports | 142 passed, zero failed | v2.3-role-login.trx, v2.3-portable-tests.txt |
| Windows LocalDB cases | 64 skipped on Linux; not executed | Same TRX/output |
| Windows Forms tests | Compiled with zero warnings/errors; not executed | v2.3-windows-tests-build.txt |
| Application source package | 127 files, including README, 109 C# files and five SQL scripts; source matches repository | v2.3-source-package.txt, Source-SHA256.txt |
| SQL grammar | Five scripts and 24 complete persistence/security statements parse | v2.3-source-package.txt, v2.3-sql-statements.txt |
| Fresh extracted application | Locked restore and Release build passed; zero warnings/errors | v2.3-extracted-restore.txt, v2.3-extracted-build.txt |

The sign-in screen requires a choice of Advisor or Administrator before username and password. The selected role is checked against the stored account role; it never assigns privileges. Authentication tests read the actual seed SQL and verify the exact requested credentials: adonias/NKSADO001 as Advisor and sidurbes/MDBSIS003 as Administrator. Tests cover both matches, role mismatches, invalid roles, wrong passwords, unknown accounts and attempts to use a stored Learner account for staff access.

The schema 4 upgrade configures the requested pair once. It updates existing requested accounts or inserts missing ones, and renames the previous SIRDUBES row to sidurbes when the new name is free, retaining the account ID. It sets the requested passwords and roles and clears their login locks. Learner records, assessments, other staff and history are retained. A transaction and schema-version range lock protect the one-time operation. Once version 4 is recorded, reopening or repeating setup preserves changes made in Security. Fresh database seeds and the upgrade contain the same salted PBKDF2 password hashes, independently checked against both requested passwords.

Windows LocalDB tests cover sign-in after reopening, upgrades from versions 1/2/3, original account ID preservation, unrelated staff/learner preservation, and Security changes surviving repeated initialization and setup. Windows Forms tests cover the mandatory unselected role dropdown, both selected-role login paths, workspace permissions, and the sign-in-only screen. These runtime cases are compiled but remain unrun on this Linux host.

The portable suite also covers required learner profiles, exact ten-digit contacts, seven distinct subjects, complete questionnaires, saving contracts, staff searches, reports, authorization and Security changes. Its recording store exercises controller behavior, not SQL Server execution. Programme rules, learner completion, transaction-based persistence, funding displays and summaries are retained.

On Windows, run `dotnet test tests/FuturePath.Tests -c Release` and `dotnet test tests/FuturePath.Desktop.Tests -c Release` from this repository's FuturePath-Course directory. Use both README logins with the matching role, try an incorrect role, change credentials through Security, complete and save a learner assessment, restart the app and retrieve it by its FP-reference. Check layouts at the target resolution and 200% display scale, and test print/export pagination. Build and portable passes do not establish those Windows runtime outcomes.

Earlier versioned evidence is retained for historical results and does not replace current verification.
