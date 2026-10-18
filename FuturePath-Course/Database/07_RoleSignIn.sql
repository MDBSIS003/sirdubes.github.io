USE FuturePathStudentAdvisingSystem;
GO
SET XACT_ABORT ON;
IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
    THROW 51000, 'Prepare the Future Path database before upgrading it.', 1;
IF ISNULL((SELECT MAX(Version) FROM dbo.SchemaVersion), -1) NOT IN (1, 2, 3, 4)
    THROW 51000, 'Unsupported schema version. No changes were applied.', 1;
BEGIN TRANSACTION;
IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WITH (UPDLOCK,HOLDLOCK) WHERE Version = 4)
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WITH (UPDLOCK,HOLDLOCK) WHERE Username COLLATE Latin1_General_100_CI_AS=N'sidurbes')
        UPDATE dbo.UserAccount SET Username=N'sidurbes' WHERE Username COLLATE Latin1_General_100_CI_AS=N'SIRDUBES';
    UPDATE dbo.UserAccount SET Username=N'adonias',PasswordHash=N'210000:qtgmXQjiZ/6SYxJSLFLIkQ==:941Hlw6vIwHxBNnA/22xFwmOxrWT1wOxX5wYOaOCbMw=',Role=N'Advisor',Active=1,FailedAttempts=0,LockedUntilUtc=NULL
    WHERE Username COLLATE Latin1_General_100_CI_AS=N'adonias';
    IF @@ROWCOUNT=0
        INSERT dbo.UserAccount(Username,PasswordHash,Role,Active) VALUES(N'adonias',N'210000:qtgmXQjiZ/6SYxJSLFLIkQ==:941Hlw6vIwHxBNnA/22xFwmOxrWT1wOxX5wYOaOCbMw=',N'Advisor',1);
    UPDATE dbo.UserAccount SET Username=N'sidurbes',PasswordHash=N'210000:zflF6Pntl5oiPxBr3NFvgA==:PPtDcC0VRqDFAkDHdpyOIHBuRcGIJ3KTIJOcJASsUD8=',Role=N'Administrator',Active=1,FailedAttempts=0,LockedUntilUtc=NULL
    WHERE Username COLLATE Latin1_General_100_CI_AS=N'sidurbes';
    IF @@ROWCOUNT=0
        INSERT dbo.UserAccount(Username,PasswordHash,Role,Active) VALUES(N'sidurbes',N'210000:zflF6Pntl5oiPxBr3NFvgA==:PPtDcC0VRqDFAkDHdpyOIHBuRcGIJ3KTIJOcJASsUD8=',N'Administrator',1);
    INSERT dbo.SchemaVersion(Version) VALUES(4);
END;
COMMIT;
GO
