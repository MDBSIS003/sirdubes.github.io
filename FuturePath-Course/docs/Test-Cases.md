# Windows acceptance test cases

The workbook contains these 50 cases, all initially **Not run**, plus exact inputs for ten supplied learners, a Windows 10/11 scaling matrix and a defect log. Actual executed checks are separately recorded in Validation.md. Use a disposable database for failure/maintenance tests.

## M01 — Startup

Steps/data: Extract All; open FuturePath.sln; press F5 on Windows with prerequisites.

Expected: Windows GUI opens; no test console starts.

## M02 — Profile

Steps/data: Leave School blank or spaces.

Expected: Next disabled; school validation error on leaving field.

## M03 — Profile

Steps/data: School supplied, Province unselected.

Expected: Next disabled; province required.

## M04 — Profile

Steps/data: Choose Grade 11; repeat Grade 12.

Expected: Both accepted; Grade 11 outcome is provisional.

## M05 — Profile

Steps/data: School and Province valid; leave name/email/phone blank.

Expected: Next enabled; no account or unnecessary personal details required.

## M06 — Profile

Steps/data: Enter bad email, e.g. x@; then clear it.

Expected: Invalid email blocks Next; blank optional email accepted.

## M07 — Profile

Steps/data: Enter letters in phone; then +27 71 123 4567.

Expected: Invalid contact blocks Next; valid optional contact accepted.

## M08 — Subjects

Steps/data: Add one active subject with 0, then 100, then 72.35.

Expected: Bounds and decimal accepted; displayed marks retain precision.

## M09 — Subjects

Steps/data: Try typing -1 or 101 into mark input.

Expected: Numeric control restricts 0–100; business tests also reject invalid values.

## M10 — Subjects

Steps/data: Add the same subject twice, changing its mark.

Expected: One row remains; mark updated, no duplicate.

## M11 — Subjects

Steps/data: Select a captured subject; remove it; remove the last row.

Expected: Rows removed; Next disabled when none remain.

## M12 — Subjects

Steps/data: Do not confirm complete list; finish an assessment.

Expected: More Information Needed; no eligible alternatives asserted.

## M13 — Interests

Steps/data: Choose one of five radio responses; change it.

Expected: Exactly one response per question, score 1–5.

## M14 — Interests

Steps/data: Leave an answer missing without the incomplete checkbox.

Expected: Next disabled until all required answers are present.

## M15 — Interests

Steps/data: Deliberately continue incomplete; go Back then Next.

Expected: Incomplete path classified More Information Needed; chosen answers retained.

## M16 — Degree

Steps/data: Change selected faculty.

Expected: Degree list contains only degrees in that faculty; requirement text updates.

## M17 — Required sample

Steps/data: Run Gadija sample inputs from Sample inputs sheet.

Expected: BCom Information Systems; Strong Match; interest 93.75%; overall 98.44%.

## M18 — Required sample

Steps/data: Run Aadiel-Raboo sample inputs.

Expected: BSc Computer Science; Not Currently Eligible; Mathematics missing.

## M19 — Required sample

Steps/data: Run Labeeqah sample inputs.

Expected: Biological Sciences; Possible Match; Mathematics 58 versus 60; gap 2.

## M20 — Required sample

Steps/data: Run Siyabonga sample inputs.

Expected: Mechanical Engineering; Strong Match.

## M21 — Required sample

Steps/data: Run Derek sample inputs.

Expected: Health Sciences; Not Currently Eligible; Mathematics and Physical Sciences missing.

## M22 — Rules

Steps/data: Start from Gadija: set Mathematics 60,59,55,54.

Expected: Strong,Possible,Possible,Not Currently Eligible respectively; high total cannot hide gap.

## M23 — Rules

Steps/data: Inspect alternatives for Aadiel and the Mathematical Literacy OR pathway.

Expected: At most five, excludes selected/inactive/ineligible degrees; explicit OR accepted; stable ranking.

## M24 — Funding

Steps/data: Opt out at funding step and save.

Expected: Summary says funding not requested; no funding list is claimed.

## M25 — Funding

Steps/data: Opt in and inspect linked funding fields.

Expected: Name/type/eligibility/documents/closing date/contact shown; sample notice visible.

## M26 — Funding

Steps/data: In a disposable demo, deactivate/expire an opportunity or set a restrictive programme link.

Expected: Inactive/expired/unrelated funding excluded; deadline today inclusive; programme links respected.

## M27 — Navigation

Steps/data: Use Back/Next through all steps, altering draft inputs before save.

Expected: Same parent window; input retained; preview does not add database rows.

## M28 — Persistence

Steps/data: Save and view summary; click Finish; restart and retrieve as advisor.

Expected: One complete new assessment persists; summary includes linked inputs/results.

## M29 — Follow-up

Steps/data: Advisor retrieves a session and creates a follow-up; edit marks and save.

Expected: Same learner, new session ID; original result/reasons unchanged.

## M30 — Navigation

Steps/data: End an unsaved session; test both confirmation answers.

Expected: Cancel retains draft; Yes returns home and clears guest session access.

## M31 — Reports

Steps/data: Before adding cases, report September 2026 demo sessions.

Expected: Ten sessions and ten distinct learners; required seeded outcomes represented.

## M32 — Reports

Steps/data: Filter by faculty, degree and outcome, separately and together.

Expected: Only matching saved rows; labels identify chosen filters; no recomputation of historical outcomes.

## M33 — Reports

Steps/data: Reverse dates; then choose an empty period.

Expected: Clear invalid-range message; empty report says no sessions match.

## M34 — Reports

Steps/data: Generate exception report and inspect common gap sections.

Expected: Only Not Currently Eligible/More Information Needed rows; saved subject/mark gap explanations counted.

## M35 — Export

Steps/data: Export summary as HTML and text; include <script> in optional alias on a test profile.

Expected: Readable report; HTML encodes alias, no active injected script; export metadata only on success.

## M36 — Printing

Steps/data: Print summary and staff report to Microsoft Print to PDF; try cancellation.

Expected: All pages readable and numbered; cancel does not claim a successful export.

## M37 — Security

Steps/data: Guest attempts staff-only actions through normal navigation.

Expected: Staff functions unavailable; business authorization tests also deny direct calls.

## M38 — Security

Steps/data: Sign in as advisor_test with documented password.

Expected: Saved sessions/reports available; administration/account maintenance denied.

## M39 — Security

Steps/data: Sign in as admin_test; sign out.

Expected: Admin functions available only while authenticated; sign-out clears workspace/role.

## M40 — Security

Steps/data: In a disposable test database, enter five wrong passwords; retry; wait five minutes.

Expected: Generic failure; temporary lockout; correct password works after lockout.

## M41 — Security

Steps/data: Change password; verify old/new; try deactivating or demoting the signed-in admin.

Expected: New password used; old rejected; current admin cannot disable/demote itself.

## M42 — Maintenance

Steps/data: Create a degree inactive; add requirements and weights; activate it.

Expected: Cannot activate incomplete configuration; valid complete degree appears for learners.

## M43 — Maintenance

Steps/data: Try duplicate reference names, invalid linked IDs and removing the last mapped question.

Expected: Validation/constraints reject invalid data and protect active programme dependencies.

## M44 — Maintenance

Steps/data: Edit funding, question/category, OR requirement and staff account using their Designer-backed forms.

Expected: Named controls are bound; validated saves visible; deactivation preserves history; password never displayed.

## M45 — Backup

Steps/data: Create new .bak as admin; try existing filename; review latest audit events.

Expected: New backup verified; overwrite refused; actions audited; restore only to a separate test destination.

## M46 — Setup

Steps/data: On a separate demo database run Prepare twice; separately test unknown unversioned database.

Expected: No duplicate seed/reset; unknown database refused without adding schema tables. Never use a live database for destructive tests.

## M47 — Display

Steps/data: Run the ten OS/scale cases in Display matrix; resize/maximise learner and staff forms.

Expected: Titles readable; radio answers wrap; body scrolls; Back/Next visible; no cramped sidebar.

## M48 — Accessibility

Steps/data: Complete capture using Tab, Shift+Tab, arrows and Space/Enter.

Expected: Logical focus order; visible focus; inputs and actions reachable without mouse.

## M49 — Offline/instances

Steps/data: After restore, disconnect internet; run advising; launch second app for same DB.

Expected: Local advising works; duplicate instance refused; different named demo DB remains separate.

## M50 — Designer

Steps/data: Build then open every Presentation form with View Designer, including inherited steps.

Expected: Controls visible/editable; no LocalDB connection required to open Designer; no missing Designer files.

## Run the automated Windows checks

Open FuturePath.Development.sln, build, then Test → Test Explorer → Run All. Install/start LocalDB first. The test helper creates uniquely named FuturePathTest_ databases and removes only those disposable databases; it does not delete the application database. Check skipped/failed results explicitly.

Optional terminal commands from solution root:

```text
dotnet restore FuturePath.Development.sln --locked-mode
dotnet build FuturePath.Development.sln -c Release --no-restore
dotnet test tests/FuturePath.Tests -c Release --no-build
dotnet test tests/FuturePath.Desktop.Tests -c Release --no-build
```

Windows tests cannot verify the physical appearance at every monitor/DPI through compilation alone. Complete the display matrix and actual printing/PDF checks before presenting.
