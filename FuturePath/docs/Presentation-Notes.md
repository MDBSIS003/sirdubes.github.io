# Presentation notes and code walkthrough

## Suggested demonstration sequence (approximately 20 minutes plus questions)

1. **Context (1 minute):** standalone advising tool, local SQL data and rules; guidance rather than admission or funding guarantees.
2. **Learner journey (5 minutes):** register/login, update profile, select degree/grade, capture marks, confirm full subject list, save questionnaire, assess, inspect alternatives, optional funding, export.
3. **Specified examples (3 minutes):** contrast Gadija's Strong Match, Aadiel-Raboo's missing Mathematics and Labeeqah's 2-point Mathematics gap. Mention separate academic eligibility.
4. **Advisor (3 minutes):** explain assignment-based privacy, find assigned learner, inspect marks/results, save a guidance note and export summary.
5. **Administrator (3 minutes):** edit a sample programme requirement, manage funding and links, deactivate an outdated record, show role/password management without exposing hashes.
6. **Reports (2 minutes):** export eligibility activity and exception reports; show missing-information case9.
7. **Implementation/testing (3 minutes):** explain layers, run console tests, point to Windows acceptance evidence. Every member should be able to explain the paths below.

## Major classes to explain

- `Program`: Windows Forms initialization and Welcome entry point.
- `frmWelcome`: owns the modal page loop. Each `FPPage.NextPage` changes route; navigation does not log the user out or leave a modeless orphan behind.
- `BrandedForm`, `Theme`, `RoleCard`: original reusable branding, inherited layout and composed interactive controls.
- `FPPage`: shared sidebar, consistent cards, error handling, export dialog and role entry checks. Derived forms provide workflow-specific controls.
- `AppSession` and `AccessPolicy`: authenticated identity and fast entry/ownership checks. `SqlStore.Actor` rechecks active account/role in SQL for backend operations.
- `PasswordHasher`: PBKDF2 hash format contains algorithm/version, iteration count, salt and derived key. Different salts mean identical passwords do not have identical stored hashes.
- `UserAccountDAL`: safe registration, hash verification, profile update by existing user ID, password change. It does not insert a new account when saving a profile.
- `AdvisingRecord`: composes one session's marks/responses and selected degree. Session IDs join normalised tables.
- `AdvisingWorkflow`: coordinates a saved learner session or an in-memory guest session. Completing a saved session stores all result rows in a transaction and freezes the inputs.
- `RecommendationService`: pure local rule/scoring engine; no form controls, internet, SQL or AI calls. Explain gates, explicit alternative groups and weighted category alignment.
- `AdvisingDAL`: parameterised session/mark/response/funding/advisor operations. Multirow replacement uses transactions; invalid foreign keys/duplicates fail without partial data.
- `AdminDAL` / `ManagementForm`: fixed metadata defines allowed tables and typed fields. Values are parameters; the user cannot supply a table/column identifier. Dropdowns handle FK references. Deactivate historical entities; enforce deletion restrictions on lookups.
- `ReportDAL` / `ReportService`: SQL aggregate/exception projections plus an explanatory learner summary. Advisor SQL always filters to assignments.
- `FuturePath.Tests`: simple console harness testing the real source files, exact sample fixtures and T-SQL grammar. `--database` exercises real LocalDB in its own temporary database on Windows.

## Algorithm example

For a degree requiring English60 and Maths70, a learner with English70, Maths68, maximum mapped interests and matching faculty has:

- Subject score100 (both subjects present).
- Mark score `(100 + 68/70 × 100)/2 = 98.5714`.
- Interest100, faculty100.
- Overall `30 + 29.5714 + 30 + 10 = 99.57`.

Despite the high ranking score, Mathematics is below its stored minimum. Academic status remains **Not eligible**, and the advisory outcome is **Possible Match**, explaining the 2-point gap. Scores rank guidance; they do not override admission gates. The test suite asserts this behavior explicitly.

If Maths is absent from a confirmed complete subject list, the outcome is **Not Currently Eligible**. If the list has not been confirmed or questionnaire answers are missing, it is **More Information Needed**. Mathematical Literacy counts only where an explicit alternative group in the database permits it.

## Likely lecturer questions

**Where is inheritance?** BrandedForm and FPPage provide shared form behavior; each derived screen is displayed through the base Form/FPPage API.

**Why store the grade on both profile/session?** The profile stores current details; a session records the grade used for that assessment. Old results should not change just because the learner advances a grade.

**How are alternatives selected?** Evaluate every active programme with the same marks/interests; rank academic eligibility first and weighted score second; mark the selected degree separately.

**Why not use one giant table?** Marks, responses, requirements, mappings and funding links have separate rows and FK relationships. A comma-separated subject list would lose integrity and make queries difficult.

**How are passwords protected?** Salted slow PBKDF2 with constant-time verification; no plaintext fallback, no hardcoded login bypass; local demo credentials are explicitly development-only.

**Can a learner call an admin form directly?** Constructor/entry checks reject the role, and backend DAL independently checks active persisted role before any protected operation.

**Why are advisors restricted?** Assignment is an explicit local design assumption to reduce learner-data exposure. Administrators can change assignments without granting broad browsing rights.

**What has actually been tested?** Show `docs/Phase-Reports.md` and the real command output. Portable tests and cross-compilation ran on Linux. Do not claim LocalDB/UI acceptance unless your group has run and recorded it on Windows.

**What if a bursary name/date is wrong?** Seed entries are clearly fictional samples; contacts are not real application destinations. The admin maintains local data, and every export includes a disclaimer.

## Team preparation

Map work to actual group contributions honestly. Review code together and practise changing a minimum mark, adding a question/mapping, tracing a saved subject result, and explaining one failed test. Do not misrepresent AI assistance or claim unexecuted testing.
