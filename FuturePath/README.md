# Future Path Student Advising System

Integrated continuation of the supplied Windows Forms starter, on branch `integration/futurepath-complete`. The original namespaces, solution, .NET 10 Windows Forms startup project, SQL Server LocalDB database and blue/green branding are preserved. No web server, external AI, SQLite or online runtime service is used.

**Validation boundary:** the solution has been cross-compiled and business tests executed on Linux. Windows Forms interaction, PowerShell helpers and SQL Server LocalDB integration require the Windows verification below. A successful build is not a claim that the GUI or live database tests passed.

## Start the app (not the tests)

Extract the ZIP completely using **Extract All**. On Windows, double-click **RUN-FUTUREPATH.cmd** in the extracted folder. It builds only the Windows Forms project and launches its executable. It does not run the test runner. The .NET 10 SDK and database setup below are still required.

For Visual Studio, open **FuturePath-App.slnx** for an app-only solution, then press **F5**. The original `FuturePathStudentAdvisingSystem.slnx` includes both the app and automated tests. If that solution prints `PASS` in a terminal, right-click **FuturePathStudentAdvisingSystem** in Solution Explorer and choose **Set as Startup Project**. Do not select **FuturePath.Tests** to launch the GUI.

`PASS` output means the automated tests ran; it is not the application screen. The expected application screen is the branded **Future Path — Welcome** window with Student, Guest, Career Advisor and Administrator cards.

## Open and build on Windows

1. Install a Visual Studio version supporting .NET 10 and `.slnx` (Visual Studio 2026 recommended), with **.NET desktop development** and **SQL Server Express LocalDB** components. Alternatively use the .NET 10 SDK CLI and SSMS for the database.
2. Extract the solution to a writable local directory. Open `FuturePathStudentAdvisingSystem.slnx`.
3. Set `FuturePathStudentAdvisingSystem` as the startup project, not `FuturePath.Tests`.
4. Restore NuGet packages and build. From the solution root:

   ```powershell
   dotnet restore FuturePathStudentAdvisingSystem.slnx --locked-mode
   dotnet build FuturePathStudentAdvisingSystem.slnx
   ```

The tests are a deliberately small console test runner with nonzero failure exit codes. They appear as a separate solution project and run with `dotnet run`, not Test Explorer or `dotnet test`.

## Prepare LocalDB safely

Back up an existing `FuturePathAdvisingSystem` database before upgrading. Do not execute SQL scripts from the original ZIP: its original create scripts dropped data. The updated scripts here do not drop or recreate existing application databases or tables.

Start the installed instance if necessary:

```powershell
sqllocaldb start MSSQLLocalDB
```

With `sqlcmd` installed, run from the solution root:

```powershell
.\tools\Setup-Database.ps1 -SeedSampleData -DemoStaff
```

Alternatively connect SSMS to `(localdb)\MSSQLLocalDB` using Windows authentication and execute the **root `Database`** scripts in this order:

1. `01_CreateDatabase.sql` — create only if missing.
2. `02_CreateTables.sql` — create only missing tables and indexes.
3. `05_UpgradeIntegration.sql` — add integration columns, widen password/phone storage, disable legacy plaintext-password accounts.
4. `03_InsertSampleData.sql` — optional, for an **empty** development database only. On a nonempty database it skips the entire dataset, avoiding ID collisions and overwriting records.
5. `04_SetupDemoStaffAccounts.sql` — optional, inserts missing local demo accounts; never resets or promotes an existing account.
6. `06_NormaliseSampleRules.sql` — assign explicit alternative groups to the starter's sample alternative rules; preserve existing historic results.

New empty-database seeds include 10 learners, 18 subjects, 5 faculties, 10 degrees, 10 questions, 8 interest categories, 10 funding records, 10 completed advising sessions, 100 computed results, and sample export metadata. Three funding records are inactive. Missing-information learner 9 intentionally has incomplete subjects and no responses. The starter had added questions 9–10 without sample answers; demo answers for these copy the same-category answers to questions 1–2. This is a documented fictional test-data choice, not a lecturer requirement.

The seed includes all five required named learners and generates their recommendation rows with the production algorithm. Original hardcoded scores were replaced. New seed data is explicitly fictional; no funding email address or link is presented as a real application destination. Existing user funding records are preserved during upgrades and must be reviewed by the team for accuracy.

The project-local `Database` folder mirrors root `Database` for compatibility; root is canonical. Run scripts in the order above in either location, never both as separate setup procedures.

Default connection: Windows integrated authentication to `(localdb)\MSSQLLocalDB`, database `FuturePathAdvisingSystem`. LocalDB's development-only self-signed certificate uses the starter's `TrustServerCertificate=True`. No download/package verification is disabled. Optional `FUTUREPATH_CONNECTION_STRING` overrides the connection for local test/development SQL Server instances; do not commit credentials. No environment variable is required for normal use.

If an existing database is nonempty, seed setup does not inject a second sample dataset. Add missing reference data through the administrator screens. The upgrade supports the supplied starter schema; a separately modified teammate database requires schema review before applying the scripts.

## Local demonstration accounts

**Local development only; never use these credentials for real accounts.**

| Role | Username | Password |
|---|---|---|
| Career Advisor | `advisor_test` | `DemoAdvisor2026!` |
| Administrator | `admin_test` | `DemoAdmin2026!` |

These passwords are documented for demonstration, but only salted PBKDF2 hashes are stored in SQL. The application does not contain automatic staff-account provisioning or hardcoded login bypasses. Change passwords through the sidebar after initial testing.

Existing usernames are not overwritten by the staff script. If your old `admin_test` or `advisor_test` was disabled because it held a plaintext password, use the explicit local DBA recovery helper (Windows PowerShell with .NET Framework 4.8), entering a new password securely:

```powershell
.\tools\Reset-LocalAccount.ps1 -Username admin_test -Enable
```

The helper resets only that existing account, retains its role and records, and does not promote it to administrator. An absent administrator can be inserted by script 04; a username already belonging to another role is never silently promoted. Resolve such a collision explicitly as the database owner. Password resets through the application require an authenticated administrator.

Sample learner hashes deliberately have no shared documented password. Create a learner through **Student → Create account** (10–128 characters), or reset a sample learner through administrator management. Usernames are unique; existing sample usernames need not be emails. New learner registration requires an email.

## Run the learner workflow

```powershell
dotnet run --project FuturePathStudentAdvisingSystem
```

1. Student logs in, or Guest starts an anonymous memory-only session.
2. Learner saves personal/school details, Grade 11 or 12 and optional contact number. Account email is read-only. Profile updates do not create accounts.
3. Select a faculty and active degree, then **Start new advising session**.
4. Add subjects, update/remove results, confirm the complete subject list, and **Save results**.
5. Answer **every** database questionnaire question and **Save responses**.
6. **Calculate & complete session** on Recommendations. Funding information is optional. View full explanations and export the advisory summary as a UTF-8 text file.
7. Completed sessions preserve their marks/responses/results. Load them through the dashboard. Start a new session to change a completed assessment. Unsaved screen edits must be saved before navigating.
8. Logout returns to Welcome and clears the working session. Closing a page also returns to Welcome. Navigation between pages preserves login and working data.

Guests can explore the same advising rules and public funding/reference data without persisting personal data, sessions or exports in the database. They cannot see learner histories or staff screens.

## Advisors and administrators

Advisors see **only assigned learners**. Log in as administrator, open **Manage UserAccount**, select a learner, set `AdvisorUserID` using the dropdown and save. No learners are assigned by default. This least-privilege assignment rule is an implementation assumption, not an explicit case-study rule. It prevents an advisor from browsing all learner records.

Advisor: search assigned learners, load sessions, inspect marks/results, add notes, view notes, export learner summaries and generate the two staff reports. Administrators can access cross-learner reports but do not impersonate the advisor role to add notes.

Administrator: CRUD faculties, subjects, degrees, admission requirements, interest categories, questions, degree-interest mappings, funding options/programme links, and accounts/roles. Foreign-key dropdowns replace raw ID entry. Set an `AlternativeGroup` on all accepted OR choices; leave ordinary optional recommendations ungrouped. New degrees need admission rules and at least one positive interest mapping before meaningful assessment.

Delete/deactivate actions require confirmation. Degrees, funding and accounts are deactivated to preserve history; referenced lookup data cannot be deleted. Current administrators cannot disable/demote themselves. Admins can reset passwords. Historical learner accounts cannot be reassigned to staff roles. Account edits do not expose password hashes.

## Automated validation

Run from the **solution root**:

```powershell
dotnet run --project FuturePath.Tests
```

This executes portable tests, including the exact five required learner scenarios and SQL Server grammar checks. It explicitly reports the LocalDB integration group as skipped.

On Windows with LocalDB installed, run:

```powershell
dotnet run --project FuturePath.Tests -- --database
```

This additionally creates a uniquely named `FuturePath_IntegrationTests_<guid>` database, applies and repeats setup, tests authentication, profile persistence, mark CRUD, questionnaire persistence, recommendation persistence, funding, admin CRUD, advisor assignment/notes, ownership and reports, then removes **only that temporary test database** in `finally`. It never targets or drops `FuturePathAdvisingSystem`. Setup/cleanup failures return a nonzero exit code. The database suite has not been executed in this Linux environment.

Follow [Windows acceptance cases](docs/Windows-Acceptance.md) before presentation. See [validation evidence and phase reports](docs/Phase-Reports.md), [architecture and recommendation method](docs/Architecture.md), [feature checklist](docs/Feature-Checklist.md), [changed files](docs/Changed-Files.md), and [presentation notes](docs/Presentation-Notes.md).

## Maintaining sample assessments

Only when intentionally editing demo data/rules, from the solution root:

```powershell
python tools/extract_sample_fixtures.py
dotnet run --project FuturePath.Tests -- --emit-sample-sql
python tools/update_sample_sql.py
```

Commit the reviewed SQL, JSON fixture and generated SQL together. Generation uses the same production `RecommendationService`. This changes seed files only; it never updates an existing database. The fixtures contain no account passwords or contact details. Team-approved design artifacts and any additional lecturer data not present in the attachments remain the team's responsibility.
