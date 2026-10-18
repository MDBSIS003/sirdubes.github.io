using FuturePath.Business;

namespace FuturePath.Tests;
public class CourseValidationTests
{
    [Theory]
    [InlineData("", "Western Cape")]
    [InlineData("   ", "Western Cape")]
    [InlineData("Sample school", "")]
    [InlineData("Sample school", "Unknown")]
    public void NewProfileRequiresSchoolAndValidProvince(string school, string province)
    {
        LearnerProfile learner = new LearnerProfile();
        learner.Grade = 12;
        learner.School = school;
        learner.Province = province;
        Assert.Throws<ValidationException>(() => AdvisingValidation.ProfileCapture(learner));
    }
    [Fact]
    public void AllProfileFieldsAreRequiredForNewCapture()
    {
        foreach (string province in ReferenceCatalog.Provinces)
        {
            LearnerProfile learner = new LearnerProfile();
            learner.Grade = 11;
            learner.School = "  Sample school  ";
            learner.Province = province;
            Assert.Throws<ValidationException>(() => AdvisingValidation.ProfileCapture(learner));
            learner.Name = "QA Learner";
            learner.Email = "learner@example.com";
            learner.Phone = "0712345678";
            AdvisingValidation.ProfileCapture(learner);
            Assert.Equal("Sample school", learner.School);
            Assert.Equal("QA Learner", learner.Name);
        }
    }
    [Fact]
    public void FundingChoicesRemainConfiguredAfterCatalogInitialisation()
    {
        ReferenceDefinition definition = ReferenceCatalog.Get("FundingOption");
        ReferenceField type = definition.Fields.Single(field => field.Name == "Type");
        Assert.Equal(new[] { "Scholarship", "Bursary", "Financial aid", "External funding", "Merit award" }, type.Choices);
    }
    [LocalDbFact]
    public void CrossFacultyDegreeIsRejectedWithoutSavingSession()
    {
        using TestDatabase db = new TestDatabase();
        AdvisoryInput input = db.Input();
        DegreeProgramme selected = db.Store.GetProgrammes(true).Single(degree => degree.Id == input.DegreeId);
        input.PreferredFacultyId = db.Store.GetLookup("Faculty").First(faculty => faculty.Id != selected.FacultyId).Id;
        int before = db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count;
        Assert.Throws<ValidationException>(() => db.Advising.Assess(input));
        Assert.Equal(before, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
    }
    [LocalDbFact]
    public void PreviewDoesNotWriteAndSaveWritesExactlyOneAssessment()
    {
        using TestDatabase db = new TestDatabase();
        int before = db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count;
        AdvisoryInput input = db.Input();
        AdvisorySession preview = db.Advising.Preview(input);
        Assert.Equal(0, preview.Id);
        Assert.Equal(before, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        AdvisorySession saved = db.Advising.Assess(input);
        Assert.True(saved.Id > 0);
        Assert.Equal(before + 1, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
    }
}

public sealed class RequiredProfilePersistenceTests
{
    [LocalDbFact]
    public void CompletedNewProfileStoresEveryFieldMarkAndAnswerAfterReconnect()
    {
        using TestDatabase db = new();
        AdvisoryInput input = db.Input();
        input.Learner.Name = "QA Learner";
        input.Learner.School = "QA School";
        input.Learner.Province = "Gauteng";
        input.Learner.Email = "qa.learner@example.com";
        input.Learner.Phone = "0712345678";
        var saved = db.Advising.Assess(input);
        var reopened = new FuturePath.Data.SqlServerStore(db.Store.DatabaseName).GetSession(saved.Id)!;
        Assert.Equal("QA Learner", reopened.Input.Learner.Name);
        Assert.Equal("QA School", reopened.Input.Learner.School);
        Assert.Equal("Gauteng", reopened.Input.Learner.Province);
        Assert.Equal("qa.learner@example.com", reopened.Input.Learner.Email);
        Assert.Equal("0712345678", reopened.Input.Learner.Phone);
        Assert.Equal(input.Marks.OrderBy(m => m.SubjectId).Select(m => (m.SubjectId,m.Mark)), reopened.Input.Marks.OrderBy(m => m.SubjectId).Select(m => (m.SubjectId,m.Mark)));
        Assert.Equal(input.Responses.OrderBy(r => r.QuestionId).Select(r => (r.QuestionId,r.Score)), reopened.Input.Responses.OrderBy(r => r.QuestionId).Select(r => (r.QuestionId,r.Score)));
        db.Advisor();
        Assert.Contains(db.Advising.Search(DateOnly.MinValue,DateOnly.MaxValue,"QA Learner"), s => s.Id == saved.Id);
    }
    [LocalDbFact]
    public void StaffCanCompleteMissingHistoricalDetailsWithoutDuplicatingLearner()
    {
        using TestDatabase db = new();
        db.Advisor();
        var input = db.Advising.Load(1).Input;
        input.Learner.Email = "completed@example.com";
        input.Learner.Phone = "0712345678";
        input.Marks.Add(new SubjectMark(2,"Afrikaans",70));
        var saved = db.Advising.Assess(input);
        Assert.Equal(1,saved.Input.Learner.Id);
        var profile = db.Store.GetLearner(1)!;
        Assert.Equal("completed@example.com",profile.Email);
        Assert.Equal("0712345678",profile.Phone);
        Assert.Equal("Gadija Jacobs",profile.Name);
        Assert.Equal(11,db.Advising.Search(DateOnly.MinValue,DateOnly.MaxValue).Count);
        Assert.Equal(98.44m,db.Advising.Load(1).Selected.OverallScore);
    }
}
