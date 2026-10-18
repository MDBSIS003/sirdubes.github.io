# Future Path

C# Windows Forms student advising application with a local SQL Server database and three projects: Desktop, Business and Data.

## Run

1. Install Visual Studio 2026 with .NET desktop development, the .NET 10 SDK and SQL Server Express LocalDB.
2. Extract the complete ZIP and open `FuturePath.sln`.
3. Allow package restore, then press F5. `FuturePath.Desktop` is the startup project.
4. If the database is missing, choose **Prepare demonstration database** in the application.

The default database is `FuturePathStudentAdvisingSystem` on `(localdb)\MSSQLLocalDB`. Existing records and staff passwords are retained.

## Learner entry

Name, school, province, grade, email and contact number are required. At least seven different subjects and their marks must be captured and confirmed. Every active interest question must be answered before continuing. Requirements and results are display-only, wrapping sections. Complete the funding step with **Save and view summary** to store the profile, captured marks, answers, scores, outcomes, alternatives and funding preference together.

## Staff sign-in

| Role | Username | Demonstration password |
|---|---|---|
| Career Advisor | advisor_test | FuturePathAdvisor2026! |
| Administrator | admin_test | FuturePathAdmin2026! |

These credentials apply to an unchanged demonstration account. Sign-in opens the staff workspace. Advisors can retrieve learner sessions and reports. Administrators can also maintain reference data and staff accounts. Staff can complete missing historical profile fields when creating a follow-up without replacing the learner or deleting earlier assessments.

## Verification

See [docs/Validation.md](docs/Validation.md). The required-entry fixes passed 102 portable tests and 260 engine comparisons. Windows LocalDB cases and Windows Forms runtime/display checks require Windows. `FuturePath.Development.sln` contains the test projects; normal use starts from `FuturePath.sln`.
