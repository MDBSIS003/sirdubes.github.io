/* =====================================================================
   Notes
   - Primary keys are INT IDENTITY(1,1) so the C# app can insert new rows
     without calculating IDs. 03_InsertSampleData.sql uses
     SET IDENTITY_INSERT to load stable, explicit IDs.
   - Mark and MinimumMark use DECIMAL(5,2), not DECIMAL(4,2), because
     DECIMAL(4,2) cannot store 100 (max 99.99). Needed so the 0-100 CHECK
     works as specified. Confirm with the group.
   - IsActive (DegreeProgramme): 1 = active, 0 = inactive.
   - InActive (FundingOption)  : 0 = active, 1 = inactive/outdated.
   ===================================================================== */

USE FuturePathAdvisingSystem;
GO

GO

/* =====================================================================
   A. User, grade and academic reference tables
   ===================================================================== */

IF OBJECT_ID(N'dbo.UserAccount', N'U') IS NULL
CREATE TABLE UserAccount (
    UserID         INT IDENTITY(1,1) NOT NULL,
    Username       VARCHAR(50)  NOT NULL,
    Password       VARCHAR(50)  NOT NULL,
    UserRole       VARCHAR(20)  NOT NULL,
    FirstName      VARCHAR(50)  NOT NULL,
    LastName       VARCHAR(50)  NOT NULL,
    SchoolName     VARCHAR(100) NULL,
    Province       VARCHAR(50)  NULL,
    ContactNumber  INT          NULL,
    CreatedAt      DATETIME     NOT NULL CONSTRAINT DF_UserAccount_CreatedAt DEFAULT (GETDATE()),
    CONSTRAINT PK_UserAccount PRIMARY KEY (UserID),
    CONSTRAINT UQ_UserAccount_Username UNIQUE (Username),
    CONSTRAINT CK_UserAccount_UserRole
        CHECK (UserRole IN ('Learner', 'Career Advisor', 'System Administrator'))
);
GO

IF OBJECT_ID(N'dbo.GradeLevel', N'U') IS NULL
CREATE TABLE GradeLevel (
    GradeLevelID  INT IDENTITY(1,1) NOT NULL,
    GradeName     VARCHAR(20) NOT NULL,
    CONSTRAINT PK_GradeLevel PRIMARY KEY (GradeLevelID),
    CONSTRAINT UQ_GradeLevel_GradeName UNIQUE (GradeName)
);
GO

IF OBJECT_ID(N'dbo.Faculty', N'U') IS NULL
CREATE TABLE Faculty (
    FacultyID    INT IDENTITY(1,1) NOT NULL,
    FacultyName  VARCHAR(100) NOT NULL,
    CONSTRAINT PK_Faculty PRIMARY KEY (FacultyID),
    CONSTRAINT UQ_Faculty_FacultyName UNIQUE (FacultyName)
);
GO

IF OBJECT_ID(N'dbo.Subject', N'U') IS NULL
CREATE TABLE Subject (
    SubjectID    INT IDENTITY(1,1) NOT NULL,
    SubjectName  VARCHAR(150) NOT NULL,
    CONSTRAINT PK_Subject PRIMARY KEY (SubjectID),
    CONSTRAINT UQ_Subject_SubjectName UNIQUE (SubjectName)
);
GO

IF OBJECT_ID(N'dbo.InterestCategory', N'U') IS NULL
CREATE TABLE InterestCategory (
    CategoryID    INT IDENTITY(1,1) NOT NULL,
    CategoryName  VARCHAR(100) NOT NULL,
    CONSTRAINT PK_InterestCategory PRIMARY KEY (CategoryID),
    CONSTRAINT UQ_InterestCategory_CategoryName UNIQUE (CategoryName)
);
GO

/* =====================================================================
   Tables that depend on reference data
   ===================================================================== */

IF OBJECT_ID(N'dbo.DegreeProgramme', N'U') IS NULL
CREATE TABLE DegreeProgramme (
    DegreeID       INT IDENTITY(1,1) NOT NULL,
    FacultyID      INT           NOT NULL,
    ProgrammeName  VARCHAR(150)  NOT NULL,
    Description    VARCHAR(MAX)  NULL,
    IsActive       BIT           NOT NULL CONSTRAINT DF_DegreeProgramme_IsActive DEFAULT (1),
    CONSTRAINT PK_DegreeProgramme PRIMARY KEY (DegreeID),
    CONSTRAINT UQ_DegreeProgramme_ProgrammeName UNIQUE (ProgrammeName),
    CONSTRAINT FK_DegreeProgramme_Faculty
        FOREIGN KEY (FacultyID) REFERENCES Faculty (FacultyID)
);
GO

IF OBJECT_ID(N'dbo.InterestQuestion', N'U') IS NULL
CREATE TABLE InterestQuestion (
    QuestionID    INT IDENTITY(1,1) NOT NULL,
    CategoryID    INT          NOT NULL,
    QuestionText  VARCHAR(MAX) NOT NULL,
    CONSTRAINT PK_InterestQuestion PRIMARY KEY (QuestionID),
    CONSTRAINT FK_InterestQuestion_Category
        FOREIGN KEY (CategoryID) REFERENCES InterestCategory (CategoryID)
);
GO

IF OBJECT_ID(N'dbo.DegreeInterestMapping', N'U') IS NULL
CREATE TABLE DegreeInterestMapping (
    MappingID   INT IDENTITY(1,1) NOT NULL,
    DegreeID    INT           NOT NULL,
    CategoryID  INT           NOT NULL,
    Weight      DECIMAL(4,2)  NOT NULL,
    CONSTRAINT PK_DegreeInterestMapping PRIMARY KEY (MappingID),
    CONSTRAINT UQ_DegreeInterestMapping_Degree_Category UNIQUE (DegreeID, CategoryID),
    CONSTRAINT FK_DegreeInterestMapping_Degree
        FOREIGN KEY (DegreeID) REFERENCES DegreeProgramme (DegreeID),
    CONSTRAINT FK_DegreeInterestMapping_Category
        FOREIGN KEY (CategoryID) REFERENCES InterestCategory (CategoryID)
);
GO

IF OBJECT_ID(N'dbo.ProgrammeRequirement', N'U') IS NULL
CREATE TABLE ProgrammeRequirement (
    RequirementID  INT IDENTITY(1,1) NOT NULL,
    DegreeID       INT           NOT NULL,
    SubjectID      INT           NOT NULL,
    MinimumMark    DECIMAL(5,2)  NOT NULL,
    IsMandatory    BIT           NOT NULL CONSTRAINT DF_ProgrammeRequirement_IsMandatory DEFAULT (1),
    AdvisoryNote   VARCHAR(MAX)  NULL,
    CONSTRAINT PK_ProgrammeRequirement PRIMARY KEY (RequirementID),
    CONSTRAINT UQ_ProgrammeRequirement_Degree_Subject UNIQUE (DegreeID, SubjectID),
    CONSTRAINT CK_ProgrammeRequirement_MinimumMark CHECK (MinimumMark >= 0 AND MinimumMark <= 100),
    CONSTRAINT FK_ProgrammeRequirement_Degree
        FOREIGN KEY (DegreeID) REFERENCES DegreeProgramme (DegreeID),
    CONSTRAINT FK_ProgrammeRequirement_Subject
        FOREIGN KEY (SubjectID) REFERENCES Subject (SubjectID)
);
GO

/* =====================================================================
   C. Advising, responses and recommendation tables
   ===================================================================== */

IF OBJECT_ID(N'dbo.AdvisorySession', N'U') IS NULL
CREATE TABLE AdvisorySession (
    SessionID     INT IDENTITY(1,1) NOT NULL,
    UserID        INT      NOT NULL,
    GradeLevelID  INT      NOT NULL,
    FacultyID     INT      NOT NULL,
    DegreeID      INT      NOT NULL,
    SessionDate   DATETIME NOT NULL CONSTRAINT DF_AdvisorySession_SessionDate DEFAULT (GETDATE()),
    CONSTRAINT PK_AdvisorySession PRIMARY KEY (SessionID),
    CONSTRAINT FK_AdvisorySession_User
        FOREIGN KEY (UserID) REFERENCES UserAccount (UserID),
    CONSTRAINT FK_AdvisorySession_GradeLevel
        FOREIGN KEY (GradeLevelID) REFERENCES GradeLevel (GradeLevelID),
    CONSTRAINT FK_AdvisorySession_Faculty
        FOREIGN KEY (FacultyID) REFERENCES Faculty (FacultyID),
    CONSTRAINT FK_AdvisorySession_Degree
        FOREIGN KEY (DegreeID) REFERENCES DegreeProgramme (DegreeID)
);
GO

IF OBJECT_ID(N'dbo.LearnerSubject', N'U') IS NULL
CREATE TABLE LearnerSubject (
    LearnerSubjectID  INT IDENTITY(1,1) NOT NULL,
    SessionID         INT           NOT NULL,
    SubjectID         INT           NOT NULL,
    Mark              DECIMAL(5,2)  NOT NULL,
    CONSTRAINT PK_LearnerSubject PRIMARY KEY (LearnerSubjectID),
    CONSTRAINT UQ_LearnerSubject_Session_Subject UNIQUE (SessionID, SubjectID),
    CONSTRAINT CK_LearnerSubject_Mark CHECK (Mark >= 0 AND Mark <= 100),
    CONSTRAINT FK_LearnerSubject_Session
        FOREIGN KEY (SessionID) REFERENCES AdvisorySession (SessionID),
    CONSTRAINT FK_LearnerSubject_Subject
        FOREIGN KEY (SubjectID) REFERENCES Subject (SubjectID)
);
GO

IF OBJECT_ID(N'dbo.LearnerInterestResponse', N'U') IS NULL
CREATE TABLE LearnerInterestResponse (
    ResponseID  INT IDENTITY(1,1) NOT NULL,
    SessionID   INT           NOT NULL,
    QuestionID  INT           NOT NULL,
    Score       DECIMAL(4,2)  NOT NULL,
    CONSTRAINT PK_LearnerInterestResponse PRIMARY KEY (ResponseID),
    CONSTRAINT UQ_LearnerInterestResponse_Session_Question UNIQUE (SessionID, QuestionID),
    CONSTRAINT FK_LearnerInterestResponse_Session
        FOREIGN KEY (SessionID) REFERENCES AdvisorySession (SessionID),
    CONSTRAINT FK_LearnerInterestResponse_Question
        FOREIGN KEY (QuestionID) REFERENCES InterestQuestion (QuestionID)
);
GO

IF OBJECT_ID(N'dbo.RecommendationResult', N'U') IS NULL
CREATE TABLE RecommendationResult (
    RecommendationResultID  INT IDENTITY(1,1) NOT NULL,
    SessionID               INT           NOT NULL,
    DegreeID                INT           NOT NULL,
    Outcome                 VARCHAR(50)   NOT NULL,
    OverallScore            DECIMAL(5,2)  NULL,
    GapExplanation          VARCHAR(MAX)  NULL,
    IsAlternative           BIT           NOT NULL CONSTRAINT DF_RecommendationResult_IsAlternative DEFAULT (0),
    CONSTRAINT PK_RecommendationResult PRIMARY KEY (RecommendationResultID),
    CONSTRAINT FK_RecommendationResult_Session
        FOREIGN KEY (SessionID) REFERENCES AdvisorySession (SessionID),
    CONSTRAINT FK_RecommendationResult_Degree
        FOREIGN KEY (DegreeID) REFERENCES DegreeProgramme (DegreeID)
);
GO

/* =====================================================================
   D. Funding and export tables
   ===================================================================== */

IF OBJECT_ID(N'dbo.FundingOption', N'U') IS NULL
CREATE TABLE FundingOption (
    FundingID               INT IDENTITY(1,1) NOT NULL,
    FacultyID               INT           NOT NULL,
    FundingName             VARCHAR(250)  NOT NULL,
    FundingType             VARCHAR(100)  NOT NULL,
    EligibilityDescription  VARCHAR(MAX)  NULL,
    RequiredDocuments       VARCHAR(MAX)  NULL,
    ClosingDate             DATETIME      NULL,
    ContactInfo             VARCHAR(250)  NULL,
    InActive                BIT           NOT NULL CONSTRAINT DF_FundingOption_InActive DEFAULT (0),
    CONSTRAINT PK_FundingOption PRIMARY KEY (FundingID),
    CONSTRAINT FK_FundingOption_Faculty
        FOREIGN KEY (FacultyID) REFERENCES Faculty (FacultyID)
);
GO

IF OBJECT_ID(N'dbo.FundingProgramme', N'U') IS NULL
CREATE TABLE FundingProgramme (
    FundingProgrammeID  INT IDENTITY(1,1) NOT NULL,
    FundingID           INT NOT NULL,
    DegreeID            INT NOT NULL,
    CONSTRAINT PK_FundingProgramme PRIMARY KEY (FundingProgrammeID),
    CONSTRAINT UQ_FundingProgramme_Funding_Degree UNIQUE (FundingID, DegreeID),
    CONSTRAINT FK_FundingProgramme_Funding
        FOREIGN KEY (FundingID) REFERENCES FundingOption (FundingID),
    CONSTRAINT FK_FundingProgramme_Degree
        FOREIGN KEY (DegreeID) REFERENCES DegreeProgramme (DegreeID)
);
GO

IF OBJECT_ID(N'dbo.ExportReport', N'U') IS NULL
CREATE TABLE ExportReport (
    ReportID       INT IDENTITY(1,1) NOT NULL,
    SessionID      INT      NOT NULL,
    DateGenerated  DATETIME NOT NULL CONSTRAINT DF_ExportReport_DateGenerated DEFAULT (GETDATE()),
    CONSTRAINT PK_ExportReport PRIMARY KEY (ReportID),
    CONSTRAINT FK_ExportReport_Session
        FOREIGN KEY (SessionID) REFERENCES AdvisorySession (SessionID)
);
GO

/* =====================================================================
   Indexes on foreign keys / columns the C# queries will filter on
   (UNIQUE constraints above already index the composite keys.)
   ===================================================================== */

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_DegreeProgramme_Faculty_IsActive' AND object_id=OBJECT_ID(N'dbo.DegreeProgramme'))
CREATE INDEX IX_DegreeProgramme_Faculty_IsActive ON DegreeProgramme (FacultyID, IsActive);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_InterestQuestion_Category' AND object_id=OBJECT_ID(N'dbo.InterestQuestion'))
CREATE INDEX IX_InterestQuestion_Category ON InterestQuestion (CategoryID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_DegreeInterestMapping_Category' AND object_id=OBJECT_ID(N'dbo.DegreeInterestMapping'))
CREATE INDEX IX_DegreeInterestMapping_Category ON DegreeInterestMapping (CategoryID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_ProgrammeRequirement_Subject' AND object_id=OBJECT_ID(N'dbo.ProgrammeRequirement'))
CREATE INDEX IX_ProgrammeRequirement_Subject ON ProgrammeRequirement (SubjectID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_AdvisorySession_User' AND object_id=OBJECT_ID(N'dbo.AdvisorySession'))
CREATE INDEX IX_AdvisorySession_User ON AdvisorySession (UserID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_AdvisorySession_Degree' AND object_id=OBJECT_ID(N'dbo.AdvisorySession'))
CREATE INDEX IX_AdvisorySession_Degree ON AdvisorySession (DegreeID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_LearnerSubject_Subject' AND object_id=OBJECT_ID(N'dbo.LearnerSubject'))
CREATE INDEX IX_LearnerSubject_Subject ON LearnerSubject (SubjectID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_LearnerInterestResponse_Question' AND object_id=OBJECT_ID(N'dbo.LearnerInterestResponse'))
CREATE INDEX IX_LearnerInterestResponse_Question ON LearnerInterestResponse (QuestionID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_RecommendationResult_Session' AND object_id=OBJECT_ID(N'dbo.RecommendationResult'))
CREATE INDEX IX_RecommendationResult_Session ON RecommendationResult (SessionID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_RecommendationResult_Degree' AND object_id=OBJECT_ID(N'dbo.RecommendationResult'))
CREATE INDEX IX_RecommendationResult_Degree ON RecommendationResult (DegreeID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_FundingOption_Faculty_InActive' AND object_id=OBJECT_ID(N'dbo.FundingOption'))
CREATE INDEX IX_FundingOption_Faculty_InActive ON FundingOption (FacultyID, InActive);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_FundingProgramme_Degree' AND object_id=OBJECT_ID(N'dbo.FundingProgramme'))
CREATE INDEX IX_FundingProgramme_Degree ON FundingProgramme (DegreeID);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_ExportReport_Session' AND object_id=OBJECT_ID(N'dbo.ExportReport'))
CREATE INDEX IX_ExportReport_Session ON ExportReport (SessionID);
GO