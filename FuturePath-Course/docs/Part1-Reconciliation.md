# Relationship to the group's submitted Part 1

The supplied **INF2011S_SystemSpecification_Group.pdf** is the group's Part 1 reference. This document records implementation decisions; it does not rewrite the submitted PDF or claim approval by the lecturer.

| Part 1 design | Implemented source / decision |
|---|---|
| Profile, subjects, interests, eligibility selection, funding, summary boundaries | frmLearnerProfile, frmSubjectMarks, frmInterestQuestionnaire, frmDegreeEligibility, frmFundingOptions, frmAdvisorySummary; frmAdvisoryResults separates the explained result before funding |
| Login, reports and administrator boundaries | frmLogin, frmReports, frmAdminMaintenance, ordinary named editor forms and frmLearnerSessions |
| No learner login or learner menu | Private learner workspace; seven Back/Next steps inside one MDI parent, with no learner sidebar |
| Controller / domain / DAL tiers | Three projects. AdvisorySessionController coordinates the learner use case; RecommendationEngine contains eligibility, interest scoring and alternatives. AuthenticationController, AdminMaintenanceController and ReportController handle staff tasks |
| Separate Interest/Eligibility/Recommendation/Funding controller boxes | Their small operations are consolidated into AdvisorySessionController and the pure RecommendationEngine, avoiding four forwarding-only classes. The corresponding behavior remains present; no unused classes are added merely to match a diagram |
| Aggregate DAL grouping | Six grouped DALs inheriting DB, with the SqlServerStore contract adapter; no one-query-per-table repository framework |
| 16-entity dictionary | Existing compatible 20 domain tables plus SchemaVersion are retained. Learner separates guest identity from staff UserAccount; FundingFaculty permits faculty-wide awards; AuditEvent supports the required controls. See Data-Dictionary.md and ERD.dot |
| DECIMAL(4,2) marks in draft dictionary | DECIMAL(7,2) retained, because 100.00 cannot fit DECIMAL(4,2) |
| School and Province mandatory; personal name/contact optional | New profile capture enforces School/Province/Grade. Legacy database strings remain readable; there is no destructive data migration |
| Save session at profile capture | Keep a draft in memory and commit at Save and view summary. Abandoning a draft does not create a misleading completed session; a deliberately incomplete assessment can be saved as More Information Needed |
| Screen standard | 16pt bold Segoe UI page title; 10pt body; labels above fields; required learner fields marked; field validation/ErrorProvider; disabled Next until required capture is valid; scrollable body and separate footer. Outcome text also has green/amber/red colour |
| Report period/faculty/degree/outcome filters | Implemented in frmReports and ReportController.Activity; saved selected-result gaps are counted and sorted |
| Most selected faculties/degrees and outcomes | Counts describe saved advisory sessions; the report also shows distinct learners. Follow-up sessions remain separate activity rather than overwriting previous records |
| Funding requests by category, but no separate category-request entity/field in supplied design | A funding opt-in requests all linked guidance. Each requested session is counted once per currently open linked funding type; the report explains that categories may overlap and use current reference data. It does not invent individual applications, awards or historical category selections |

The latest course/specification request takes precedence over the older pasted request's learner registration/dashboard and older screenshots' sidebar. The older implementation prompt's useful requirements—single parent window, LocalDB, safe setup, readable code, preserved behavior and honest tests—remain followed. Staff tasks use conventional modal forms; learner steps remain inside the parent. This follows the practical's Form/MDI pattern rather than maintaining a runtime-generated interface.

The current uploaded DisplayFix package targets .NET 10. Practical projects vary between .NET Framework 4.6.1/4.7.2/4.8. The runtime is retained, with Microsoft.Data.SqlClient; the ADO.NET pattern is the same. Password hashing, safe backups, printer pagination, nullable annotations and first-run setup are supporting framework code, not claims that every API appeared verbatim in lectures.

Existing learner data, passwords and historical scores are not reset. The optional demo seed remains unchanged. The two SQL02 setup guards and all schema creation now share a single batch, with a transaction around DDL, so manual Visual Studio execution cannot proceed past a rejected database into later creation batches.
