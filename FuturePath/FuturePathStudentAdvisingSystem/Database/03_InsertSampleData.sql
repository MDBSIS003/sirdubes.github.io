USE FuturePathAdvisingSystem;
GO
SET XACT_ABORT ON;
IF EXISTS (SELECT 1 FROM dbo.Subject) OR
    EXISTS (SELECT 1 FROM dbo.GradeLevel) OR
    EXISTS (SELECT 1 FROM dbo.Faculty) OR
    EXISTS (SELECT 1 FROM dbo.DegreeProgramme) OR
    EXISTS (SELECT 1 FROM dbo.InterestCategory) OR
    EXISTS (SELECT 1 FROM dbo.InterestQuestion) OR
    EXISTS (SELECT 1 FROM dbo.ProgrammeRequirement) OR
    EXISTS (SELECT 1 FROM dbo.DegreeInterestMapping) OR
    EXISTS (SELECT 1 FROM dbo.FundingOption) OR
    EXISTS (SELECT 1 FROM dbo.FundingProgramme) OR
    EXISTS (SELECT 1 FROM dbo.UserAccount) OR
    EXISTS (SELECT 1 FROM dbo.AdvisorySession) OR
    EXISTS (SELECT 1 FROM dbo.LearnerSubject) OR
    EXISTS (SELECT 1 FROM dbo.LearnerInterestResponse) OR
    EXISTS (SELECT 1 FROM dbo.RecommendationResult) OR
    EXISTS (SELECT 1 FROM dbo.ExportReport)
BEGIN
    PRINT 'Sample seed skipped: database already contains records. No data changed. Use management screens for additions.';
    RETURN;
END;
BEGIN TRANSACTION;


-- New, empty development database sample data only.
/* ---------- Subject (16 rows, fixed IDs) ---------- */
SET IDENTITY_INSERT Subject ON;

INSERT INTO Subject (SubjectID, SubjectName) VALUES
(1,  'English'),
(2,  'Afrikaans'),
(3,  'Mathematics'),
(4,  'Mathematical Literacy'),
(5,  'Physical Sciences'),
(6,  'Life Sciences'),
(7,  'Accounting'),
(8,  'Business Studies'),
(9,  'Economics'),
(10, 'Geography'),
(11, 'History'),
(12, 'Computer Applications Technology'),
(13, 'Life Orientation'),
(14, 'Tourism'),
(15, 'Engineering Graphics and Design'),
(16, 'Visual Arts'),
(17, 'Zulu'),
(18, 'Xhosa');
SET IDENTITY_INSERT Subject OFF;

/* inserting gradelevels*/
SET IDENTITY_INSERT GradeLevel ON;
INSERT INTO GradeLevel (GradeLevelID, GradeName) VALUES
(1, 'Grade 11'),
(2, 'Grade 12');
SET IDENTITY_INSERT GradeLevel OFF;

/* inserting faculties*/
SET IDENTITY_INSERT Faculty ON;

INSERT INTO Faculty (FacultyID, FacultyName) VALUES
(1, 'Commerce'),
(2, 'Humanities'),
(3, 'Science'),
(4, 'Engineering'),
(5, 'Health Sciences');

SET IDENTITY_INSERT Faculty OFF;

/* inserting degree programmes*/
SET IDENTITY_INSERT DegreeProgramme ON;

INSERT INTO DegreeProgramme (DegreeID, FacultyID, ProgrammeName, Description, IsActive) VALUES
(1,  1, 'BCom Information Systems',
    'Combining business knowledge with information technology and systems. Sample data only, not official university information, please check with official university policies', 1),
(2,  1, 'BCom Accounting',
    'Focuses on financial accounting, auditing, management accounting and taxation. Sample data only, not official university information, please check with official university policies', 1),
(3,  1, 'BCom General',
    'A broad commerce degree covering business, economics and management. Sample data only, not official university information, please check with official university policies', 1),
(4,  3, 'BSc Computer Science',
    'Programming, algorithms and software development. Sample data only, not official university information, please check with official university policies', 1),
(5,  3, 'BSc Biological Sciences',
    'Study of living organisms, ecology and biological research. Sample data only, not official university information, please check with official university policies', 1),
(6,  4, 'BSc Engineering (Mechanical Engineering)',
    'Design and analysis of mechanical systems and machines. Sample data only, not official university information, please check with official university policies', 1),
(7,  2, 'BA Humanities',
    'A broad arts degree in languages, history and the social world. Sample data only, not official university information, please check with official university policies', 1),
(8,  2, 'BSocSci',
    'Social science degree covering society, politics and human behaviour. Sample data only, not official university information, please check with official university policies', 1),
(9,  2, 'Law-related qualification',
    'A law-related pathway focused on legal reasoning and society. Sample data only, not official university information, please check with official university policies', 1),
(10, 5, 'Bachelor of Health Sciences',
    'A health sciences pathway into clinical and related health careers. Sample data only, not official university information, please check with official university policies', 1);

SET IDENTITY_INSERT DegreeProgramme OFF;

/* inserting interest categories*/
SET IDENTITY_INSERT InterestCategory ON;

INSERT INTO InterestCategory (CategoryID, CategoryName) VALUES
(1, 'Numeracy'),
(2, 'Helping People'),
(3, 'Technology'),
(4, 'Science and Research'),
(5, 'Business and Finance'),
(6, 'Design Thinking'),
(7, 'Society and Politics'),
(8, 'Reading, Writing and Analysis');

SET IDENTITY_INSERT InterestCategory OFF;

/* inserting interest questions*/
SET IDENTITY_INSERT InterestQuestion ON;

INSERT INTO InterestQuestion (QuestionID, CategoryID, QuestionText) VALUES
(1, 1, 'I enjoy solving problems using numbers or data.'),
(2, 2, 'I enjoy helping people and communities.'),
(3, 3, 'I enjoy working with technology.'),
(4, 4, 'I enjoy science experiments and research.'),
(5, 8, 'I enjoy reading, writing and analysing ideas.'),
(6, 5, 'I enjoy business, entrepreneurship and finance.'),
(7, 6, 'I enjoy designing or building practical solutions.'),
(8, 7, 'I enjoy understanding society, law or politics.'),
(9,1, ' I can make several complex calculations in my mind'),
(10,2, ' I help other students understand concepts');

SET IDENTITY_INSERT InterestQuestion OFF;

/* ---------- ProgrammeRequirement (SAMPLE thresholds, not official) ----------
   Subject IDs: 1 English, 3 Mathematics, 4 Mathematical Literacy,
   5 Physical Sciences, 6 Life Sciences, 7 Accounting, 11 History,
   12 Computer Applications Technology, 15 Engineering Graphics and Design
   IsMandatory = 1 : strict requirement
   IsMandatory = 0 : recommended, or one of several accepted alternatives  */
SET IDENTITY_INSERT ProgrammeRequirement ON;

INSERT INTO ProgrammeRequirement (RequirementID, DegreeID, SubjectID, MinimumMark, IsMandatory, AdvisoryNote) VALUES
-- 1 BCom Information Systems
(1,  1, 1,  60, 1, 'Sample requirement: English at 60% or higher is needed for academic reading and writing. Sample data only, not official university information, please check with official university policies'),
(2,  1, 3,  60, 1, 'Sample requirement: Mathematics at 60% or higher is needed for the quantitative and systems modules. Sample data only, not official university information, please check with official university policies'),
(3,  1, 12, 50, 0, 'Sample recommendation: Computer Applications Technology at 50% is helpful but not compulsory. Sample data only, not official university information, please check with official university policies'),
-- 2 BCom Accounting
(4,  2, 1,  60, 1, 'Sample requirement: English at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(5,  2, 3,  60, 1, 'Sample requirement: Mathematics at 60% or higher is needed for financial and quantitative modules. Sample data only, not official university information, please check with official university policies'),
(6,  2, 7,  60, 1, 'Sample requirement: Accounting at 60% or higher shows readiness for accounting modules. Sample data only, not official university information, please check with official university policies'),
-- 3 BCom General (accepts Mathematics OR Mathematical Literacy)
(7,  3, 1,  55, 1, 'Sample requirement: English at 55% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(8,  3, 3,  50, 0, 'Sample alternative: Mathematics at 50% OR Mathematical Literacy at 60% is accepted. Sample data only, not official university information, please check with official university policies'),
(9,  3, 4,  60, 0, 'Sample alternative: Mathematical Literacy at 60% is accepted instead of Mathematics. Sample data only, not official university information, please check with official university policies'),
-- 4 BSc Computer Science
(10, 4, 1,  60, 1, 'Sample requirement: English at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(11, 4, 3,  70, 1, 'Sample requirement: Mathematics at 70% or higher is needed for programming and algorithm modules. Mathematical Literacy is not accepted. Sample data only, not official university information, please check with official university policies'),
-- 5 BSc Biological Sciences
(12, 5, 1,  60, 1, 'Sample requirement: English at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(13, 5, 3,  60, 1, 'Sample requirement: Mathematics at 60% or higher is needed for statistics and quantitative biology. Sample data only, not official university information, please check with official university policies'),
(14, 5, 5,  60, 1, 'Sample requirement: Physical Sciences at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(15, 5, 6,  60, 1, 'Sample requirement: Life Sciences at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
-- 6 BSc Engineering (Mechanical Engineering)
(16, 6, 1,  60, 1, 'Sample requirement: English at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(17, 6, 3,  70, 1, 'Sample requirement: Mathematics at 70% or higher is needed for engineering mathematics. Sample data only, not official university information, please check with official university policies'),
(18, 6, 5,  70, 1, 'Sample requirement: Physical Sciences at 70% or higher is needed for mechanics and physics. Sample data only, not official university information, please check with official university policies'),
(19, 6, 15, 60, 0, 'Sample recommendation: Engineering Graphics and Design at 60% is helpful but not compulsory. Sample data only, not official university information, please check with official university policies'),
-- 7 BA Humanities (accepts Mathematics OR Mathematical Literacy)
(20, 7, 1,  60, 1, 'Sample requirement: English at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(21, 7, 11, 50, 0, 'Sample recommendation: History at 50% is helpful but not compulsory. Sample data only, not official university information, please check with official university policies'),
(22, 7, 3,  50, 0, 'Sample alternative: Mathematics at 50% OR Mathematical Literacy at 60% is accepted. Sample data only, not official university information, please check with official university policies'),
(23, 7, 4,  60, 0, 'Sample alternative: Mathematical Literacy at 60% is accepted instead of Mathematics. Sample data only, not official university information, please check with official university policies'),
-- 8 BSocSci (accepts Mathematics OR Mathematical Literacy)
(24, 8, 1,  60, 1, 'Sample requirement: English at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(25, 8, 3,  50, 0, 'Sample alternative: Mathematics at 50% OR Mathematical Literacy at 60% is accepted. Sample data only, not official university information, please check with official university policies'),
(26, 8, 4,  60, 0, 'Sample alternative: Mathematical Literacy at 60% is accepted instead of Mathematics. Sample data only, not official university information, please check with official university policies'),
(27,8, 11, 50, 0, 'Sample recommendation: History at 50% is helpful but not compulsory. Sample data only, not official university information, please check with official university policies'),
(30, 9, 4,  60, 0, 'Sample alternative: Mathematical Literacy at 60% is accepted instead of Mathematics. Sample data only, not official university information, please check with official university policies'),
(31, 9, 11, 55, 0, 'Sample recommendation: History at 55% is helpful but not compulsory. Sample data only, not official university information, please check with official university policies'),
-- 10 Bachelor of Health Sciences
(32, 10, 1, 60, 1, 'Sample requirement: English at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(33, 10, 3, 60, 1, 'Sample requirement: Mathematics at 60% or higher is needed. Mathematical Literacy is not accepted. Sample data only, not official university information, please check with official university policies'),
(34, 10, 5, 60, 1, 'Sample requirement: Physical Sciences at 60% or higher is needed. Sample data only, not official university information, please check with official university policies'),
(35, 10, 6, 60, 1, 'Sample requirement: Life Sciences at 60% or higher is needed. Sample data only, not official university information, please check with official university policies');
SET IDENTITY_INSERT ProgrammeRequirement OFF;

SET IDENTITY_INSERT DegreeInterestMapping ON;

INSERT INTO DegreeInterestMapping (MappingID, DegreeID, CategoryID, Weight) VALUES
-- 1 BCom Information Systems
(1, 1, 3, 1.00), (2, 1, 5, 0.80), (3, 1, 1, 0.60),
-- 2 BCom Accounting
(4, 2, 5, 1.00), (5, 2, 1, 0.90),
-- 3 BCom General
(6, 3, 5, 1.00), (7, 3, 1, 0.50), (8, 3, 8, 0.40),
-- 4 BSc Computer Science
(9, 4, 3, 1.00), (10, 4, 1, 0.90), (11, 4, 6, 0.50),
-- 5 BSc Biological Sciences
(12, 5, 4, 1.00), (13, 5, 1, 0.50), (14, 5, 2, 0.40),
-- 6 BSc Engineering (Mechanical Engineering)
(15, 6, 6, 1.00), (16, 6, 1, 0.80), (17, 6, 4, 0.60), (18, 6, 3, 0.40),
-- 7 BA Humanities
(19, 7, 8, 1.00), (20, 7, 7, 0.80), (21, 7, 2, 0.50),
-- 8 BSocSci
(22, 8, 7, 1.00), (23, 8, 2, 0.80), (24, 8, 8, 0.70),
-- 9 Law-related qualification
(25, 9, 7, 1.00), (26, 9, 8, 0.90),
-- 10 Bachelor of Health Sciences
(27, 10, 2, 1.00), (28, 10, 4, 0.80), (29, 10, 1, 0.40);

SET IDENTITY_INSERT DegreeInterestMapping OFF;

/* ---------- FundingOption ----------
   SAMPLE information requiring verification. Not real funding offers.
   Faculty IDs: 1 Commerce, 2 Humanities, 3 Science, 4 Engineering, 5 Health Sciences
   InActive = 0 : active    InActive = 1 : inactive / outdated
   Dates use YYYYMMDD format so they work on any SQL Server language setting. */
SET IDENTITY_INSERT FundingOption ON;

INSERT INTO FundingOption
    (FundingID, FacultyID, FundingName, FundingType, EligibilityDescription, RequiredDocuments, ClosingDate, ContactInfo, InActive)
VALUES
(1, 1, 'SAMPLE - Commerce Merit Award', 'Academic merit',
    'Verify before use. For learners with an average of 75% or higher and a Mathematics mark of 70% or higher.',
    'Certified copy of ID; latest school report; proof of acceptance or application',
    '20270331', 'Sample contact unavailable; consult your advisor', 0),

(2, 1, 'SAMPLE - Commerce Need Grant', 'Financial need',
    'Verify before use. For learners whose combined household income is below a R150000 annual threshold.',
    'Certified copy of ID; proof of household income; latest school report',
    '20270228', 'Sample contact unavailable; consult your advisor', 0),

(3, 3, 'SAMPLE - Science Excellence Award', 'Academic merit',
    'Verify before use. For learners with 70% or higher in Mathematics and Physical Sciences or Life Sciences.',
    'Certified copy of ID; latest school report; motivation letter',
    '20270415', 'Sample contact unavailable; consult your advisor', 0),

(4, 3, 'SAMPLE - Science Access Grant', 'Financial need',
    'Verify before use. For learners from low-income households entering a science degree.',
    'Certified copy of ID; proof of household income; latest school report',
    '20270315', 'Sample contact unavailable; consult your advisor', 0),

(5, 4, 'SAMPLE - Engineering Merit Award', 'Academic merit',
    'Verify before use. For learners with 75% or higher in Mathematics and Physical Sciences.',
    'Certified copy of ID; latest school report; proof of application',
    '20270531', 'Sample contact unavailable; consult your advisor', 0),

(6, 4, 'SAMPLE - Engineering Industry Award (Outdated)', 'Corporate bursary',
    'Verify before use. This option is outdated and no longer open for applications.',
    'Certified copy of ID; latest school report',
    '20260930', 'Sample contact unavailable; consult your advisor', 1),

(7, 5, 'SAMPLE - Health Access Grant', 'Financial need',
    'Verify before use. For learners from low-income households entering a health sciences degree.',
    'Certified copy of ID; proof of household income; latest school report',
    '20270331', 'Sample contact unavailable; consult your advisor', 0),

(8, 5, 'SAMPLE - Health Merit Award (Closed)', 'Academic merit',
    'Verify before use. This option has closed for the current intake.',
    'Certified copy of ID; latest school report; motivation letter',
    '20260630', 'Sample contact unavailable; consult your advisor', 1),

(9, 2, 'SAMPLE - Humanities Future Leaders Award', 'Academic merit',
    'Verify before use. For learners with an average of 70% or higher entering a humanities or law-related programme.',
    'Certified copy of ID; latest school report; motivation letter',
    '20270430', 'Sample contact unavailable; consult your advisor', 0),

(10, 2, 'SAMPLE - Commerce Industry Award (Closed)', 'Corporate bursary',
    'Verify before use. This option is outdated and no longer available.',
    'Certified copy of ID; latest school report',
    '20251031', 'Sample contact unavailable; consult your advisor', 1);

SET IDENTITY_INSERT FundingOption OFF;

/* ---------- FundingProgramme ----------
   Links each FundingOption to the degree(s) it supports.
   Degree IDs: 1 BCom IS, 2 BCom Accounting, 3 BCom General, 4 BSc Computer Science,
   5 BSc Biological Sciences, 6 BSc Mechanical Eng, 7 BA Humanities, 8 BSocSci,
   9 Law-related, 10 Bachelor of Health Sciences */
SET IDENTITY_INSERT FundingProgramme ON;

INSERT INTO FundingProgramme (FundingProgrammeID, FundingID, DegreeID) VALUES
(1,  1, 1), (2,  1, 2), (3,  1, 3),
(4,  2, 1), (5,  2, 2), (6,  2, 3),
(7,  3, 4), (8,  3, 5),
(9,  4, 4), (10, 4, 5),
(11, 5, 6),
(12, 6, 6),
(13, 7, 10),
(14, 8, 10),
(15, 9, 7), (16, 9, 8), (17, 9, 9),
(18, 10, 1), (19, 10, 2);

SET IDENTITY_INSERT FundingProgramme OFF;

/* ---------- UserAccount ----------
   IDs 1-5  : the five required learners (exact names)
   IDs 6-10 : extra sample learners (fictional)
   ID 11    : Career Advisor test account
   ID 12    : System Administrator test account
   All school, province and contact details are FICTIONAL sample values; passwords are salted hashes.
   Sample learner hashes have no documented password. Use admin reset or create your own learner account. */
SET IDENTITY_INSERT UserAccount ON;

INSERT INTO UserAccount (UserID, Username, Password, UserRole, FirstName, LastName, SchoolName, Province, ContactNumber, CreatedAt) VALUES
(1,  'gadija.jacobs','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Gadija','Jacobs','Kuilsriver High School', 'Western Cape', 0649315511, '20260915 09:30:00'),
(2,  'aadiel-raboo.magwa','PBKDF2-SHA256$600000$OzBIMcpqjZ67aaEHa22U9w==$+McvBSoDwqDZZdRsWHoMLYnyQob0mCwqS1Z9z5+rN6k=', 'Learner', 'Aadiel-Raboo', 'Magwa','Tsomo High School', 'Eastern Cape',0713824874, '20260916 10:00:00'),
(3,  'labeeqah.rademeyer','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Labeeqah','Rademeyer','Paarl High School ', 'Western Cape',0827699392, '20260917 11:00:00'),
(4,  'siyabonga.dickson','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Siyabonga','Dickson','Pretoria Boys High School 4', 'Gauteng',0711524262, '20260918 13:30:00'),
(5,  'derek.diesel','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Derek Vin','Diesel',' Hilton College', 'KwaZulu-Natal',0835836402, '20260921 08:45:00'),
(6,  'thandi.nkosi','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Thandi','Nkosi','St Marys DSG', 'Gauteng', 0719251831, '20260922 09:15:00'),
(7,  'stef.vanwyk','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Stef',  'van Wyk', 'St Andrews School', 'Free State',0721721112, '20260924 14:00:00'),
(8,  'paxton.kgomo','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Paxton','Kgomo','Turf High', 'Limpopo', 0686401588, '20260929 10:45:00'),
(9,  'yusuf.patel','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Yusuf','Patel',NULL, NULL, NULL, '20261001 15:00:00'),
(10, 'zama.zulu','PBKDF2-SHA256$600000$xV+OHaelvDMMng2+Lq5xCg==$BLuyF8n2osQd/VdSE0IVPgW2QNybtskP29DkiU+GZps=', 'Learner', 'Zama','Zulu', 'Hazyview Combined High School','Mpumalanga',   550100110, '20261005 11:30:00'),
(11, 'advisor_test','PBKDF2-SHA256$600000$rrZAZ4iL5qsCQzWbSe0hYw==$vu+QGz4MWkqkjadsA7to3PYkj0Inx43UPwPzG2KjLbw=', 'Career Advisor','Test', 'Advisor', NULL, NULL, NULL, '20260901 08:00:00'),
(12, 'admin_test','PBKDF2-SHA256$600000$9rlzZkrmNggZ3qyU1uZtzw==$nzGzSShP0CANnaAVPMSHrVTMkQmjIqveRiLm96DlEbA=','System Administrator',  'Test', 'Admin',   NULL, NULL, NULL, '20261001 15:00:00');

SET IDENTITY_INSERT UserAccount OFF;

/* ---------- AdvisorySession ----------
   GradeLevel: 1 = Grade 11, 2 = Grade 12
   Faculty: 1 Commerce, 2 Humanities, 3 Science, 4 Engineering, 5 Health Sciences
   Degree : 1 BCom IS, 2 BCom Acc, 4 BSc CS, 5 BSc Bio, 6 Mech Eng, 7 BA Hum,
            9 Law-related, 10 Health Sciences */
SET IDENTITY_INSERT AdvisorySession ON;

INSERT INTO AdvisorySession (SessionID, UserID, GradeLevelID, FacultyID, DegreeID, SessionDate) VALUES
(1,  1,  2, 1, 1,  '20260915 10:00:00'),
(2,  2,  2, 3, 4,  '20260916 10:30:00'),
(3,  3,  1, 3, 5,  '20260917 11:30:00'),
(4,  4,  2, 4, 6,  '20260918 14:00:00'),
(5,  5,  2, 5, 10, '20260921 09:15:00'),
(6,  6,  2, 1, 2,  '20260922 09:45:00'),
(7,  7,  2, 3, 4,  '20260924 14:30:00'),
(8,  8,  2, 2, 9,  '20260929 11:15:00'),
(9,  9,  1, 3, 5,  '20261001 15:30:00'),
(10, 10, 1, 2, 7,  '20261005 12:00:00');

SET IDENTITY_INSERT AdvisorySession OFF;

/* ---------- LearnerSubject ----------
   Subject IDs: 1 English, 2 Afrikaans, 3 Mathematics, 4 Mathematical Literacy,
   5 Physical Sciences, 6 Life Sciences, 7 Accounting, 8 Business Studies,
   9 Economics, 10 Geography, 11 History, 12 CAT, 13 Life Orientation,
   14 Tourism, 15 Engineering Graphics and Design, 16 Visual Arts */
SET IDENTITY_INSERT LearnerSubject ON;

INSERT INTO LearnerSubject (LearnerSubjectID, SessionID, SubjectID, Mark) VALUES
-- Session 1: Gadija Jacobs, Grade 12
(1, 1, 1, 72), (2, 1, 3, 70), (3, 1, 7, 74), (4, 1, 8, 79), (5, 1, 9, 70), (6, 1, 13, 80),
-- Session 2: Aadiel-Raboo Magwa, Grade 12
(7, 2, 1, 65), (8, 2, 4, 78), (9, 2, 8, 71), (10, 2, 11, 69), (11, 2, 14, 75), (12, 2, 13, 82),
-- Session 3: Labeeqah Rademeyer, Grade 11
(13, 3, 1, 80), (14, 3, 3, 58), (15, 3, 5, 62), (16, 3, 6, 70), (17, 3, 10, 66), (18, 3, 13, 85),
-- Session 4: Siyabonga Dickson, Grade 12
(19, 4, 1, 70), (20, 4, 3, 75), (21, 4, 5, 78), (22, 4, 15, 73), (23, 4, 13, 80),
-- Session 5: Derek Vin Diesel, Grade 12
(24, 5, 1, 82), (25, 5, 4, 76), (26, 5, 11, 79), (27, 5, 6, 68), (28, 5, 16, 85), (29, 5, 13, 90),
-- Session 6: Thandi Nkosi, Grade 12 (Accounting below threshold)
(30, 6, 1, 68), (31, 6, 3, 62), (32, 6, 7, 55), (33, 6, 8, 70), (34, 6, 9, 64), (35, 6, 13, 75),
-- Session 7: Stef van Wyk, Grade 12 (meets requirements, low technology interest)
(36, 7, 1, 66), (37, 7, 2, 64), (38, 7, 3, 72), (39, 7, 5, 68), (40, 7, 12, 75), (41, 7, 13, 78),
-- Session 8: Paxton Kgomo, Grade 12 (Mathematical Literacy accepted)
(42, 8, 1, 74), (43, 8, 4, 72), (44, 8, 11, 68), (45, 8, 10, 65), (46, 8, 6, 60), (47, 8, 13, 80),
-- Session 9: Yusuf Patel, Grade 11 (incomplete: only two subjects entered)
(48, 9, 1, 63), (49, 9, 13, 70),
-- Session 10: Zama Zulu, Grade 11 (English below threshold)
(50, 10, 1, 58), (51, 10, 4, 65), (52, 10, 11, 72), (53, 10, 10, 66), (54, 10, 13, 77);

SET IDENTITY_INSERT LearnerSubject OFF;

/* ---------- LearnerInterestResponse ----------
   Score scale: 1.00 to 5.00 (1 = strongly disagree, 5 = strongly agree)
   Question IDs: 1 numbers/data, 2 helping people, 3 technology, 4 science,
   5 reading/writing, 6 business/finance, 7 designing/building, 8 society/law/politics
   Session 9 has NO responses (missing-information case). */
SET IDENTITY_INSERT LearnerInterestResponse ON;

INSERT INTO LearnerInterestResponse (ResponseID, SessionID, QuestionID, Score) VALUES
(1, 1, 1, 4.00), (2, 1, 2, 3.00), (3, 1, 3, 5.00), (4, 1, 4, 2.00),
(5, 1, 5, 3.00), (6, 1, 6, 5.00), (7, 1, 7, 3.00), (8, 1, 8, 2.00),
(9, 2, 1, 4.00), (10, 2, 2, 2.00), (11, 2, 3, 5.00), (12, 2, 4, 1.00),
(13, 2, 5, 3.00), (14, 2, 6, 3.00), (15, 2, 7, 4.00), (16, 2, 8, 2.00),
(17, 3, 1, 3.00), (18, 3, 2, 4.00), (19, 3, 3, 2.00), (20, 3, 4, 5.00),
(21, 3, 5, 4.00), (22, 3, 6, 1.00), (23, 3, 7, 2.00), (24, 3, 8, 3.00),
(25, 4, 1, 5.00), (26, 4, 2, 2.00), (27, 4, 3, 3.00), (28, 4, 4, 4.00),
(29, 4, 5, 2.00), (30, 4, 6, 2.00), (31, 4, 7, 5.00), (32, 4, 8, 1.00),
(33, 5, 1, 2.00), (34, 5, 2, 5.00), (35, 5, 3, 1.00), (36, 5, 4, 3.00),
(37, 5, 5, 4.00), (38, 5, 6, 2.00), (39, 5, 7, 3.00), (40, 5, 8, 3.00),
(41, 6, 1, 5.00), (42, 6, 2, 3.00), (43, 6, 3, 2.00), (44, 6, 4, 1.00),
(45, 6, 5, 3.00), (46, 6, 6, 5.00), (47, 6, 7, 2.00), (48, 6, 8, 2.00),
(49, 7, 1, 2.00), (50, 7, 2, 5.00), (51, 7, 3, 2.00), (52, 7, 4, 3.00),
(53, 7, 5, 3.00), (54, 7, 6, 1.00), (55, 7, 7, 2.00), (56, 7, 8, 3.00),
(57, 8, 1, 2.00), (58, 8, 2, 4.00), (59, 8, 3, 1.00), (60, 8, 4, 2.00),
(61, 8, 5, 5.00), (62, 8, 6, 2.00), (63, 8, 7, 2.00), (64, 8, 8, 5.00),
(65, 10, 1, 2.00), (66, 10, 2, 4.00), (67, 10, 3, 1.00), (68, 10, 4, 1.00),
(69, 10, 5, 5.00), (70, 10, 6, 2.00), (71, 10, 7, 3.00), (72, 10, 8, 5.00);

SET IDENTITY_INSERT LearnerInterestResponse OFF;

-- Recommendation rows are generated from the production rule engine below.

/* ---------- ExportReport ---------- */
SET IDENTITY_INSERT ExportReport ON;

INSERT INTO ExportReport (ReportID, SessionID, DateGenerated) VALUES
(1, 1, '20260915 10:30:00'),
(2, 4, '20260918 14:20:00'),
(3, 8, '20260929 11:40:00');

SET IDENTITY_INSERT ExportReport OFF;
UPDATE dbo.ProgrammeRequirement SET AlternativeGroup='Sample numeracy choice'
WHERE IsMandatory=0 AND AdvisoryNote LIKE 'Sample alternative:%';
-- BEGIN GENERATED ASSESSMENTS
-- Generated with the production RecommendationService and canonical sample fixtures. Empty-database seed only.
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=1,IsFinalized=1 WHERE SessionID=1;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(1,9,4.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(1,10,3.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,1,'Strong Match',98.12,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 60% minimum. Interest alignment 93.75%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,2,'Strong Match',96.45,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 60% minimum. Accounting: 74% meets the stored 60% minimum. Interest alignment 88.16%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,3,'Strong Match',94.87,'English: 72% meets the stored 55% minimum. Mathematics: 70% meets the stored 50% minimum. Interest alignment 82.89%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,4,'Strong Match',84.06,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 70% minimum. Interest alignment 80.21%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,7,'Possible Match',72.39,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 50% minimum. Interest alignment 41.3%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,8,'Possible Match',72.00,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 50% minimum. Interest alignment 40%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,6,'Not Currently Eligible',57.68,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 70% minimum. Missing required subject: Physical Sciences >= 70%. Interest alignment 58.93%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,10,'Not Currently Eligible',43.64,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 60% minimum. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 45.45%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,5,'Not Currently Eligible',43.03,'English: 72% meets the stored 60% minimum. Mathematics: 70% meets the stored 60% minimum. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 43.42%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(1,9,'Not Currently Eligible',11.05,'Missing required subject: Mathematical Literacy >= 60%. Interest alignment 36.84%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=0,IsFinalized=1 WHERE SessionID=2;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(2,9,4.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(2,10,2.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,3,'Possible Match',76.97,'English: 65% meets the stored 55% minimum. Mathematical Literacy: 78% meets the stored 60% minimum. Interest alignment 56.58%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,9,'Possible Match',71.05,'Mathematical Literacy: 78% meets the stored 60% minimum. Interest alignment 36.84%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,7,'Possible Match',70.76,'English: 65% meets the stored 60% minimum. Mathematical Literacy: 78% meets the stored 60% minimum. Interest alignment 35.87%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,8,'Possible Match',69.60,'English: 65% meets the stored 60% minimum. Mathematical Literacy: 78% meets the stored 60% minimum. Interest alignment 32%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,4,'Not Currently Eligible',65.63,'English: 65% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Interest alignment 85.42%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,1,'Not Currently Eligible',53.12,'English: 65% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Interest alignment 77.08%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,6,'Not Currently Eligible',38.75,'English: 65% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Missing required subject: Physical Sciences >= 70%. Interest alignment 62.5%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,2,'Not Currently Eligible',38.55,'English: 65% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Accounting >= 60%. Interest alignment 61.84%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,5,'Not Currently Eligible',32.50,'English: 65% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 25%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(2,10,'Not Currently Eligible',22.50,'English: 65% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 25%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=0,IsFinalized=1 WHERE SessionID=3;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(3,9,3.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(3,10,4.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,7,'Strong Match',79.89,'English: 80% meets the stored 60% minimum. Mathematics: 58% meets the stored 50% minimum. Interest alignment 66.3%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,8,'Strong Match',79.50,'English: 80% meets the stored 60% minimum. Mathematics: 58% meets the stored 50% minimum. Interest alignment 65%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,3,'Possible Match',68.68,'English: 80% meets the stored 55% minimum. Mathematics: 58% meets the stored 50% minimum. Interest alignment 28.95%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,5,'Possible Match',94.22,'English: 80% meets the stored 60% minimum. Mathematics: 58%; minimum 60%; gap 2 percentage points. Physical Sciences: 62% meets the stored 60% minimum. Life Sciences: 70% meets the stored 60% minimum. Interest alignment 81.58%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,10,'Possible Match',83.61,'English: 80% meets the stored 60% minimum. Mathematics: 58%; minimum 60%; gap 2 percentage points. Physical Sciences: 62% meets the stored 60% minimum. Life Sciences: 70% meets the stored 60% minimum. Interest alignment 79.55%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,4,'Possible Match',77.74,'English: 80% meets the stored 60% minimum. Mathematics: 58%; minimum 70%; gap 12 percentage points. Interest alignment 34.38%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,6,'Possible Match',71.61,'English: 80% meets the stored 60% minimum. Mathematics: 58%; minimum 70%; gap 12 percentage points. Physical Sciences: 62%; minimum 70%; gap 8 percentage points. Interest alignment 48.21%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,1,'Possible Match',66.38,'English: 80% meets the stored 60% minimum. Mathematics: 58%; minimum 60%; gap 2 percentage points. Interest alignment 22.92%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,2,'Not Currently Eligible',46.77,'English: 80% meets the stored 60% minimum. Mathematics: 58%; minimum 60%; gap 2 percentage points. Missing required subject: Accounting >= 60%. Interest alignment 23.68%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(3,9,'Not Currently Eligible',18.55,'Missing required subject: Mathematical Literacy >= 60%. Interest alignment 61.84%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=1,IsFinalized=1 WHERE SessionID=4;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(4,9,5.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(4,10,2.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,6,'Strong Match',96.25,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 70% minimum. Physical Sciences: 78% meets the stored 70% minimum. Interest alignment 87.5%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,4,'Strong Match',83.75,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 70% minimum. Interest alignment 79.17%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,1,'Possible Match',76.25,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 60% minimum. Interest alignment 54.17%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,3,'Possible Match',73.42,'English: 70% meets the stored 55% minimum. Mathematics: 75% meets the stored 50% minimum. Interest alignment 44.74%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,7,'Possible Match',64.89,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 50% minimum. Interest alignment 16.3%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,8,'Possible Match',64.50,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 50% minimum. Interest alignment 15%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,5,'Not Currently Eligible',66.32,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 60% minimum. Physical Sciences: 78% meets the stored 60% minimum. Missing required subject: Life Sciences >= 60%. Interest alignment 71.05%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,10,'Not Currently Eligible',62.05,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 60% minimum. Physical Sciences: 78% meets the stored 60% minimum. Missing required subject: Life Sciences >= 60%. Interest alignment 56.82%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,2,'Not Currently Eligible',58.16,'English: 70% meets the stored 60% minimum. Mathematics: 75% meets the stored 60% minimum. Missing required subject: Accounting >= 60%. Interest alignment 60.53%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(4,9,'Not Currently Eligible',3.55,'Missing required subject: Mathematical Literacy >= 60%. Interest alignment 11.84%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=0,IsFinalized=1 WHERE SessionID=5;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(5,9,2.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(5,10,5.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,8,'Strong Match',81.90,'English: 82% meets the stored 60% minimum. Mathematical Literacy: 76% meets the stored 60% minimum. Interest alignment 73%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,7,'Strong Match',81.52,'English: 82% meets the stored 60% minimum. Mathematical Literacy: 76% meets the stored 60% minimum. Interest alignment 71.74%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,9,'Strong Match',78.55,'Mathematical Literacy: 76% meets the stored 60% minimum. Interest alignment 61.84%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,3,'Possible Match',70.66,'English: 82% meets the stored 55% minimum. Mathematical Literacy: 76% meets the stored 60% minimum. Interest alignment 35.53%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,10,'Not Currently Eligible',60.45,'English: 82% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Life Sciences: 68% meets the stored 60% minimum. Interest alignment 68.18%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,5,'Not Currently Eligible',46.18,'English: 82% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Life Sciences: 68% meets the stored 60% minimum. Interest alignment 53.95%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,4,'Not Currently Eligible',35.94,'English: 82% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Interest alignment 19.79%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,1,'Not Currently Eligible',34.38,'English: 82% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Interest alignment 14.58%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,6,'Not Currently Eligible',30.71,'English: 82% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Missing required subject: Physical Sciences >= 70%. Interest alignment 35.71%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(5,2,'Not Currently Eligible',27.50,'English: 82% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Accounting >= 60%. Interest alignment 25%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=0,IsFinalized=1 WHERE SessionID=6;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(6,9,5.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(6,10,3.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,3,'Strong Match',96.84,'English: 68% meets the stored 55% minimum. Mathematics: 62% meets the stored 50% minimum. Interest alignment 89.47%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,1,'Strong Match',90.62,'English: 68% meets the stored 60% minimum. Mathematics: 62% meets the stored 60% minimum. Interest alignment 68.75%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,7,'Possible Match',72.39,'English: 68% meets the stored 60% minimum. Mathematics: 62% meets the stored 50% minimum. Interest alignment 41.3%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,8,'Possible Match',72.00,'English: 68% meets the stored 60% minimum. Mathematics: 62% meets the stored 50% minimum. Interest alignment 40%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,2,'Possible Match',99.17,'English: 68% meets the stored 60% minimum. Mathematics: 62% meets the stored 60% minimum. Accounting: 55%; minimum 60%; gap 5 percentage points. Interest alignment 100%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,4,'Possible Match',74.22,'English: 68% meets the stored 60% minimum. Mathematics: 62%; minimum 70%; gap 8 percentage points. Interest alignment 53.13%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,6,'Not Currently Eligible',51.18,'English: 68% meets the stored 60% minimum. Mathematics: 62%; minimum 70%; gap 8 percentage points. Missing required subject: Physical Sciences >= 70%. Interest alignment 41.07%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,10,'Not Currently Eligible',42.27,'English: 68% meets the stored 60% minimum. Mathematics: 62% meets the stored 60% minimum. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 40.91%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,5,'Not Currently Eligible',41.05,'English: 68% meets the stored 60% minimum. Mathematics: 62% meets the stored 60% minimum. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 36.84%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(6,9,'Not Currently Eligible',11.05,'Missing required subject: Mathematical Literacy >= 60%. Interest alignment 36.84%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=0,IsFinalized=1 WHERE SessionID=7;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(7,9,2.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(7,10,5.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,8,'Strong Match',79.80,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 50% minimum. Interest alignment 66%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,7,'Strong Match',78.26,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 50% minimum. Interest alignment 60.87%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,4,'Possible Match',77.50,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 70% minimum. Interest alignment 25%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,3,'Possible Match',65.13,'English: 66% meets the stored 55% minimum. Mathematics: 72% meets the stored 50% minimum. Interest alignment 17.11%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,1,'Possible Match',65.00,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 60% minimum. Interest alignment 16.67%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,6,'Possible Match',68.82,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 70% minimum. Physical Sciences: 68%; minimum 70%; gap 2 percentage points. Interest alignment 30.36%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,5,'Not Currently Eligible',71.18,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 60% minimum. Physical Sciences: 68% meets the stored 60% minimum. Missing required subject: Life Sciences >= 60%. Interest alignment 53.95%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,10,'Not Currently Eligible',65.45,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 60% minimum. Physical Sciences: 68% meets the stored 60% minimum. Missing required subject: Life Sciences >= 60%. Interest alignment 68.18%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,2,'Not Currently Eligible',43.55,'English: 66% meets the stored 60% minimum. Mathematics: 72% meets the stored 60% minimum. Missing required subject: Accounting >= 60%. Interest alignment 11.84%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(7,9,'Not Currently Eligible',15.00,'Missing required subject: Mathematical Literacy >= 60%. Interest alignment 50%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=1,IsFinalized=1 WHERE SessionID=8;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(8,9,2.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(8,10,4.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,9,'Strong Match',100.00,'Mathematical Literacy: 72% meets the stored 60% minimum. Interest alignment 100%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,7,'Strong Match',98.37,'English: 74% meets the stored 60% minimum. Mathematical Literacy: 72% meets the stored 60% minimum. Interest alignment 94.57%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,8,'Strong Match',97.60,'English: 74% meets the stored 60% minimum. Mathematical Literacy: 72% meets the stored 60% minimum. Interest alignment 92%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,3,'Possible Match',72.24,'English: 74% meets the stored 55% minimum. Mathematical Literacy: 72% meets the stored 60% minimum. Interest alignment 40.79%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,10,'Not Currently Eligible',44.32,'English: 74% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Life Sciences: 60% meets the stored 60% minimum. Interest alignment 47.73%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,5,'Not Currently Eligible',40.66,'English: 74% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Life Sciences: 60% meets the stored 60% minimum. Interest alignment 35.53%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,1,'Not Currently Eligible',34.38,'English: 74% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Interest alignment 14.58%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,4,'Not Currently Eligible',34.38,'English: 74% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Interest alignment 14.58%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,2,'Not Currently Eligible',27.50,'English: 74% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Accounting >= 60%. Interest alignment 25%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(8,6,'Not Currently Eligible',26.43,'English: 74% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Missing required subject: Physical Sciences >= 70%. Interest alignment 21.43%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=0,FundingRequested=0,IsFinalized=1 WHERE SessionID=9;
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,4,'More Information Needed',40.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,7,'More Information Needed',30.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 50% OR Mathematical Literacy >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,3,'More Information Needed',30.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 55% minimum. Missing required subject: Mathematics >= 50% OR Mathematical Literacy >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,1,'More Information Needed',30.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,8,'More Information Needed',30.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 50% OR Mathematical Literacy >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,5,'More Information Needed',25.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,2,'More Information Needed',20.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Accounting >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,6,'More Information Needed',20.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 70%. Missing required subject: Physical Sciences >= 70%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,10,'More Information Needed',15.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. English: 63% meets the stored 60% minimum. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(9,9,'More Information Needed',0.00,'Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment. Missing required subject: Mathematical Literacy >= 60%. Interest alignment 0%. Academic status: Incomplete. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
UPDATE dbo.AdvisorySession SET SubjectsComplete=1,FundingRequested=0,IsFinalized=1 WHERE SessionID=10;
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(10,9,2.0);
INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(10,10,4.0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,9,'Strong Match',100.00,'Mathematical Literacy: 65% meets the stored 60% minimum. Interest alignment 100%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,3,'Possible Match',72.24,'English: 58% meets the stored 55% minimum. Mathematical Literacy: 65% meets the stored 60% minimum. Interest alignment 40.79%. Academic status: Eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,7,'Possible Match',97.87,'English: 58%; minimum 60%; gap 2 percentage points. Mathematical Literacy: 65% meets the stored 60% minimum. Interest alignment 94.57%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',0);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,8,'Possible Match',97.10,'English: 58%; minimum 60%; gap 2 percentage points. Mathematical Literacy: 65% meets the stored 60% minimum. Interest alignment 92%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,4,'Not Currently Eligible',35.44,'English: 58%; minimum 60%; gap 2 percentage points. Missing required subject: Mathematics >= 70%. Interest alignment 19.79%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,1,'Not Currently Eligible',33.88,'English: 58%; minimum 60%; gap 2 percentage points. Missing required subject: Mathematics >= 60%. Interest alignment 14.58%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,2,'Not Currently Eligible',27.17,'English: 58%; minimum 60%; gap 2 percentage points. Missing required subject: Mathematics >= 60%. Missing required subject: Accounting >= 60%. Interest alignment 25%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,6,'Not Currently Eligible',27.17,'English: 58%; minimum 60%; gap 2 percentage points. Missing required subject: Mathematics >= 70%. Missing required subject: Physical Sciences >= 70%. Interest alignment 25%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,10,'Not Currently Eligible',26.34,'English: 58%; minimum 60%; gap 2 percentage points. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 38.64%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES(10,5,'Not Currently Eligible',21.46,'English: 58%; minimum 60%; gap 2 percentage points. Missing required subject: Mathematics >= 60%. Missing required subject: Physical Sciences >= 60%. Missing required subject: Life Sciences >= 60%. Interest alignment 22.37%. Academic status: Not eligible. Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.',1);
-- END GENERATED ASSESSMENTS
COMMIT TRANSACTION;
GO
