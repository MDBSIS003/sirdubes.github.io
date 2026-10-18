# Requirements traceability

Source documents: Future Path Student Advising System Case Study (Parts A/B), Future Path Part 1 Instructions, Overall Guidelines 2026, and the supplied system-specification template. Case-specific requirements govern the application. Generic template examples about hotel bookings and occupancy are not Future Path requirements.

| Requirement | Implementation / design | Evidence / demonstration |
|---|---|---|
| Standalone C# Windows Forms | FuturePath.Desktop; self-contained Windows x64 package | All projects compile; Windows launch still requires acceptance |
| Three-tier OOP architecture | Desktop → Business contracts/services; Data implements IFuturePathStore; constructor composition | System-Specification sections 3/4; Code-Walkthrough |
| Local relational database, 3NF | Database/02_CreateTables.sql, SQL Server LocalDB FK/unique/check constraints; normalized junctions | Ported Windows LocalDB integration/integrity tests (not executed on Linux); Data-Dictionary and ERD |
| No external APIs / AI / cloud at runtime | Local RecommendationEngine and SQL Server LocalDB; no HTTP client/network feature | Source inspection; offline Windows TC25 pending |
| Learner profile, Grade 11/12 | AdvisingPage + AdvisingService validation | Profile/grade/email/anonymous tests; TC01 |
| Subject/mark capture and duplicates | BindingList grid; 0–100 numeric input; service and database constraints | Range, decimal, duplicate and integrity tests; TC02 |
| Preferred faculty/degree | Active faculty/programme lookups, stored preference | Programme filtering and eligibility tests; TC03–07 |
| At least 10 learners, required five | Seed.sql exact case-study names/marks plus five starter learners | Seed count and seven computed outcome scenarios |
| At least 16 subjects | 18 seeded, including Zulu/Xhosa | Seed count test |
| At least 5 faculties and 10 degrees | Five faculties; 10 programmes | Seed counts, each degree has requirements/mapping |
| Programme requirement variety | Mathematics/Physical Sciences rules and explicit Literacy OR groups | Count and OR-rule tests; TC11 |
| At least 5 interest categories, 8 questions | Eight categories, eight active questions, per-session responses | Counts, response-range, missing-answer and low-interest tests |
| Four explained outcomes | RecommendationEngine with stored reasons and rule version | Exact Part B cases, boundary/incomplete/low-interest tests |
| Labeeqah possible with Mathematics gap | Biology minimum Maths60; entered58; near-miss <=5 | Exact two-point-gap test and TC05 |
| Alternatives based on subjects/marks/interests | Evaluate all active degrees, filter hard gaps/incomplete, stable weighted ranking | Alternatives/Literacy/archive tests and TC04 |
| At least 8 funding records | 10 records; merit/need and faculty/programme junctions | Counts; active/open/related filters; TC12/13 |
| Funding fields and opt-in | FundingOption model/typed editor; profile session FundingRequested | Opt-out export and funding tests; TC12/21 |
| At least 10 advisory sessions incl incomplete | 10 sessions seeded, scored by real engine | Repeatable seed, required outcomes and missing-info tests |
| Exported report metadata | Successful user/test exports recorded; setup does not invent export metadata | Export files and ExportReport assertions |
| Learner Advisory Summary | ReportComposer/ReportService; viewer, text/HTML, Windows print | Full section/export/HTML encoding tests; TC14 |
| Degree Interest and Eligibility report | ReportService.Activity(false), inclusive filters, counts/detail | Saved-session totals, date/search tests; TC15 |
| Not Eligible exception report | ReportService.Activity(true), exact outcome filters | Include/exclude assertions; TC16 |
| Career Advisor authentication and work | AuthenticationService, AccessSession, AdvisorPage, follow-ups | Role/auth/follow-up tests; TC17/18 |
| Administrator-only maintenance | MaintenanceService, ReferenceEditor, AccountEditor | Guest/advisor denials; maintenance/rule activation tests |
| Add/update/deactivate degrees/funding | Typed editors, active status, no destructive record deletion | Archive/new-degree/funding tests; TC19–21 |
| Security and integrity | PBKDF2, login lockout, role checks, parameters, transaction, consistent backup | Security and database/export/backup tests; TC17–23 |
| Sample/advisory warnings and dates | Engine disclaimer + report headings, funding notices | Summary/HTML tests and design section 7 |
| Screen/navigation design | MainForm shell, validated Next/Back panels, embedded staff/editor/report pages, wireframes | System-Specification section 2, screen-layout image |
| Two design sequence diagrams | Eligibility and alternative recommendation sequences | Specification section 3 and diagram files |
| Class diagram / ERD / data dictionary | Actual interfaces, classes, table columns, keys and relationships | Specification sections 4/5 and diagrams |
| Output/control/security/implementation/test plan | Implementation-aligned design draft | Specification sections 6–12; team review required |
| Demonstration test cases and source walkthrough | TC01–26 and Code-Walkthrough | Portable rule tests run; real LocalDB/UI/manual evidence pending |

A traceability entry demonstrates coverage in the implementation and tests; it does not guarantee a particular mark. Team submission details, previous-design reconciliation and Windows acceptance remain the team's responsibility.

## PDF migration requirements

| Requirement | Implementation | Verification |
|---|---|---|
| SQL Server LocalDB only, integrated security | DatabaseConnection, SqlServerStore, Database/01–04 | Provider/source and SQL grammar checked; Windows integration pending |
| One window, retained inputs, validated Next/Back | MainForm, AdvisingPage, AdvisingValidation | Compiled; portable validation passed; Windows retention tests supplied |
| Summary and staff editors embedded | ReportViewer/ReferenceEditor/AccountEditor UserControls | Compiled; Windows one-window tests supplied |
| Whole assessment plus audit in one transaction | SqlServerStore.SaveSession | Rollback/follow-up/reopen tests ported; Windows execution pending |
| Keep only necessary dependencies | Data csproj, Directory.Packages.props, lockfiles, Dependencies.md | Restore/build and dependency audit |
| Beginner documentation and honest validation | README, Database-Setup, Code-Walkthrough, Presentation-Guide, Changes-from-reference, Validation | Delivered; no claim of unexecuted Windows checks |
