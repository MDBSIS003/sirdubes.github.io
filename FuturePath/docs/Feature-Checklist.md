# Implementation checklist and remaining validation

“Implemented” means integrated source exists and compiles. It does not substitute for Windows GUI/LocalDB acceptance testing.

| Capability | Source status | Verification |
|---|---|---|
| Preserve starter solution, namespace, WinForms and LocalDB | Implemented | Solution builds on Linux with Windows targeting |
| Welcome/login/registration and four role paths | Implemented | Password and permission tests executed; interactive login pending Windows |
| Salted hashes, no plaintext fallback, logout | Implemented | Portable cryptographic/role tests passed; live login tests supplied, not run |
| Backend ownership and assigned advisor access | Implemented | Rejection tests passed before SQL; live assignment suite pending |
| Profile load/update, school/province/grade/phone | Implemented | Validation passed; SQL profile persistence suite pending |
| Subject add/update/remove, 0–100, duplicates, persistence | Implemented | Boundary/duplicate tests passed; database CRUD pending |
| Database-driven questionnaire, required answers and persistence | Implemented | Validation passed; live load/save pending |
| Faculty/degree selection and session history | Implemented | Compiled; GUI/LocalDB acceptance pending |
| Mandatory/alternative subject rules, mark gaps, interest ranking | Implemented | Exact five required learner scenarios and formula tests passed |
| Stored recommendations and completed-session protection | Implemented | Compiled; SQL persistence/freeze tests pending |
| Funding search/type/faculty/degree filters and details | Implemented | Compiled and SQL parsed; live filtering pending |
| Advisor dashboard, assigned learners, marks/results, notes | Implemented | Compiled; assignment/note tests pending |
| Admin CRUD for all required reference data, roles, assignments | Implemented | Field validation and whitelist tests passed; live CRUD pending |
| Destructive confirmations, deactivation and FK protection | Implemented | Compiled; Windows acceptance pending |
| Three reports: advisory summary, eligibility activity, exceptions | Implemented | Compiled and SQL parsed; export/live data acceptance pending |
| Repeatable nondestructive setup and legacy password recovery | Implemented | T-SQL grammar parsed; repeated setup suite and PowerShell require Windows |
| >=10 learners/degrees/sessions, >=8 questions/funding, >=5 faculties/categories | Supplied/updated sample data | Canonical fixture tests verify programmes and required learner outcomes |
| README, architecture, changed-file list, phase reports, presentation notes | Delivered | Documentation review completed |

Remaining before claiming a presentation-ready running application:

1. Execute `dotnet run --project FuturePath.Tests -- --database` on Windows; fix any runtime SQL/LocalDB failures found.
2. Execute the Windows acceptance matrix, especially navigation/logout, tab order, small-screen/high-DPI layouts, profile reload, exporting and database-disconnect handling.
3. Run PowerShell setup/recovery helpers on Windows, or use the documented SSMS sequence.
4. Review the implementation assumptions against your team's approved design (not included in the attachments).
5. Have each member practise explaining their work; add any required diagrams/assignment submission formatting. Do not claim this handoff is the team's complete Part 1 design submission.

No online funding verification, real applications, real university admission guarantee, cloud API, production deployment or unrequested website changes are included.
