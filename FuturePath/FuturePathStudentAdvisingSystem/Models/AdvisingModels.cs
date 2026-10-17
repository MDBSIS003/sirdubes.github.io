namespace FuturePathStudentAdvisingSystem.Models;

public class LearnerProfile
{
    public int UserID { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string SchoolName { get; set; } = "";
    public string Province { get; set; } = "";
    public string Phone { get; set; } = "";
    public int? GradeLevelID { get; set; }
}
public record SubjectMark(int SubjectID, string SubjectName, decimal Mark);
public record InterestQuestion(int QuestionID, int CategoryID, string QuestionText);
public record InterestResponse(int QuestionID, decimal Score);
public record ProgrammeRequirement(int RequirementID, int DegreeID, int SubjectID, string SubjectName,
    decimal MinimumMark, bool IsMandatory, string? AlternativeGroup, string AdvisoryNote);
public record InterestMapping(int DegreeID, int CategoryID, decimal Weight);
public record FundingOpportunity(int FundingID, string FundingName, string FundingType,
    string EligibilityDescription, string RequiredDocuments, DateTime? ClosingDate, string ContactInfo,
    string FacultyName, bool IsSample);
public record Recommendation(int DegreeID, string ProgrammeName, string FacultyName, string AcademicEligibility,
    string Outcome, decimal SubjectScore, decimal MarkScore, decimal InterestScore, decimal OverallScore,
    string Explanation, bool IsAlternative);
public class AdvisingRecord
{
    public int SessionID { get; set; }
    public int UserID { get; set; }
    public int GradeLevelID { get; set; }
    public int FacultyID { get; set; }
    public int DegreeID { get; set; }
    public bool SubjectsComplete { get; set; }
    public bool FundingRequested { get; set; }
    public bool IsFinalized { get; set; }
    public DateTime SessionDate { get; set; }
    public List<SubjectMark> Marks { get; set; } = [];
    public List<InterestResponse> Responses { get; set; } = [];
}
