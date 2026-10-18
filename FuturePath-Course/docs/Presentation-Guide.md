# A 5–10 minute presentation

Review and practise with your group before presenting. Explain what you understand, and acknowledge assistance according to course rules. No document guarantees marks.

1. **Open the solution (30 sec).** Show FuturePath.sln and the three projects. Explain: forms collect/display; business validates/calculates; data classes use LocalDB.
2. **Show existing reports (1 min).** Sign in as advisor_test. Use Learners and sessions to retrieve Gadija, Labeeqah and Derek. Explain Strong Match, the Mathematics 58 versus 60 gap, and the missing Mathematics/Physical Sciences requirements. Run reports for September 2026 before adding new sessions; the initial demo contains ten sessions.
3. **Capture a new assessment (2–3 min).** Start advisory. Use Grade 12, a sample school and Western Cape. Name/contact may be blank. Add English Home Language 72, Mathematics 70, Accounting 74, Business Studies 79, Economics 70 and Life Orientation 80. Confirm the list complete. Answer questions 1–8 with **4, 3, 5, 2, 3, 5, 3, 2**. Select Commerce → BCom Information Systems. Next previews Strong Match, interest 93.75%, total 98.44%. Back retains data. Choose funding guidance, then Save and view summary. Export HTML/text or use Microsoft Print to PDF.
4. **Walk through simple code (2 min).** Show frmSubjectMarks.btnAdd_Click, SubjectMark's fields/properties/constructors, frmMain.GoNext, AdvisorySessionController.Assess, RecommendationEngine.Evaluate and ReferenceDataDAL.GetLookup. Follow one object from the form into the controller and database, then back to a ListView/report.
5. **Show staff maintenance (1 min).** Sign out, then sign in as admin_test. Show one degree or funding editor in the Designer. New degrees begin inactive until their requirements and weights are configured. Show the role check in AdminMaintenanceController; an advisor cannot use that operation.
6. **Show evidence (30 sec).** Open docs/Validation.md. Distinguish automated Linux checks from Windows LocalDB/UI/manual tests. Do not call unrun tests passed.

## Short explanations you can use

- **Encapsulation:** SubjectMark stores values in private fields and exposes properties.
- **Construction/overloading:** SubjectMark has default and parameterised constructors; forms have Designer-safe and runtime constructors.
- **Composition:** AdvisoryInput contains a LearnerProfile and lists of marks/responses.
- **Abstraction:** the form asks a controller for a result; it does not need SQL details. IFuturePathStore defines the data operations.
- **Inheritance/polymorphism:** DAL classes inherit DB; learner forms inherit frmAdvisoryStep and override SaveInput/RefreshStep; ReportPrinter overrides PrintDocument.
- **Transaction:** all related assessment rows succeed together, or none is committed.
- **Rule-based guidance:** normal conditions and arithmetic use stored sample rules; no online AI or official admission decision.

The course ZIPs teach these patterns. Their waiter/horse classes are not copied into Future Path. The working application retains its .NET 10 target; the practicals used several .NET Framework versions. See Dependencies.md and Part1-Reconciliation.md.
