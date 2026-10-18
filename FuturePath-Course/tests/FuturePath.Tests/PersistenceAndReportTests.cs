using FuturePath.Business;
using FuturePath.Data;
using Microsoft.Data.SqlClient;

namespace FuturePath.Tests;
public sealed class PersistenceAndReportTests
{
    [LocalDbFact]
    public void SeedMeetsPartBCountsAndIsRepeatable()
    {
        using var db = new TestDatabase();
        db.Admin();
        db.Database.ApplySetupAgain();
        Assert.True(db.Advising.Subjects().Count >= 16);
        Assert.True(db.Advising.Faculties().Count >= 5);
        Assert.True(db.Advising.Programmes().Count >= 10);
        Assert.True(db.Advising.Questions().Count >= 8);
        Assert.True(db.Maintenance.Rows("InterestCategory").Count >= 5);
        Assert.True(db.Maintenance.Rows("FundingOption").Count >= 8);
        var sessions = db.Advising.Search(DateOnly.MinValue, DateOnly.MaxValue);
        Assert.Equal(10, sessions.Count);
        Assert.Equal(10, sessions.Select(s => s.LearnerId).Distinct().Count());
        Assert.All(db.Advising.Programmes(), p =>
        {
            Assert.NotEmpty(p.Requirements);
            Assert.NotEmpty(p.Interests);
        });
        Assert.True(db.Advising.Programmes().Count(p => p.Requirements.Any(r => r.Subject == "Mathematics" && r.Mandatory)) >= 3);
        Assert.True(db.Advising.Programmes().Count(p => p.Requirements.Any(r => r.Subject == "Physical Sciences" && r.Mandatory)) >= 2);
        Assert.True(db.Advising.Programmes().Count(p => p.Requirements.Any(r => r.Subject == "Mathematical Literacy" && r.AlternativeGroup > 0)) >= 2);
    }

    [LocalDbFact]
    public void DatabaseEnforcesDuplicateRangeAndForeignKeyIntegrity()
    {
        using var db = new TestDatabase();
        using var c = new SqlConnection(db.Store.ConnectionString);
        c.Open();
        foreach (var sql in new[]
        {
            "INSERT INTO LearnerSubject VALUES(1,1,70)",
            "UPDATE LearnerSubject SET Mark=101 WHERE SessionId=1",
            "INSERT INTO LearnerSubject VALUES(99999,1,70)"
        }

        )
        {
            using var cmd = c.CreateCommand();
            cmd.CommandText = sql;
            Assert.Throws<SqlException>(() => cmd.ExecuteNonQuery());
        }

        using var integrity = c.CreateCommand();
        integrity.CommandText = "DBCC CHECKCONSTRAINTS WITH ALL_CONSTRAINTS";
        using var reader = integrity.ExecuteReader();
        Assert.False(reader.Read());
    }

    [LocalDbFact]
    public void NewSessionRoundTripsAndDoesNotOverwriteHistoricalResults()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        var original = db.Store.GetSession(1)!;
        input.Marks.Single(m => m.SubjectId == 3).Mark = 40;
        var s = db.Advising.Assess(input);
        Assert.True(s.Id > 10);
        var reopened = new SqlServerStore(db.Store.DatabaseName);
        reopened.Initialize();
        var restored = reopened.GetSession(s.Id)!;
        Assert.Equal(s.Selected.Explanation, restored.Selected.Explanation);
        Assert.Equal(s.Alternatives.Count, restored.Alternatives.Count);
        Assert.Equal(s.InterestScores.OrderBy(k => k.Key), restored.InterestScores.OrderBy(k => k.Key));
        Assert.Equal(original.Selected.OverallScore, reopened.GetSession(1)!.Selected.OverallScore);
    }

    [LocalDbFact]
    public void FundingFiltersExpiredInactiveAndUnrelatedRecords()
    {
        using var db = new TestDatabase();
        var commerce = db.Advising.Funding(1);
        Assert.NotEmpty(commerce);
        Assert.All(commerce, f =>
        {
            Assert.True(f.Sample);
            Assert.True(f.ClosingDate >= db.Clock.Today);
        });
        Assert.DoesNotContain(commerce, f => f.Name.Contains("Science") || f.Name.Contains("archived"));
        Assert.DoesNotContain(db.Advising.Funding(6), f => f.Name.Contains("archived"));
        db.Clock.UtcNow = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        Assert.Empty(db.Advising.Funding(1));
    }

    [LocalDbFact]
    public void AdvisorySummaryExportsRealFilesWithAllRequiredSectionsAndEscapesHtml()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Learner.School = "<script>alert('learner')</script>";
        var session = db.Advising.Assess(input);
        var report = db.Reports.Summary(session.Id);
        foreach (var field in new[]
        {
            "Grade 12",
            "BCom Information Systems",
            "Mathematics",
            "INTEREST CATEGORY",
            "Strong Match",
            "ALTERNATIVE PROGRAMMES",
            "FUNDING INFORMATION",
            "Generated:",
            "not an admission decision",
            "Documents:",
            "Privacy:"
        }

        )
            Assert.Contains(field, report.Content);
        var html = Path.Combine(db.DirectoryPath, "summary.html");
        var txt = Path.Combine(db.DirectoryPath, "summary.txt");
        db.Reports.Export(report, html);
        db.Reports.Export(report, txt);
        Assert.Equal(report.Content, File.ReadAllText(txt));
        Assert.Contains("&lt;script&gt;", File.ReadAllText(html));
        Assert.DoesNotContain("<script>", File.ReadAllText(html));
        using var c = new SqlConnection(db.Store.ConnectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT count(*) FROM ExportReport WHERE SessionId=" + session.Id;
        Assert.Equal(2, Convert.ToInt32(cmd.ExecuteScalar()));
    }

    [LocalDbFact]
    public void FundingOptOutDoesNotLeakFundingIntoSummary()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.FundingRequested = false;
        var s = db.Advising.Assess(input);
        var content = db.Reports.Summary(s.Id).Content;
        Assert.Contains("not requested", content);
        Assert.DoesNotContain("Commerce Merit Award", content);
    }

    [LocalDbFact]
    public void AllThreeReportsUseSavedSessionsAndRespectDateFilters()
    {
        using var db = new TestDatabase();
        db.Advisor();
        var from = new DateOnly(2026, 9, 1);
        var to = new DateOnly(2026, 9, 30);
        var activity = db.Reports.Activity(from, to, false);
        Assert.Contains("Sessions: 10", activity.Content);
        Assert.Contains("Distinct learners: 10", activity.Content);
        var exception = db.Reports.Activity(from, to, true);
        Assert.Contains("Aadiel-Raboo Magwa", exception.Content);
        Assert.Contains("Yusuf Patel", exception.Content);
        Assert.DoesNotContain("Gadija Jacobs", exception.Content);
        Assert.Contains("No sessions match", db.Reports.Activity(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 2), true).Content);
        Assert.Contains("Sessions: 1", db.Reports.Activity(from, to, false, "Gadija").Content);
        Assert.Throws<ValidationException>(() => db.Reports.Activity(to, from, false));
    }

    [LocalDbFact]
    public void AdminMaintainsRulesAndArchivedDegreesDisappearWithoutLosingHistory()
    {
        using var db = new TestDatabase();
        db.Admin();
        var row = db.Maintenance.Rows("DegreeProgramme").Single(r => r.Id == 1);
        row.Values["Active"] = false;
        db.Maintenance.Save("DegreeProgramme", row.Id, row.Values);
        Assert.DoesNotContain(db.Advising.Programmes(), p => p.Id == 1);
        Assert.Equal("BCom Information Systems", db.Advising.Load(1).Selected.Programme);
        var input = db.Input();
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
        var eligible = db.Advising.Assess(db.Input(2));
        Assert.DoesNotContain(eligible.Alternatives, r => r.DegreeId == 1);
    }

    [LocalDbFact]
    public void NewProgrammesMustHaveRulesAndMappingsBeforeActivation()
    {
        using var db = new TestDatabase();
        db.Admin();
        var values = new Dictionary<string, object?>
        {
            ["Name"] = "BCom Test",
            ["FacultyId"] = 1,
            ["Description"] = "Sample",
            ["MinimumAverage"] = 0,
            ["Active"] = true
        };
        Assert.Throws<ValidationException>(() => db.Maintenance.Save("DegreeProgramme", 0, values));
        values["Active"] = false;
        var id = db.Maintenance.Save("DegreeProgramme", 0, values);
        db.Maintenance.Save("ProgrammeRequirement", 0, new Dictionary<string, object?> { ["DegreeId"] = id, ["SubjectId"] = 1, ["MinimumMark"] = 60, ["Mandatory"] = true, ["AlternativeGroup"] = 0, ["Note"] = "Sample", ["Active"] = true });
        db.Maintenance.Save("DegreeInterestMapping", 0, new Dictionary<string, object?> { ["DegreeId"] = id, ["CategoryId"] = 5, ["Weight"] = 1, ["Active"] = true });
        values["Active"] = true;
        db.Maintenance.Save("DegreeProgramme", id, values);
        Assert.Contains(db.Advising.Programmes(), p => p.Id == id);
    }

    [LocalDbFact]
    public void InvalidMaintenanceValuesAndDuplicateNamesAreRejected()
    {
        using var db = new TestDatabase();
        db.Admin();
        Assert.Throws<ValidationException>(() => db.Maintenance.Save("Faculty", 0, new Dictionary<string, object?> { ["Name"] = "", ["Active"] = true }));
        Assert.Throws<ValidationException>(() => db.Maintenance.Save("Faculty", 0, new Dictionary<string, object?> { ["Name"] = "Commerce", ["Active"] = true }));
        var row = db.Maintenance.Rows("ProgrammeRequirement")[0];
        row.Values["MinimumMark"] = 101;
        Assert.Throws<ValidationException>(() => db.Maintenance.Save("ProgrammeRequirement", row.Id, row.Values));
        var f = db.Maintenance.Rows("FundingOption")[0];
        f.Values["Type"] = "Lottery";
        Assert.Throws<ValidationException>(() => db.Maintenance.Save("FundingOption", f.Id, f.Values));
        f.Values["Type"] = "Bursary";
        f.Values["ClosingDate"] = "not a date";
        Assert.Throws<ValidationException>(() => db.Maintenance.Save("FundingOption", f.Id, f.Values));
    }

    [LocalDbFact]
    public void BackupsAreConsistentAndDoNotOverwriteExistingFiles()
    {
        using var db = new TestDatabase();
        db.Admin();
        var path = Path.Combine(db.DirectoryPath, "backup.bak");
        db.Maintenance.Backup(path);
        using var restored = new LocalDbDatabase(seed: false);
        restored.RestoreBackup(path);
        Assert.Equal(10, restored.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        Assert.Throws<ValidationException>(() => db.Maintenance.Backup(path));
        Assert.Throws<ValidationException>(() => db.Maintenance.Backup(Path.Combine(db.DirectoryPath, "invalid.db")));
    }

    [LocalDbFact]
    public void FollowUpReusesLearnerAndRetainsOriginalAssessment()
    {
        using var db = new TestDatabase();
        db.Advisor();
        var input = db.Advising.Load(3).Input;
        input.Learner.Email = "learner@example.com";
        input.Learner.Phone = "0712345678";
        if (input.Marks.Count < 7) input.Marks.Add(new SubjectMark(2, "Afrikaans", 70));
        input.Marks.Single(m => m.SubjectId == 3).Mark = 65;
        var s = db.Advising.Assess(input);
        Assert.Equal(3, s.Input.Learner.Id);
        Assert.Equal(AdvisoryOutcome.StrongMatch, s.Selected.Outcome);
        Assert.Equal(AdvisoryOutcome.PossibleMatch, db.Advising.Load(3).Selected.Outcome);
    }

    [LocalDbFact]
    public void StoredAverageRuleAffectsNewAssessmentsOnly()
    {
        using var db = new TestDatabase();
        db.Admin();
        var programme = db.Maintenance.Rows("DegreeProgramme").Single(r => r.Id == 1);
        programme.Values["MinimumAverage"] = 85;
        db.Maintenance.Save("DegreeProgramme", programme.Id, programme.Values);
        var newResult = db.Advising.Assess(db.Input()).Selected;
        Assert.Equal(AdvisoryOutcome.NotCurrentlyEligible, newResult.Outcome);
        Assert.Contains("Overall average", newResult.Explanation);
        Assert.Equal(AdvisoryOutcome.StrongMatch, db.Advising.Load(1).Selected.Outcome);
    }

    [LocalDbFact]
    public void RemovingLastQuestionOfActiveProgrammeCategoryIsPrevented()
    {
        using var db = new TestDatabase();
        db.Admin();
        var question = db.Maintenance.Rows("InterestQuestion").Single(r => r.Id == 3);
        question.Values["Active"] = false;
        Assert.Throws<ValidationException>(() => db.Maintenance.Save("InterestQuestion", question.Id, question.Values));
        Assert.Equal(8, db.Advising.Questions().Count);
    }

    [LocalDbFact]
    public void FailedMultiTableSaveRollsBackLearnerSessionAndAuditTogether()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        var session = new RecommendationEngine().Assess(input, db.Advising.Programmes(), db.Advising.Questions(), db.Clock.UtcNow);
        input.Marks.Add(new SubjectMark { SubjectId = 999999, SubjectName = "Invalid foreign key", Mark = 70 });
        Assert.Throws<SqlException>(() => db.Store.SaveSession(session));
        using var connection = new SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Learner";
        Assert.Equal(10, Convert.ToInt32(command.ExecuteScalar()));
        Assert.Equal(10, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        command.CommandText = "SELECT COUNT(*) FROM AuditEvent WHERE Action='Advisory session created'";
        Assert.Equal(0, Convert.ToInt32(command.ExecuteScalar()));
    }

    [LocalDbFact]
    public void FailedExportDoesNotCreateMetadataOrTruncateOtherReports()
    {
        using var db = new TestDatabase();
        var session = db.Advising.Assess(db.Input());
        var report = db.Reports.Summary(session.Id);
        string path = Path.Combine(db.DirectoryPath, "missing-directory", "summary.txt");
        Assert.Throws<DirectoryNotFoundException>(() => db.Reports.Export(report, path));
        using var connection = new SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM ExportReport";
        Assert.Equal(0, Convert.ToInt32(command.ExecuteScalar()));
    }

    [LocalDbFact]
    public void SeededOutcomesAndScoresMatchTheProductionEngineWithoutRewritingHistory()
    {
        using var db = new TestDatabase();
        var expected = new[]
        {
            AdvisoryOutcome.StrongMatch,
            AdvisoryOutcome.NotCurrentlyEligible,
            AdvisoryOutcome.PossibleMatch,
            AdvisoryOutcome.StrongMatch,
            AdvisoryOutcome.NotCurrentlyEligible
        };
        for (int id = 1; id <= 5; id++)
        {
            var stored = db.Store.GetSession(id)!;
            var computed = new RecommendationEngine().Assess(stored.Input, db.Advising.Programmes(), db.Advising.Questions(), stored.CreatedUtc);
            Assert.Equal(expected[id - 1], stored.Selected.Outcome);
            Assert.Equal(computed.Selected.OverallScore, stored.Selected.OverallScore);
            Assert.Equal(computed.Selected.Explanation.Replace("\r\n", "\n"), stored.Selected.Explanation.Replace("\r\n", "\n"));
        }
    }
}
