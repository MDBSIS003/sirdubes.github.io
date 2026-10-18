USE FuturePathStudentAdvisingSystem;
GO
SET XACT_ABORT ON;
IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
    THROW 51000, 'Prepare the Future Path database before upgrading it.', 1;
IF ISNULL((SELECT MAX(Version) FROM dbo.SchemaVersion), -1) NOT IN (1, 2, 3)
    THROW 51000, 'Unsupported schema version. No changes were applied.', 1;
BEGIN TRANSACTION;
IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WITH (UPDLOCK,HOLDLOCK) WHERE Username=N'Adonias')
    INSERT dbo.UserAccount(Username,PasswordHash,Role,Active) VALUES(N'Adonias',N'210000:loKRTa0L3rLCHmEIoy2oeA==:LrSt3+eHt16oK+SnEIIx6NiX7wNKTKWadwdIt7iDlYI=',N'Administrator',1);
IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WITH (UPDLOCK,HOLDLOCK) WHERE Username=N'SIRDUBES')
    INSERT dbo.UserAccount(Username,PasswordHash,Role,Active) VALUES(N'SIRDUBES',N'210000:I1KnRv64/7DdTEVw43y9Yw==:hcjFeSR4aZ0VhcRQJvzJrgnHzsEjsTiMqRGZlU4xwL0=',N'Advisor',1);
IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 3)
    INSERT dbo.SchemaVersion(Version) VALUES(3);
COMMIT;
GO
