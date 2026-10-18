# Future Path test cases and demonstration guide

Use a fresh demonstration database, not real learner records. Record actual results, tester, date and evidence during the Windows demonstration. Automated results are documented separately in Validation.md. “Expected” below is a test specification, not a claim of a completed Windows manual test.

| ID | Function and initial state | Test data / steps | Expected result |
|---|---|---|---|
| TC01 | New learner profile, guest workspace | New advisory session; Grade 12; optional name; English 72, Mathematics 70, Accounting 74, Business Studies 79, Economics 70, Life Orientation 80; confirm complete | Valid capture; each subject appears once; optional personal fields can remain blank |
| TC02 | Subject validation, unsaved session | Attempt duplicate English; attempt mark outside 0–100; select a row, update to 72.35, then remove it | Duplicate rejected; numeric control restricts range; service rejects bypasses; update/remove affect only selected entry; decimals retained |
| TC03 | Gadija sample, staff signed in | Search Gadija; view summary | BCom Information Systems; Strong Match; English/Mathematics requirements met; subject, interest and scoring details present |
| TC04 | Aadiel-Raboo sample | Search Aadiel-Raboo; summary and alternatives | Computer Science Not Currently Eligible; Mathematics missing; Mathematical Literacy does not substitute; alternatives include suitable programmes accepting Literacy |
| TC05 | Labeeqah sample | Search Labeeqah; view summary; create follow-up and change Mathematics 58→65 | Original: Biology Possible Match with two-point Mathematics gap; follow-up: Strong Match; original session preserved |
| TC06 | Siyabonga sample | Search Siyabonga | Mechanical Engineering Strong Match; Mathematics 75 and Physical Sciences 78 satisfy 70% minima |
| TC07 | Derek sample | Search Derek Vin Diesel | Health Sciences Not Currently Eligible; both Mathematics and Physical Sciences explained as missing |
| TC08 | Missing information | Search Yusuf; then try Next with an unanswered questionnaire; separately create a session with unconfirmed subject capture | Yusuf/unconfirmed capture: More Information Needed with reasons. Questionnaire Next rejects unanswered items. The engine/service still supports incomplete imported inputs; no false eligible alternatives |
| TC09 | Interest alignment | Stef's sample; or all subject requirements pass but responses all 1 | Possible Match because interest is low; suitable alternatives ranked by outcome, then score |
| TC10 | Eight-question questionnaire | Answer 1–5; leave one blank in another session | Responses stored; 1 maps to 0%, 3 to 50%, 5 to 100%; Next rejects a blank answer; engine/service classify incomplete imported data as More Information Needed |
| TC11 | Explicit OR rule | BCom General, English ≥55 and Mathematical Literacy ≥60, no Mathematics; complete answers | OR group accepted; Mathematics not separately required; no false missing-Mathematics gap |
| TC12 | Funding opt-in | Gadija session requesting funding; review funding section | Commerce awards shown with type, criteria, documents, closing date, contact and sample warning; no unrelated science/archived awards |
| TC13 | Funding opt-out / expiry | New session with funding unticked; in admin view archived/expired funding records | Opt-out summary says funding not requested; inactive or past-closing-date awards never appear to learners |
| TC14 | Learner export | Export summary as .txt and .html; reopen saved files; print to Microsoft Print to PDF if installed | Full profile, marks, interests, outcome, reasons, alternatives, funding, date and disclaimer; HTML escapes learner text; real export metadata recorded |
| TC15 | Activity report, advisor login | Reports → Degree Interest and Eligibility; dates covering demo data; then date range in 2020 | Demo totals: 10 sessions, 10 learners, counts by faculty/programme/outcome; empty range clearly says no sessions; reversed dates rejected |
| TC16 | Exception report | Reports → exception report, demo dates | Aadiel-Raboo, Derek and Yusuf included; Gadija excluded; outcomes/gaps/dates provided |
| TC17 | Authentication | Wrong password; then correct advisor credentials; five failures on a separate account | Wrong details denied; correct role granted; fifth failure locks for five minutes; reset/expiry restores access |
| TC18 | Role separation | Guest and advisor attempt admin functions; advisor uses reports; admin signs in | Guest cannot browse other learners; advisor can review/report but cannot maintain reference/account data; admin can maintain |
| TC19 | Programme maintenance | Admin: add inactive degree, add mandatory requirements and mappings, activate, assess, then deactivate | Incomplete programme cannot activate; lookups enforce valid relationships; inactive programme disappears from learner selection/alternatives while saved history remains |
| TC20 | Reference data | Add/edit faculty, subject, category, question, degree weight; use duplicate names/invalid values | Valid changes persist and affect new assessments; duplicates, missing required values, invalid marks/weights/references rejected |
| TC21 | Funding maintenance | Create funding option, select valid type/date, link faculty/programme; deactivate | Only proper matching active/open funding appears; faculty-wide and programme-specific rules follow documented semantics |
| TC22 | Staff maintenance | Admin creates advisor, resets password, disables account; attempts self-demotion | Advisor authenticates with new password; inactive account denied; current administrator cannot disable/demote itself; hashes not displayed |
| TC23 | Backup/restart | Create session; export; admin backup; close/reopen same database; restore a .bak to a new LocalDB database and connect to it | Session and scores survive restart; copy-only checksummed SQL backup restores its records; existing .bak files are not overwritten |
| TC24 | Navigation and layout | Use keyboard/Tab, resize to minimum, Windows 125%/150% DPI; navigate away from unsaved session | Visible labels and focus; scrolling works; confirmation protects unsaved data; sign-out clears learner information from screen |
| TC25 | Privacy and offline operation | Disconnect network after setup; create complete session and export; restart as guest | Workflow works offline; guest cannot retrieve other or prior-workspace sessions; staff can retrieve persisted sessions after login |
| TC26 | Fault tolerance | Choose unwritable export destination; open the same LocalDB database twice; cancel save/print dialogs | Clear error without truncating old report; second app instance denied; cancellation leaves data intact |

## Suggested 30-minute demonstration

1. **0–3 minutes:** show offline desktop startup, layered solution and seeded counts. Identify sample information and the advisory disclaimer.
2. **3–9:** create a learner, show validation, complete questionnaire, check eligibility, funding and export.
3. **9–15:** advisor login; walk through the five named scenarios; show Yusuf and a follow-up for Labeeqah.
4. **15–19:** generate and export the activity and exception reports with filters.
5. **19–24:** administrator maintenance; add inactive programme, rules and mappings; activate/deactivate; maintain funding; show advisor restriction and account management.
6. **24–27:** backup, persistence, test evidence and edge cases.
7. **27–30:** explain a UI→service→repository→database call and the recommendation rules. Every team member should understand the source they present.

Before presenting, complete TC14, TC24–TC26 on the actual Windows demonstration machine, run both automated test projects, and record evidence. Keep the extracted application folder intact, retain a clean demo database backup, and verify the printer/PDF device in advance.

## LocalDB/single-window acceptance additions

| ID | State and action | Expected result |
|---|---|---|
| TC27 | Fresh Windows LocalDB; run SETUP-DEMO.cmd or Database/01–04 | Correct named database,20 domain tables plus SchemaVersion; expected counts and five scenario results; no SQLite file created |
| TC28 | Edit a profile/reference row; record counts; rerun setup and restart | Existing records/credentials/results retained; demo seed skips all inserts on nonempty database |
| TC29 | Enter name, Next, Back; enter marks/questions and navigate | One main window; retained input; step indicator; no assessment saved until Check eligibility & save |
| TC30 | Leave grade invalid, no marks or unanswered required questions; use Next | Validation identifies the current step and keeps it visible; no premature write |
| TC31 | Complete assessment; summary; Back; Finish | Embedded summary with export/print; Back returns to saved outcome; Finish returns to overview in same window |
| TC32 | Staff edit a reference/account; Cancel, Save, or navigate away | Editor stays inside main window; Save validates/persists; Cancel returns list; leaving unsaved edit asks for confirmation |
| TC33 | Invalid/unavailable LocalDB instance or unknown schema | Actionable setup error; no automatic database creation/reset or loss of existing records |
| TC34 | Run the full automated solution tests on Windows | Real isolated LocalDB setup/repeat/CRUD/rollback/auth/backup/restore tests and four UI tests complete; save TRX results; investigate failures |

The Linux portable run skips LocalDB cases explicitly. Complete these actual Windows checks, including125%/150% DPI, before presenting. SQL grammar parsing does not prove schema execution or data persistence.
