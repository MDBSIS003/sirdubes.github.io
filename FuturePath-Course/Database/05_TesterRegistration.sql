USE FuturePathStudentAdvisingSystem;
GO
SET XACT_ABORT ON;
IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
    THROW 51000, 'Prepare the Future Path database before upgrading it.', 1;
IF ISNULL((SELECT MAX(Version) FROM dbo.SchemaVersion), -1) NOT IN (1, 2)
    THROW 51000, 'Unsupported schema version. No changes were applied.', 1;
BEGIN TRANSACTION;
IF OBJECT_ID(N'dbo.TesterRegistration', N'U') IS NULL
CREATE TABLE dbo.TesterRegistration (
    UserId INT NOT NULL PRIMARY KEY REFERENCES dbo.UserAccount(Id),
    Email NVARCHAR(150) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE,
    CreatedUtc DATETIME2(7) NOT NULL
);
IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 2)
    INSERT dbo.SchemaVersion(Version) VALUES(2);
COMMIT;
GO
