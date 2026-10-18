# Future Path — SQL Server LocalDB edition

This is a controlled update of the supplied **FuturePath_VisualStudio_Project.zip**, following **FuturePath_AI_Implementation_Master_Prompt.pdf**. It preserves the .NET 10 Windows Forms application, its three projects, local advising rules, reference data, staff functions and reports. Persistence now uses **SQL Server LocalDB**, and learner pages, staff editors and reports stay inside one main window.

The app is Windows desktop software. It does not call an online AI/API, run a web server, or use SQLite. The input ZIP and earlier integration remain separate and unchanged.

## Start on Windows — three steps

1. **Install prerequisites:** Visual Studio 2026 with **.NET desktop development**, .NET 10 SDK, and **SQL Server Express LocalDB** (Visual Studio Installer → Individual components). Extract the entire ZIP to a writable folder.
2. **Prepare the demonstration database:** double-click **SETUP-DEMO.cmd**. This explicitly creates/prepares `(localdb)\MSSQLLocalDB` → **FuturePathStudentAdvisingSystem**, using your Windows identity. Existing records are preserved; a nonempty database is not reseeded. If PowerShell policy blocks the helper, use the four SQL scripts in SSMS/SQL Server Object Explorer as described in [Database-Setup](docs/Database-Setup.md). Do not change your machine's execution policy just to run this project.
3. **Run the app:** double-click **RUN-FUTUREPATH.cmd**, or open **FuturePath-App.sln**, build with **Ctrl+Shift+B**, and press **F5**. In the full `FuturePath.sln`, set **FuturePath.Desktop** as startup project. `FuturePath.Tests` is for tests; PASS output in a terminal is not the app.

The expected screen is the navy/white **Future Path** overview, with **New advisory session** and **Staff sign in**. The status bar identifies the LocalDB database in use.

These are editable source packages, not a claim of a Windows runtime validation. Current build/test evidence and remaining Windows checks are in [Validation](docs/Validation.md).

## What is preserved and improved

- Three tiers: `FuturePath.Desktop` (UI), `FuturePath.Business` (rules/services/models), `FuturePath.Data` (ADO.NET SQL Server persistence).
- Overview → profile/grade → subjects/marks → interests → faculty/degree and funding opt-in → outcome/alternatives → summary. Next validates the current page; Back retains its controls and input. Leaving unsaved work requires confirmation.
- No database writes on Next/Back. **Check eligibility & save** writes learner/session, marks, responses, category scores, selected result, ranked alternatives and the creation audit in one SQL transaction.
- Saved sessions are immutable. Staff follow-ups create a new session for the same learner. Finish returns to overview in the same window.
- Eight questionnaire themes, all five required named learner scenarios, the original additional learners, explicit OR requirements, configured average rules and current funding filters.
- Staff sign-in with database-loaded role, salted PBKDF2 hashes, five-attempt/five-minute lockout, administration, account maintenance, audit review and SQL Server backups.
- Three reports, shown inside the main window, with text/HTML export and Windows Print/Microsoft Print to PDF. Report HTML encodes user text.

## Database and safe setup

Default connection: `Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FuturePathStudentAdvisingSystem;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;`.

The certificate setting is for the local development SQL Server instance. No package/artifact verification is disabled. No SQL password is hardcoded or required. The schema keeps the reference entity/column names, with SQL Server types, identities, keys, checks and indexes; `SchemaVersion` is installation metadata.

Run scripts in order:

1. `Database/01_CreateDatabase.sql`
2. `Database/02_CreateTables.sql`
3. `Database/03_SeedData.sql` — explicit fictional demo data, for an empty database only.
4. `Database/04_VerifyDatabase.sql`

Startup **verifies** this installation and never creates, drops, resets or silently seeds a database. Script02 rejects unversioned/unrelated tables rather than modifying unknown data. Script03 skips a nonempty database in its entirety to avoid stable-ID collisions and accidental overwrites. Preserve backups before applying setup to an existing database.

The sample data contains10 learners,18 subjects,5 faculties,10 degrees,8 categories,8 questions,10 funding opportunities and10 computed advisory sessions. Requirement thresholds, additional learner marks, funding dates and names come from the supplied reference. All are sample information requiring verification; nothing guarantees admission or funding. Export metadata is created only when a real export succeeds; setup does not invent exported files.

Existing SQLite learner databases were not attached. Their files are not opened, deleted or imported by this app. The supplied schema, complete seed dataset and all store operations have been translated; LocalDB is the only application database. See [Changes-from-reference](docs/Changes-from-reference.md).

For a separate demo without deleting anything:

```powershell
powershell -NoProfile -File scripts/setup-database.ps1 -SeedDemo -DatabaseName FuturePathDemo2026
.\RUN-FUTUREPATH.cmd --database FuturePathDemo2026
```

`--database` now accepts a **LocalDB database name**, not a `.db` file path. The old `FuturePathAdvisingSystem` database used in earlier uploads is not reused or modified.

## Demonstration staff accounts

Only the explicitly requested demo seed installs these public, fictional local test accounts:

| Username | Password | Role |
|---|---|---|
| `advisor_test` | `FuturePathAdvisor2026!` | Career Advisor |
| `admin_test` | `FuturePathAdmin2026!` | System Administrator |

Only salted hashes are stored. There is no runtime hardcoded-login bypass or role selector. Change passwords before storing actual learner information. Learners keep the reference's private, anonymous-capable workspace and do not need accounts. Guest access is limited to sessions created in the current workspace; signing in/out or closing clears that access. Staff can retrieve saved sessions after authentication.

Advisor: search by learner/programme/session/date, review summaries, create follow-ups, and generate activity/exception reports. Administrator: those features plus reference/staff maintenance, audit and backup. Deactivation preserves history; removing an active requirement/category is controlled by the existing business rules. Staff editors now appear inside the main content area, with Save/Cancel.

## Rules you can explain

`35% subject coverage +35% mark match +25% interest alignment +5% preferred faculty alignment`.

Missing information/configuration takes priority. A confirmed missing mandatory subject or >5-point mandatory/average gap gives **Not Currently Eligible**. A gap up to5 points or weak interest gives **Possible Match**. All hard requirements and interest≥60% give **Strong Match**. Near matches do **not** meet entry criteria. OR choices are explicit stored groups. Alternatives use the same engine, exclude inactive/selected degrees and return at most five with stable ranking. Stored rule version/scores/explanations preserve history.

## Restore, build and test

From the solution root:

```powershell
dotnet restore FuturePath.sln --locked-mode
dotnet build FuturePath.sln -c Release --no-restore
# On Windows with LocalDB installed:
dotnet test FuturePath.sln -c Release --no-build --logger trx --results-directory TestResults
```

On Linux, run only the portable test project; LocalDB cases are explicitly skipped:

```bash
dotnet test tests/FuturePath.Tests -c Release --no-build --logger trx --results-directory TestResults
```

Portable tests use exact reference **input fixtures**, the real recommendation engine, shared validation and report composition. They do not substitute another database or claim SQL CRUD passed. Real LocalDB tests create uniquely named `FuturePathTest_<guid>` databases, apply and repeat setup, test persistence/constraints/roles/reports/backups and remove only those disposable databases. The four Windows UI tests exercise control construction, workflow saving, Back/Next retention and the one-window editor/report design.

.NET 10 needs a SQL Server provider: the only direct application NuGet reference is **Microsoft.Data.SqlClient**. Its existing PKCS dependency is pinned to a patched version through central dependency management. This is **not package-free**. Existing xUnit/Test SDK packages are test-only. See [Dependencies](docs/Dependencies.md).

## Project and presentation material

- [Beginner code walkthrough](docs/Code-Walkthrough.md)
- [LocalDB setup/troubleshooting](docs/Database-Setup.md)
- [Data dictionary](docs/Data-Dictionary.md)
- [System specification](docs/System-Specification.md)
- [Requirements traceability](docs/Requirements-Traceability.md)
- [Acceptance cases](docs/Test-Cases.md)
- [Presentation guide](docs/Presentation-Guide.md)
- [Migration/change record](docs/Changes-from-reference.md)
- [Build/test evidence and unverified checks](docs/Validation.md)

To regenerate the reviewed SQL demo outcomes from unchanged reference inputs, run `dotnet run --project tools/SeedGenerator` from the solution root. This maintenance tool calls the production engine and writes `Database/03_SeedData.sql`; it never connects to a database. Demo password salts change on regeneration. Do not edit outcome names by hand or run the seed tool against live data.

Complete the actual Windows/LocalDB, keyboard/DPI, backup/restore and printing acceptance cases before claiming demonstration readiness. Team details, contributions, reconciliation with any approved design and disclosure of assistance remain for your group to complete honestly.
