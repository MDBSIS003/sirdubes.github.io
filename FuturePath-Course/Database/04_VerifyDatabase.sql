USE FuturePathStudentAdvisingSystem;
GO

SELECT 'Learner' AS Entity, COUNT(*) AS Rows FROM dbo.Learner
UNION ALL SELECT 'Subject',COUNT(*) FROM dbo.Subject
UNION ALL SELECT 'Faculty',COUNT(*) FROM dbo.Faculty
UNION ALL SELECT 'DegreeProgramme',COUNT(*) FROM dbo.DegreeProgramme
UNION ALL SELECT 'InterestCategory',COUNT(*) FROM dbo.InterestCategory
UNION ALL SELECT 'InterestQuestion',COUNT(*) FROM dbo.InterestQuestion
UNION ALL SELECT 'FundingOption',COUNT(*) FROM dbo.FundingOption
UNION ALL SELECT 'AdvisorySession',COUNT(*) FROM dbo.AdvisorySession;
SELECT l.Name,d.Name AS Programme,r.Outcome,expected.ExpectedOutcome,
 CASE WHEN r.Outcome=expected.ExpectedOutcome THEN 'PASS' ELSE 'CHECK' END AS ScenarioCheck,
 r.OverallScore,r.Explanation,s.RuleVersion
FROM dbo.AdvisorySession s JOIN dbo.Learner l ON l.Id=s.LearnerId
JOIN dbo.DegreeProgramme d ON d.Id=s.DegreeId
JOIN dbo.RecommendationResult r ON r.SessionId=s.Id AND r.[Rank]=0
JOIN (VALUES (N'Gadija Jacobs',N'StrongMatch'),(N'Aadiel-Raboo Magwa',N'NotCurrentlyEligible'),
 (N'Labeeqah Rademeyer',N'PossibleMatch'),(N'Siyabonga Dickson',N'StrongMatch'),
 (N'Derek Vin Diesel',N'NotCurrentlyEligible')) expected(LearnerName,ExpectedOutcome) ON expected.LearnerName=l.Name
WHERE l.Name IN (N'Gadija Jacobs',N'Aadiel-Raboo Magwa',N'Labeeqah Rademeyer',N'Siyabonga Dickson',N'Derek Vin Diesel')
ORDER BY s.Id;
IF EXISTS(SELECT 1 FROM dbo.LearnerSubject WHERE Mark<0 OR Mark>100)
 THROW 51000,'Invalid subject mark.',1;
IF EXISTS(SELECT 1 FROM dbo.LearnerInterestResponse WHERE Score NOT BETWEEN 1 AND 5)
 THROW 51000,'Invalid interest response.',1;
IF EXISTS(SELECT 1 FROM dbo.AdvisorySession s WHERE NOT EXISTS(SELECT 1 FROM dbo.RecommendationResult r WHERE r.SessionId=s.Id AND r.[Rank]=0))
 THROW 51000,'Assessment is missing its selected recommendation.',1;
DBCC CHECKCONSTRAINTS WITH ALL_CONSTRAINTS;
GO
