# Future Path 2.3

C# Windows Forms student advising system with Desktop, Business and Data projects.

## Open and run

1. Install Visual Studio 2026 with .NET desktop development, the .NET 10 SDK and SQL Server Express LocalDB.
2. Extract the entire ZIP before opening `FuturePath.sln`.
3. Allow NuGet package restore to finish, then press F5. `FuturePath.Desktop` is the startup project.
4. If there is no database, select **Prepare demonstration database** in the application.

The database is `FuturePathStudentAdvisingSystem` on `(localdb)\MSSQLLocalDB`. Existing databases are upgraded automatically. Learner records, assessments and unrelated staff accounts are preserved. The upgrade sets the two accounts below to the requested roles and passwords once; subsequent starts preserve changes made in Security.

## Staff login details

Select **Staff sign in**, choose the correct role under **Sign in as**, then enter the matching username and password:

| Role | Username | Password |
|---|---|---|
| Advisor | adonias | NKSADO001 |
| Administrator | sidurbes | MDBSIS003 |

Enter the passwords exactly as shown, including uppercase letters. The selected role must match the account; selecting Administrator does not give an Advisor account administrative access. There is no registration or email-code step. These accounts are stored in the database. If an account's details have been changed in Security or by an administrator, use the updated details; reopening the application does not reset them.

The Administrator can use learner searches, reports, reference-data maintenance, staff-account administration and database backup. The Advisor can search learners, retrieve assessments, create follow-up assessments and use reports. Administrative functions require the Administrator account.

Both roles can open **Security** to change their username or password after confirming the current password. Leave both new-password fields empty to keep the existing password. New passwords entered through Security or staff administration require 12–128 characters with a letter and a number. The two initial login passwords above are already configured.

## Learner assessments

Name, school, province, grade, email and contact number are required. Contact numbers must contain exactly ten digits, including the leading zero. Capture at least seven different subjects and marks, confirm the subject list, and answer every interest question before continuing.

Programme requirements, outcomes and funding information are display-only. Funding information wraps without an inner textbox scrollbar. Use **Complete and save** to store the learner profile, subjects, marks, responses, scores, recommendations, alternatives and funding preference in one assessment. The final summary shows its saved reference, such as **FP-13**, and offers export or printing.

Staff can search saved assessments by learner name, school, email, contact number, province, degree or FP-reference. Learner summaries contain the advising outcome and next steps; staff reports include the full assessment details.
