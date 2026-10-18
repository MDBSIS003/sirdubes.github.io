using FuturePath.Business;

namespace FuturePath.Tests;
public sealed class AdvisingTests
{
    [LocalDbTheory]
    [InlineData(1, "Gadija Jacobs", AdvisoryOutcome.StrongMatch)]
    [InlineData(2, "Aadiel-Raboo Magwa", AdvisoryOutcome.NotCurrentlyEligible)]
    [InlineData(3, "Labeeqah Rademeyer", AdvisoryOutcome.PossibleMatch)]
    [InlineData(4, "Siyabonga Dickson", AdvisoryOutcome.StrongMatch)]
    [InlineData(5, "Derek Vin Diesel", AdvisoryOutcome.NotCurrentlyEligible)]
    [InlineData(7, "Stef Van Wyk", AdvisoryOutcome.PossibleMatch)]
    [InlineData(9, "Yusuf Patel", AdvisoryOutcome.MoreInformationNeeded)]
    public void CaseStudyScenariosAreComputedByRealEngine(int id, string name, AdvisoryOutcome expected)
    {
        using var db = new TestDatabase();
        var input = db.Input(id);
        var session = db.Advising.Assess(input);
        Assert.Equal(name, session.Input.Learner.Name);
        Assert.Equal(expected, session.Selected.Outcome);
        Assert.InRange(session.Selected.OverallScore, 0m, 100m);
        Assert.Equal(expected, db.Store.GetSession(session.Id)!.Selected.Outcome);
    }

    [LocalDbFact]
    public void BiologyNamesTheExactMathematicsGap()
    {
        using var db = new TestDatabase();
        var s = db.Advising.Assess(db.Input(3));
        Assert.Contains("Mathematics: 58%", s.Selected.Explanation);
        Assert.Contains("gap 2 percentage points", s.Selected.Explanation);
    }

    [LocalDbFact]
    public void HealthSciencesExplainsBothMissingRequiredSubjects()
    {
        using var db = new TestDatabase();
        var s = db.Advising.Assess(db.Input(5));
        Assert.Contains("Required: Mathematics", s.Selected.Explanation);
        Assert.Contains("Required: Physical Sciences", s.Selected.Explanation);
    }

    [LocalDbFact]
    public void AlternativesAcceptMathematicalLiteracyAndExcludeIneligibleDegrees()
    {
        using var db = new TestDatabase();
        var s = db.Advising.Assess(db.Input(2));
        Assert.NotEmpty(s.Alternatives);
        Assert.DoesNotContain(s.Alternatives, r => r.DegreeId == s.Input.DegreeId);
        Assert.All(s.Alternatives, r => Assert.True(r.Outcome is AdvisoryOutcome.StrongMatch or AdvisoryOutcome.PossibleMatch));
        Assert.Contains(s.Alternatives, r => r.Programme.Contains("BCom General"));
        var groups = s.Alternatives.GroupBy(r => r.Outcome);
        foreach (var g in groups)
            Assert.Equal(g.OrderByDescending(r => r.OverallScore).Select(r => r.DegreeId), g.Select(r => r.DegreeId));
    }

    [LocalDbFact]
    public void AnOrRequirementDoesNotRequireBothMathsAndLiteracy()
    {
        using var db = new TestDatabase();
        var input = db.Input(8);
        var s = db.Advising.Assess(input);
        Assert.Equal(100, s.Selected.SubjectScore);
        Assert.DoesNotContain("Required: Mathematics", s.Selected.Explanation);
    }

    [LocalDbTheory]
    [InlineData(60, AdvisoryOutcome.StrongMatch)]
    [InlineData(59, AdvisoryOutcome.PossibleMatch)]
    [InlineData(55, AdvisoryOutcome.PossibleMatch)]
    [InlineData(54, AdvisoryOutcome.NotCurrentlyEligible)]
    public void MandatoryMarkBoundariesAreExplicit(int mark, AdvisoryOutcome outcome)
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Marks.Single(m => m.SubjectId == 3).Mark = mark;
        Assert.Equal(outcome, db.Advising.Assess(input).Selected.Outcome);
    }

    [LocalDbFact]
    public void MissingAnswersProduceMoreInformationAndNoFalseAlternatives()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Responses.RemoveAt(0);
        var result = db.Advising.Assess(input);
        Assert.Equal(AdvisoryOutcome.MoreInformationNeeded, result.Selected.Outcome);
        Assert.Empty(result.Alternatives);
    }

    [LocalDbFact]
    public void UnconfirmedSubjectsProduceMoreInformationEvenWhenMarksPass()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.SubjectsComplete = false;
        Assert.Equal(AdvisoryOutcome.MoreInformationNeeded, db.Advising.Assess(input).Selected.Outcome);
    }

    [LocalDbFact]
    public void LowInterestCannotBeCalledStrongMatch()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Responses.ForEach(r => r.Score = 1);
        var result = db.Advising.Assess(input).Selected;
        Assert.Equal(AdvisoryOutcome.PossibleMatch, result.Outcome);
        Assert.Equal(0, result.InterestScore);
    }

    [LocalDbTheory]
    [InlineData(-1)]
    [InlineData(101)]
    public void MarksOutsideTheRangeAreRejected(int mark)
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Marks[0].Mark = mark;
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
    }

    [LocalDbTheory]
    [InlineData(0)]
    [InlineData(100)]
    public void BoundaryMarksAreAcceptedAndRoundTrip(int mark)
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Marks[0].Mark = mark;
        var s = db.Advising.Assess(input);
        Assert.Equal(mark, db.Advising.Load(s.Id).Input.Marks.Single(m => m.SubjectId == input.Marks[0].SubjectId).Mark);
    }

    [LocalDbFact]
    public void DecimalMarksArePreserved()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Marks[0].Mark = 72.35m;
        var s = db.Advising.Assess(input);
        Assert.Equal(72.35m, db.Advising.Load(s.Id).Input.Marks.Single(m => m.SubjectId == input.Marks[0].SubjectId).Mark);
    }

    [LocalDbFact]
    public void DuplicateSubjectsAreRejectedWithoutSavingAnything()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Marks.Add(input.Marks[0]);
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
        Assert.Equal(10, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
    }

    [LocalDbTheory]
    [InlineData(10)]
    [InlineData(13)]
    public void OnlyGradesElevenAndTwelveAreAccepted(int grade)
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Learner.Grade = grade;
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
    }

    [LocalDbTheory]
    [InlineData("bad email")]
    [InlineData("x@")]
    public void InvalidOptionalEmailIsRejected(string email)
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Learner.Email = email;
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
    }

    [LocalDbFact]
    public void AnonymousProfileIsSupported()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Learner.Name = "";
        var session = db.Advising.Assess(input);
        Assert.Contains("Anonymous learner", db.Reports.Summary(session.Id).Content);
    }

    [LocalDbTheory]
    [InlineData(0)]
    [InlineData(6)]
    public void OutOfRangeInterestScoresAreRejected(int score)
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Responses[0].Score = score;
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
    }

    [LocalDbFact]
    public void UnknownSubjectAndDuplicateResponseAreRejected()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Marks[0].SubjectId = 999;
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
        input = db.Input();
        input.Responses.Add(input.Responses[0]);
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
    }

    [LocalDbFact]
    public void BlankSubjectListIsRejected()
    {
        using var db = new TestDatabase();
        var input = db.Input();
        input.Marks.Clear();
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
    }

    [LocalDbFact]
    public void WeightedScoresMatchDocumentedFormula()
    {
        using var db = new TestDatabase();
        var s = db.Advising.Assess(db.Input());
        var r = s.Selected;
        Assert.Equal(93.75m, r.InterestScore);
        Assert.Equal(98.44m, r.OverallScore);
    }
}
