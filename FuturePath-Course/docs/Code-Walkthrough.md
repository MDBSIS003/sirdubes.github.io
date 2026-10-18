# Code walkthrough — start with these files

## 1. A button, an object and a list (Practical 1)

Open `Presentation/frmSubjectMarks.cs`, then `btnAdd_Click`. The ComboBox supplies the selected Subject object. The NumericUpDown supplies a mark from 0 to 100. A foreach loop checks whether that subject is already in `main.Input.Marks`. An existing row is updated; a new subject creates a `SubjectMark`. `PopulateList` puts these objects into a ListView. Removing a subject removes it from the draft list. Nothing here contains SQL or calculates an admission outcome.

`Entities/SubjectMark.cs` has private fields, public properties, a default constructor and a parameterised constructor. `InterestResponse` supplies another constructor example. These are the same concepts as Horse/July and Person/Employee in the practicals, applied to the project's own entities.

## 2. Forms and their Designer files (Practical 4)

Every screen has a partial class and a matching `.Designer.cs`. `InitializeComponent` creates standard controls; named methods such as `btnAdd_Click` handle events. Parameterless constructors are safe for the Designer and do not connect to a database. Runtime constructors receive the shared `AppController` and parent form.

`frmMain` is the MDI parent, as in the workshop's MDI example. `ShowStep` creates a step once, hides the previous child, shows the selected child inside the same parent and keeps the controls for Back. `GoNext` calls that child's `SaveInput`, then shows the next step. `frmAdvisoryStep` supplies the shared title, content panel and footer. Its virtual `RefreshStep` and `SaveInput` methods are overridden by the actual screens. This is a real use of inheritance and polymorphism, rather than an invented business hierarchy.

`frmInterestQuestionnaire` adds one Designer-backed `QuestionRow` per active question in a foreach loop. Its five RadioButtons produce scores 1–5. An explicitly incomplete questionnaire can continue, but the engine returns More Information Needed. Adding database questions therefore does not require redesigning the form.

## 3. Controllers and the three tiers (Practical 3)

`AppController` creates one AccessSession and the four business controllers, then shares them with forms. It is ordinary construction, not a dependency-injection framework.

`AdvisorySessionController.Preview` validates the draft and calls the pure engine. `Assess` saves the calculated session and records that the current workspace owns it. `Load` checks access before reading a session. `AuthenticationController`, `AdminMaintenanceController` and `ReportController` handle their respective staff tasks.

`IFuturePathStore` is a list of database operations the business layer needs. It lets the business project describe those operations without referencing a particular SQL provider. `IClock` supplies the date/time, which lets tests use a fixed date. Interfaces were introduced in PL 2.0; there is no container or generic repository framework.

## 4. The recommendation algorithm

Open `RecommendationEngine.Evaluate`. Read the helper calls in order:

1. `CheckConfiguration`: requirements and mapped interest questions must exist.
2. `CheckSubjects`: check each mandatory subject or explicit OR group against captured marks.
3. `CheckAverage`: check any configured overall-average minimum.
4. `CheckQuestionnaire`: detect missing active answers.
5. `CalculateDegreeInterest`: combine category scores with the stored degree weights.
6. `ChooseOutcome`: missing information first; then a hard gap; then a near gap or weak interest; otherwise a strong match.

An answer of 1/2/3/4/5 becomes 0/25/50/75/100%. Questions in the same category are averaged. Degree mappings weight those category averages.

`overall = 0.35 × subject coverage + 0.35 × mark match + 0.25 × interest alignment + 0.05 × faculty preference`.

A required mark shortfall up to five percentage points is a near match. More than five, or an absent required subject on a confirmed list, is Not Currently Eligible. Strong Match also needs interest alignment of at least 60%. A high weighted score cannot override these checks. All captured subjects count toward any overall-average rule.

`GenerateAlternatives` evaluates other active degrees with the same method, keeps only Strong/Possible matches and inserts them in order: outcome, descending score, name and ID. It keeps at most five. The names of sample learners are not used to choose outcomes.

## 5. Data classes and transactions (Practical 3)

`DB` holds the connection helper and shared command/read methods. `ReferenceDataDAL`, `DegreeProgrammeDAL`, `AdvisorySessionDAL`, `FundingDAL`, `UserAccountDAL` and `ReportDAL` inherit it. `SqlServerStore` forwards the business contract to those grouped classes.

Start with `ReferenceDataDAL.GetLookup`: open connection; create command; fill a DataSet using SqlDataAdapter; loop through DataRows; create business objects; return the list. The using blocks dispose resources.

Then open `AdvisorySessionDAL.SaveSession`. The transaction writes a new learner when needed, one session, marks, responses, selected result, alternatives, category scores and an audit entry. Commit happens after all writes succeed. A failure rolls back instead of saving half an assessment. Session and learner IDs are database-generated keys. A follow-up uses the same learner ID and a new session ID.

SQL values use parameters (`@p0`, `@p1` etc.). `ReferenceCatalog` is a fixed allowlist of editable tables and fields; it does not build screens. Actual administration screens have named controls and separate Designer files.

## 6. Roles, reports and supporting code

`AccessSession.RequireStaff`, `RequireAdmin` and `RequireSession` enforce permissions inside the business tier. A hidden button is not the access check. Ending a workspace clears guest ownership; staff sign-out also clears the role. `PasswordHasher` delegates password hashing to .NET's salted PBKDF2 implementation; do not replace it with a handmade hash to make it look shorter.

`ReportComposer` writes the learner summary using saved scores/reasons. `ReportController.Activity` filters saved sessions and uses loops to count selections, outcomes and gaps. Funding category counts use funding opt-ins and currently open linked categories; the report states that definition. `ReportFiles` uses native SaveFileDialog/PrintDialog. `ReportPrinter` overrides PrintDocument to paginate the report. HTML output encodes user text.

`Program` starts the Windows application, validates an optional database name and prevents duplicate instances. Database setup is explicit in `frmDatabaseSetup`. The asynchronous startup work only keeps the window responsive while LocalDB opens; it is supporting platform code, not the recommendation algorithm.

The source follows the workshops' concepts and form/controller/data separation. Framework setup, security APIs, transactions, printer pagination and test infrastructure remain necessary supporting code and are explained here rather than presented as workshop exercises copied verbatim.
