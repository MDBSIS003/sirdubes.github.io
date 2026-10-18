# Future Path 1.0.1 — complete practical test checklist

167 planned acceptance cases, plus the OS/DPI matrix and seven exact reference scenarios. Each boundary-input combination within a row must be exercised. These are expected results, not a claim that tests passed.

[Download Excel checklist](https://github.com/MDBSIS003/sirdubes.github.io/raw/refs/heads/integration/localdb-single-window/Test-Material/FuturePath_Test_Checklist.xlsx) · [Download CSV](https://github.com/MDBSIS003/sirdubes.github.io/raw/refs/heads/integration/localdb-single-window/Test-Material/FuturePath_Test_Checklist.csv)

**Purpose:** Comprehensive practical acceptance checklist for Future Path 1.0.1. Each row is a test case; some rows enumerate multiple boundary inputs. No checklist can enumerate every arbitrary input combination.

**How to use:** Filter Checklist by Priority/Area. Execute P0 first, then P1, then P2. Set Status to Pass/Fail/Blocked/Not applicable. Record actual results, screenshot path, tester/date and defect reference.

**Safety of test data:** Use fictional data. Take a baseline backup and use a separate disposable demo DB for admin edits, restore, schema/failure and load tests. Do not corrupt/delete the working database to test failures.

**Expected versus verified:** Expected result describes what should happen. All workbook statuses start Not run. This workbook is a plan, not evidence that the app passes these checks on your machines.

**Automation scope:** Run All Tests executes implemented tests only. Rows labelled Advanced/developer or extra boundary combinations may require new fixtures/tests; they are not all already automated. Manually exercise the real interface as well.

**Existing evidence:** The Linux workspace previously verified 36 portable passes/48 LocalDB skips. The current app and test projects compile. Windows Forms/LocalDB/DPI acceptance must run on Windows; skipped is not passed.

**Windows requirements:** Visual Studio 2026; .NET desktop development; .NET 10 SDK; SQL Server Express LocalDB. The runtime is Windows Forms, including when Windows runs on a Mac.

**Normal startup:** Open FuturePath.sln and choose Debug > Start Debugging/F5.

**Automated tests in Visual Studio:** Open FuturePath.Development.sln; build; choose Test > Test Explorer; then Run All Tests. Inspect failures and skipped/not-run results. The normal app-only solution does not contain test projects.

**Automated restore:** dotnet restore FuturePath.Development.sln --locked-mode

**Automated build:** dotnet build FuturePath.Development.sln -c Release --no-restore

**Automated run:** dotnet test FuturePath.Development.sln -c Release --no-build --logger trx --results-directory TestResults

**Disposable test databases:** Existing integration tests create uniquely named FuturePathTest_<guid> LocalDB databases and clean up only those guarded names. They do not drop the application DB.

**Demo accounts:** advisor_test / FuturePathAdvisor2026! ; admin_test / FuturePathAdmin2026! . Fictional public seed accounts only. Use a spare account for lockout/password reset testing.

**Baseline counts:** Fresh untouched seed: 10 learners;18 subjects;5 faculties;10 degrees;8 categories;8 questions;10 funding options;10 sessions;20 domain tables + SchemaVersion. Counts change legitimately after new sessions/maintenance.

**Report date range:** Seed session dates are 15-24 September 2026 (UTC). Use an inclusive range covering these dates; update expectations after making follow-ups.

**Viewing versus recomputing:** Viewing a seeded summary checks persisted display. For complete scenarios also make an unchanged follow-up (or re-enter exact inputs) to verify a newly computed result. Yusuf is deliberately incomplete; UI prevents unanswered questionnaire Next.

**Question order:** Scenario responses Q1-Q8 refer to the original seeded question IDs/order. If questionnaire reference data changes, restore the baseline fixture before using exact scenario expected results.

**Display priority:** The user confirmed Windows 10 on the Mac uses 200%. Test that first, then all Windows 10/11 x 100/125/150/200 combinations on Display matrix. Different resolutions can still produce different physical sizes.

**Priority meaning:** P0: core correctness/data/privacy/display for demonstration; P1: important boundaries/robustness/usability; P2: extended accessibility/load/longevity checks. All failures still need review.

**Failure logging:** On Failure log a short title, exact steps, expected/actual result, app version, OS, display scale/resolution, test DB, evidence and whether repeatable. No real learner/password data in shared screenshots.

**DPI failure example:** Title: Sidebar wraps at 200%; Steps: Windows 10 scale 200%, open 1.0.1 and maximise; Expected: FUTURE and PATH each fit one line; Actual: attach observed screenshot; record resolution/VM mode.

**Test environment record:** Record app/commit version, Windows edition/build, machine/VM, screen resolution, scale, test DB name, date and tester before starting.

**Source validation:** Read docs/Validation.md in the project for the exact scope of prior checks. This checklist adds detailed manual/proposed advanced coverage beyond the existing TC01-TC37 document.

## Coverage

| Area | Cases |
|---|---:|
| Startup and setup | 11 |
| Display, keyboard and accessibility | 13 |
| Learner profile | 10 |
| Subjects and marks | 12 |
| Interest questionnaire | 8 |
| Faculty and programme selection | 6 |
| Recommendation rules | 16 |
| Required and additional sample scenarios | 7 |
| Navigation and saving | 9 |
| Authentication and staff accounts | 12 |
| Roles and learner privacy | 7 |
| Administrator reference maintenance | 14 |
| Funding guidance | 8 |
| Staff search and reports | 10 |
| Export and printing | 10 |
| Database integrity and recovery | 9 |
| Responsiveness and final regression | 5 |

## Startup and setup

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP001 | P0 / Manual | **Open the normal solution** — Open FuturePath.sln; choose Debug > Start Debugging. | FuturePath.Desktop launches a graphical Future Path window. No console test runner is the startup app. |
| FP002 | P0 / Manual | **Clean restore and build** — On a clean Windows extraction with prerequisites installed, allow NuGet restore and build Release. | All three app projects restore/build successfully. No missing source/resources or machine-specific paths. |
| FP003 | P0 / Manual / advanced setup | **First-run setup** — Launch against a new, separate test database and use Prepare demonstration database. | The same app window shows setup and then the overview. The named database/schema/demo data are created. |
| FP004 | P0 / Manual | **Existing installation** — Close and reopen against an already configured database. | Overview opens; existing records and staff passwords are preserved; no reset/reseed. |
| FP005 | P0 / Existing automated + manual | **Repeat demo setup** — On a disposable test DB, edit a fictional learner/password, then repeat explicit setup. | Existing changes and row counts are preserved; demo seed skips all inserts into a nonempty database. |
| FP006 | P1 / Manual / VM | **Missing LocalDB** — Use a clean VM without LocalDB, or the controlled connection-failure test environment. | A visible, actionable setup error appears; app does not silently reset data or disappear. |
| FP007 | P0 / Existing automated | **Unrelated database** — Use a disposable database containing a dummy unrelated table and no SchemaVersion; request setup. | Setup refuses it. The unrelated table/data are unchanged. Do not use a real unrelated database. |
| FP008 | P1 / Advanced / developer | **Unsupported schema version** — On a separate fixture DB only, simulate an unsupported SchemaVersion and start/setup. | Clear unsupported-version failure; no silent downgrade, reseed or modification of unknown data. |
| FP009 | P1 / Existing automated + manual | **Wrong database argument** — Test --database with a valid separate name, then test a .db path, master and an invalid SQL-like name. | Valid separate name is used; file paths/system/invalid names are rejected without changing another database. |
| FP010 | P1 / Manual | **Second instance** — Launch the app twice for the same database in the same Windows login session. | Second instance reports that the app is already open; first instance remains usable. |
| FP011 | P0 / Manual | **Offline startup and use** — After restore/build/setup, disconnect the network and run the built Windows app. | Profile, advising, saving, staff lookup and local export work without online AI/API/network services. |

Preconditions: Windows; extract the complete version 1.0.1 ZIP. Use a separate fictional demo database for setup/change/failure tests.

## Display, keyboard and accessibility

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP012 | P0 / Manual | **Windows 10 at 200%** — Keep Windows 10 scale at 200%; close/reopen the corrected app and visit every page. | FUTURE/PATH remains two lines; sidebar words/buttons are readable; headers/footer actions remain accessible. |
| FP013 | P0 / Manual | **Full DPI matrix** — Run the Display matrix sheet: Windows 10/11 at 100%,125%,150%,200%; restart after changing scale. | No clipped text or inaccessible actions. Different physical sizes are acceptable; proportions remain usable. |
| FP014 | P0 / Existing automated + manual | **Resize overview** — Resize through narrow, normal and wide windows; also run the 960/1280/1600 logical-pixel automated cases. | Cards fit the overview viewport without unwanted horizontal scrolling; sidebar content remains readable. |
| FP015 | P1 / Manual | **Maximise and restore** — Maximise, restore, minimise and reopen from taskbar. | Layout recalculates correctly; working data is retained; form can be brought back into view. |
| FP016 | P1 / Manual | **Small display** — Use the smallest demonstration resolution, including 1366x768 where available. | Essential controls remain reachable by layout/scrolling; large data grids may need deliberate scrolling. |
| FP017 | P1 / Manual | **Move across monitors** — With different display scales on two monitors, move the app back and forth. | Sidebar, header, footer and page content rescale coherently; controls do not repeatedly grow/shrink. |
| FP018 | P0 / Manual | **Pages opened after launch** — At 200%, open a new session, all steps, sign-in, staff pages, editors and reports after launch. | Dynamically created pages scale as well as the overview; text/input controls remain aligned. |
| FP019 | P1 / Manual | **Repeated navigation and scaling** — Go Back/Next repeatedly, open/close reports, resize and revisit pages. | No cumulative scaling or spacing drift; entered values remain unchanged. |
| FP020 | P1 / Manual | **Long labels and names** — Use long fictional names, degree names, explanations and funding text within allowed limits. | Text wraps or scrolls; it does not cover buttons or hide required information. |
| FP021 | P1 / Manual | **Keyboard-only use** — Complete a session with Tab/Shift+Tab, arrows, Space and Enter where supported. | All essential inputs/actions can be reached in a logical order with visible focus. |
| FP022 | P1 / Manual | **Dialogs and cancellation** — Open login, export, password and print dialogs; use Cancel/Escape where supported. | Dialogs stay on-screen; cancellation returns to the same workspace without unintended changes. |
| FP023 | P2 / Manual | **Contrast and screen reader** — Inspect focus/high-contrast readability and use a screen reader on labelled inputs if available. | Fields have understandable names, focus is visible and content remains readable. Record limitations as defects. |
| FP024 | P1 / Manual / VM | **Virtual machine display changes** — If running Windows in a VM, resize its display/window, change full-screen mode, then revisit the app. | App remains accessible after guest resolution/DPI changes; no lost or off-screen dialog. |

Preconditions: Run on Windows 10 on the Mac and on Windows 11. Record physical resolution and display scale.

## Learner profile

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP025 | P0 / Existing automated + manual | **Anonymous learner** — Leave optional personal/contact fields blank; choose Grade 11 or 12 and proceed. | Profile is accepted without requiring an account or optional personal data. |
| FP026 | P0 / Manual | **Both supported grades** — Complete separate sessions for Grade 11 and Grade 12. | Both accepted; correct grade saved/reported; Grade 11 guidance is provisional. |
| FP027 | P1 / Existing automated / controlled UI | **Invalid grade** — Try blank/10/13 via the service test when the dropdown prevents direct entry. | Invalid grade is rejected before saving; UI only offers valid choices. |
| FP028 | P1 / Existing automated + manual | **Valid contact** — Enter learner@example.invalid and +27 71 123 4567. | Accepted and retained correctly. |
| FP029 | P1 / Existing automated + manual | **Invalid email** — Try bad email, x@ and display-name email syntax instead of a plain address. | Clear email validation; stay on current step; no premature save. |
| FP030 | P1 / Manual + service boundary checks | **Phone characters and lengths** — Try a valid 7/20-character phone; try 6/21 characters and letters. | Allowed-length/character values accepted; invalid values rejected or prevented by control. |
| FP031 | P1 / Manual + service boundary checks | **Maximum name/school length** — Try 120 characters, then paste 121+ into name/school; test service bypass separately. | 120 supported; excess prevented/rejected without silently corrupting stored data. |
| FP032 | P2 / Advanced / developer | **Email maximum length** — Use valid email examples around the 150-character limit; test over-limit service input. | Valid within-limit address accepted; malformed/over-limit value prevented/rejected. |
| FP033 | P1 / Manual | **Whitespace and characters** — Enter leading/trailing spaces, an apostrophe (O'Neil), accented characters and a hyphenated name. | Whitespace trimmed; supported Unicode/punctuation retained safely in DB and reports. |
| FP034 | P2 / Existing automated + manual | **Province selection** — Test all nine configured provinces, blank optional province and an invalid service value. | Configured values/blank accepted; unknown province rejected. |

Preconditions: New advisory session; fictional details only.

## Subjects and marks

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP035 | P0 / Manual | **Add subject** — Add a valid active subject and mark. | Exactly one row appears with the correct name/value. |
| FP036 | P0 / Manual | **Multiple subjects** — Add the complete fictional learner subject list. | Every row remains visible/reachable and retains the correct mark. |
| FP037 | P0 / Existing automated + manual | **Duplicate subject** — Add English twice in the same session. | Duplicate rejected; one row remains; no duplicate saved. |
| FP038 | P0 / Existing automated + manual | **Empty subject list** — Click Next with no subjects. | Validation blocks progression and saving. |
| FP039 | P0 / Existing automated + manual | **Mark endpoints** — Test 0% and 100%. | Both accepted and round-trip unchanged. |
| FP040 | P0 / Existing automated / controlled UI | **Out-of-range marks** — Try -1 and 101; use service tests if numeric control prevents entry. | Control prevents or validation rejects; no invalid stored mark. |
| FP041 | P0 / Existing automated + manual | **Decimal precision** — Enter 72.35 and boundary decimals such as 59.99. | Supported two-decimal values remain accurate through save/reopen/report. |
| FP042 | P1 / Manual | **Non-numeric paste** — Paste letters, an empty string or unusual numeric text into the mark control. | Control prevents invalid input or gives a clear validation response; no crash. |
| FP043 | P1 / Manual | **Update selected subject** — Select one row and update its mark. | Only the intended row changes; no duplicate or unrelated edit. |
| FP044 | P1 / Manual | **Remove and cancel removal** — Select a row; cancel removal, then confirm removal. | Cancel keeps the row; confirmed removal deletes only that unsaved row. |
| FP045 | P1 / Existing automated + manual | **Missing/inactive subject ID** — Pass an unknown/inactive subject via an isolated service test. | Rejected before persistence; learners cannot select archived subjects. |
| FP046 | P0 / Existing automated + manual | **Subject completeness** — Leave completeness unticked, finish the other steps and assess; then repeat with confirmed complete capture. | Unconfirmed capture gives More Information Needed; confirmed absence of a mandatory subject gives Not Currently Eligible. |

Preconditions: New advisory session at the subjects step.

## Interest questionnaire

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP047 | P0 / Manual | **All eight questions** — Check the seeded questionnaire and answer all eight. | Eight expected active questions shown with response choices 1 through 5. |
| FP048 | P0 / Existing automated + manual | **Missing response** — Leave one response unselected and click Next. | Remain on the interest step with a clear message; no save. |
| FP049 | P0 / Existing automated + manual | **Back/Next retention** — Answer some/all questions, go Back, then return. | Selected responses are retained. |
| FP050 | P0 / Existing automated + manual | **Response mapping** — Verify 1,2,3,4,5 map to 0%,25%,50%,75%,100%; use all-equal response sets. | Correct category/interest values; no use of raw 1..5 as percentages. |
| FP051 | P0 / Existing automated + manual | **Low interest** — Meet academic requirements but choose all responses 1. | Possible Match due to low interest, not Strong Match. |
| FP052 | P1 / Existing automated | **Invalid/duplicate responses** — Via service tests pass scores 0/6, an unknown question ID and two answers for one question. | All invalid forms rejected before persistence. |
| FP053 | P0 / Existing automated + manual | **Imported incomplete input** — Use the incomplete seeded Yusuf session or an isolated service input with missing responses. | More Information Needed with explanation; incomplete alternatives do not falsely appear eligible. |
| FP054 | P1 / Advanced / developer | **No configured questionnaire** — Use a separate controlled fixture with no active questions. | UI cannot proceed as if complete; engine reports missing configuration without crashing. |

Preconditions: Reach the interest step with valid profile and marks.

## Faculty and programme selection

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP055 | P0 / Manual | **Faculty lookup** — Open the faculty list. | Only active faculties offered. |
| FP056 | P0 / Manual | **Degree filtering** — Switch faculties and inspect the degree list. | Appropriate active degrees appear; stale degree/requirement selection is cleared/refreshed. |
| FP057 | P0 / Manual | **Requirements preview** — Select several degrees, including Engineering and an OR-requirement programme. | Displayed subjects, minima and optional/OR information match stored rules. |
| FP058 | P1 / Existing automated + manual | **Blank/inactive selection** — Try assessment without a valid selection and via a service test with an inactive degree. | Clear validation; invalid programme is not assessed or saved. |
| FP059 | P1 / Manual / admin fixture | **Empty faculty** — Use a test faculty with no active degrees. | No crash/stale degree; assessment requires a valid available choice. |
| FP060 | P1 / Manual | **Funding choice retention** — Toggle funding opt-in, move Back/Next and change programme. | Chosen funding preference retained and final results use the final selected programme. |

Preconditions: Reference data initially unchanged.

## Recommendation rules

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP061 | P0 / Existing automated + manual follow-ups | **Maths minimum boundary** — Using Gadija-like inputs and Maths minimum 60, test marks 60,59,55,54. | Respectively Strong Match, Possible Match, Possible Match, Not Currently Eligible; other requirements/interest must pass. |
| FP062 | P1 / Advanced / developer | **Decimal gap boundary** — For a 60 minimum, test 59.99,55.00,54.99. | Gaps 0.01 and 5.00 are Possible Match; gap 5.01 is Not Currently Eligible. |
| FP063 | P1 / Advanced / developer | **Interest threshold boundary** — Construct weighted-interest scores immediately below 60, exactly 60 and above 60, with all academics met. | Below 60: Possible Match; 60 or above: Strong Match. Weighted fixtures are needed for exact values. |
| FP064 | P0 / Existing automated + manual | **Confirmed missing mandatory subject** — Confirm the complete list while omitting a required subject. | Not Currently Eligible; missing subject named in reasons. |
| FP065 | P0 / Existing automated / advanced combinations | **Missing-information priority** — Combine incomplete questionnaire/capture with an academic failure. | More Information Needed takes priority and explains missing information. |
| FP066 | P0 / Existing automated + manual | **OR group: either subject** — For BCom General, supply English >=55 and Maths OR Mathematical Literacy >=60; test each accepted option separately. | Either qualifying OR option satisfies the group; both are not required. |
| FP067 | P1 / Advanced / developer | **OR group: neither passes** — Test both OR subjects absent, both below threshold and one qualifying alternative. | Confirmed absent group fails; gaps evaluated using the best available qualifying option; one passing option satisfies it. |
| FP068 | P1 / Advanced / developer | **Optional requirement** — Omit or fall below an optional subject while meeting mandatory rules and strong interest. | Helpful-subject guidance appears; optional absence alone does not cause ineligibility. |
| FP069 | P1 / Existing automated + advanced boundaries | **Overall average rule** — Configure a test average minimum; test equality, <=5-point shortfall, >5 shortfall and minimum 0. | Equality passes; small gap Possible; large gap Not Eligible; 0 disables average gate. All captured subjects included. |
| FP070 | P1 / Existing automated / advanced combinations | **Incomplete programme configuration** — Evaluate a fixture missing mandatory groups, interest mappings or mapped active questions. | More Information Needed with a configuration explanation; no divide-by-zero crash or invented Strong Match. |
| FP071 | P0 / Existing automated + manual | **Weighted formula** — Recalculate 35% subject coverage +35% mark match +25% interest +5% faculty preference using controlled inputs. | Weights total 100%; displayed scores agree within documented rounding; Gadija reference overall 98.44 and interest 93.75. |
| FP072 | P0 / Existing automated | **Hard rules override ranking** — Give strong interests/high other scores while a mandatory subject has a >5-point gap. | Not Currently Eligible despite a high overall weighted score. |
| FP073 | P1 / Advanced / developer | **Score limits and rounding** — Check all-low/all-high marks/answers, 0-minimum fixture and fractional averages. | No negative/>100 component scores, NaN/infinity or inconsistent two-decimal display. |
| FP074 | P0 / Existing automated + manual | **Alternative eligibility** — Assess an ineligible programme using complete data. | Alternatives use the same rules; only Strong/Possible matches; no selected, inactive or duplicate programme. |
| FP075 | P1 / Existing automated + advanced ties | **Alternative order and count** — Use fixtures with many candidates and tied outcomes/scores. | At most five; Strong before Possible; descending score; stable ordinal name then ID tie-break. |
| FP076 | P0 / Existing automated + manual | **Historical stability** — Save a session; change a reference threshold/weight; review the old result and create a follow-up. | Old scores/reasons/rule version remain stored; new assessment uses the new rules. |

Preconditions: Use unchanged seeded rules and complete interest/capture data except where a case says otherwise. Advanced cases use an isolated engine/service fixture.

## Required and additional sample scenarios

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP077 | P0 / Existing automated + manual | **Gadija Jacobs** — Search the seeded learner; inspect summary. For a complete scenario, create an unchanged follow-up to check a newly computed outcome as well. | BCom Information Systems: Strong Match. Requirements met; reference interest 93.75%, overall 98.44%. |
| FP078 | P0 / Existing automated + manual | **Aadiel-Raboo Magwa** — Search the seeded learner; inspect summary. For a complete scenario, create an unchanged follow-up to check a newly computed outcome as well. | BSc Computer Science: Not Currently Eligible. Mathematics missing; Mathematical Literacy does not substitute for Computer Science. |
| FP079 | P0 / Existing automated + manual | **Labeeqah Rademeyer** — Search the seeded learner; inspect summary. For a complete scenario, create an unchanged follow-up to check a newly computed outcome as well. | BSc Biological Sciences: Possible Match. Mathematics 58 against 60: exactly a two-point shortfall. |
| FP080 | P0 / Existing automated + manual | **Siyabonga Dickson** — Search the seeded learner; inspect summary. For a complete scenario, create an unchanged follow-up to check a newly computed outcome as well. | BSc Engineering (Mechanical Engineering): Strong Match. Mathematics 75 and Physical Sciences 78 meet 70 minima. |
| FP081 | P0 / Existing automated + manual | **Derek Vin Diesel** — Search the seeded learner; inspect summary. For a complete scenario, create an unchanged follow-up to check a newly computed outcome as well. | Bachelor of Health Sciences: Not Currently Eligible. Both Mathematics and Physical Sciences are missing. |
| FP082 | P1 / Existing automated + manual | **Stef Van Wyk** — Search the seeded learner; inspect summary. For a complete scenario, create an unchanged follow-up to check a newly computed outcome as well. | BSc Computer Science: Possible Match. Academic requirements pass; interest alignment is weak. |
| FP083 | P1 / Existing automated + manual | **Yusuf Patel** — Search the seeded learner; inspect summary. For a complete scenario, create an unchanged follow-up to check a newly computed outcome as well. | BSc Biological Sciences: More Information Needed. Incomplete subject confirmation/questionnaire; no eligible result should be invented. |

Preconditions: Fresh/unchanged seed rules; advisor signed in. Use a date range covering 15-24 September 2026.

## Navigation and saving

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP084 | P0 / Existing automated + manual | **Full learner journey** — Complete overview > profile > subjects > interests > programme > outcome > summary. | Correct step progress; one main window; all entered data appears correctly in the saved result. |
| FP085 | P0 / Existing automated + manual/DB | **No early database writes** — Enter data and use Next/Back before eligibility/save; inspect counts with a read-only query/test. | No learner/session/input/result/audit creation until explicit assessment save. |
| FP086 | P0 / Existing automated + manual | **Back keeps input** — Edit profile, marks, responses, programme and funding preference; navigate back/forward. | All values remain unchanged unless explicitly edited; controls are not rebuilt into blank defaults. |
| FP087 | P0 / Manual | **Invalid Next stays put** — At each validated step, provide invalid or missing data and click Next. | Useful validation, same step remains visible, values retained, no partial save. |
| FP088 | P1 / Manual | **Leave unsaved work** — Navigate to Overview or close the app with unsaved entries; first cancel, then confirm. | Cancel stays with data; confirmation leaves without accidentally saving an incomplete session. |
| FP089 | P0 / Manual + DB check | **Single deliberate save** — Record counts, click Check eligibility & save once, then attempt rapid repeated clicks. | Exactly one intended session is saved; no duplicate from repeated activation; saved state is read-only. |
| FP090 | P0 / Existing automated + manual | **Summary Back/Finish** — Open summary, return Back, reopen it, then Finish. | Summary stays in the main window; Back returns to saved outcome; Finish returns to overview. |
| FP091 | P0 / Existing automated + manual | **Follow-up preserves identity** — Advisor opens an existing learner and creates a follow-up; changes marks, then saves. | New session linked to the same learner; profile identity retained; original session/results unchanged. |
| FP092 | P0 / Existing automated + manual | **Restart and staff retrieval** — Save and close; restart; sign in as advisor and find the saved session. | Inputs, category scores, outcome, alternatives and reasons survive restart. |

Preconditions: Use a new fictional learner; note learner/session counts before the case.

## Authentication and staff accounts

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP093 | P0 / Existing automated + manual | **Advisor login** — Sign in advisor_test / FuturePathAdvisor2026!. | Career Advisor workspace opens; role loaded from DB. |
| FP094 | P0 / Existing automated + manual | **Administrator login** — Sign in admin_test / FuturePathAdmin2026!. | Administrator workspace and administration actions available. |
| FP095 | P0 / Existing automated + manual | **Wrong/unknown/blank credentials** — Try wrong password, unknown username and blank fields. | No access granted; clear generic sign-in failure; password remains masked by default. |
| FP096 | P1 / Manual | **Password case sensitivity** — Change only the case of a correct password. | Wrong-case password denied. |
| FP097 | P0 / Existing automated + manual | **Five-attempt lockout** — On a spare test account, submit five incorrect passwords, then the correct password immediately. | Account locked for five minutes; immediate correct-password login still denied. |
| FP098 | P1 / Existing automated + manual | **Lockout recovery** — Wait five minutes or use the automated fake clock/admin reset on the spare account, then sign in correctly. | Access restored as intended; failed-attempt state resets after successful login/reset. |
| FP099 | P0 / Existing automated + manual | **Inactive account** — Admin disables a spare account; try to sign in with it. | Inactive account cannot authenticate. |
| FP100 | P2 / Manual | **Show/hide password** — Toggle show password on the login dialog, then cancel/return. | Visibility follows the toggle; cancellation does not authenticate or change password. |
| FP101 | P0 / Existing automated + manual | **Change password** — Use correct current password and a valid new matching pair; log out and test old/new passwords. | New password works; old password fails. |
| FP102 | P1 / Existing automated + manual/service boundaries | **Password validation** — Try wrong current password, mismatched confirmations, <12/>128 characters, letters-only and digits-only. | Rejected/prevented; valid 12-128-character password with letter+digit accepted; old password kept on failure. |
| FP103 | P0 / Existing automated | **Salted password storage** — Use the security test to hash the same password twice and verify each hash; inspect staff listing. | Different salted hashes verify correctly; no plaintext password/hash shown in normal account listings or audit. |
| FP104 | P1 / Existing automated | **Injection-style login** — Try a username such as apostrophe OR 1=1 comment syntax with an arbitrary password. | No bypass or SQL error; unrelated records unchanged. |

Preconditions: Use fictional demo credentials; use a separate test staff account for lockout/password/destructive-account cases.

## Roles and learner privacy

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP105 | P0 / Existing automated + manual | **Guest menu** — Start a fresh guest workspace. | Guest can create an assessment but cannot browse other learners, reports or administration. |
| FP106 | P0 / Existing automated + manual | **Guest own summary** — Save a guest assessment and open/export that session summary. | Current-workspace session allowed; arbitrary other learner/session ID denied by service. |
| FP107 | P0 / Existing automated + manual | **Guest privacy after restart** — Close/restart after a guest assessment; try to retrieve it without staff login. | Prior guest ownership not retained; staff authentication required to retrieve stored sessions. |
| FP108 | P0 / Existing automated + manual | **Advisor permissions** — Use search, summary, follow-up, activity and exception reports as advisor. | Allowed staff actions work. |
| FP109 | P0 / Existing automated | **Advisor denied admin** — Try admin-only maintenance, account and backup service calls as advisor. | Denied even through direct service access; hidden menu is not the only protection. |
| FP110 | P0 / Existing automated + manual | **Administrator permissions** — Use authorised reference/account maintenance and backup as administrator. | Allowed operations validate and persist; audit records identify the actor. |
| FP111 | P0 / Manual + service checks | **Sign-out clears workspace** — Open staff learner information, sign out, then inspect the screen and guest access. | Staff data disappears; role/ownership reset; staff-only pages no longer accessible. |

Preconditions: Test separately as guest, Career Advisor and System Administrator.

## Administrator reference maintenance

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP112 | P0 / Existing automated + manual | **Edit/save/cancel** — Open each editor; change a field and Cancel, then repeat and Save. | Cancel leaves DB unchanged; valid Save persists and returns to the list in the same window. |
| FP113 | P1 / Manual | **Unsaved editor navigation** — Change an editor and try to leave; cancel the prompt, then confirm. | Cancel retains editor/data; confirmation discards unsaved changes; no extra main window. |
| FP114 | P1 / Existing automated + manual | **Required/length/duplicate values** — For each reference type, try missing required text, excessive length and duplicate unique names/links. | Validation/constraint errors explained; no duplicate or partial row. |
| FP115 | P0 / Existing automated + manual | **New degree workflow** — Create a degree inactive, add mandatory requirements and interest mappings/questions, then activate. | Incomplete new programme cannot activate; complete valid programme becomes selectable. |
| FP116 | P0 / Existing automated + manual | **Archive a degree** — Deactivate a test degree after saving a session against it. | Excluded from new choices/alternatives; historical session still viewable with original result. |
| FP117 | P1 / Manual + service checks | **Faculty/subject/category changes** — Add/edit fictional records; deactivate a faculty; try deactivating subjects/categories still required by active degrees. | Reference changes persist; inactive faculty degrees disappear from learner choices; unsafe dependency changes are refused. |
| FP118 | P1 / Existing automated + advanced boundaries | **Requirement configuration** — Add independent mandatory, OR-group and optional requirements; test minima 0/100 and invalid service values. | Valid relationships/group semantics persist; out-of-range/fractional group IDs and invalid lookups rejected. |
| FP119 | P1 / Existing automated + advanced boundaries | **Interest weights** — Test 0.01 and 10; try 0 and 10.01 via service when UI restricts them. | Valid positive weights accepted; invalid weights rejected; calculation uses configured weighting. |
| FP120 | P0 / Existing automated + manual | **Last active question dependency** — Try removing/moving the only active question used by an active programme category. | Blocked until affected programmes are deactivated/configuration is safely maintained. |
| FP121 | P1 / Existing automated + manual | **Structural rule changes** — Try removing/moving active programme requirements/mappings; then deactivate the programme and repeat safely. | Unsafe changes are blocked; controlled inactive-programme editing works; new results reflect final rules. |
| FP122 | P0 / Existing automated + manual | **Staff creation** — Create a spare advisor with valid username/password; test duplicate/invalid usernames and blank new password. | Valid account can log in; invalid/duplicate/blank-password creation rejected. |
| FP123 | P0 / Existing automated + manual | **Account reset/deactivate/self-protection** — Reset a spare account password, change its role, disable it; attempt to demote/disable the current admin. | Reset/role changes work on next login; disabled account denied; current admin cannot demote/disable itself. |
| FP124 | P1 / Manual + service checks | **Existing account blank password** — Edit an existing staff account with new-password fields blank, then log in. | Existing password retained; blank does not replace the hash. |
| FP125 | P1 / Existing automated + manual | **Audit review** — Perform login/failure, session save, maintenance, export and backup; inspect audit as administrator. | Expected events/actor/time present; audit is read-only and contains no passwords. |

Preconditions: Separate fictional test DB; sign in as administrator. Preserve a clean baseline backup before changes.

## Funding guidance

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP126 | P0 / Existing automated + manual | **Opt in** — Complete a session with funding requested and related open records. | Related active/open funding shown with type, criteria, required documents, date, contact and sample warning. |
| FP127 | P0 / Existing automated + manual | **Opt out** — Complete a session with funding unticked and export its summary. | No funding choices leaked into the opt-out report; states funding was not requested. |
| FP128 | P1 / Manual / controlled fixture | **Past/today/future closing dates** — Create related active fictional awards closing yesterday, today and tomorrow. | Yesterday excluded; today and tomorrow included under the documented inclusive closing-date rule. |
| FP129 | P0 / Existing automated + manual | **Inactive/unrelated awards** — Deactivate one related award and create an unrelated faculty/programme award. | Inactive and unrelated funding excluded. |
| FP130 | P1 / Advanced fixture + manual | **Faculty-wide funding** — Link an award to a faculty with no programme-specific links. | Available to the appropriate programmes in that faculty. |
| FP131 | P1 / Advanced fixture + manual | **Programme-specific funding** — Add programme links to a test award that also has a faculty link. | Programme links restrict the award; it does not leak to every programme in the faculty. |
| FP132 | P1 / Existing automated + manual | **No open matches** — Use a programme with no open awards or the automated clock advanced beyond all deadlines. | Helpful no-open-funding message; no crash or invented award. |
| FP133 | P1 / Manual + service checks | **Funding maintenance validity** — Create/edit each allowed funding type, dates and links; try invalid type/date/foreign key via service tests. | Valid data persists; invalid controlled values rejected; no duplicate link. |

Preconditions: Use test funding records with controlled active flags, links and dates; do not change the Windows system clock.

## Staff search and reports

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP134 | P0 / Existing automated + manual/DB | **Seed baseline counts** — Inspect Database/04_VerifyDatabase.sql results or read-only DB queries on a fresh demo. | 10 learners,18 subjects,5 faculties,10 degrees,8 categories,8 questions,10 funding options,10 sessions; 20 domain tables + SchemaVersion. |
| FP135 | P0 / Manual | **Search variants** — Search learner name, programme, session ID, blank term and leading/trailing spaces. | Expected matching records only; blank returns date-filtered results; trim handled. |
| FP136 | P0 / Existing automated + manual | **Date range and endpoints** — Choose 15-24 September 2026 for sample sessions; test each endpoint and reversed dates. | Samples in range included; UTC dates inclusive; reversed dates rejected clearly. |
| FP137 | P1 / Existing automated + manual | **No matches** — Use a search/date range with no records. | Clear empty result/report; no crash, stale learner or fabricated totals. |
| FP138 | P0 / Existing automated + manual | **Learner advisory summary** — Review profile, grade, subject marks, category scores, selected outcome, reasons, alternatives, funding and disclaimer. | All sections match the saved session; dates/identity accurate; Grade 11 provisional notice. |
| FP139 | P0 / Existing automated + manual | **Activity report totals** — Generate Degree Interest and Eligibility for all untouched demo sessions. | 10 sessions and 10 distinct learners initially; category/faculty/degree/outcome/funding counts reconcile with stored sessions. |
| FP140 | P0 / Existing automated + manual | **Exception report membership** — Generate Learners Not Currently Eligible with a range covering all seed sessions. | Includes Aadiel-Raboo, Derek and incomplete Yusuf; excludes Strong Match Gadija; Possible Match-only cases not included. |
| FP141 | P0 / Existing automated + manual | **Follow-up counting** — Create one follow-up for an existing learner; regenerate reports. | Session count increases by 1; distinct learner count does not; both historical/new sessions visible. |
| FP142 | P0 / Existing automated + manual | **Stored-history report** — Change a test admission rule after saving; generate old summary/activity/exception outputs. | Persisted historic outcomes/reasons/scores are not silently recalculated using the edited rule. |
| FP143 | P0 / Existing automated + manual | **Report permissions and filters** — Apply staff search/date filters and try aggregate report creation without staff access. | Filters agree with displayed data; guests cannot generate aggregate staff reports. |

Preconditions: Advisor/admin. For initial baseline totals only, use a fresh seed before creating follow-ups or new learners.

## Export and printing

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP144 | P0 / Existing automated + manual | **Text export** — Export .txt, reopen and compare against displayed report. | UTF-8 content complete, readable and consistent with the saved assessment. |
| FP145 | P0 / Existing automated + manual | **HTML export** — Export .html and open it in a browser. | Readable formatted report with correct content and advisory/sample notices. |
| FP146 | P1 / Existing automated + manual | **HTML special characters** — Use a fictional name containing <script>alert(1)</script>, ampersands, quotes or an apostrophe; export HTML. | Text is escaped/rendered literally; no executable script or broken HTML. |
| FP147 | P0 / Manual | **Export each report type** — Export learner summary, activity report and exception report to both supported file formats. | All three types export their own correct content; no stale/mixed report. |
| FP148 | P1 / Manual | **Cancel export** — Cancel the save dialog. | No file or new export metadata created; current report remains usable. |
| FP149 | P1 / Manual | **Overwrite choice** — Choose an existing test export; cancel overwrite, then intentionally confirm an update. | Cancel preserves the file; confirmed successful overwrite produces the complete new report. |
| FP150 | P0 / Existing automated + manual | **Failed export** — Use a controlled unwritable/nonexistent destination or injected file failure. | Clear error, old reports not truncated, no successful-export metadata for a failed write; retry possible. |
| FP151 | P1 / Existing automated + DB | **Successful export metadata** — Export a learner summary and inspect ExportReport/audit in a test DB. | Correct session/path/time metadata only after successful write; aggregate exports audited without inventing summary metadata. |
| FP152 | P0 / Manual | **Print to PDF** — Use Print / PDF and Microsoft Print to PDF; save and open the PDF. | Readable, complete pages, correct content, margins and page numbering; no cut-off final text. |
| FP153 | P1 / Manual | **Print cancellation/failure/long report** — Cancel printing, use no-printer/failure condition where possible and print a long multi-page report. | Cancellation preserves data; failures explained; long text neither lost nor duplicated across pages. |

Preconditions: Saved session/authorised report; use a writable test output folder and a controlled unwritable destination.

## Database integrity and recovery

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP154 | P0 / Existing automated + DB | **Complete assessment transaction** — Save a valid session and inspect all related tables. | Learner/session, marks, responses, category scores, selected/alternative results and creation audit persist together. |
| FP155 | P0 / Existing automated | **Rollback on failed save** — Use the existing failing multi-table-save test/controlled injected DB fault. | No partial learner/session/input/result/audit rows remain; unsaved identity is not treated as successfully saved. |
| FP156 | P0 / Existing automated | **Keys, duplicates and numeric constraints** — Use isolated SQL tests for nonexistent foreign keys, duplicate junction keys and marks/status outside limits. | Database rejects invalid writes as well as the UI/service layer; DBCC CHECKCONSTRAINTS reports no violations. |
| FP157 | P1 / Existing automated + manual | **Round-trip native values** — Save/reload decimals, dates, UTC timestamps, Unicode and multiline reasons. | Values remain accurate; no locale corruption or malformed line endings. |
| FP158 | P0 / Existing automated + manual | **Backup new file** — As administrator, back up to a new writable .bak file. | Copy-only checksummed backup created and verified; audit records success. |
| FP159 | P0 / Existing automated + manual | **Backup existing/unwritable file** — Choose an existing test .bak or controlled unwritable destination. | Existing backup is not overwritten; failure explained; original database remains intact. |
| FP160 | P0 / Existing automated / advanced | **Restore to a separate database** — Run automated restore or restore the backup into a NEW uniquely named test DB and launch with that name. | Learners/sessions/credentials/history match the backup; source DB untouched. The UI does not provide a restore wizard. |
| FP161 | P1 / Advanced / VM | **Connection interrupted** — In a disposable fixture, interrupt SQL availability during read/save, then restore it and retry. | Controlled error; failed saves do not leave partial data; saved history survives; retry/restart works. |
| FP162 | P1 / Advanced / VM | **Process interrupted and restarted** — On disposable data, terminate the app during an unsaved workflow and separately exercise controlled in-flight save interruption. | Unsaved capture is not falsely saved; committed sessions survive; interrupted transaction is all-or-nothing. |

Preconditions: Run these through the existing isolated LocalDB integration tests or a disposable developer fixture, never by corrupting the working demo.

## Responsiveness and final regression

| ID | Priority / execution | Test and steps | Expected result |
|---|---|---|---|
| FP163 | P1 / Manual | **Repeated workflow cycles** — Create/view/finish sessions and open/close staff reports/editors repeatedly (for example 30 cycles). | No accumulating extra windows, disposed-control exceptions, retained previous learner details or obvious progressive slowdown. |
| FP164 | P2 / Advanced / developer | **Larger dataset** — On a disposable test DB, load 100/1000 fictional sessions and search/generate/export reports. | Correct totals and usable response times; no freeze or missing results. Record actual elapsed time and agree an acceptable target. |
| FP165 | P2 / Manual | **Long-running session** — Leave the app open, minimise/restore and continue later; test sleep/wake if applicable. | No lost committed data or unusable state; temporary connection failures handled coherently. |
| FP166 | P0 / Manual + existing automated | **Post-change full regression** — After changing a rule/reference record or applying a fix, rerun the five named scenarios, full learner journey, roles, reports/export and DPI priority cases. | Previously working behaviour remains correct; failures logged and retested after repair. |
| FP167 | P0 / Manual | **Clean second-machine handoff** — Extract on the other Windows machine/account with prerequisites and prepare its separate demo DB if needed. | Source builds and app runs without paths/dependencies from the first machine. LocalDB data is not assumed to synchronise automatically. |

Preconditions: Fictional test data and consistent machine specifications; record timings rather than assuming an undocumented performance limit.

## Exact reference inputs

Question numbers below refer to original seeded question IDs. Keep original rules unchanged when expecting these results.

**Gadija Jacobs** — Grade 12; BCom Information Systems; expected **Strong Match**. Requirements met; reference interest 93.75%, overall 98.44%.
- Marks: English: 72; Mathematics: 70; Accounting: 74; Business Studies: 79; Economics: 70; Life Orientation: 80
- Responses: Q1=4; Q2=3; Q3=5; Q4=2; Q5=3; Q6=5; Q7=3; Q8=2
- Subjects complete: True

**Aadiel-Raboo Magwa** — Grade 12; BSc Computer Science; expected **Not Currently Eligible**. Mathematics missing; Mathematical Literacy does not substitute for Computer Science.
- Marks: English: 65; Mathematical Literacy: 78; Business Studies: 71; History: 69; Tourism: 75; Life Orientation: 82
- Responses: Q1=4; Q2=2; Q3=5; Q4=1; Q5=3; Q6=3; Q7=4; Q8=2
- Subjects complete: True

**Labeeqah Rademeyer** — Grade 11; BSc Biological Sciences; expected **Possible Match**. Mathematics 58 against 60: exactly a two-point shortfall.
- Marks: English: 80; Mathematics: 58; Physical Sciences: 62; Life Sciences: 70; Geography: 66; Life Orientation: 85
- Responses: Q1=3; Q2=4; Q3=2; Q4=5; Q5=4; Q6=1; Q7=2; Q8=3
- Subjects complete: True

**Siyabonga Dickson** — Grade 12; BSc Engineering (Mechanical Engineering); expected **Strong Match**. Mathematics 75 and Physical Sciences 78 meet 70 minima.
- Marks: English: 70; Mathematics: 75; Physical Sciences: 78; Engineering Graphics and Design: 73; Life Orientation: 80
- Responses: Q1=5; Q2=2; Q3=3; Q4=4; Q5=2; Q6=2; Q7=5; Q8=1
- Subjects complete: True

**Derek Vin Diesel** — Grade 12; Bachelor of Health Sciences; expected **Not Currently Eligible**. Both Mathematics and Physical Sciences are missing.
- Marks: English: 82; Mathematical Literacy: 76; History: 79; Life Sciences: 68; Visual Arts: 85; Life Orientation: 90
- Responses: Q1=2; Q2=5; Q3=1; Q4=3; Q5=4; Q6=2; Q7=3; Q8=3
- Subjects complete: True

**Stef Van Wyk** — Grade 12; BSc Computer Science; expected **Possible Match**. Academic requirements pass; interest alignment is weak.
- Marks: English: 66; Afrikaans: 64; Mathematics: 72; Physical Sciences: 68; Computer Applications Technology: 75; Life Orientation: 78
- Responses: Q1=2; Q2=5; Q3=2; Q4=3; Q5=3; Q6=1; Q7=2; Q8=3
- Subjects complete: True

**Yusuf Patel** — Grade 11; BSc Biological Sciences; expected **More Information Needed**. Incomplete subject confirmation/questionnaire; no eligible result should be invented.
- Marks: English: 63; Life Orientation: 70
- Responses: No responses (incomplete seeded/service scenario; UI Next blocks missing answers)
- Subjects complete: False

