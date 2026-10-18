using FuturePath.Business;
using FuturePath.Data;
using FuturePath.Fixtures;
using Microsoft.Data.SqlClient;

namespace FuturePath.Tests;
/// <summary>Pure rules and validation against exact input fixtures. These do not verify SQL persistence.</summary>
public sealed class PortableRuleTests
{
    private readonly ReferenceFixture fixture = new(Path.Combine(AppContext.BaseDirectory, "sample-data.json"));
    private readonly RecommendationEngine engine = new();
    private static readonly DateTime Now = new(2026, 10, 18, 10, 0, 0, DateTimeKind.Utc);
    private AdvisorySession Assess(AdvisoryInput input) => engine.Assess(input, fixture.Programmes(), fixture.Questions(), Now);
    [Theory]
    [InlineData(1, AdvisoryOutcome.StrongMatch)]
    [InlineData(2, AdvisoryOutcome.NotCurrentlyEligible)]
    [InlineData(3, AdvisoryOutcome.PossibleMatch)]
    [InlineData(4, AdvisoryOutcome.StrongMatch)]
    [InlineData(5, AdvisoryOutcome.NotCurrentlyEligible)]
    [InlineData(7, AdvisoryOutcome.PossibleMatch)]
    [InlineData(9, AdvisoryOutcome.MoreInformationNeeded)]
    public void ExactReferenceScenariosAreComputedWithoutDatabase(int id, AdvisoryOutcome expected)
    {
        var result = Assess(fixture.Input(id));
        Assert.Equal(expected, result.Selected.Outcome);
        Assert.InRange(result.Selected.OverallScore, 0m, 100m);
    }

    [Theory]
    [InlineData(60, AdvisoryOutcome.StrongMatch)]
    [InlineData(59, AdvisoryOutcome.PossibleMatch)]
    [InlineData(55, AdvisoryOutcome.PossibleMatch)]
    [InlineData(54, AdvisoryOutcome.NotCurrentlyEligible)]
    public void FivePointGapBoundaryOverridesHighWeightedScores(int mark, AdvisoryOutcome expected)
    {
        var input = fixture.Input(1);
        input.Marks.Single(m => m.SubjectId == 3).Mark = mark;
        Assert.Equal(expected, Assess(input).Selected.Outcome);
    }

    [Fact]
    public void GapExplanationAndWeightedFormulaMatchReference()
    {
        var biology = Assess(fixture.Input(3)).Selected;
        Assert.Contains("Mathematics: 58%", biology.Explanation);
        Assert.Contains("gap 2 percentage points", biology.Explanation);
        var commerce = Assess(fixture.Input(1)).Selected;
        Assert.Equal(93.75m, commerce.InterestScore);
        Assert.Equal(98.44m, commerce.OverallScore);
    }

    [Fact]
    public void BothHealthScienceSubjectsAreExplainedAsMissing()
    {
        var explanation = Assess(fixture.Input(5)).Selected.Explanation;
        Assert.Contains("Required: Mathematics", explanation);
        Assert.Contains("Required: Physical Sciences", explanation);
    }

    [Fact]
    public void ExplicitOrGroupAcceptsLiteracyWithoutRequiringMathematics()
    {
        var result = Assess(fixture.Input(8)).Selected;
        Assert.Equal(100m, result.SubjectScore);
        Assert.DoesNotContain("Required: Mathematics", result.Explanation);
    }

    [Fact]
    public void IncompleteAnswersAndUnconfirmedSubjectsCannotProduceEligibleAlternatives()
    {
        var input = fixture.Input(1);
        input.Responses.RemoveAt(0);
        var result = Assess(input);
        Assert.Equal(AdvisoryOutcome.MoreInformationNeeded, result.Selected.Outcome);
        Assert.Empty(result.Alternatives);
        input = fixture.Input(1);
        input.SubjectsComplete = false;
        Assert.Equal(AdvisoryOutcome.MoreInformationNeeded, Assess(input).Selected.Outcome);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(3, 50)]
    [InlineData(5, 100)]
    public void LikertResponsesProduceDocumentedCategoryScores(int answer, decimal score)
    {
        var input = fixture.Input(1);
        input.Responses.ForEach(r => r.Score = answer);
        Assert.All(Assess(input).InterestScores.Values, value => Assert.Equal(score, value));
        if (answer == 1)
            Assert.Equal(AdvisoryOutcome.PossibleMatch, Assess(input).Selected.Outcome);
    }

    [Fact]
    public void AlternativesAreLimitedRankedAndExcludeSelectedAndInactiveProgrammes()
    {
        var result = Assess(fixture.Input(2));
        Assert.NotEmpty(result.Alternatives);
        Assert.True(result.Alternatives.Count <= 5);
        Assert.DoesNotContain(result.Alternatives, r => r.DegreeId == result.Input.DegreeId);
        Assert.All(result.Alternatives, r => Assert.True(r.Outcome is AdvisoryOutcome.StrongMatch or AdvisoryOutcome.PossibleMatch));
        var programmes = fixture.Programmes().Where(p => p.Id != 3).ToList();
        var reduced = engine.Assess(fixture.Input(2), programmes, fixture.Questions(), Now);
        Assert.DoesNotContain(reduced.Alternatives, r => r.DegreeId == 3);
        var expected = result.Alternatives.OrderBy(r => r.Outcome == AdvisoryOutcome.StrongMatch ? 0 : 1).ThenByDescending(r => r.OverallScore).ThenBy(r => r.Programme).Select(r => r.DegreeId);
        Assert.Equal(expected, result.Alternatives.Select(r => r.DegreeId));
    }

    [Fact]
    public void InactiveSelectionAndIncompleteConfigurationAreRejectedOrExplained()
    {
        var input = fixture.Input(1);
        Assert.Throws<ValidationException>(() => engine.Assess(input, [], fixture.Questions(), Now));
        var programme = new DegreeProgramme
        {
            Id = 1,
            FacultyId = 1,
            Active = true
        };
        Assert.Equal(AdvisoryOutcome.MoreInformationNeeded, engine.Evaluate(input, programme, fixture.Questions()).Outcome);
    }

    [Fact]
    public void StoredAverageIsAnIndependentHardGate()
    {
        var programme = new DegreeProgramme
        {
            Id = 1,
            FacultyId = 1,
            Name = "Average test",
            Active = true,
            MinimumAverage = 85
        };
        var source = fixture.Programmes().Single(p => p.Id == 1);
        programme.Requirements.AddRange(source.Requirements);
        programme.Interests.AddRange(source.Interests);
        var result = engine.Evaluate(fixture.Input(1), programme, fixture.Questions());
        Assert.Equal(AdvisoryOutcome.NotCurrentlyEligible, result.Outcome);
        Assert.Contains("Overall average", result.Explanation);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void InvalidMarksAreRejectedBeforePersistence(decimal value)
    {
        var marks = fixture.Input(1).Marks;
        marks[0].Mark = value;
        Assert.Throws<ValidationException>(() => AdvisingValidation.Marks(marks, Subjects()));
    }

    [Fact]
    public void BoundaryDecimalDuplicateAndUnknownSubjectsAreHandled()
    {
        var marks = fixture.Input(1).Marks;
        marks[0].Mark = 0;
        marks[1].Mark = 100;
        marks[2].Mark = 72.35m;
        AdvisingValidation.Marks(marks, Subjects());
        marks.Add(marks[0]);
        Assert.Throws<ValidationException>(() => AdvisingValidation.Marks(marks, Subjects()));
        marks.RemoveAt(marks.Count - 1);
        marks[0].SubjectId = 99999;
        Assert.Throws<ValidationException>(() => AdvisingValidation.Marks(marks, Subjects()));
    }

    private List<LookupItem> Subjects() => fixture.Rows("Subject").Select(r => new LookupItem { Id = ReferenceFixture.Number(r, "Id"), Name = ReferenceFixture.Text(r, "Name") }).ToList();
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(13)]
    public void MissingOrInvalidGradesAreRejected(int grade)
    {
        var learner = fixture.Input(1).Learner;
        learner.Grade = grade;
        Assert.Throws<ValidationException>(() => AdvisingValidation.Profile(learner));
    }

    [Theory]
    [InlineData("bad email")]
    [InlineData("x@")]
    public void InvalidOptionalEmailIsRejected(string email)
    {
        var learner = fixture.Input(1).Learner;
        learner.Email = email;
        Assert.Throws<ValidationException>(() => AdvisingValidation.Profile(learner));
    }

    [Fact]
    public void AnonymousProfileAndValidOptionalContactAreSupported()
    {
        var learner = new LearnerProfile
        {
            Grade = 11,
            Email = "learner@example.invalid",
            Phone = "+27 71 123 4567"
        };
        AdvisingValidation.Profile(learner);
        learner.Email = "";
        learner.Phone = "";
        AdvisingValidation.Profile(learner);
        learner.Province = "Unknown";
        Assert.Throws<ValidationException>(() => AdvisingValidation.Profile(learner));
    }

    [Fact]
    public void QuestionnaireStepRejectsMissingDuplicateAndInvalidResponses()
    {
        var input = fixture.Input(1);
        AdvisingValidation.Interests(input.Responses, fixture.Questions(), true);
        input.Responses.RemoveAt(0);
        Assert.Throws<ValidationException>(() => AdvisingValidation.Interests(input.Responses, fixture.Questions(), true));
        AdvisingValidation.Interests(input.Responses, fixture.Questions(), false);
        input.Responses[0].Score = 6;
        Assert.Throws<ValidationException>(() => AdvisingValidation.Interests(input.Responses, fixture.Questions(), false));
        input = fixture.Input(1);
        input.Responses.Add(input.Responses[0]);
        Assert.Throws<ValidationException>(() => AdvisingValidation.Interests(input.Responses, fixture.Questions(), false));
    }

    [Fact]
    public void AllReportSectionsAndHtmlEncodingUseRealComputedResults()
    {
        var input = fixture.Input(1);
        input.Learner.Name = "<script>alert('x')</script>";
        var session = Assess(input);
        var report = new ReportComposer().Summary(session, [], Now);
        foreach (var section in new[]
        {
            "Grade 12",
            "BCom Information Systems",
            "Mathematics",
            "INTEREST CATEGORY",
            "EXPLANATION",
            "ALTERNATIVE",
            "FUNDING",
            "Generated:",
            "not an admission decision"
        }

        )
            Assert.Contains(section, report.Content);
        Assert.Contains("&lt;script&gt;", report.ToHtml());
        Assert.DoesNotContain("<script>", report.ToHtml());
    }

    [Fact]
    public void GuestEntryPointsDenyStaffActionsBeforeReachingStore()
    {
        var access = new AccessSession();
        Assert.Throws<AccessDeniedException>(() => access.RequireStaff());
        Assert.Throws<AccessDeniedException>(() => access.RequireAdmin());
        Assert.Throws<AccessDeniedException>(() => access.RequireSession(1));
    }

    [Fact]
    public void ConnectionIsLocalIntegratedAndRejectsFilePathsOrSqlInjection()
    {
        var connection = new DatabaseConnection();
        var builder = new SqlConnectionStringBuilder(connection.ConnectionString);
        Assert.Equal(@"(localdb)\MSSQLLocalDB", builder.DataSource);
        Assert.Equal("FuturePathStudentAdvisingSystem", builder.InitialCatalog);
        Assert.True(builder.IntegratedSecurity);
        Assert.Empty(builder.Password);
        Assert.Throws<ValidationException>(() => new DatabaseConnection("test.db"));
        Assert.Throws<ValidationException>(() => new DatabaseConnection("master"));
        Assert.Throws<ValidationException>(() => new DatabaseConnection("db]; DROP DATABASE master;--"));
    }

    [Fact]
    public void ActivityGapTotalsUseStoredExplanationsRatherThanCurrentRules()
    {
        var sessions = new List<SessionSummary>();
        for (int id = 1; id <= 10; id++)
            sessions.Add(new SessionSummary { Explanation = Assess(fixture.Input(id)).Selected.Explanation });
        string totals = ReportComposer.GapTotals(sessions);
        Assert.Contains("Missing/unconfirmed required-subject data: 3", totals);
        Assert.Contains("Mark/average shortfalls: 3", totals);
        Assert.Contains("Incomplete questionnaires: 1", totals);
    }
}
