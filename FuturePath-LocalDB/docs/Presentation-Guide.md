# 5–10 minute presentation guide

Before the presentation, install LocalDB, open FuturePath.sln, press F5 and choose Prepare demonstration database on first run, execute the Windows tests and record the manual cases in Test-Cases.md. Confirm that the startup project is FuturePath.Desktop. Use fictional data and protect exported files.

| Time | Demonstrate | Explain |
|---|---|---|
|0–1 min | Overview, LocalDB status, solution projects | Standalone Windows Forms; local rules; no runtime AI/API; three tiers |
|1–4 | New learner, Next/Back, marks, questionnaire, degree, Check & Save | One window; shared validation; retained in-memory draft; one transaction at confirmation |
|4–5 | Outcome, alternatives, optional funding, embedded summary | High scores cannot hide mandatory gaps; sample disclaimer; HTML/text export and Windows print |
|5–6 | Advisor sign-in; view Labeeqah and create follow-up | Roles come from hashed account data; follow-up preserves the original result |
|6–8 | Activity/exception report; admin edit/deactivate; account/audit/backup | Business authorization, parameters, stored explanations, history retained, copy-only backup |
|8–10 | Show key methods and actual test evidence | Explain the algorithm and distinguish executed portable checks from Windows verification |

Suggested source path: Program → MainForm.ShowPage → AdvisingPage.GoNext/SubmitAssessment → AdvisingService.Assess → RecommendationEngine.Evaluate → SqlServerStore.SaveSession → ReportService/ReportViewer.

Examples every member should be able to explain:

- **Encapsulation:** only authenticated business code can establish AccessSession's private identity.
- **Abstraction/polymorphism:** services use IFuturePathStore; SqlServerStore supplies the SQL implementation.
- **Composition:** MainForm hosts controls; AdvisorySession contains marks, responses and results.
- **Inheritance:** MainForm extends AppForm; pages/report/editor extend UserControl. Do not claim ReportViewer is still a Form.
- **SQL safety:** parameters carry values; reference identifiers are fixed allowlisted definitions.
- **Transaction:** if a response/subject write fails, learner/session/results/audit all roll back.
- **Rules:**35% subjects+35% marks+25% interests+5% faculty; explicit OR groups; >5-point gap is not eligible; near match is guidance only.

Required examples: Gadija Strong Match; Aadiel-Raboo missing Mathematics; Labeeqah Maths58/60 Possible Match; Siyabonga Strong Match; Derek missing Maths and Physical Sciences. Use stored examples first, then demonstrate one live new session after Windows verification.

Use the group's actual responsibilities and explain assistance honestly. Do not promise a grade, call sample criteria official, or describe skipped LocalDB/UI tests as passing.
