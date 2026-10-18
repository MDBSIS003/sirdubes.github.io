# Dependencies and restore

The app remains .NET 10 Windows Forms. No framework downgrade, ORM, third-party UI/report library, cloud SDK feature, web server or runtime AI/API was introduced.

| Direct package | Version | Scope | Reason |
|---|---|---|---|
| Microsoft.Data.SqlClient |7.1.1 | Application runtime (`FuturePath.Data`) | Supported ADO.NET SQL Server/LocalDB connection, commands, parameters and transactions |
| Microsoft.NET.Test.Sdk |17.14.1 | Tests only, retained | Existing Visual Studio/dotnet test host |
| xunit |2.9.3 | Tests only, retained | Existing unit/integration test framework |
| xunit.runner.visualstudio |3.1.1 | Tests only, retained; PrivateAssets=all | Existing test discovery/runner |

`Directory.Packages.props` records versions centrally. **System.Security.Cryptography.Pkcs10.0.13** pins an already required transitive SQL client dependency to a patched version. It is not an unrelated added runtime feature. The provider also brings its normal native SNI, configuration, identity-model and framework helper dependencies; exact resolved versions/hashes are in each project's `packages.lock.json`. Windows integrated security is the only configured application authentication route to SQL Server; no cloud identity service is called by the app.

This is not a package-free application. Modern .NET 10 needs a SQL Server provider package. Using the old framework-bundled System.Data.SqlClient would require a major target-framework change, which was not made. The small Windows setup helper uses Windows PowerShell's built-in provider only to execute the explicit setup scripts; the actual .NET 10 application consistently uses Microsoft.Data.SqlClient.

The runtime Microsoft.Data.Sqlite/native SQLite packages have been removed. No SQLite provider is retained as a parallel test or application database. Portable rule tests read JSON **input fixtures**, not a replacement persistence engine. Real persistence tests require Windows LocalDB.

Restore/build from solution root:

```powershell
dotnet restore FuturePath.Development.sln --locked-mode
dotnet build FuturePath.Development.sln -c Release --no-restore
```

Internet is needed for the initial NuGet/tool restore. After installation, advising, SQL queries, export and printing run locally/offline. SQL Server Express LocalDB is a separate Windows prerequisite; a self-contained .NET publish does not install it.

The seed generator uses only the Business project and built-in System.Text.Json. Documentation uses source Mermaid/DOT and generated images. A developer-side SQL grammar check used Microsoft's parser available in the validation environment; it is not an added application or test-package dependency. Package signatures, checksums and TLS verification were kept enabled.
