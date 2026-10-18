# SQL Server LocalDB data dictionary

Authoritative schema: `Database/02_CreateTables.sql`. The20 reference domain tables and exact entity/column names are preserved. `SchemaVersion` is additional installation metadata. No cascading delete is introduced. Empty strings represent optional learner fields; only optional staff lockout/audit identity values are NULL.

`INT IDENTITY(1,1)` replaces implicit file-database row identifiers. `BIT` represents flags, `DECIMAL(7,2)` stores marks/scores/weights, `DATE` stores inclusive funding deadlines, and `DATETIME2(7)` stores UTC instants. Unicode strings have explicit limits. Case-insensitive unique reference labels and username constraints are retained.

## AdvisorySession

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| LearnerId | INT | FK → Learner.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| GradeId | INT | FK → GradeLevel.Id | Yes | 11 or 12; session grade is at assessment time |
| DegreeId | INT | FK → DegreeProgramme.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| PreferredFacultyId | INT | FK → Faculty.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| CreatedUtc | DATETIME2(7) | — | Yes | UTC timestamp |
| SubjectsComplete | BIT | — | Yes | 0 = false;1 = true |
| FundingRequested | BIT | — | Yes | 0 = false;1 = true |
| RuleVersion | NVARCHAR(200) | — | Yes | Snapshot of the scoring/outcome policy |

## AuditEvent

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| UserId | INT | FK → UserAccount.Id | No | See schema CHECK/UNIQUE/default and business validation |
| Action | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Detail | NVARCHAR(2000) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| CreatedUtc | DATETIME2(7) | — | Yes | UTC timestamp |

## DegreeInterestMapping

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| DegreeId | INT | FK → DegreeProgramme.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| CategoryId | INT | FK → InterestCategory.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Weight | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## DegreeProgramme

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Name | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| FacultyId | INT | FK → Faculty.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Description | NVARCHAR(2000) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| MinimumAverage | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## ExportReport

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| SessionId | INT | FK → AdvisorySession.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Path | NVARCHAR(2000) | — | Yes | Exported file path; metadata written only after file creation |
| GeneratedUtc | DATETIME2(7) | — | Yes | UTC timestamp |

## Faculty

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Name | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## FundingFaculty

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| FundingId | INT | FK → FundingOption.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| FacultyId | INT | FK → Faculty.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## FundingOption

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Name | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Type | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Eligibility | NVARCHAR(2000) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Documents | NVARCHAR(2000) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| ClosingDate | DATE | — | Yes | Inclusive local closing date |
| Contact | NVARCHAR(2000) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Sample | BIT | — | Yes | 0 = false;1 = true |
| Active | BIT | — | Yes | 0 = false;1 = true |

## FundingProgramme

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| FundingId | INT | FK → FundingOption.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| DegreeId | INT | FK → DegreeProgramme.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## GradeLevel

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT | PK  | Yes | See schema CHECK/UNIQUE/default and business validation |
| Name | NVARCHAR(20) | — | Yes | See schema CHECK/UNIQUE/default and business validation |

## InterestCategory

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Name | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## InterestQuestion

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Text | NVARCHAR(400) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| CategoryId | INT | FK → InterestCategory.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## Learner

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Name | NVARCHAR(120) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| GradeId | INT | FK → GradeLevel.Id | Yes | 11 or 12; session grade is at assessment time |
| School | NVARCHAR(120) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Province | NVARCHAR(50) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Email | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Phone | NVARCHAR(20) | — | Yes | See schema CHECK/UNIQUE/default and business validation |

## LearnerInterestResponse

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| SessionId | INT | PK FK → AdvisorySession.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| QuestionId | INT | PK FK → InterestQuestion.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Score | INT | — | Yes | See schema CHECK/UNIQUE/default and business validation |

## LearnerSubject

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| SessionId | INT | PK FK → AdvisorySession.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| SubjectId | INT | PK FK → Subject.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Mark | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |

## ProgrammeRequirement

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| DegreeId | INT | FK → DegreeProgramme.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| SubjectId | INT | FK → Subject.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| MinimumMark | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Mandatory | BIT | — | Yes | 0 = false;1 = true |
| AlternativeGroup | INT | — | Yes | 0 = independent; positive number = OR choices within the degree |
| Note | NVARCHAR(2000) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## RecommendationResult

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| SessionId | INT | PK FK → AdvisorySession.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| DegreeId | INT | PK FK → DegreeProgramme.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Outcome | NVARCHAR(30) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| SubjectScore | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| MarkScore | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| InterestScore | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| PreferenceScore | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| OverallScore | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Explanation | NVARCHAR(2000) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Rank | INT | — | Yes | 0 = selected;1–5 = ranked alternatives |

## SchemaVersion

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Version | INT | PK  | Yes | Supported schema version (currently1) |
| AppliedUtc | DATETIME2(7) | — | Yes | UTC timestamp |

## SessionInterestScore

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| SessionId | INT | PK FK → AdvisorySession.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| CategoryId | INT | PK FK → InterestCategory.Id | Yes | See schema CHECK/UNIQUE/default and business validation |
| Score | DECIMAL(7,2) | — | Yes | See schema CHECK/UNIQUE/default and business validation |

## Subject

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Name | NVARCHAR(150) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |

## UserAccount

| Column | SQL Server type | Key | Required | Purpose / constraint |
|---|---|---|---|---|
| Id | INT IDENTITY | PK  | Yes | Generated identity; not supplied by normal application inserts |
| Username | NVARCHAR(60) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| PasswordHash | NVARCHAR(300) | — | Yes | PBKDF2 iteration count:salt:derived key (never plaintext) |
| Role | NVARCHAR(20) | — | Yes | See schema CHECK/UNIQUE/default and business validation |
| Active | BIT | — | Yes | 0 = false;1 = true |
| FailedAttempts | INT | — | Yes | 0–5, reset on success/expiry/password reset |
| LockedUntilUtc | DATETIME2(7) | — | No | UTC timestamp; optional lock expiry |

## Integrity and history

Per-session subjects/responses/category scores use composite primary keys. Degree/subject, degree/category, funding/faculty and funding/programme combinations are unique. Recommendation rank is unique per session. Marks and component scores are0–100; questionnaire responses1–5; weight >0 and ≤10; OR group0–99. Grade IDs are11/12. Foreign keys reject orphan records.

A session transaction includes learner creation (when needed), session metadata, all inputs, persisted results/category scores and its audit row. The profile identity remains unchanged on staff follow-ups. Historic scores/reasons/rule version are not recalculated on startup or reference edits.

Reference labels and current funding are read for reports at report time, as in the input design. Schema/script grammar has been checked here; real SQL Server constraint, save, restart, backup/restore and repeatability tests still require Windows LocalDB.
