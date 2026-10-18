# Future Path 2.1

C# Windows Forms student advising application with a local SQL Server database and three projects: Desktop, Business and Data.

## Run

1. Install Visual Studio 2026 with .NET desktop development, the .NET 10 SDK and SQL Server Express LocalDB.
2. Extract the complete ZIP and open `FuturePath.sln`.
3. Allow package restore, then press F5. `FuturePath.Desktop` is the startup project.
4. If the database is missing, choose **Prepare demonstration database** in the application.

The default database is `FuturePathStudentAdvisingSystem` on `(localdb)\MSSQLLocalDB`. On first use, version 1 databases receive an additive upgrade for tester registration. Existing learner records and staff passwords are retained.

## Learner entry

Name, school, province, grade, email and contact number are required. Contact numbers must contain exactly ten digits, including a leading zero where applicable. At least seven different subjects and their marks must be captured and confirmed. Every active interest question must be answered before continuing. Requirements, funding and results are display-only, wrapping sections without inner scrollbars. Complete the funding step with **Complete and save** to store the profile, captured marks, answers, scores, outcomes, alternatives and funding preference together. The saved summary shows a reference such as **FP-13** and can be exported or printed.

## Staff sign-in

To create an account for assessment testing:

1. Select **Staff sign in**, then **Create tester staff account**.
2. Enter an email, username, password and confirmation. Passwords require 12–128 characters, including a letter and a digit.
3. Choose **Administrator** to test all staff functions, or **Advisor** to test advisor access.
4. Select **Request test code**, then **Code inserted (test)** within five minutes.

This is an offline tester registration flow: no email is sent and no real email verification occurs. Account creation, the email link and its audit entry commit together in the database before sign-in. Closing the application does not delete the account. Later, sign in using the username or registered email and chosen password. An existing username or registered tester email cannot be used to create a replacement account.

Original demonstration accounts remain available:

| Role | Username | Demonstration password |
|---|---|---|
| Career Advisor | advisor_test | FuturePathAdvisor2026! |
| Administrator | admin_test | FuturePathAdmin2026! |

These credentials apply to an unchanged demonstration account. Sign-in opens the staff workspace. Advisors can search saved learner sessions by name, school, email, contact number, province, degree or reference and retrieve their reports. Administrators can also maintain reference data and staff accounts. **Security** allows staff to change their own username and password after confirming their current password; leaving both new-password fields empty keeps the existing password. Staff can complete missing historical profile fields when creating a follow-up without replacing the learner or deleting earlier assessments.

## Verification

See [docs/Validation.md](docs/Validation.md). The current portable suite passed 143 tests. Windows LocalDB cases and Windows Forms runtime/display checks require Windows and were not executed on the Linux build host. `FuturePath.Development.sln` contains the test projects; normal use starts from `FuturePath.sln`. The downloadable application ZIP contains the solution, its three projects and database scripts; development tests and verification evidence remain in the repository.
