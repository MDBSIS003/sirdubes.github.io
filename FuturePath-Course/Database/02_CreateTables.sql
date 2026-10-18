USE FuturePathStudentAdvisingSystem;
GO
SET XACT_ABORT ON;


IF OBJECT_ID(N'dbo.SchemaVersion',N'U') IS NULL AND EXISTS(SELECT 1 FROM sys.tables WHERE is_ms_shipped=0)
 THROW 51000,'This database already contains unversioned tables. Back it up and choose a new demonstration database name.',1;
IF OBJECT_ID(N'dbo.SchemaVersion',N'U') IS NOT NULL
 EXEC(N'IF ISNULL((SELECT MAX(Version) FROM dbo.SchemaVersion),-1)<>1 THROW 51000,''Unsupported schema version. No changes were applied.'',1;');

BEGIN TRANSACTION;
IF OBJECT_ID(N'dbo.GradeLevel', N'U') IS NULL
CREATE TABLE dbo.GradeLevel (Id INT PRIMARY KEY CHECK(Id IN (11,12)), Name NVARCHAR(20) NOT NULL UNIQUE
);

IF OBJECT_ID(N'dbo.Learner', N'U') IS NULL
CREATE TABLE dbo.Learner (
 Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(120) NOT NULL CHECK(LEN(Name)<=120), GradeId INT NOT NULL REFERENCES dbo.GradeLevel(Id),
 School NVARCHAR(120) NOT NULL DEFAULT '', Province NVARCHAR(50) NOT NULL DEFAULT '', Email NVARCHAR(150) NOT NULL DEFAULT '', Phone NVARCHAR(20) NOT NULL DEFAULT ''

);

IF OBJECT_ID(N'dbo.Subject', N'U') IS NULL
CREATE TABLE dbo.Subject (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(150) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE, Active BIT NOT NULL CHECK(Active IN (0,1))
);

IF OBJECT_ID(N'dbo.Faculty', N'U') IS NULL
CREATE TABLE dbo.Faculty (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(150) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE, Active BIT NOT NULL CHECK(Active IN (0,1))
);

IF OBJECT_ID(N'dbo.InterestCategory', N'U') IS NULL
CREATE TABLE dbo.InterestCategory (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(150) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE, Active BIT NOT NULL CHECK(Active IN (0,1))
);

IF OBJECT_ID(N'dbo.DegreeProgramme', N'U') IS NULL
CREATE TABLE dbo.DegreeProgramme (
 Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(150) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE, FacultyId INT NOT NULL REFERENCES dbo.Faculty(Id),
 Description NVARCHAR(2000) NOT NULL, MinimumAverage DECIMAL(7,2) NOT NULL DEFAULT 0 CHECK(MinimumAverage BETWEEN 0 AND 100), Active BIT NOT NULL CHECK(Active IN (0,1))

);

IF OBJECT_ID(N'dbo.ProgrammeRequirement', N'U') IS NULL
CREATE TABLE dbo.ProgrammeRequirement (
 Id INT IDENTITY(1,1) PRIMARY KEY, DegreeId INT NOT NULL REFERENCES dbo.DegreeProgramme(Id), SubjectId INT NOT NULL REFERENCES dbo.Subject(Id),
 MinimumMark DECIMAL(7,2) NOT NULL CHECK(MinimumMark BETWEEN 0 AND 100), Mandatory BIT NOT NULL CHECK(Mandatory IN (0,1)),
 AlternativeGroup INT NOT NULL DEFAULT 0 CHECK(AlternativeGroup BETWEEN 0 AND 99), Note NVARCHAR(2000) NOT NULL DEFAULT '', Active BIT NOT NULL CHECK(Active IN (0,1)),
 UNIQUE(DegreeId,SubjectId)

);

IF OBJECT_ID(N'dbo.InterestQuestion', N'U') IS NULL
CREATE TABLE dbo.InterestQuestion (Id INT IDENTITY(1,1) PRIMARY KEY, Text NVARCHAR(400) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE, CategoryId INT NOT NULL REFERENCES dbo.InterestCategory(Id), Active BIT NOT NULL CHECK(Active IN (0,1))
);

IF OBJECT_ID(N'dbo.DegreeInterestMapping', N'U') IS NULL
CREATE TABLE dbo.DegreeInterestMapping (
 Id INT IDENTITY(1,1) PRIMARY KEY, DegreeId INT NOT NULL REFERENCES dbo.DegreeProgramme(Id), CategoryId INT NOT NULL REFERENCES dbo.InterestCategory(Id),
 Weight DECIMAL(7,2) NOT NULL CHECK(Weight>0 AND Weight<=10), Active BIT NOT NULL CHECK(Active IN (0,1)), UNIQUE(DegreeId,CategoryId)

);

IF OBJECT_ID(N'dbo.FundingOption', N'U') IS NULL
CREATE TABLE dbo.FundingOption (
 Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(150) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE,
 Type NVARCHAR(150) NOT NULL CHECK(Type IN ('Scholarship','Bursary','Financial aid','External funding','Merit award')),
 Eligibility NVARCHAR(2000) NOT NULL, Documents NVARCHAR(2000) NOT NULL, ClosingDate DATE NOT NULL,
 Contact NVARCHAR(2000) NOT NULL, Sample BIT NOT NULL CHECK(Sample IN (0,1)), Active BIT NOT NULL CHECK(Active IN (0,1))

);

IF OBJECT_ID(N'dbo.FundingFaculty', N'U') IS NULL
CREATE TABLE dbo.FundingFaculty (Id INT IDENTITY(1,1) PRIMARY KEY, FundingId INT NOT NULL REFERENCES dbo.FundingOption(Id), FacultyId INT NOT NULL REFERENCES dbo.Faculty(Id), Active BIT NOT NULL CHECK(Active IN (0,1)), UNIQUE(FundingId,FacultyId)
);

IF OBJECT_ID(N'dbo.FundingProgramme', N'U') IS NULL
CREATE TABLE dbo.FundingProgramme (Id INT IDENTITY(1,1) PRIMARY KEY, FundingId INT NOT NULL REFERENCES dbo.FundingOption(Id), DegreeId INT NOT NULL REFERENCES dbo.DegreeProgramme(Id), Active BIT NOT NULL CHECK(Active IN (0,1)), UNIQUE(FundingId,DegreeId)
);

IF OBJECT_ID(N'dbo.UserAccount', N'U') IS NULL
CREATE TABLE dbo.UserAccount (
 Id INT IDENTITY(1,1) PRIMARY KEY, Username NVARCHAR(60) COLLATE Latin1_General_100_CI_AS NOT NULL UNIQUE, PasswordHash NVARCHAR(300) NOT NULL,
 Role NVARCHAR(20) NOT NULL CHECK(Role IN ('Advisor','Administrator')), Active BIT NOT NULL CHECK(Active IN (0,1)),
 FailedAttempts INT NOT NULL DEFAULT 0 CHECK(FailedAttempts BETWEEN 0 AND 5), LockedUntilUtc DATETIME2(7) NULL

);

IF OBJECT_ID(N'dbo.AdvisorySession', N'U') IS NULL
CREATE TABLE dbo.AdvisorySession (
 Id INT IDENTITY(1,1) PRIMARY KEY, LearnerId INT NOT NULL REFERENCES dbo.Learner(Id), GradeId INT NOT NULL REFERENCES dbo.GradeLevel(Id),
 DegreeId INT NOT NULL REFERENCES dbo.DegreeProgramme(Id), PreferredFacultyId INT NOT NULL REFERENCES dbo.Faculty(Id), CreatedUtc DATETIME2(7) NOT NULL,
 SubjectsComplete BIT NOT NULL CHECK(SubjectsComplete IN (0,1)), FundingRequested BIT NOT NULL CHECK(FundingRequested IN (0,1)), RuleVersion NVARCHAR(200) NOT NULL

);

IF OBJECT_ID(N'dbo.LearnerSubject', N'U') IS NULL
CREATE TABLE dbo.LearnerSubject (
 SessionId INT NOT NULL REFERENCES dbo.AdvisorySession(Id), SubjectId INT NOT NULL REFERENCES dbo.Subject(Id),
 Mark DECIMAL(7,2) NOT NULL CHECK(Mark BETWEEN 0 AND 100), PRIMARY KEY(SessionId,SubjectId)

);

IF OBJECT_ID(N'dbo.LearnerInterestResponse', N'U') IS NULL
CREATE TABLE dbo.LearnerInterestResponse (
 SessionId INT NOT NULL REFERENCES dbo.AdvisorySession(Id), QuestionId INT NOT NULL REFERENCES dbo.InterestQuestion(Id),
 Score INT NOT NULL CHECK(Score BETWEEN 1 AND 5), PRIMARY KEY(SessionId,QuestionId)

);

IF OBJECT_ID(N'dbo.SessionInterestScore', N'U') IS NULL
CREATE TABLE dbo.SessionInterestScore (
 SessionId INT NOT NULL REFERENCES dbo.AdvisorySession(Id), CategoryId INT NOT NULL REFERENCES dbo.InterestCategory(Id),
 Score DECIMAL(7,2) NOT NULL CHECK(Score BETWEEN 0 AND 100), PRIMARY KEY(SessionId,CategoryId)

);

IF OBJECT_ID(N'dbo.RecommendationResult', N'U') IS NULL
CREATE TABLE dbo.RecommendationResult (
 SessionId INT NOT NULL REFERENCES dbo.AdvisorySession(Id), DegreeId INT NOT NULL REFERENCES dbo.DegreeProgramme(Id),
 Outcome NVARCHAR(30) NOT NULL CHECK(Outcome IN ('StrongMatch','PossibleMatch','NotCurrentlyEligible','MoreInformationNeeded')),
 SubjectScore DECIMAL(7,2) NOT NULL CHECK(SubjectScore BETWEEN 0 AND 100), MarkScore DECIMAL(7,2) NOT NULL CHECK(MarkScore BETWEEN 0 AND 100), InterestScore DECIMAL(7,2) NOT NULL CHECK(InterestScore BETWEEN 0 AND 100), PreferenceScore DECIMAL(7,2) NOT NULL CHECK(PreferenceScore BETWEEN 0 AND 100),
 OverallScore DECIMAL(7,2) NOT NULL CHECK(OverallScore BETWEEN 0 AND 100), Explanation NVARCHAR(2000) NOT NULL, [Rank] INT NOT NULL CHECK([Rank] BETWEEN 0 AND 5),
 PRIMARY KEY(SessionId,DegreeId), UNIQUE(SessionId,[Rank])

);

IF OBJECT_ID(N'dbo.ExportReport', N'U') IS NULL
CREATE TABLE dbo.ExportReport (Id INT IDENTITY(1,1) PRIMARY KEY, SessionId INT NOT NULL REFERENCES dbo.AdvisorySession(Id), Path NVARCHAR(2000) NOT NULL, GeneratedUtc DATETIME2(7) NOT NULL
);

IF OBJECT_ID(N'dbo.AuditEvent', N'U') IS NULL
CREATE TABLE dbo.AuditEvent (Id INT IDENTITY(1,1) PRIMARY KEY, UserId INT NULL REFERENCES dbo.UserAccount(Id), Action NVARCHAR(150) NOT NULL, Detail NVARCHAR(2000) NOT NULL, CreatedUtc DATETIME2(7) NOT NULL
);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.AdvisorySession') AND name=N'IX_AdvisorySession_Date')
CREATE INDEX IX_AdvisorySession_Date ON dbo.AdvisorySession(CreatedUtc);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.AdvisorySession') AND name=N'IX_AdvisorySession_Learner')
CREATE INDEX IX_AdvisorySession_Learner ON dbo.AdvisorySession(LearnerId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.ProgrammeRequirement') AND name=N'IX_Requirement_Degree')
CREATE INDEX IX_Requirement_Degree ON dbo.ProgrammeRequirement(DegreeId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.ExportReport') AND name=N'IX_Export_Session')
CREATE INDEX IX_Export_Session ON dbo.ExportReport(SessionId);

IF OBJECT_ID(N'dbo.SchemaVersion',N'U') IS NULL
BEGIN
 CREATE TABLE dbo.SchemaVersion(Version INT NOT NULL PRIMARY KEY, AppliedUtc DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME());
 INSERT dbo.SchemaVersion(Version) VALUES(1);
END;
COMMIT TRANSACTION;
GO
