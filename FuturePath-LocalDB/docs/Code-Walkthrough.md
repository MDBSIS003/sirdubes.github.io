# Beginner code walkthrough

## 1. Start the real app

Open **FuturePath-App.sln**, build and press F5. In the full solution the startup project is **FuturePath.Desktop**, not a test project. `RUN-FUTUREPATH.cmd` builds only Desktop and starts its Windows executable.

`src/FuturePath.Desktop/Program.cs` configures Windows Forms, chooses the LocalDB database name (default `FuturePathStudentAdvisingSystem`), prevents a second app instance for that name, verifies setup with `SqlServerStore.Initialize()`, and opens **one MainForm**. It does not create/seed/delete databases at startup. `AppServices` constructs the access session and business services. This is direct construction, not a dependency-injection framework.

## 2. One window, different contents

`MainForm` owns the fixed sidebar/header/status and central `content` panel. `ShowPage()` changes the UserControl in that panel. Home is step1 of7. `NewAdvisory()` creates `AdvisingPage` for steps2–6. Staff workspaces and editors are controls in this same shell. Only small credential/native message/file/print dialogs open separately.

`AdvisingPage` builds each step's controls once and keeps them in `steps`. `ShowStep()` removes the previous panel without disposing it, then displays the next panel. That is why Back retains values. `GoNext()` calls shared validation before changing the step. `GoBack()` never writes data. `Dirty()`/`HasUnsavedInput` allow MainForm to warn before leaving an unfinished draft. Finish goes back to Home.

`MainForm.ViewReport()` hosts `ReportViewer` (a UserControl) as step7, retaining the originating page so Back preserves its inputs or staff filters. `ReportViewer` handles SaveFileDialog and standard Windows printing. It does not contain SQL or calculate eligibility.

## 3. Validate and save an assessment

`AdvisingPage.ReadProfile()` and `ReadResponses()` translate controls to model values. `SubmitAssessment()` creates `AdvisoryInput` and calls `AdvisingService.Assess()`.

`AdvisingValidation` checks grade/optional contact details,0–100 marks, duplicate subjects, active subject IDs, answer range/duplicates and required answers for Next. Business service validation repeats these checks independently of the UI. Imported/incomplete inputs can still be assessed as More Information Needed; the ordinary questionnaire Next action requires answers.

`AdvisingService` retrieves current rules/questions through `IFuturePathStore`, calls the pure `RecommendationEngine`, and passes the finished session to `SqlServerStore.SaveSession()`. That method opens a transaction, inserts the learner if needed, inserts the session, all marks/responses/category scores, selected/alternative results and its creation audit. SQL Server returns identity IDs using `OUTPUT INSERTED.Id`. Commit happens only after every write succeeds; disposal rolls back a failed transaction. No record is saved for each Next click.

After success, `AccessSession.OwnSession()` permits that guest workspace to load/export the new session. The UI renders the persisted result and becomes read-only. A staff follow-up loads previous inputs and saves a **new** session for the existing learner; it does not overwrite the old result or replace learner identity.

## 4. Explain the recommendation engine

`RecommendationEngine` has no form controls, database connections or online calls. Named constants define the 35/35/25/5 weights, five-point near-match limit and60% strong-interest threshold.

`RequiredGroups()` separates independent mandatory requirements from explicitly stored OR groups. `Evaluate()` compares subject presence and marks, checks any configured average, converts1–5 questionnaire answers to0–100 category scores and calculates interest alignment from degree mappings. It collects specific reasons and applies outcome precedence:

1. Incomplete capture/questionnaire/configuration → More Information Needed.
2. Confirmed absent required subject or >5-point mandatory/average gap → Not Currently Eligible.
3. A positive gap up to5 points or weak interest → Possible Match.
4. All hard requirements met plus interest≥60 → Strong Match.

A high score never overrides a hard gap. A Possible Match caused by a mark shortfall is not academically eligible. `Assess()` evaluates alternatives with the same rules, excludes selected/inactive degrees, keeps strong/possible results, sorts stably and takes at most five. Rule version, component scores and explanation are saved with the session.

Walk through Labeeqah: Mathematics58 versus stored60 produces a2-point gap, so Possible Match rather than inventing acceptance. Aadiel-Raboo's Mathematical Literacy cannot replace compulsory Mathematics in Computer Science because no such OR group is configured.

## 5. Follow the data access

`DatabaseConnection` constructs one Windows-integrated LocalDB connection. `SqlServerStore` implements the existing domain-specific `IFuturePathStore` contract. Methods have a clear purpose:

| Method | Reads/writes |
|---|---|
| `GetLookup("Subject")` | Active school subjects |
| `GetProgrammes(true)` | Active faculties/degrees plus requirements and interest weights |
| `GetQuestions()` | Active questions and categories |
| `SaveSession()` | Complete assessment in one transaction |
| `GetSession()` | Stored inputs/results/rule version/category scores; never recalculates history |
| `GetFunding(degreeId,today)` | Active/open programme-specific or applicable faculty-wide funding |
| `FindSessions(from,to,search)` | Saved-session projection used by staff/report services |
| `SaveReference()` | Validated allowlisted entity/field insert/update |
| `RecordExport()` | File metadata after a real export |
| `Backup()` | New copy-only SQL Server `.bak`, checksummed and verified |

Connections/readers/commands are disposed with using. User values use parameters. SQL identifiers come from the existing fixed `ReferenceCatalog`, never user-entered table names. SQL keys/checks/unique constraints independently protect data. The schema has the same20 domain entities; SchemaVersion is installation metadata.

## 6. Staff security and maintenance

`AuthenticationService.Login()` retrieves the account and its actual role, checks active state/lockout, verifies salted PBKDF2, records success/failure and then sets identity internally. There is no role selector or hardcoded login bypass. Five failures lock for five minutes; expiry/success/reset clears the counter.

`AccessSession.RequireStaff()`, `RequireAdmin()` and `RequireSession()` protect business operations independently of hidden navigation. Guests can access only their current workspace's created sessions. Staff can review saved sessions as in the reference specification. Signing in/out clears guest ownership and screen information.

`MaintenanceService` validates typed values, links and activation rules. `MaintenancePage` lists reference records; `ReferenceEditor`/`AccountEditor` are now embedded controls with Save/Cancel. Programme/funding deactivation retains history. Account lists omit password hashes. The current administrator cannot disable/demote itself. Audit messages never contain passwords.

## 7. Reports, export and print

`ReportService.Summary()` authorizes and reads the saved assessment, then `ReportComposer.Summary()` builds readable text. `Activity()` authorizes staff, applies date/search filters and groups saved sessions by faculty/degree/outcome. Exception reports include Not Currently Eligible and More Information Needed. Gap totals count sessions containing the stored subject/mark/questionnaire reasons, not today's rules.

`ReportDocument.ToHtml()` HTML-encodes user text. `Export()` writes to a temporary file on the same destination volume and then moves it into place, so a failed write does not truncate an existing report. Only after creation does it record export metadata. `ReportViewer` previews the document inside MainForm and uses PrintDocument/PrintDialog for printing or Microsoft Print to PDF.

## OOP tied to actual code

| Concept | Concrete example |
|---|---|
| Encapsulation | AccessSession's private identity setters and ownership set; MainForm owns navigation state |
| Abstraction | IFuturePathStore exposes advising operations without exposing SQL to controls |
| Polymorphism | SqlServerStore implements IFuturePathStore; SystemClock/TestClock implement IClock |
| Composition | AppServices constructs services; session contains input/results; MainForm contains page controls |
| Inheritance | MainForm/LoginDialog extend AppForm; page/editor/report controls extend UserControl; validation exceptions extend Exception |
| Separation of concerns | UI gathers/displays; Business validates/calculates/authorizes; Data executes SQL |

The portable checks execute real rules/validation/report composition against reference input fixtures. They are **not** SQL integration tests. Run the complete suite on Windows with LocalDB and collect manual layout/printing evidence before presenting. Do not claim an unrun test or an invented group contribution.
