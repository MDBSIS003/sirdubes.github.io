# Phase reports and validation evidence

Work is saved on local branch `integration/futurepath-complete`. The ZIP had no Git metadata, so its contents were captured in a baseline commit before integration; no teammate branch was reset and no remote push occurred. The website repository is unchanged.

## Phase 1 — inspect and compile

Changed: identified actual `.slnx` and startup project, all active/legacy forms, duplicate handoff modules, DAL/security/model files and both SQL script trees. Documented the original architecture in `01-Inspection.md`. Fixed the `allowed` scope conflict (CS0136), enabled Windows cross-target compilation and excluded obsolete screens/insert-only profile DAL while retaining their source history. Installed the .NET10 SDK for validation.

Tested: full combined solution build after integration; final output has zero errors/warnings. Initial new-code compiler findings were corrected before the passing build. No claim is made that the original ZIP compiled unchanged.

Remaining blocker: Windows Forms cannot be executed on this Linux host. Designer/interactive rendering requires Windows acceptance tests.

## Phase 2 — authentication, sessions, permissions, navigation

Changed: salted PBKDF2 password storage, hash verification, no plaintext fallback, registration bounds, logout, password change/reset and local demo setup. Added page entry checks plus independently enforced persisted active roles, learner ownership and advisor assignment in protected DAL operations. Replaced modeless sidebar navigation with a welcome-owned modal route loop.

Tested: executed portable password correctness/salts/invalid-format/length tests; guest/staff restriction tests; learner ownership and role separation tests. Compiled the UI routing and account screens.

Remaining blocker: real SQL login, account revocation behavior and interactive role navigation require Windows/LocalDB. Those paths have explicit tests/cases rather than claimed passes.

## Phase 3 — profile, subjects/marks and interests

Changed: guarded existing-account profile update, grade/phone persistence, complete subject-list confirmation, subject add/update/remove, transaction-backed mark replacement, database-driven questionnaire save/reload and required-answer validation. Anonymous guest state stays in memory. Added persisted sessions and loading of saved history; completed sessions are immutable through the learner workflow.

Tested: profile validation, mark 0/100 boundaries and out-of-range rejection, duplicate prevention, required/valid/unique questionnaire responses. Solution compiles.

Remaining blocker: actual SQL read/write/reload/rollback verification needs LocalDB. The integration suite covers it but has not run here.

## Phase 4 — recommendations and funding

Changed: pure local recommendation engine using stored required/alternative subjects and minimum marks, explicit academic status, weighted questionnaire categories/faculty preference, ranked alternatives and gap explanations. Funding searches/filtering use active/nonexpired local data and faculty/programme links. Replaced unverified real-organisation sample labels/contacts with explicit fictional sample labels. Generated the empty-database sample recommendations with the production engine rather than preserving contradictory hardcoded scores.

Tested: exact minimums, below-minimum gap and 99.57 formula, absent mandatory Mathematics, incomplete capture, weak interests, accepted OR alternatives, optional subjects, absent mappings/rules, active ranking, and all five exact case-study learner scenarios. Sample programme rule/mapping coverage passed. T-SQL scripts/query grammar passed.

Remaining blocker: live funding queries and final display/export must be exercised on Windows. Formula weights/strong-interest threshold/assignment policy are explicit design assumptions in `Architecture.md`.

## Phase 5 — advisor/admin operations and reports

Changed: assigned-learner search/session review, advising notes, typed CRUD editors for every reference-data table required by the brief, account roles/assignments/password reset, deactivation, confirmation and FK-error handling. Added learner advisory summary, Degree Interest and Eligibility activity report, and Not Currently Eligible/More Information Needed exception report. Historical report lookup still includes a selected programme after it is deactivated.

Tested: management field/numeric validation and SQL identifier whitelist rejection; SQL Server grammar parsing of setup scripts and actual raw DAL queries; full compile.

Remaining blocker: interactive management and real database reports require the Windows suite. Text export format is an allowed saved-file report format, not a claim of PDF generation.

## Phase 6 — test, document, package

Changed: added real-source portable console tests, canonical sample fixtures and sample-assessment regeneration tools, optional dedicated LocalDB integration suite, SQL Server grammar validation, safe ordered setup/recovery helpers, package lockfiles, README, architecture/assumptions, checklist, changed-file list, presentation notes and 30 Windows acceptance cases. Fixed the vulnerable transitive PKCS package versions flagged during restore by explicitly pinning a patched version; warnings were not suppressed. Formatted modified/new source with the .NET formatter.

Executed final checks:

| Check | Outcome |
|---|---|
| `dotnet restore FuturePathStudentAdvisingSystem.slnx --locked-mode` | Passed |
| `dotnet build FuturePathStudentAdvisingSystem.slnx --no-restore --nologo` | Passed; 0 warnings, 0 errors |
| `dotnet run --project FuturePath.Tests --no-build` | **25 passed, 0 failed; 1 LocalDB group skipped** |
| Five required case-study learner outcomes | All passed within the 25 tests |
| SQL scripts and actual raw DAL SQL grammar | Passed within the 25 tests; not live database execution |
| `--database` integration suite | **Not executed**; Windows LocalDB required |
| Interactive GUI/high DPI/Designer checks | **Not executed**; Windows required |
| PowerShell setup/recovery helpers | **Not executed**; Windows required; underlying SQL scripts syntax-checked |

See `build-output.txt`, `portable-test-output.txt` and `locked-restore-output.txt` for actual output. The harness returns nonzero on failure; no zero-test run is described as a pass. Database setup and integration tests are not represented as complete simply because their source compiles.

Ready for handoff: integrated source, SQL/scripts, tests and documentation. Before describing the system as fully demonstrated, run the Windows database suite and acceptance cases, record results and resolve any runtime findings. No publication/deployment or fresh Windows execution is claimed.

Additional packaging checks passed: deterministic fixture/production-assessment regeneration, byte-identical root/project SQL mirrors, and Git whitespace validation.
