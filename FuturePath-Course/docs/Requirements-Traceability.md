# Requirements to implementation and checks

| Requirement | Main source | Data | Check |
|---|---|---|---|
| Standalone editable WinForms startup | Program, frmMain, partial/Designer files | SchemaVersion/setup | Full build; Windows launch/Designer acceptance |
| Private learner capture, required school/province/grade | frmLearnerProfile, AdvisingValidation.ProfileCapture | Learner, GradeLevel | CourseValidationTests; M01–M07 |
| Subject add/update/remove and valid marks | frmSubjectMarks, AdvisingValidation.Marks | LearnerSubject, Subject | PortableRuleTests; DesktopTests journey; M08–M12 |
| Questionnaire/interest categories | frmInterestQuestionnaire, QuestionRow, engine | InterestQuestion, InterestCategory, LearnerInterestResponse | Radio/portable tests; M13–M15 |
| Faculty/degree and eligibility | frmDegreeEligibility, AdvisorySessionController, RecommendationEngine | DegreeProgramme, ProgrammeRequirement, DegreeInterestMapping | Ten original scenario regression cases; M16–M23 |
| Explained alternatives | frmAdvisoryResults, engine.GenerateAlternatives | RecommendationResult | Boundary/ranking tests; engine comparison |
| Funding opt-in and linked current guidance | frmFundingOptions, FundingDAL | FundingOption, FundingFaculty, FundingProgramme | LocalDB funding tests; M24–M26 |
| Atomic persistence and follow-ups | AdvisorySessionController.Assess, AdvisorySessionDAL.SaveSession | AdvisorySession and child tables | Transaction rollback/round-trip tests; M27–M30 |
| Summary, activity and exception reports | ReportComposer, ReportController, frmReports | Saved results, ExportReport | Report/filter tests; M31–M36 |
| Authentication/roles/account maintenance | AuthenticationController, AccessSession, PasswordHasher | UserAccount, AuditEvent | Hash/role/lockout tests; M37–M41 |
| Validated reference maintenance | AdminMaintenanceController, named editor forms | Allowlisted reference tables | LocalDB maintenance tests; M42–M44 |
| Backup/audit and safe setup | DatabaseSetup, DB, ReportDAL | SQL01–04, AuditEvent | First-run/repeat/refusal/backup tests; M45–M46 |
| DPI, keyboard and single-window flow | frmAdvisoryStep, frmStaffForm, frmMain | None | Windows layout tests; M47–M50 plus ten-row OS/scale matrix |

PortableRuleTests, CourseValidationTests and OriginalScenarioRegressionTests run without a database. LocalDB and DesktopTests require Windows; compilation alone does not count as their execution. Manual case IDs and expected data are in Test-Cases.md and the workbook.
