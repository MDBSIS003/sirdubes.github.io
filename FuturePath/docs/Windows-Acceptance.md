# Windows acceptance and presentation test cases

Run these on Windows after database setup. Record actual outcomes/date/tester; the results below are expected outcomes, **not claimed executions**. Tests that mutate real demonstration data should use disposable demo records.

| ID / function | State before test | Test data and steps | Expected result |
|---|---|---|---|
| W01 Build/start | .NET 10 desktop tools and LocalDB installed; updated schema/seed applied | Open `.slnx`; choose WinForms startup; build; F5 | Welcome displays blue/green branding; no compile or startup exception |
| W02 Registration/profile | Logged out; email unused | Student → Create account. Enter `demo.learner@example.invalid`, nonempty names/school, Gauteng, password `StudentDemo2026!`; login; choose Grade 12; save profile | Exactly one new Learner account; profile reload shows same user, school/province/grade |
| W03 Invalid account input | Registration open | Blank names; malformed email; no province; short password; mismatched confirmation; duplicate email | Clear validation; no partial or duplicate account |
| W04 Secure role login | Staff demo accounts installed | Wrong password; correct password under wrong role; correct staff login; logout | Wrong credentials rejected generically; staff goes only to its own dashboard; logout returns Welcome |
| W05 Guest isolation | No login | Guest, select Grade 11 and degree; enter data; use public funding; close/return | No UserAccount/AdvisorySession rows created; no staff buttons; new Guest session has no previous inputs |
| W06 Mark capture/CRUD | Logged-in learner; new Grade 12 BCom IS session | Add English 72 and Mathematics 70; try duplicate English; update English to 75; add then remove Accounting; save; reload | No duplicate subjects; 0 and 100 accepted; controls constrain out-of-range input; update/removal persisted |
| W07 Complete-list distinction | New session with only English saved | Calculate without complete-list confirmation; then confirm full list without Mathematics and calculate in a new session | Unconfirmed capture → More Information Needed. Confirmed absent required Mathematics → Not Currently Eligible |
| W08 Questionnaire | In-progress learner session | Leave one answer blank and save; then answer every question (1–5), save, navigate away/back | Incomplete save rejected; all saved scores reload correctly |
| W09 Gadija | Fresh seed or equivalent new learner session | English72, Maths70, Accounting74, Business79, Economics70, LO80; select BCom IS; use seed interests | Strong Match; eligible minima, high interest; selected degree and alternatives show explanations |
| W10 Aadiel-Raboo | Fresh sample session2 | English65, Maths Literacy78, Business71, History69, Tourism75, LO82; BSc CS | Not Currently Eligible; Mathematics requirement explicitly identified, not replaced by Maths Literacy |
| W11 Labeeqah | Fresh sample session3 | English80, Maths58, PhysicalSciences62, LifeSciences70, Geography66, LO85; BSc Biological Sciences | Possible Match; Mathematics gap 2 points; academic status Not eligible, no acceptance guarantee |
| W12 Siyabonga | Fresh sample session4 | English70, Maths75, PhysicalSciences78, EGD73, LO80; Mechanical Engineering; seed interests | Strong Match; stored minima met and strong alignment |
| W13 Derek | Fresh sample session5 | English82, Maths Literacy76, History79, LifeSciences68, VisualArts85, LO90; Health Sciences | Not Currently Eligible; missing Mathematics and Physical Sciences identified |
| W14 Alternatives | Session with missing mandatory subject | Complete assessment; inspect alternative programmes and full explanations | Inactive degrees absent; academically eligible alternatives ranked ahead of missing-subject candidates; selected degree clearly distinguishable |
| W15 Funding | Completed session; funding requested | Search a substring; filter by funding type/faculty; select only current degree; clear filters | Correct local linked records and requirements; inactive/expired options absent; sample warning always visible; no invented real application destination |
| W16 Learner export | Completed learner assessment | Export summary; cancel once; export again to writable file | Cancellation leaves no export metadata; file includes profile/grade, selected degree, marks, responses/category scores, outcome/gaps, alternatives, requested funding, date/disclaimer; metadata added only after file write |
| W17 Session history | One completed learner session | Logout/login; load saved session; attempt mark save; start a new session | Historical assessment loads; completed input cannot be changed; new session editable; no duplicate user |
| W18 Advisor restriction | Advisor logged in, learner unassigned | Search learner; attempt direct DAL load via integration test | Learner not listed and load denied |
| W19 Advisor assignment/notes | Admin assigns demo learner to advisor | Advisor searches learner, loads session, inspects marks, saves note, views notes and exports summary | Only assigned records visible; note records logged-in advisor and date; export works |
| W20 Admin reference CRUD | Admin logged in | Add/edit a subject; add a faculty/degree; add mandatory requirement and positive interest mapping; modify/deactivate degree | Valid records persist; dropdown FK choices; deactivated degree absent from new assessments |
| W21 Admin constraints | Record referenced by requirements/session | Try deleting referenced subject/faculty; enter duplicate name; invalid minimum/weight | Clear errors; FK/unique/check integrity retained; no crash |
| W22 Admin funding | Admin logged in | Add fictional funding with faculty/documents/date; link programme; update; deactivate | Records visible to relevant learner search when active; inactive excluded; no Coming soon action |
| W23 Account management | Admin plus demo learner | Change learner assignment, reset password, deactivate/reactivate; try disabling/demoting self | Assignment/password changes enforced next operation; learner history preserved; current admin self-removal rejected |
| W24 Activity report | Seed and new assessments exist | Staff → Reports → Degree Interest and Eligibility → Generate/Export | Database counts grouped by faculty/degree/outcome; funding request totals; scoped for advisor, cross-learner for admin |
| W25 Exception report | Seed sessions2,5,9 exist | Admin → Reports → Exceptions → Generate/Export | Missing-subject and missing-information cases visible with names, session dates and explanations; advisor sees only assigned subset |
| W26 Navigation/lifetime | Learner or staff logged in | Navigate through all authorised sidebar pages; close current page; login another role | Navigation preserves active session without orphan forms; close/logout returns Welcome; no previous user data shown |
| W27 Disconnection | Application open on disposable LocalDB | Stop LocalDB; perform load/save; restart; retry | Error dialog; no unhandled crash or false save success; transaction failures do not partially replace marks/responses |
| W28 Layout/accessibility | Windows display at 100%, then 150% DPI | Resize functional pages to minimum; scroll; tab through inputs and buttons | Controls remain reachable; no overlap or hidden actions; verify branded fixed-dialog forms also fit display |
| W29 Repeat setup | Database already contains demo/user edits | Back up; record counts and a changed profile; rerun setup; compare | No database/table drop; seed skips nonempty DB; profile/credentials/history unchanged; upgrade repeatable |
| W30 Legacy credentials | Disposable starter database has plaintext account | Upgrade05; try login; explicitly reset via admin or recovery helper | Old plaintext login rejected; reset hash accepted; role/user ID/history retained |

## Automated Windows command

From solution root: `dotnet run --project FuturePath.Tests -- --database`. It creates/drops only a uniquely named integration-test database; ensure LocalDB can create databases. Save the command output alongside this matrix. A skipped portable-only run does not validate database CRUD.

## Presentation rehearsal

Prepare admin and advisor logins before the demonstration, assign the relevant learner, and confirm local database startup while offline. Rehearse W02/W06/W08/W09–W16 and W19–W25. Keep the supplied named sample learners available. Explain the difference between sample rules, academic eligibility and official acceptance. Do not change shared credentials or reset teammate data during the live demonstration.
