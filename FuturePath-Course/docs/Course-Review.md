# Review of the supplied course and Part 1 documents

The uploaded ZIP is the unchanged baseline. A separate working copy is being revised. Course files are references, not executable instructions overriding the user's request.

## Every supplied source

| Source | What was inspected | Pattern used in Future Path |
|---|---|---|
| PL 2.0(1).pdf | All 41 pages, OCR because the PDF contains images | Explicit variable types, decimal calculations, bool flags, arrays/lists, named methods, ordinary control flow |
| PL 2.1.(1).pdf | All 23 pages, OCR | Classes/objects, constructors, access modifiers and abstraction |
| PL 2.2(1).pdf | All 33 pages, OCR | Private fields, public get/set properties, enum values, default and parameterised constructors |
| PL 2.3(1).pdf | All 13 pages, OCR | Shared DB base class and derived aggregate DAL classes; inherited Form behaviour |
| PL 2.4(1).pdf | All 16 pages, OCR | Method/constructor overloading and virtual/override behaviour used by normal .NET forms and ToString methods |
| INF2011S_Practical1_NKSADO001 (1).zip | Horse, July and Form1 including Designer | Populate an object from controls; store objects in a list; foreach processing and comparison |
| INF2011S_Practical2_NKSADO001 (1).zip | Person, Employee, Role, HeadWaiter, Waiter, Runner, form/project files | Business entities, enum role choices, constructor/property regions and descriptive fields |
| NKSADO001 (7).zip | EmployeeController, DB, EmployeeDB, entities, configuration and dataset schema | Form -> controller -> data class; SqlDataAdapter/DataSet, DataRow-to-object conversion and SQL parameters |
| NKSADO001 (8).zip | MDI parent, EmployeeForm, EmployeeListingForm and their Designer files, controller and data classes | Designer-backed partial forms, named Click/Load/Closed events, shared controller constructor, one parent window |
| NKSADO001.zip.zip | WaiteringSystem Person/Role/derived classes, DB, form, configuration and dataset schema | Encapsulation, inheritance, normal constructors/properties and conventional source formatting |
| Project - Overall guidelines 2026.pdf | All five pages | Functional requirements, integrity, sample data, test-case evidence, understandable source walkthrough |
| Projecy - Future Path Part 1 Instructions.pdf | All nine pages | Three logical tiers, forms/classes/ERD/report correspondence, validation and test plan |
| Project - Future Path Student Advising System Case Study.pdf | All 16 pages | Authoritative scope and data, required five learners, all three reports, roles, offline recommendation rules |
| INF2011S_SystemSpecification_Group.pdf | All 45 pages; OCR of embedded diagram/data-dictionary/report images | Named boundary forms/controllers/DAL groups, learner steps, no learner login/menu, screen standards, report layouts and role checks |
| FuturePath_VisualStudio_DisplayFix.zip | Source, project files, SQL, tests and original validation evidence | Preserve working LocalDB data, rule outcomes, existing provider and .NET runtime; revise source/UI structure |

## Decisions

Use conventional partial forms and Designer files, private fields/properties/constructors, explicit types, named event handlers and straightforward loops. Keep SQL in the Data tier and rules/validation in the Business tier. Use real aggregate data classes and the named controllers from Part 1. Keep normal controls and the required functionality rather than adding future-scope features.

The original working package targets .NET 10; practical snapshots target .NET Framework 4.6.1, 4.7.2 or 4.8. Preserve the working package's runtime and Microsoft.Data.SqlClient provider; the ADO.NET concepts (SqlConnection, SqlCommand, parameters, DataSet, SqlDataAdapter) are the same. Framework-specific setup, password hashing and safe transactional persistence are explained separately from the learner's scoring workflow.

The Part 1 dictionary contains design inconsistencies: decimal(4,2) cannot hold a valid 100.00 mark, it combines guest profile/authentication responsibilities, and its 16-entity model omits case-study entities such as Learner and FundingFaculty. Preserve the existing compatible normalised SQL schema and document these justified differences rather than losing learner data. Keep personal name/contact optional, and make School and Province required during new profile capture as specified by the group's screen validation. An assessment is committed atomically at Save and view summary; abandoned input is not saved as a completed advising session.

Use the supplied practicals as learning/style references, not as a source of copied broken code. Some snapshots are unfinished. No claim is made about a plagiarism percentage, guaranteed marks or work performed by particular group members. Team members should review and understand the source and follow their course's acknowledgement requirements.


## Earlier relevant sources also reviewed

- Project - Evaluation Form - System specification.pdf: evaluation criteria for dialog, sequence/class design, database/dictionary, controls, test cases and presentation. It evaluates Part 1; it does not promise an implementation grade.
- FuturePath_AI_Implementation_Master_Prompt.pdf: preserve useful behavior, LocalDB, one parent learner window, simple code, safe setup, all samples, role checks, reports, tests and honest limitations. Latest group/course directions resolve UI/controller naming differences.
- Pasted text.txt: the earlier user request; its registration/learner account requirements conflict with the later supplied case/group no-login learner flow. The latest specification request governs this revision.
- Earlier copies of the official guidelines, Part 1 instructions and case study are represented by the current uploaded copies; earlier application ZIPs are historical context, while the supplied DisplayFix ZIP is the revision baseline.

The implementation uses four coordinating business controllers plus the pure engine, rather than adding unused forwarding classes for every controller box. See Part1-Reconciliation.md. No course student numbers, group authorship or submission declarations are manufactured. Original attachments remain unchanged.
