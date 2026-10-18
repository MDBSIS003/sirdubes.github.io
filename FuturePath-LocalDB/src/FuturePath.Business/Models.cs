namespace FuturePath.Business;
public enum UserRole
{
    Learner,
    Advisor,
    Administrator
}

public enum AdvisoryOutcome
{
    StrongMatch,
    PossibleMatch,
    NotCurrentlyEligible,
    MoreInformationNeeded
}

public sealed class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}

public sealed class AccessDeniedException : Exception
{
    public AccessDeniedException() : base("Please sign in with an account authorised for this action.")
    {
    }
}

public sealed class LookupItem
{
    public int Id { get; init; }
    public string Name { get; init; } = "";

    public override string ToString() => Name;
}

public sealed class LearnerProfile
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Grade { get; set; } = 12;
    public string School { get; set; } = "";
    public string Province { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
}

public sealed class SubjectMark
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = "";
    public decimal Mark { get; set; }
}

public sealed class InterestQuestion
{
    public int Id { get; init; }
    public int CategoryId { get; init; }
    public string Category { get; init; } = "";
    public string Text { get; init; } = "";
}

public sealed class InterestResponse
{
    public int QuestionId { get; set; }
    public int Score { get; set; }
}

public sealed class ProgrammeRequirement
{
    public int SubjectId { get; init; }
    public string Subject { get; init; } = "";
    public decimal MinimumMark { get; init; }
    // Zero means an independent requirement; positive groups are OR choices.
    public int AlternativeGroup { get; init; }
    public bool Mandatory { get; init; } = true;
    public string Note { get; init; } = "";
}

public sealed class InterestWeight
{
    public int CategoryId { get; init; }
    public decimal Weight { get; init; }
}

public sealed class DegreeProgramme
{
    public int Id { get; init; }
    public int FacultyId { get; init; }
    public string Faculty { get; init; } = "";
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public bool Active { get; init; }
    public decimal MinimumAverage { get; init; }
    public List<ProgrammeRequirement> Requirements { get; } = [];
    public List<InterestWeight> Interests { get; } = [];

    public override string ToString() => Name;
}

public sealed class FundingOption
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Type { get; init; } = "";
    public string Eligibility { get; init; } = "";
    public string Documents { get; init; } = "";
    public DateOnly ClosingDate { get; init; }
    public string Contact { get; init; } = "";
    public bool Sample { get; init; }
}

public sealed class AdvisoryInput
{
    public LearnerProfile Learner { get; set; } = new();
    public int DegreeId { get; set; }
    public int PreferredFacultyId { get; set; }
    public bool SubjectsComplete { get; set; }
    public bool FundingRequested { get; set; }
    public List<SubjectMark> Marks { get; set; } = [];
    public List<InterestResponse> Responses { get; set; } = [];
}

public sealed class Recommendation
{
    public int DegreeId { get; init; }
    public string Programme { get; init; } = "";
    public string Faculty { get; init; } = "";
    public AdvisoryOutcome Outcome { get; init; }
    public decimal SubjectScore { get; init; }
    public decimal MarkScore { get; init; }
    public decimal InterestScore { get; init; }
    public decimal PreferenceScore { get; init; }
    public decimal OverallScore { get; init; }
    public string Explanation { get; init; } = "";
    public string OutcomeText => Outcome switch
    {
        AdvisoryOutcome.StrongMatch => "Strong Match",
        AdvisoryOutcome.PossibleMatch => "Possible Match",
        AdvisoryOutcome.NotCurrentlyEligible => "Not Currently Eligible",
        _ => "More Information Needed"
    };
}

public sealed class AdvisorySession
{
    public int Id { get; set; }
    public DateTime CreatedUtc { get; init; }
    public string RuleVersion { get; init; } = RecommendationEngine.RuleVersion;
    public AdvisoryInput Input { get; init; } = new();
    public Recommendation Selected { get; init; } = new();
    public List<Recommendation> Alternatives { get; init; } = [];
    public Dictionary<string, decimal> InterestScores { get; init; } = [];
}

public sealed class SessionSummary
{
    public int Id { get; init; }
    public int LearnerId { get; init; }
    public string Learner { get; init; } = "";
    public int Grade { get; init; }
    public DateTime CreatedUtc { get; init; }
    public string Programme { get; init; } = "";
    public string Faculty { get; init; } = "";
    public string Outcome { get; init; } = "";
    public decimal Score { get; init; }
    public bool FundingRequested { get; init; }
    public string Explanation { get; init; } = "";
}

public sealed class UserAccount
{
    public int Id { get; init; }
    public string Username { get; init; } = "";
    public string PasswordHash { get; init; } = "";
    public UserRole Role { get; init; }
    public bool Active { get; init; }
    public int FailedAttempts { get; init; }
    public DateTime? LockedUntilUtc { get; init; }
}

public interface IClock
{
    DateTime UtcNow { get; }

    DateOnly Today { get; }
}

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}

public interface IFuturePathStore
{
    void Initialize();
    List<LookupItem> GetLookup(string key);
    List<DegreeProgramme> GetProgrammes(bool activeOnly);
    List<InterestQuestion> GetQuestions(bool activeOnly = true);
    List<FundingOption> GetFunding(int degreeId, DateOnly today);
    LearnerProfile? GetLearner(int id);
    int SaveSession(AdvisorySession session, int? actorId = null);
    AdvisorySession? GetSession(int id);
    List<SessionSummary> FindSessions(DateOnly from, DateOnly to, string search);
    UserAccount? FindUser(string username);
    UserAccount? GetUser(int id);
    void RecordLogin(int userId, bool success, DateTime now);
    void SavePassword(int userId, string hash);
    void SaveUser(int id, string username, UserRole role, bool active, string? passwordHash);
    List<ReferenceRow> GetReference(string key);
    int SaveReference(string key, int id, IReadOnlyDictionary<string, object?> values);
    void Audit(int? userId, string action, string detail, DateTime now);
    void RecordExport(int sessionId, string path, DateTime now);
    void Backup(string path);
}
