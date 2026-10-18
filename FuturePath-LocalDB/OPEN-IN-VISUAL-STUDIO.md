# Open and run in Visual Studio

1. **Extract All** from the ZIP. Keep every folder together.
2. Double-click **FuturePath.sln** in the extracted project folder.
3. In Visual Studio, press **F5** or the green **Start** button.
4. First run only: click **Prepare demonstration database** in the Future Path window. The app then opens its overview. Existing records are preserved.

Requirements: Windows, Visual Studio 2026 with **.NET desktop development**, .NET 10 SDK and **SQL Server Express LocalDB** (Visual Studio Installer → Modify → Individual components). Visual Studio restores the required NuGet packages automatically; allow the initial restore to finish.

`FuturePath.sln` contains the Windows Forms application and its Business/Data libraries. Its startup project is **FuturePath.Desktop**, which produces **FuturePath.exe** with no console. Double-clicking the solution opens Visual Studio; F5 runs the application.

The separate **FuturePath.Development.sln** includes automated tests. Use it only when developing/running tests. No GitHub account, terminal or command file is needed to open and run the normal solution.

Default connection: `(localdb)\MSSQLLocalDB` / `FuturePathStudentAdvisingSystem`, Windows authentication. Setup uses the embedded copies of Database/01–03, refuses unknown tables, and skips demo insertion into any nonempty database. Optional manual setup and troubleshooting are in docs/Database-Setup.md. See docs/Validation.md for completed checks and Windows checks still pending.

For a Mac running Windows or a high-scale display, use this version 1.0.1 from a fresh extraction. See DISPLAY-LAYOUT-FIX.md. Existing LocalDB data on the same Windows installation/account is reused.
