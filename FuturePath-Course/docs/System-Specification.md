# Future Path — implementation companion, course edition 2.0

The group's **INF2011S_SystemSpecification_Group.pdf** remains the supplied Part 1. This companion describes the implementation and documented reconciliations, not a new submission or claimed approval.

## Scope and users

Standalone Windows Forms software for Grade 11/12 preliminary study guidance. A learner captures school/province/grade, optional personal/contact details, subjects/marks and interests, then selects a faculty and degree. Local deterministic rules explain eligibility, gaps and up to five alternatives. Funding guidance is optional. Staff authenticate to retrieve saved sessions, create follow-ups, report and maintain reference data according to role. No external AI/API, application submission, official admission, guaranteed funding, parent accounts or live university data is implemented.

## Dialog and class design

See diagrams/Navigation.png, Class-Model.png and Screen-Layouts-1/2.png. The MDI parent owns navigation; only one learner child is visible. Forms are editable partial/Designer pairs. Shared base form behavior, named handlers, constructors, fields/properties and loops follow the practicals. Back retains input. Next validates. The results step previews. Saving on the funding step commits all assessment rows together and opens the summary. Finish clears guest access and returns to overview. Staff dialogs remain within the same application process.

Three projects separate UI, business and data access. AdvisorySessionController coordinates the learner use case. RecommendationEngine checks configuration/subjects/average/questions, calculates scores, selects the outcome and ranks alternatives. Other controllers enforce staff roles and handle maintenance/reports. SqlServerStore delegates to aggregate DAL classes inheriting DB. The data dictionary and ERD describe the preserved relational design. See Part1-Reconciliation.md for consolidated controller boxes and dictionary inconsistencies.

## Rules and controls

New profiles require School, Province and Grade 11/12; name/contact remain optional and are validated when entered. Marks are 0–100 inclusive and unique per subject/session; active subjects/questions/programmes are loaded from LocalDB. A confirmed subject list distinguishes a missing mandatory subject from incomplete capture. Questionnaire responses are 1–5; a deliberate incomplete assessment receives More Information Needed. The selected degree must belong to the chosen faculty.

Scores weight subject coverage 35%, mark match 35%, interests 25% and faculty preference 5%. Missing information takes precedence; a mandatory missing subject or shortfall over five points is Not Currently Eligible; a shortfall up to five or weak interest is Possible Match; Strong Match requires all hard rules and interest ≥60%. Explicit OR groups, configured averages, active flags and inclusive funding closing dates are respected. Rule version, scores and reasons are stored, not recalculated for historical reports. Reference names and open funding are read at report time.

School/province are required by new capture validation without rewriting legacy rows. Setup never drops data; demo seeding is explicit and skips nonempty databases. SQL parameters protect user values; allowlisted metadata limits editable identifiers; keys/checks/unique constraints protect relationships; transactions prevent partial saves. Staff passwords use salted PBKDF2 and five-attempt/five-minute lockout. Guest workspaces can read only their own saved sessions; business methods enforce staff/admin access. Deactivation preserves history. New verified backups require a new filename.

## Required outputs

Learner Advisory Summary includes profile/grade, marks, interests, selected faculty/degree, stored outcome/components/reasons, alternatives, optional funding, dates/rule version and a preliminary-guidance disclaimer. Degree Interest and Eligibility Report counts selections/outcomes, common gaps and funding requests; Exception Report lists Not Currently Eligible/More Information Needed. Reports support period/faculty/degree/outcome and search filters, native printing/PDF and HTML/text export. Session activity and distinct learners are labelled separately; funding category counting is explicitly defined in Part1-Reconciliation.md.

## Testing and evidence

Test-Cases.md and the workbook define cases, input and expected behavior; record actual Windows results, tester, date and evidence. Validation.md separates executed portable/build/syntax checks from Windows-only LocalDB/UI and manual display/printing checks. All five named case-study outcomes and the five additional seeds are preserved. A working build is not a guaranteed mark. Review the project, run the remaining acceptance checks and record any defects before presentation.
