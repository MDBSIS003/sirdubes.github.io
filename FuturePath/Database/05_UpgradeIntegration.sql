-- Additive, repeatable upgrade. Run after 02, before 03/04. Back up existing databases first.
USE FuturePathAdvisingSystem;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
ALTER TABLE dbo.UserAccount ALTER COLUMN [Password] VARCHAR(300) NOT NULL;
ALTER TABLE dbo.UserAccount ALTER COLUMN ContactNumber VARCHAR(25) NULL;
IF COL_LENGTH('dbo.UserAccount','IsActive') IS NULL
 ALTER TABLE dbo.UserAccount ADD IsActive BIT NOT NULL CONSTRAINT DF_UserAccount_IsActive DEFAULT 1;
IF COL_LENGTH('dbo.UserAccount','GradeLevelID') IS NULL
 ALTER TABLE dbo.UserAccount ADD GradeLevelID INT NULL REFERENCES dbo.GradeLevel(GradeLevelID);
IF COL_LENGTH('dbo.UserAccount','AdvisorUserID') IS NULL
 ALTER TABLE dbo.UserAccount ADD AdvisorUserID INT NULL REFERENCES dbo.UserAccount(UserID);
IF COL_LENGTH('dbo.ProgrammeRequirement','AlternativeGroup') IS NULL
 ALTER TABLE dbo.ProgrammeRequirement ADD AlternativeGroup VARCHAR(50) NULL;
IF COL_LENGTH('dbo.AdvisorySession','SubjectsComplete') IS NULL
 ALTER TABLE dbo.AdvisorySession ADD SubjectsComplete BIT NOT NULL DEFAULT 0;
IF COL_LENGTH('dbo.AdvisorySession','FundingRequested') IS NULL
 ALTER TABLE dbo.AdvisorySession ADD FundingRequested BIT NOT NULL DEFAULT 0;
IF COL_LENGTH('dbo.AdvisorySession','IsFinalized') IS NULL
 ALTER TABLE dbo.AdvisorySession ADD IsFinalized BIT NOT NULL DEFAULT 0;
IF COL_LENGTH('dbo.FundingOption','IsSample') IS NULL
 ALTER TABLE dbo.FundingOption ADD IsSample BIT NOT NULL DEFAULT 1;
IF OBJECT_ID('dbo.AdvisingNote','U') IS NULL
 CREATE TABLE dbo.AdvisingNote(NoteID INT IDENTITY PRIMARY KEY,SessionID INT NOT NULL REFERENCES dbo.AdvisorySession(SessionID),
 AdvisorUserID INT NOT NULL REFERENCES dbo.UserAccount(UserID),NoteText NVARCHAR(2000) NOT NULL,CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME());
COMMIT;
GO
-- Never accept pre-existing plaintext passwords. An administrator must explicitly reset these accounts.
UPDATE dbo.UserAccount SET IsActive=0 WHERE [Password] NOT LIKE 'PBKDF2-SHA256$%';
GO
