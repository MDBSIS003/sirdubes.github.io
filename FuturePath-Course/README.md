# Future Path — workshop-style Visual Studio edition (2.0)

This editable C# Windows Forms solution revises **FuturePath_VisualStudio_DisplayFix.zip** using the supplied course PDFs, all five practical ZIPs and the group's **INF2011S_SystemSpecification_Group.pdf** as references. It keeps the existing three projects, LocalDB schema, sample data and advising rules. See **docs/Course-Review.md** for the source-by-source review and **docs/Part1-Reconciliation.md** for explicit design decisions.

## Open and run

1. **Extract All** to a new, writable folder on Windows. Keep the whole folder together.
2. Double-click **FuturePath.sln** to open Visual Studio.
3. Press **F5**. The startup project is **FuturePath.Desktop**, a Windows application.
4. If the database is missing, choose **Prepare demonstration database** inside the app. Existing records are preserved.

Required: Visual Studio 2026, **.NET desktop development**, .NET 10 SDK and **SQL Server Express LocalDB**. Install LocalDB through Visual Studio Installer → Modify → Individual components. Internet is needed for the first package restore; advising runs locally afterwards. No GitHub account or terminal is needed to run the application. Opening a `.sln` opens the project; F5 starts it.

The project uses the existing `(localdb)\MSSQLLocalDB` database **FuturePathStudentAdvisingSystem** under your Windows account. It does not import a database from another Windows installation or another Windows account. Do not delete an existing database to resolve setup errors. Optional manual instructions are in **docs/Database-Setup.md**.

## What changed

- Ordinary partial forms with editable **.Designer.cs** files and named Click handlers, following the practicals.
- One MDI parent window, with one visible learner step: **Profile → Subjects → Interests → Degree → Results → Funding → Summary**. Back keeps the previous controls. There is no learner sidebar/menu.
- Shared DPI-aware layout with a fixed navigation footer and scrollable content. Staff dialogs fit the available desktop. This addresses the narrow sidebar and clipped content seen at 200% scaling; actual Windows 10/11 display checks remain necessary.
- Private fields and get/set properties, constructor overloads, explicit variable types, small methods, ordinary loops and normal `using` blocks. Production source uses no LINQ query chains, positional records or reflection-built forms.
- `Form → controller → aggregate DAL → LocalDB`. DAL classes inherit `DB`; reads use `SqlDataAdapter`, `DataSet`, `DataTable` and `DataRow`, as in the workshops. Parameters and transactions remain in place.
- Required School, Province and Grade; optional name/contact; faculty/degree validation; deliberate incomplete-data guidance; staff roles, administration, three reports, export and printing.
- Report filters for period, faculty, degree and outcome; ranked selections, saved gap counts and funding category guidance counts.

The results screen previews without writing. **Save and view summary** commits the complete assessment in one transaction. The summary is read-only; a staff follow-up creates a new assessment and preserves the earlier result.

## Fictional staff accounts

| Username | Password | Role |
|---|---|---|
| advisor_test | FuturePathAdvisor2026! | Career Advisor |
| admin_test | FuturePathAdmin2026! | Administrator |

Only the explicit demo seed creates these accounts. Password hashes are stored; there is no hardcoded sign-in bypass. Learners do not need an account. End the learner workspace before another person uses the computer.

## Where to start reading

Open **docs/Code-Walkthrough.md** and **docs/Presentation-Guide.md**. For forms, expand **FuturePath.Desktop → Presentation** in Solution Explorer. Right-click a form and choose **View Designer**; build first if Visual Studio needs the inherited base form assembly. Edit events in the matching `.cs`, not generated Designer code.

**FuturePath.Development.sln** additionally includes automated tests. Use **FuturePath.sln** for normal application use. Runtime packages and the framework choice are explained in **docs/Dependencies.md**. `tools/SeedGenerator` and test fixtures are developer tools, not startup programs.

The five required learners and five additional supplied learners remain in the demo dataset. Admissions requirements and funding records are samples and require verification. No official admission or funding decision is made.

## Verification

See **docs/Validation.md** for actual build/test results and Windows limitations. **docs/Test-Cases.md** and **Test-Material/FuturePath_Course_Test_Checklist.xlsx** contain the Windows acceptance steps. No guaranteed mark or plagiarism score is claimed; review the code and follow your course's acknowledgement rules.
