# Files changed and why

Baseline is the uploaded ZIP captured in the first local Git commit; integration work is on `integration/futurepath-complete`. No remote was provided in the ZIP and nothing was pushed. The separate website checkout was not changed.

| File/group | Reason |
|---|---|
| `FuturePathStudentAdvisingSystem.slnx` | Keep the original executable and add the console test project. |
| `FuturePathStudentAdvisingSystem/FuturePathStudentAdvisingSystem.csproj` | Enable Windows cross-target compilation; exclude obsolete Form1/2/3 and insert-only DAL from runtime; retain SQL client; pin patched PKCS dependency. |
| `DataAccess/DatabaseConnection.cs` | Preserve LocalDB default; optional test/development connection override. |
| `DataAccess/UserAccountDAL.cs` | Replace plaintext login/registration with salted hashes, add guarded profile update/read and own-password changes. |
| `DataAccess/DegreeProgrammeDAL.cs` | Add historical programme lookup so deactivation does not remove names/faculties from old exports. |
| `DataAccess/GradeLevelDAL.cs` | Restrict choices to Grade11/12 from the actual lookup data. |
| `DataAccess/SqlStore.cs` (new) | Internal parameter helpers, transactions' command creation, live role/ownership/assignment enforcement. |
| `DataAccess/AdvisingDAL.cs` (new) | Session/marks/questionnaire/results persistence, linked active funding, learner lookup, notes and export metadata. |
| `DataAccess/AdminDAL.cs` (new) | Allowlisted reference-data/account CRUD, assignments, deactivation and password reset. |
| `DataAccess/ReportDAL.cs` (new) | Scoped eligibility activity and missing-information/ineligibility exception queries. |
| `Models/AdvisingModels.cs` (new) | Profile, session, marks, questionnaire, requirements, mappings, funding and recommendation data objects. |
| `Models/ManagementModels.cs` (new) | Simple typed metadata for manageable records. |
| `Security/AppSession.cs` | Clear workflow state at login/logout; validate session values; internal authentication state establishment. |
| `Security/AccessPolicy.cs` (new) | Central page/role/ownership restrictions. |
| `Security/PasswordHasher.cs` (new) | Versioned PBKDF2-SHA256 hashing and verification. |
| `BusinessLogic/Validation.cs` (new) | Profile, subject mark and questionnaire validation separate from controls. |
| `BusinessLogic/ReferenceDataValidation.cs` (new) | Validate typed reference fields and numeric bounds outside GUI/SQL code. |
| `BusinessLogic/RecommendationService.cs` (new) | Deterministic gates, alternative choices, weighted interest scoring, rank and explanations. |
| `BusinessLogic/AdvisingWorkflow.cs` (new) | Coordinate saved learner and anonymous guest sessions and completion. |
| `BusinessLogic/ReportService.cs` (new) | Learner advisory summary and readable report output. |
| `Presentation/FuturePathScreens.cs` | Replace active prototypes with functional profile/marks/interests/recommendations/funding/advisor/admin/report pages; fix scope conflict and navigation lifecycle. |
| `Presentation/ManagementForm.cs` (new) | Typed CRUD editor, reference dropdowns, confirmations and secure password entry. |
| `Presentation/frmWelcome.cs` | Modal navigation loop retains login and current advising session. |
| `Presentation/frmLogin.cs`, `frmCreateAccount.cs` | Support validated 10–128-character passwords and hash-backed DAL. Branding retained. |
| Root `Database/01_CreateDatabase.sql`, `02_CreateTables.sql` | Replace destructive drop/recreate with create-if-missing tables/indexes. |
| `Database/03_InsertSampleData.sql` | Transactional empty-only seeding, hashes, fictional funding contacts/names, complete demo questionnaire answers and generated production-engine assessments. |
| `Database/04_SetupDemoStaffAccounts.sql` | Insert missing demo accounts with hashes; do not reset/promote existing accounts. |
| `Database/05_UpgradeIntegration.sql` (new) | Additive profile/assignment/completion/funding/notes schema, widen password/contact storage, disable unsafe legacy passwords. |
| `Database/06_NormaliseSampleRules.sql` (new) | Explicit groups for existing sample alternative rules; preserve existing history. |
| Project-local `Database/*.sql` | Exact compatibility mirrors of root canonical scripts; eliminate dangerous stale copies. |
| Both project `packages.lock.json` files (new) | Record resolved dependencies for locked restore. |
| `FuturePath.Tests/FuturePath.Tests.csproj`, `Program.cs` (new) | Portable real-source tests, T-SQL grammar checks, optional real LocalDB CRUD/security/integration suite. |
| `FuturePath.Tests/sample-fixtures.json` (new) | Noncredential sample input fixtures extracted from canonical SQL. |
| `tools/Setup-Database.ps1`, `Reset-LocalAccount.ps1` (new) | Safe ordered local setup and explicit existing-account recovery. |
| `tools/extract_sample_fixtures.py`, `update_sample_sql.py`, `sample-assessments.sql` (new) | Repeatable production-engine sample assessment generation and mirror synchronisation. |
| `README.md`, `docs/*.md` (new/updated) | Build/setup/security instructions, assumptions, architecture, test evidence, acceptance matrix and presentation preparation. |
| `docs/build-output.txt`, `portable-test-output.txt`, `locked-restore-output.txt` (new) | Actual validation output for this handoff. |

Unchanged starter assets include `Program.cs`, `DegreeProgramme`, the original active degree/subject DAL reads, reusable branded UI controls, and legacy screens/handoff modules. Legacy files are documented rather than blindly deleted or imported. Full line-level changes are available with `git diff master..integration/futurepath-complete` after importing the supplied Git bundle.
