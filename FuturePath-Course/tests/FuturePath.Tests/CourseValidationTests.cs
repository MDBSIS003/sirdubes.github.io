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
    public void SchoolAndProvinceDoNotRequirePersonalNameOrContact()
    {
        foreach (string province in ReferenceCatalog.Provinces)
        {
            LearnerProfile learner = new LearnerProfile();
            learner.Grade = 11;
            learner.School = "  Sample school  ";
            learner.Province = province;
            AdvisingValidation.ProfileCapture(learner);
            Assert.Equal("Sample school", learner.School);
            Assert.Equal("", learner.Name);
            Assert.Equal("", learner.Email);
            Assert.Equal("", learner.Phone);
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
