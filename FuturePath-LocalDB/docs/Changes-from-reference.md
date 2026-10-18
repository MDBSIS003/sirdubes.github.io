# Changes from the supplied Visual Studio reference

The input ZIP is unchanged. A separate working copy/branch preserves the baseline. The smaller SQL-only ZIP and earlier integration remain supporting/history material; this implementation continues the newly supplied FuturePath.sln project.

## Preserved

- .NET 10 Windows Forms, FuturePath.sln, FuturePath.Desktop/Business/Data namespaces and project boundaries.
- Existing MainForm shell, control names, navy/white/teal style, anonymous/alias learner behavior and staff roles.
- All20 domain entities/relationships;10 required/additional learners and their exact marks;18 subjects;5 faculties;10 degrees;8 categories/questions;10 funding records and10 computed sessions.
-35/35/25/5 formula, explicit OR subjects, average rules, five-point near-match threshold,60% strong-interest threshold, up to five alternatives and required outcomes.
- Staff hashing/lockout/account maintenance, reference controls/activation rules, audit, immutable history/follow-ups, funding, three reports, text/HTML export and standard Windows printing.

## Changed and why

| Files/area | Change and reason |
|---|---|
| `src/FuturePath.Data/DatabaseConnection.cs` | One named LocalDB database, Windows authentication, validated database-name override |
| `SqliteStore.cs` → `SqlServerStore.cs` | Ordinary Microsoft.Data.SqlClient ADO.NET; OUTPUT identity retrieval, typed dates/decimals, T-SQL filtering, copy-only/checksummed SQL Server backup |
| Data project and package lockfiles | Remove SQLite provider/native runtime; one direct SQL client package; patched existing PKCS dependency pin; retain test packages |
| Root `Database/01–04` | Explicit create/schema/seed/verify; no destructive startup action; refuse unknown schema; skip nonempty demo seed |
| Old Data `Schema.sql`/`Seed.sql` | Replaced by canonical LocalDB scripts; original definitions remain in baseline history, not as a competing runtime database |
| `validation/reference/sample-data.json`, seed generator | Exact noncredential input fixture retained from reference; production engine generates persisted SQL demo results; no outcome-name hardcoding |
| `Program.cs` | Select database name instead of file path; keep mutex; open MainForm before background verification and offer explicit in-app demo setup |
| `AdvisingPage.cs` | Replace freely selectable tabs with retained in-window panels and validated Next/Back; clear progress and Finish |
| `AdvisingValidation.cs`, `AdvisingService.cs` | Shared page/service validation and conventional constructors; assessment audit joins the same save transaction |
| `RecommendationEngine.cs` | Named weights/thresholds, readable mandatory/OR grouping, deterministic tie-breaking; same policy/results |
| `ReportViewer.cs`, `MainForm.cs` | Embed report preview, retain the originating page for Back, keep shell/status and dispose controls correctly |
| `StaffPages.cs` | Reference/account editors now embedded controls; Save/Cancel; deactivation confirmation; `.bak` backup selection |
| `Reports.cs` | Preserve reports and safe exports; add counts of sessions with stored subject/mark/questionnaire gaps |
| `ReferenceCatalog.cs` | Enforce SQL-compatible question text limit and integer lookup values; retain allowlist/validation |
| `Ui.cs` | SQL Server error guidance instead of file-database errors; retain standard visual helpers |
| `tests/Shared` and original test files | Port actual database fixtures/constraints/reopen/backup/restore to LocalDB; protect cleanup with generated test names; skip explicitly on Linux |
| `PortableRuleTests.cs` | Real rules/shared validation/report tests against exact fixtures; no replacement database and no SQL persistence claim |
| Desktop tests | Retain constructor/save tests and add Back/Next/no-early-write/one-window checks |
| `FuturePath.sln`, `FuturePath.Development.sln` | Default solution contains only Desktop/Business/Data; complete test solution is separate |
| `DatabaseSetup.cs`, Data embedded resources, MainForm startup | Open the GUI first; prepare demo data explicitly inside the same window using the canonical SQL scripts |
| `SETUP-DEMO.cmd`, setup helper | Explicit beginner setup without assuming sqlcmd is installed; manual SQL route for blocked PowerShell policy |
| Docs/diagrams/Word, CI/packaging/cloud scripts | Remove stale SQLite/run-path instructions, document provider trade-off and actual validation limits |

No existing SQLite learner database was attached, so there are no actual learner files to import. Existing `.db` files are preserved untouched and are not a fallback provider. No automatic import tool or dual-source synchronization is claimed. The translated seed contains every reference learner/mark/answer and reference-data row.

Setup no longer fabricates a first-run sample export. ExportReport rows represent files actually saved by a user/test. Stored sample assessments are precomputed by the production engine during seed generation; runtime never rewrites old assessments to complete a seed.

Remaining before demonstration: real Windows LocalDB setup/repeat/save/reopen/constraint/lockout/backup tests, the four Windows UI tests, manual DPI/keyboard/print/offline/error cases and team review against the approved specification. Source, grammar validation and portable tests cannot establish those Windows outcomes.

Display correction 1.0.1 adds LayoutControls.cs (AppPage/PageStack/ContentCard), declares matching DPI baselines in AppForm, adapts MainForm navigation/header/window bounds and completes layout in each learner/staff/report page. Three Windows-only overview resize cases were added. Runtime DPI checks remain pending; no database schema/data/rule change is part of this correction.
