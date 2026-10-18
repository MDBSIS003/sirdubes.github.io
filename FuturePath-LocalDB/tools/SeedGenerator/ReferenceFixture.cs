using System.Text.Json;
using FuturePath.Business;

namespace FuturePath.Fixtures;
/// <summary>Exact reference inputs used only by the seed generator and portable tests; not a database provider.</summary>
public sealed class ReferenceFixture
{
    private readonly Dictionary<string, List<JsonElement>> tables;
    public ReferenceFixture(string path)
    {
        tables = JsonSerializer.Deserialize<Dictionary<string, List<JsonElement>>>(File.ReadAllText(path))!;
    }

    public List<JsonElement> Rows(string table) => tables[table];
    public static int Number(JsonElement row, string key) => row.GetProperty(key).GetInt32();
    public static decimal Decimal(JsonElement row, string key) => row.GetProperty(key).GetDecimal();
    public static string Text(JsonElement row, string key) => row.GetProperty(key).GetString() ?? "";
    public List<DegreeProgramme> Programmes()
    {
        var result = new List<DegreeProgramme>();
        foreach (var row in Rows("DegreeProgramme"))
        {
            int id = Number(row, "Id");
            int facultyId = Number(row, "FacultyId");
            var programme = new DegreeProgramme
            {
                Id = id,
                FacultyId = facultyId,
                Name = Text(row, "Name"),
                Faculty = Text(Rows("Faculty").Single(f => Number(f, "Id") == facultyId), "Name"),
                Description = Text(row, "Description"),
                MinimumAverage = Decimal(row, "MinimumAverage"),
                Active = Number(row, "Active") == 1
            };
            foreach (var requirement in Rows("ProgrammeRequirement").Where(r => Number(r, "DegreeId") == id && Number(r, "Active") == 1))
            {
                int subjectId = Number(requirement, "SubjectId");
                programme.Requirements.Add(new ProgrammeRequirement { SubjectId = subjectId, Subject = Text(Rows("Subject").Single(s => Number(s, "Id") == subjectId), "Name"), MinimumMark = Decimal(requirement, "MinimumMark"), Mandatory = Number(requirement, "Mandatory") == 1, AlternativeGroup = Number(requirement, "AlternativeGroup"), Note = Text(requirement, "Note") });
            }

            foreach (var mapping in Rows("DegreeInterestMapping").Where(r => Number(r, "DegreeId") == id && Number(r, "Active") == 1))
                programme.Interests.Add(new InterestWeight { CategoryId = Number(mapping, "CategoryId"), Weight = Decimal(mapping, "Weight") });
            result.Add(programme);
        }

        return result;
    }

    public List<InterestQuestion> Questions()
    {
        var result = new List<InterestQuestion>();
        foreach (var row in Rows("InterestQuestion"))
        {
            int category = Number(row, "CategoryId");
            result.Add(new InterestQuestion { Id = Number(row, "Id"), CategoryId = category, Text = Text(row, "Text"), Category = Text(Rows("InterestCategory").Single(r => Number(r, "Id") == category), "Name") });
        }

        return result;
    }

    public AdvisoryInput Input(int sessionId)
    {
        var session = Rows("AdvisorySession").Single(r => Number(r, "Id") == sessionId);
        int learnerId = Number(session, "LearnerId");
        var learner = Rows("Learner").Single(r => Number(r, "Id") == learnerId);
        var input = new AdvisoryInput
        {
            Learner = new LearnerProfile
            {
                Id = learnerId,
                Name = Text(learner, "Name"),
                Grade = Number(session, "GradeId"),
                School = Text(learner, "School"),
                Province = Text(learner, "Province"),
                Email = Text(learner, "Email"),
                Phone = Text(learner, "Phone")
            },
            DegreeId = Number(session, "DegreeId"),
            PreferredFacultyId = Number(session, "PreferredFacultyId"),
            SubjectsComplete = Number(session, "SubjectsComplete") == 1,
            FundingRequested = Number(session, "FundingRequested") == 1
        };
        foreach (var row in Rows("LearnerSubject").Where(r => Number(r, "SessionId") == sessionId))
        {
            int subjectId = Number(row, "SubjectId");
            input.Marks.Add(new SubjectMark { SubjectId = subjectId, Mark = Decimal(row, "Mark"), SubjectName = Text(Rows("Subject").Single(r => Number(r, "Id") == subjectId), "Name") });
        }

        foreach (var row in Rows("LearnerInterestResponse").Where(r => Number(r, "SessionId") == sessionId))
            input.Responses.Add(new InterestResponse { QuestionId = Number(row, "QuestionId"), Score = Number(row, "Score") });
        return input;
    }
}
