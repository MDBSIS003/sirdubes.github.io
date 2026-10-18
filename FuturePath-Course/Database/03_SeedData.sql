USE FuturePathStudentAdvisingSystem;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF EXISTS(SELECT 1 FROM dbo.[GradeLevel] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[Subject] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[Faculty] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[InterestCategory] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[DegreeProgramme] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[InterestQuestion] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[ProgrammeRequirement] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[DegreeInterestMapping] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[FundingOption] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[FundingFaculty] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[FundingProgramme] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[Learner] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[AdvisorySession] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[LearnerSubject] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[LearnerInterestResponse] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[UserAccount] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[RecommendationResult] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[SessionInterestScore] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[ExportReport] WITH (UPDLOCK,HOLDLOCK)) OR EXISTS(SELECT 1 FROM dbo.[AuditEvent] WITH (UPDLOCK,HOLDLOCK))
BEGIN PRINT 'Sample seed skipped: existing records were preserved.'; COMMIT TRANSACTION; RETURN; END;
INSERT dbo.[GradeLevel]([Id],[Name]) VALUES(11,N'Grade 11');
INSERT dbo.[GradeLevel]([Id],[Name]) VALUES(12,N'Grade 12');
SET IDENTITY_INSERT dbo.[Subject] ON;
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(1,N'English',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(2,N'Afrikaans',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(3,N'Mathematics',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(4,N'Mathematical Literacy',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(5,N'Physical Sciences',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(6,N'Life Sciences',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(7,N'Accounting',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(8,N'Business Studies',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(9,N'Economics',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(10,N'Geography',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(11,N'History',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(12,N'Computer Applications Technology',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(13,N'Life Orientation',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(14,N'Tourism',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(15,N'Engineering Graphics and Design',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(16,N'Visual Arts',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(17,N'Zulu',1);
INSERT dbo.[Subject]([Id],[Name],[Active]) VALUES(18,N'Xhosa',1);
SET IDENTITY_INSERT dbo.[Subject] OFF;
SET IDENTITY_INSERT dbo.[Faculty] ON;
INSERT dbo.[Faculty]([Id],[Name],[Active]) VALUES(1,N'Commerce',1);
INSERT dbo.[Faculty]([Id],[Name],[Active]) VALUES(2,N'Humanities',1);
INSERT dbo.[Faculty]([Id],[Name],[Active]) VALUES(3,N'Science',1);
INSERT dbo.[Faculty]([Id],[Name],[Active]) VALUES(4,N'Engineering',1);
INSERT dbo.[Faculty]([Id],[Name],[Active]) VALUES(5,N'Health Sciences',1);
SET IDENTITY_INSERT dbo.[Faculty] OFF;
SET IDENTITY_INSERT dbo.[InterestCategory] ON;
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(1,N'Numeracy',1);
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(2,N'Helping People',1);
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(3,N'Technology',1);
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(4,N'Science and Research',1);
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(5,N'Business and Finance',1);
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(6,N'Design Thinking',1);
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(7,N'Society and Politics',1);
INSERT dbo.[InterestCategory]([Id],[Name],[Active]) VALUES(8,N'Reading, Writing and Analysis',1);
SET IDENTITY_INSERT dbo.[InterestCategory] OFF;
SET IDENTITY_INSERT dbo.[DegreeProgramme] ON;
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(1,N'BCom Information Systems',1,N'Business knowledge, information technology and systems. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(2,N'BCom Accounting',1,N'Financial accounting, auditing and taxation. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(3,N'BCom General',1,N'Business, economics and management. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(4,N'BSc Computer Science',3,N'Programming, algorithms and software development. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(5,N'BSc Biological Sciences',3,N'Living organisms, ecology and biological research. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(6,N'BSc Engineering (Mechanical Engineering)',4,N'Design and analysis of mechanical systems. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(7,N'BA Humanities',2,N'Languages, history and the social world. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(8,N'BSocSci',2,N'Society, politics and human behaviour. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(9,N'Law-related qualification',2,N'Legal reasoning, debate and society. SAMPLE programme; verify official university requirements.',0,1);
INSERT dbo.[DegreeProgramme]([Id],[Name],[FacultyId],[Description],[MinimumAverage],[Active]) VALUES(10,N'Bachelor of Health Sciences',5,N'Clinical and related health sciences. SAMPLE programme; verify official university requirements.',0,1);
SET IDENTITY_INSERT dbo.[DegreeProgramme] OFF;
SET IDENTITY_INSERT dbo.[InterestQuestion] ON;
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(1,N'I enjoy solving problems using numbers or data.',1,1);
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(2,N'I enjoy helping people and communities.',2,1);
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(3,N'I enjoy working with technology.',3,1);
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(4,N'I enjoy science experiments and research.',4,1);
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(5,N'I enjoy reading, writing and analysing ideas.',8,1);
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(6,N'I enjoy business, entrepreneurship and finance.',5,1);
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(7,N'I enjoy designing or building practical solutions.',6,1);
INSERT dbo.[InterestQuestion]([Id],[Text],[CategoryId],[Active]) VALUES(8,N'I enjoy understanding society, law or politics.',7,1);
SET IDENTITY_INSERT dbo.[InterestQuestion] OFF;
SET IDENTITY_INSERT dbo.[ProgrammeRequirement] ON;
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(1,1,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(2,1,3,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(3,1,12,50,0,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(4,2,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(5,2,3,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(6,2,7,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(7,3,1,55,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(8,3,3,50,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(9,3,4,60,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(10,4,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(11,4,3,70,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(12,5,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(13,5,3,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(14,5,5,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(15,5,6,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(16,6,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(17,6,3,70,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(18,6,5,70,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(19,6,15,60,0,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(20,7,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(21,7,11,50,0,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(22,7,3,50,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(23,7,4,60,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(24,8,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(25,8,3,50,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(26,8,4,60,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(27,8,11,50,0,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(28,9,1,65,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(29,9,3,50,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(30,9,4,60,1,1,N'Sample requirement. Either subject in this OR group is accepted.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(31,9,11,55,0,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(32,10,1,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(33,10,3,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(34,10,5,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
INSERT dbo.[ProgrammeRequirement]([Id],[DegreeId],[SubjectId],[MinimumMark],[Mandatory],[AlternativeGroup],[Note],[Active]) VALUES(35,10,6,60,1,0,N'Sample requirement. Verify official entry criteria.',1);
SET IDENTITY_INSERT dbo.[ProgrammeRequirement] OFF;
SET IDENTITY_INSERT dbo.[DegreeInterestMapping] ON;
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(1,1,3,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(2,1,5,0.8,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(3,1,1,0.6,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(4,2,5,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(5,2,1,0.9,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(6,3,5,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(7,3,1,0.5,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(8,3,8,0.4,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(9,4,3,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(10,4,1,0.9,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(11,4,6,0.5,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(12,5,4,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(13,5,1,0.5,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(14,5,2,0.4,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(15,6,6,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(16,6,1,0.8,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(17,6,4,0.6,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(18,6,3,0.4,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(19,7,8,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(20,7,7,0.8,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(21,7,2,0.5,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(22,8,7,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(23,8,2,0.8,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(24,8,8,0.7,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(25,9,7,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(26,9,8,0.9,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(27,10,2,1,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(28,10,4,0.8,1);
INSERT dbo.[DegreeInterestMapping]([Id],[DegreeId],[CategoryId],[Weight],[Active]) VALUES(29,10,1,0.4,1);
SET IDENTITY_INSERT dbo.[DegreeInterestMapping] OFF;
SET IDENTITY_INSERT dbo.[FundingOption] ON;
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(1,N'Commerce Merit Award',N'Merit award',N'Academic merit: sample average 75% and Mathematics 70%.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2027-03-31',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,1);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(2,N'Commerce Access Bursary',N'Bursary',N'Financial need: sample household income threshold R150,000 annually.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2027-02-28',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,1);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(3,N'Science Excellence Scholarship',N'Scholarship',N'Academic merit: sample Mathematics and science marks at least 70%.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2027-04-15',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,1);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(4,N'Science Opportunity Grant',N'Financial aid',N'Financial need: sample support for low-income households entering science.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2027-03-15',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,1);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(5,N'Engineering Future Scholarship',N'Scholarship',N'Academic merit: sample Mathematics and Physical Sciences marks at least 75%.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2027-05-31',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,1);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(6,N'Engineering Industry Bursary (archived)',N'Bursary',N'Archived sample award, no longer open.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2026-09-30',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,0);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(7,N'Health Sciences Access Fund',N'Financial aid',N'Financial need: sample support for health sciences learners.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2027-03-31',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,1);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(8,N'Health Merit Award (closed)',N'Merit award',N'Closed sample award.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2026-06-30',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,0);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(9,N'Humanities Future Leaders Award',N'Merit award',N'Academic merit: sample average at least 70% entering humanities or law.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2027-04-30',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,1);
INSERT dbo.[FundingOption]([Id],[Name],[Type],[Eligibility],[Documents],[ClosingDate],[Contact],[Sample],[Active]) VALUES(10,N'Community Study Support (archived)',N'External funding',N'Archived sample award.',N'ID; latest school report; proof of household income where applicable; proof of application',CONVERT(date,N'2025-10-31',23),N'Fictional example only — contact your school career advisor to verify actual providers.',1,0);
SET IDENTITY_INSERT dbo.[FundingOption] OFF;
SET IDENTITY_INSERT dbo.[FundingFaculty] ON;
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(1,1,1,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(2,2,1,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(3,3,3,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(4,4,3,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(5,5,4,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(6,6,4,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(7,7,5,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(8,8,5,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(9,9,2,1);
INSERT dbo.[FundingFaculty]([Id],[FundingId],[FacultyId],[Active]) VALUES(10,10,1,1);
SET IDENTITY_INSERT dbo.[FundingFaculty] OFF;
SET IDENTITY_INSERT dbo.[FundingProgramme] ON;
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(1,1,1,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(2,1,2,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(3,1,3,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(4,2,1,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(5,2,2,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(6,2,3,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(7,3,4,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(8,3,5,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(9,4,4,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(10,4,5,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(11,5,6,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(12,6,6,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(13,7,10,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(14,8,10,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(15,9,7,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(16,9,8,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(17,9,9,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(18,10,1,1);
INSERT dbo.[FundingProgramme]([Id],[FundingId],[DegreeId],[Active]) VALUES(19,10,2,1);
SET IDENTITY_INSERT dbo.[FundingProgramme] OFF;
SET IDENTITY_INSERT dbo.[Learner] ON;
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(1,N'Gadija Jacobs',12,N'Kuilsriver High School',N'Western Cape',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(2,N'Aadiel-Raboo Magwa',12,N'Tsomo High School',N'Eastern Cape',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(3,N'Labeeqah Rademeyer',11,N'Paarl High School',N'Western Cape',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(4,N'Siyabonga Dickson',12,N'Pretoria Boys High School',N'Gauteng',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(5,N'Derek Vin Diesel',12,N'Hilton College',N'KwaZulu-Natal',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(6,N'Thandi Nkosi',12,N'St Marys DSG',N'Gauteng',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(7,N'Stef Van Wyk',12,N'St Andrews School',N'Free State',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(8,N'Paxton Kgomo',12,N'Turf High',N'Limpopo',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(9,N'Yusuf Patel',11,N'',N'',N'',N'');
INSERT dbo.[Learner]([Id],[Name],[GradeId],[School],[Province],[Email],[Phone]) VALUES(10,N'Zama Zulu',11,N'Hazyview Combined High School',N'Mpumalanga',N'',N'');
SET IDENTITY_INSERT dbo.[Learner] OFF;
SET IDENTITY_INSERT dbo.[AdvisorySession] ON;
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(1,1,12,1,1,CONVERT(datetime2(7),N'2026-09-15T10:00:00.0000000',126),1,1,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(2,2,12,4,3,CONVERT(datetime2(7),N'2026-09-16T10:00:00.0000000',126),1,0,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(3,3,11,5,3,CONVERT(datetime2(7),N'2026-09-17T10:00:00.0000000',126),1,0,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(4,4,12,6,4,CONVERT(datetime2(7),N'2026-09-18T10:00:00.0000000',126),1,1,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(5,5,12,10,5,CONVERT(datetime2(7),N'2026-09-19T10:00:00.0000000',126),1,0,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(6,6,12,2,1,CONVERT(datetime2(7),N'2026-09-20T10:00:00.0000000',126),1,1,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(7,7,12,4,3,CONVERT(datetime2(7),N'2026-09-21T10:00:00.0000000',126),1,0,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(8,8,12,9,2,CONVERT(datetime2(7),N'2026-09-22T10:00:00.0000000',126),1,1,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(9,9,11,5,3,CONVERT(datetime2(7),N'2026-09-23T10:00:00.0000000',126),0,0,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
INSERT dbo.[AdvisorySession]([Id],[LearnerId],[GradeId],[DegreeId],[PreferredFacultyId],[CreatedUtc],[SubjectsComplete],[FundingRequested],[RuleVersion]) VALUES(10,10,11,7,2,CONVERT(datetime2(7),N'2026-09-24T10:00:00.0000000',126),1,0,N'FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60');
SET IDENTITY_INSERT dbo.[AdvisorySession] OFF;
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(1,1,72);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(1,3,70);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(1,7,74);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(1,8,79);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(1,9,70);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(1,13,80);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(2,1,65);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(2,4,78);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(2,8,71);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(2,11,69);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(2,14,75);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(2,13,82);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(3,1,80);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(3,3,58);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(3,5,62);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(3,6,70);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(3,10,66);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(3,13,85);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(4,1,70);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(4,3,75);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(4,5,78);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(4,15,73);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(4,13,80);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(5,1,82);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(5,4,76);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(5,11,79);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(5,6,68);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(5,16,85);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(5,13,90);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(6,1,68);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(6,3,62);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(6,7,55);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(6,8,70);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(6,9,64);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(6,13,75);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(7,1,66);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(7,2,64);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(7,3,72);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(7,5,68);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(7,12,75);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(7,13,78);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(8,1,74);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(8,4,72);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(8,11,68);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(8,10,65);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(8,6,60);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(8,13,80);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(9,1,63);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(9,13,70);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(10,1,58);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(10,4,65);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(10,11,72);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(10,10,66);
INSERT dbo.[LearnerSubject]([SessionId],[SubjectId],[Mark]) VALUES(10,13,77);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,1,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,2,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,3,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,4,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,5,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,6,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,7,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(1,8,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,1,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,2,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,3,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,4,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,5,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,6,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,7,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(2,8,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,1,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,2,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,3,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,4,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,5,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,6,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,7,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(3,8,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,1,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,2,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,3,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,4,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,5,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,6,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,7,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(4,8,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,1,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,2,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,3,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,4,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,5,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,6,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,7,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(5,8,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,1,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,2,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,3,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,4,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,5,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,6,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,7,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(6,8,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,1,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,2,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,3,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,4,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,5,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,6,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,7,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(7,8,3);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,1,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,2,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,3,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,4,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,5,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,6,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,7,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(8,8,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,1,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,2,4);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,3,1);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,4,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,5,5);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,6,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,7,2);
INSERT dbo.[LearnerInterestResponse]([SessionId],[QuestionId],[Score]) VALUES(10,8,5);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(1,1,N'StrongMatch',100,100,93.75,100,98.44,N'Met: English 72% (minimum 60%).
Met: Mathematics 70% (minimum 60%).
Interest alignment: 93.75%. Your interests align with this study area.
Helpful, not compulsory: Computer Applications Technology at 50%. Sample requirement. Verify official entry criteria.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(1,2,N'StrongMatch',100,100,88.16,100,97.04,N'Met: English 72% (minimum 60%).
Met: Mathematics 70% (minimum 60%).
Met: Accounting 74% (minimum 60%).
Interest alignment: 88.16%. Your interests align with this study area.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(1,3,N'StrongMatch',100,100,82.89,100,95.72,N'Met: English 72% (minimum 55%).
Met: Mathematics 70% (minimum 50%).
Interest alignment: 82.89%. Your interests align with this study area.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(1,4,N'StrongMatch',100,100,80.21,0,90.05,N'Met: English 72% (minimum 60%).
Met: Mathematics 70% (minimum 70%).
Interest alignment: 80.21%. Your interests align with this study area.',3);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(1,7,N'PossibleMatch',100,100,41.30,0,80.33,N'Met: English 72% (minimum 60%).
Met: Mathematics 70% (minimum 50%).
Interest alignment: 41.3%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',4);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(1,8,N'PossibleMatch',100,100,40,0,80.00,N'Met: English 72% (minimum 60%).
Met: Mathematics 70% (minimum 50%).
Interest alignment: 40%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',5);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,75 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,50 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,100 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,25 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,50 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,100 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,50 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 1,Id,25 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(2,4,N'NotCurrentlyEligible',50,50,85.42,100,61.35,N'Met: English 65% (minimum 60%).
Required: Mathematics at 70%. No mark was supplied. This required subject is missing from your confirmed subject list.
Interest alignment: 85.42%. Your interests align with this study area.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(2,3,N'PossibleMatch',100,100,56.58,0,84.14,N'Met: English 65% (minimum 55%).
Met: Mathematical Literacy 78% (minimum 60%).
Interest alignment: 56.58%. Consider alternatives that align more closely with your interests.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(2,9,N'PossibleMatch',100,100,36.84,0,79.21,N'Met: English 65% (minimum 65%).
Met: Mathematical Literacy 78% (minimum 60%).
Interest alignment: 36.84%. Consider alternatives that align more closely with your interests.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(2,7,N'PossibleMatch',100,100,35.87,0,78.97,N'Met: English 65% (minimum 60%).
Met: Mathematical Literacy 78% (minimum 60%).
Interest alignment: 35.87%. Consider alternatives that align more closely with your interests.',3);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(2,8,N'PossibleMatch',100,100,32,0,78.00,N'Met: English 65% (minimum 60%).
Met: Mathematical Literacy 78% (minimum 60%).
Interest alignment: 32%. Consider alternatives that align more closely with your interests.',4);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,75 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,25 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,100 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,0 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,50 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,50 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,75 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 2,Id,25 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(3,5,N'PossibleMatch',100,99.17,81.58,100,95.10,N'Met: English 80% (minimum 60%).
Improve Mathematics: 58% entered; 60% required (gap 2 percentage points).
Met: Physical Sciences 62% (minimum 60%).
Met: Life Sciences 70% (minimum 60%).
Interest alignment: 81.58%. Your interests align with this study area.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(3,7,N'StrongMatch',100,100,66.30,0,86.58,N'Met: English 80% (minimum 60%).
Met: Mathematics 58% (minimum 50%).
Interest alignment: 66.3%. Your interests align with this study area.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(3,8,N'StrongMatch',100,100,65,0,86.25,N'Met: English 80% (minimum 60%).
Met: Mathematics 58% (minimum 50%).
Interest alignment: 65%. Your interests align with this study area.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(3,9,N'StrongMatch',100,100,61.84,0,85.46,N'Met: English 80% (minimum 65%).
Met: Mathematics 58% (minimum 50%).
Interest alignment: 61.84%. Your interests align with this study area.
Helpful, not compulsory: History at 55%. Sample requirement. Verify official entry criteria.',3);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(3,10,N'PossibleMatch',100,99.17,79.55,0,89.59,N'Met: English 80% (minimum 60%).
Improve Mathematics: 58% entered; 60% required (gap 2 percentage points).
Met: Physical Sciences 62% (minimum 60%).
Met: Life Sciences 70% (minimum 60%).
Interest alignment: 79.55%. Your interests align with this study area.',4);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(3,3,N'PossibleMatch',100,100,28.95,0,77.24,N'Met: English 80% (minimum 55%).
Met: Mathematics 58% (minimum 50%).
Interest alignment: 28.95%. Consider alternatives that align more closely with your interests.',5);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,50 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,75 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,25 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,100 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,75 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,0 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,25 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 3,Id,50 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(4,6,N'StrongMatch',100,100,87.5,100,96.88,N'Met: English 70% (minimum 60%).
Met: Mathematics 75% (minimum 70%).
Met: Physical Sciences 78% (minimum 70%).
Interest alignment: 87.5%. Your interests align with this study area.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(4,4,N'StrongMatch',100,100,79.17,0,89.79,N'Met: English 70% (minimum 60%).
Met: Mathematics 75% (minimum 70%).
Interest alignment: 79.17%. Your interests align with this study area.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(4,1,N'PossibleMatch',100,100,54.17,0,83.54,N'Met: English 70% (minimum 60%).
Met: Mathematics 75% (minimum 60%).
Interest alignment: 54.17%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: Computer Applications Technology at 50%. Sample requirement. Verify official entry criteria.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(4,3,N'PossibleMatch',100,100,44.74,0,81.18,N'Met: English 70% (minimum 55%).
Met: Mathematics 75% (minimum 50%).
Interest alignment: 44.74%. Consider alternatives that align more closely with your interests.',3);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(4,7,N'PossibleMatch',100,100,16.30,0,74.08,N'Met: English 70% (minimum 60%).
Met: Mathematics 75% (minimum 50%).
Interest alignment: 16.3%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',4);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(4,8,N'PossibleMatch',100,100,15,0,73.75,N'Met: English 70% (minimum 60%).
Met: Mathematics 75% (minimum 50%).
Interest alignment: 15%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',5);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,100 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,25 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,50 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,75 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,25 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,25 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,100 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 4,Id,0 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(5,10,N'NotCurrentlyEligible',50,50,68.18,100,57.05,N'Met: English 82% (minimum 60%).
Required: Mathematics at 60%. No mark was supplied. This required subject is missing from your confirmed subject list.
Required: Physical Sciences at 60%. No mark was supplied. This required subject is missing from your confirmed subject list.
Met: Life Sciences 68% (minimum 60%).
Interest alignment: 68.18%. Your interests align with this study area.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(5,8,N'StrongMatch',100,100,73,0,88.25,N'Met: English 82% (minimum 60%).
Met: Mathematical Literacy 76% (minimum 60%).
Interest alignment: 73%. Your interests align with this study area.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(5,7,N'StrongMatch',100,100,71.74,0,87.93,N'Met: English 82% (minimum 60%).
Met: Mathematical Literacy 76% (minimum 60%).
Interest alignment: 71.74%. Your interests align with this study area.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(5,9,N'StrongMatch',100,100,61.84,0,85.46,N'Met: English 82% (minimum 65%).
Met: Mathematical Literacy 76% (minimum 60%).
Interest alignment: 61.84%. Your interests align with this study area.',3);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(5,3,N'PossibleMatch',100,100,35.53,0,78.88,N'Met: English 82% (minimum 55%).
Met: Mathematical Literacy 76% (minimum 60%).
Interest alignment: 35.53%. Consider alternatives that align more closely with your interests.',4);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,25 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,100 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,0 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,50 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,75 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,25 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,50 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 5,Id,50 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(6,2,N'PossibleMatch',100,97.22,100,100,99.03,N'Met: English 68% (minimum 60%).
Met: Mathematics 62% (minimum 60%).
Improve Accounting: 55% entered; 60% required (gap 5 percentage points).
Interest alignment: 100%. Your interests align with this study area.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(6,3,N'StrongMatch',100,100,89.47,100,97.37,N'Met: English 68% (minimum 55%).
Met: Mathematics 62% (minimum 50%).
Interest alignment: 89.47%. Your interests align with this study area.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(6,1,N'StrongMatch',100,100,68.75,100,92.19,N'Met: English 68% (minimum 60%).
Met: Mathematics 62% (minimum 60%).
Interest alignment: 68.75%. Your interests align with this study area.
Helpful, not compulsory: Computer Applications Technology at 50%. Sample requirement. Verify official entry criteria.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(6,7,N'PossibleMatch',100,100,41.30,0,80.33,N'Met: English 68% (minimum 60%).
Met: Mathematics 62% (minimum 50%).
Interest alignment: 41.3%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',3);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(6,8,N'PossibleMatch',100,100,40,0,80.00,N'Met: English 68% (minimum 60%).
Met: Mathematics 62% (minimum 50%).
Interest alignment: 40%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',4);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(6,9,N'PossibleMatch',100,100,36.84,0,79.21,N'Met: English 68% (minimum 65%).
Met: Mathematics 62% (minimum 50%).
Interest alignment: 36.84%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 55%. Sample requirement. Verify official entry criteria.',5);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,100 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,50 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,25 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,0 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,50 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,100 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,25 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 6,Id,25 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(7,4,N'PossibleMatch',100,100,25,100,81.25,N'Met: English 66% (minimum 60%).
Met: Mathematics 72% (minimum 70%).
Interest alignment: 25%. Consider alternatives that align more closely with your interests.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(7,8,N'StrongMatch',100,100,66,0,86.50,N'Met: English 66% (minimum 60%).
Met: Mathematics 72% (minimum 50%).
Interest alignment: 66%. Your interests align with this study area.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(7,7,N'StrongMatch',100,100,60.87,0,85.22,N'Met: English 66% (minimum 60%).
Met: Mathematics 72% (minimum 50%).
Interest alignment: 60.87%. Your interests align with this study area.
Helpful, not compulsory: History at 50%. Sample requirement. Verify official entry criteria.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(7,9,N'PossibleMatch',100,100,50,0,82.50,N'Met: English 66% (minimum 65%).
Met: Mathematics 72% (minimum 50%).
Interest alignment: 50%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: History at 55%. Sample requirement. Verify official entry criteria.',3);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(7,6,N'PossibleMatch',100,99.05,30.36,0,77.26,N'Met: English 66% (minimum 60%).
Met: Mathematics 72% (minimum 70%).
Improve Physical Sciences: 68% entered; 70% required (gap 2 percentage points).
Interest alignment: 30.36%. Consider alternatives that align more closely with your interests.
Helpful, not compulsory: Engineering Graphics and Design at 60%. Sample requirement. Verify official entry criteria.',4);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(7,3,N'PossibleMatch',100,100,17.11,0,74.28,N'Met: English 66% (minimum 55%).
Met: Mathematics 72% (minimum 50%).
Interest alignment: 17.11%. Consider alternatives that align more closely with your interests.',5);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,25 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,100 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,25 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,50 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,50 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,0 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,25 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 7,Id,50 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(8,9,N'StrongMatch',100,100,100,100,100.00,N'Met: English 74% (minimum 65%).
Met: Mathematical Literacy 72% (minimum 60%).
Interest alignment: 100%. Your interests align with this study area.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(8,7,N'StrongMatch',100,100,94.57,100,98.64,N'Met: English 74% (minimum 60%).
Met: Mathematical Literacy 72% (minimum 60%).
Interest alignment: 94.57%. Your interests align with this study area.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(8,8,N'StrongMatch',100,100,92,100,98.00,N'Met: English 74% (minimum 60%).
Met: Mathematical Literacy 72% (minimum 60%).
Interest alignment: 92%. Your interests align with this study area.',2);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(8,3,N'PossibleMatch',100,100,40.79,0,80.20,N'Met: English 74% (minimum 55%).
Met: Mathematical Literacy 72% (minimum 60%).
Interest alignment: 40.79%. Consider alternatives that align more closely with your interests.',3);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,25 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,75 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,0 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,25 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,100 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,25 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,25 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 8,Id,100 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(9,5,N'MoreInformationNeeded',25,25,0,100,22.50,N'Met: English 63% (minimum 60%).
Required: Mathematics at 60%. No mark was supplied. Complete your subject list to confirm this requirement.
Required: Physical Sciences at 60%. No mark was supplied. Complete your subject list to confirm this requirement.
Required: Life Sciences at 60%. No mark was supplied. Complete your subject list to confirm this requirement.
Confirm that all current subjects and marks have been captured.
Complete the interest questionnaire (8 unanswered active questions).
Interest alignment: 0%. Consider alternatives that align more closely with your interests.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(10,7,N'PossibleMatch',100,98.33,94.57,100,98.06,N'Improve English: 58% entered; 60% required (gap 2 percentage points).
Met: Mathematical Literacy 65% (minimum 60%).
Interest alignment: 94.57%. Your interests align with this study area.',0);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(10,8,N'PossibleMatch',100,98.33,92,100,97.42,N'Improve English: 58% entered; 60% required (gap 2 percentage points).
Met: Mathematical Literacy 65% (minimum 60%).
Interest alignment: 92%. Your interests align with this study area.',1);
INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES(10,3,N'PossibleMatch',100,100,40.79,0,80.20,N'Met: English 58% (minimum 55%).
Met: Mathematical Literacy 65% (minimum 60%).
Interest alignment: 40.79%. Consider alternatives that align more closely with your interests.',2);
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,25 FROM dbo.InterestCategory WHERE Name=N'Numeracy';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,75 FROM dbo.InterestCategory WHERE Name=N'Helping People';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,0 FROM dbo.InterestCategory WHERE Name=N'Technology';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,25 FROM dbo.InterestCategory WHERE Name=N'Science and Research';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,100 FROM dbo.InterestCategory WHERE Name=N'Reading, Writing and Analysis';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,25 FROM dbo.InterestCategory WHERE Name=N'Business and Finance';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,25 FROM dbo.InterestCategory WHERE Name=N'Design Thinking';
INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT 10,Id,100 FROM dbo.InterestCategory WHERE Name=N'Society and Politics';
INSERT dbo.UserAccount(Username,PasswordHash,Role,Active) VALUES(N'admin_test',N'210000:8WIgqIxYkTg1AzXPfsT1YA==:MbK6L/ObrY90cOYCAHk4IqhPe521lOLoyaXFCqL4qHw=',N'Administrator',1);
INSERT dbo.UserAccount(Username,PasswordHash,Role,Active) VALUES(N'advisor_test',N'210000:rVxSH2GVNeDwXRnBiex0sQ==:n0ZHVx0A4+84+4lPhXWgU8s9WDqPA6R0B2jUw8MaCJ0=',N'Advisor',1);
COMMIT TRANSACTION;
GO
