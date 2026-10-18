using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class AdvisorySessionDAL : DB
    {
        public AdvisorySessionDAL(string databaseName = DatabaseConnection.DefaultDatabase) : base(databaseName)
        {
        }

        public LearnerProfile? GetLearner(int id)
        {
            using (SqlConnection c = Open())
            {
                using (SqlCommand cmd = Command(c, "SELECT Id,Name,GradeId,School,Province,Email,Phone FROM Learner WHERE Id=@p0", null, id))
                {
                    DataTable tableData = ReadData(cmd);
                    if (tableData.Rows.Count == 0)
                    {
                        return null;
                    }

                    DataRow row = tableData.Rows[0];
                    return new LearnerProfile
                    {
                        Id = Convert.ToInt32(row[0]),
                        Name = (Convert.ToString(row[1]) ?? ""),
                        Grade = Convert.ToInt32(row[2]),
                        School = (Convert.ToString(row[3]) ?? ""),
                        Province = (Convert.ToString(row[4]) ?? ""),
                        Email = (Convert.ToString(row[5]) ?? ""),
                        Phone = (Convert.ToString(row[6]) ?? "")
                    };
                }
            }
        }

        public int SaveSession(AdvisorySession session, int? actorId = null)
        {
            using (SqlConnection c = Open())
            {
                using (SqlTransaction tx = c.BeginTransaction())
                {
                    AdvisoryInput input = session.Input;
                    LearnerProfile l = input.Learner;
                    int learnerId = l.Id;
                    if (learnerId == 0)
                    {
                        using (SqlCommand learnerCommand = Command(c, "INSERT INTO Learner(Name,GradeId,School,Province,Email,Phone) OUTPUT INSERTED.Id VALUES(@p0,@p1,@p2,@p3,@p4,@p5)", tx, l.Name, l.Grade, l.School, l.Province, l.Email, l.Phone))
                        {
                            learnerId = Convert.ToInt32(learnerCommand.ExecuteScalar());
                        }
                    }

                    else
                    {
                        Execute(c, @"UPDATE Learner SET
Name=CASE WHEN LEN(LTRIM(RTRIM(Name)))=0 THEN @p1 ELSE Name END,
School=CASE WHEN LEN(LTRIM(RTRIM(School)))=0 THEN @p2 ELSE School END,
Province=CASE WHEN LEN(LTRIM(RTRIM(Province)))=0 THEN @p3 ELSE Province END,
Email=CASE WHEN LEN(LTRIM(RTRIM(Email)))=0 THEN @p4 ELSE Email END,
Phone=CASE WHEN LEN(Phone)<>10 OR Phone COLLATE Latin1_General_100_BIN2 LIKE '%[^0-9]%' THEN @p5 ELSE Phone END
WHERE Id=@p0", tx, learnerId, l.Name, l.School, l.Province, l.Email, l.Phone);
                    }

                    using (SqlCommand sessionCommand = Command(c, "INSERT INTO AdvisorySession(LearnerId,GradeId,DegreeId,PreferredFacultyId,CreatedUtc,SubjectsComplete,FundingRequested,RuleVersion) OUTPUT INSERTED.Id VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)", tx, learnerId, l.Grade, input.DegreeId, input.PreferredFacultyId, session.CreatedUtc, input.SubjectsComplete, input.FundingRequested, session.RuleVersion))
                    {
                        int id = Convert.ToInt32(sessionCommand.ExecuteScalar());
                        foreach (SubjectMark mark in input.Marks)
                        {
                            Execute(c, "INSERT INTO LearnerSubject VALUES(@p0,@p1,@p2)", tx, id, mark.SubjectId, mark.Mark);
                        }

                        foreach (InterestResponse answer in input.Responses)
                        {
                            Execute(c, "INSERT INTO LearnerInterestResponse VALUES(@p0,@p1,@p2)", tx, id, answer.QuestionId, answer.Score);
                        }

                        SaveResults(c, tx, id, session);
                        Execute(c, "INSERT INTO AuditEvent(UserId,Action,Detail,CreatedUtc) VALUES(@p0,@p1,@p2,@p3)", tx, actorId, "Advisory session created", "Session " + id, session.CreatedUtc);
                        tx.Commit();
                        l.Id = learnerId;
                        return id;
                    }
                }
            }
        }

        private static void SaveResults(SqlConnection c, SqlTransaction tx, int id, AdvisorySession session)
        {
            int rank = 0;
            List<Recommendation> results = new List<Recommendation>();
            results.Add(session.Selected);
            results.AddRange(session.Alternatives);
            foreach (Recommendation r in results)
            {
                Execute(c, "INSERT INTO RecommendationResult VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", tx, id, r.DegreeId, r.Outcome.ToString(), r.SubjectScore, r.MarkScore, r.InterestScore, r.PreferenceScore, r.OverallScore, r.Explanation, rank++);
            }

            foreach (KeyValuePair<string, decimal> category in session.InterestScores)
            {
                Execute(c, "INSERT INTO SessionInterestScore SELECT @p0,Id,@p2 FROM InterestCategory WHERE Name=@p1", tx, id, category.Key, category.Value);
            }
        }

        public AdvisorySession? GetSession(int id)
        {
            using (SqlConnection c = Open())
            {
                AdvisoryInput input;
                DateTime created;
                string version;
                using (SqlCommand cmd = Command(c, "SELECT LearnerId,GradeId,DegreeId,PreferredFacultyId,CreatedUtc,SubjectsComplete,FundingRequested,RuleVersion FROM AdvisorySession WHERE Id=@p0", null, id))
                {
                    DataTable tableData = ReadData(cmd);
                    if (tableData.Rows.Count == 0)
                    {
                        return null;
                    }

                    DataRow row = tableData.Rows[0];
                    input = new AdvisoryInput
                    {
                        Learner = GetLearner(Convert.ToInt32(row[0]))!,
                        DegreeId = Convert.ToInt32(row[2]),
                        PreferredFacultyId = Convert.ToInt32(row[3]),
                        SubjectsComplete = Convert.ToBoolean(row[5]),
                        FundingRequested = Convert.ToBoolean(row[6])
                    };
                    input.Learner.Grade = Convert.ToInt32(row[1]);
                    created = Utc(Convert.ToDateTime(row[4]));
                    version = (Convert.ToString(row[7]) ?? "");
                }

                using (SqlCommand cmd = Command(c, "SELECT m.SubjectId,s.Name,m.Mark FROM LearnerSubject m JOIN Subject s ON s.Id=m.SubjectId WHERE m.SessionId=@p0 ORDER BY s.Name", null, id))
                {
                    foreach (DataRow row in ReadData(cmd).Rows)
                    {
                        input.Marks.Add(new SubjectMark {
                            SubjectId = Convert.ToInt32(row[0]),
                            SubjectName = (Convert.ToString(row[1]) ?? ""),
                            Mark = Convert.ToDecimal(row[2])
                        });
                    }
                }

                using (SqlCommand cmd = Command(c, "SELECT QuestionId,Score FROM LearnerInterestResponse WHERE SessionId=@p0", null, id))
                {
                    foreach (DataRow row in ReadData(cmd).Rows)
                    {
                        input.Responses.Add(new InterestResponse {
                            QuestionId = Convert.ToInt32(row[0]),
                            Score = Convert.ToInt32(row[1])
                        });
                    }
                }

                List<Recommendation> results = new List<Recommendation>();
                using (SqlCommand cmd = Command(c, "SELECT r.DegreeId,d.Name,f.Name,r.Outcome,r.SubjectScore,r.MarkScore,r.InterestScore,r.PreferenceScore,r.OverallScore,r.Explanation FROM RecommendationResult r JOIN DegreeProgramme d ON d.Id=r.DegreeId JOIN Faculty f ON f.Id=d.FacultyId WHERE r.SessionId=@p0 ORDER BY r.[Rank]", null, id))
                {
                    foreach (DataRow row in ReadData(cmd).Rows)
                    {
                        results.Add(new Recommendation {
                            DegreeId = Convert.ToInt32(row[0]),
                            Programme = (Convert.ToString(row[1]) ?? ""),
                            Faculty = (Convert.ToString(row[2]) ?? ""),
                            Outcome = Enum.Parse<AdvisoryOutcome>((Convert.ToString(row[3]) ?? "")),
                            SubjectScore = Convert.ToDecimal(row[4]),
                            MarkScore = Convert.ToDecimal(row[5]),
                            InterestScore = Convert.ToDecimal(row[6]),
                            PreferenceScore = Convert.ToDecimal(row[7]),
                            OverallScore = Convert.ToDecimal(row[8]),
                            Explanation = Lines((Convert.ToString(row[9]) ?? ""))
                        });
                    }
                }

                Dictionary<string, decimal> interests = new Dictionary<string, decimal>();
                using (SqlCommand cmd = Command(c, "SELECT i.Name,s.Score FROM SessionInterestScore s JOIN InterestCategory i ON i.Id=s.CategoryId WHERE s.SessionId=@p0", null, id))
                {
                    foreach (DataRow row in ReadData(cmd).Rows)
                    {
                        interests[(Convert.ToString(row[0]) ?? "")] = Convert.ToDecimal(row[1]);
                    }
                }

                List<Recommendation> alternatives = new List<Recommendation>();
                for (int index = 1; index < results.Count; index++)
                {
                    alternatives.Add(results[index]);
                }

                return new AdvisorySession
                {
                    Id = id,
                    CreatedUtc = created,
                    RuleVersion = version,
                    Input = input,
                    Selected = results.Count == 0 ? new Recommendation() : results[0],
                    Alternatives = alternatives,
                    InterestScores = interests
                };
            }
        }

        public List<SessionSummary> FindSessions(DateOnly from, DateOnly to, string search)
        {
            using (SqlConnection c = Open())
            {
                using (SqlCommand cmd = Command(c, @"SELECT s.Id,l.Id,l.Name,s.GradeId,s.CreatedUtc,d.Name,f.Name,r.Outcome,r.OverallScore,s.FundingRequested,r.Explanation,s.DegreeId,d.FacultyId
FROM AdvisorySession s JOIN Learner l ON l.Id=s.LearnerId JOIN DegreeProgramme d ON d.Id=s.DegreeId
JOIN Faculty f ON f.Id=d.FacultyId JOIN RecommendationResult r ON r.SessionId=s.Id AND r.[Rank]=0
WHERE CAST(s.CreatedUtc AS date)>=@p0 AND CAST(s.CreatedUtc AS date)<=@p1
  AND (CHARINDEX(@p2,l.Name)>0 OR CHARINDEX(@p2,l.School)>0 OR CHARINDEX(@p2,l.Email)>0
       OR CHARINDEX(@p2,l.Phone)>0 OR CHARINDEX(@p2,l.Province)>0 OR CHARINDEX(@p2,d.Name)>0 OR CONVERT(nvarchar(20),s.Id)=@p2)
ORDER BY s.CreatedUtc DESC,s.Id DESC", null, from.ToDateTime(TimeOnly.MinValue), to.ToDateTime(TimeOnly.MinValue), search))
                {
                    DataTable tableData = ReadData(cmd);
                    List<SessionSummary> result = new List<SessionSummary>();
                    foreach (DataRow row in tableData.Rows)
                    {
                        result.Add(new SessionSummary {
                            Id = Convert.ToInt32(row[0]),
                            LearnerId = Convert.ToInt32(row[1]),
                            Learner = (Convert.ToString(row[2]) ?? "").Length == 0 ? "Anonymous learner" : (Convert.ToString(row[2]) ?? ""),
                            Grade = Convert.ToInt32(row[3]),
                            CreatedUtc = Utc(Convert.ToDateTime(row[4])),
                            Programme = (Convert.ToString(row[5]) ?? ""),
                            Faculty = (Convert.ToString(row[6]) ?? ""),
                            Outcome = new Recommendation { Outcome = Enum.Parse<AdvisoryOutcome>((Convert.ToString(row[7]) ?? "")) }.OutcomeText,
                            Score = Convert.ToDecimal(row[8]),
                            FundingRequested = Convert.ToBoolean(row[9]),
                            Explanation = Lines((Convert.ToString(row[10]) ?? "")),
                            DegreeId = Convert.ToInt32(row[11]),
                            FacultyId = Convert.ToInt32(row[12])
                        });
                    }

                    return result;
                }
            }
        }
    }
}
