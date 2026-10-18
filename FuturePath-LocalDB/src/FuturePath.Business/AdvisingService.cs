namespace FuturePath.Business;
public sealed class AdvisingService
{
    private readonly IFuturePathStore store;
    private readonly AccessSession access;
    private readonly IClock clock;
    public AdvisingService(IFuturePathStore store, AccessSession access, IClock clock)
    {
        this.store = store;
        this.access = access;
        this.clock = clock;
    }

    private readonly RecommendationEngine engine = new();
    public List<LookupItem> Subjects() => store.GetLookup("Subject");
    public List<LookupItem> Faculties() => store.GetLookup("Faculty");
    public List<DegreeProgramme> Programmes() => store.GetProgrammes(true);
    public List<InterestQuestion> Questions() => store.GetQuestions();
    public List<FundingOption> Funding(int degreeId) => store.GetFunding(degreeId, clock.Today);
    public AdvisorySession Assess(AdvisoryInput input)
    {
        Validate(input);
        var session = engine.Assess(input, Programmes(), Questions(), clock.UtcNow);
        session.Id = store.SaveSession(session, access.UserId);
        access.OwnSession(session.Id);
        return session;
    }

    public AdvisorySession Load(int id)
    {
        access.RequireSession(id);
        return store.GetSession(id) ?? throw new ValidationException("The advisory session no longer exists.");
    }

    public List<SessionSummary> Search(DateOnly from, DateOnly to, string search = "")
    {
        access.RequireStaff();
        if (from > to)
            throw new ValidationException("The start date must be on or before the end date.");
        return store.FindSessions(from, to, search.Trim());
    }

    private void Validate(AdvisoryInput input)
    {
        var learner = input.Learner;
        AdvisingValidation.Profile(learner);
        if (learner.Id != 0)
        {
            access.RequireStaff();
            var stored = store.GetLearner(learner.Id) ?? throw new ValidationException("Learner not found.");
            if (stored.Name != learner.Name || stored.School != learner.School || stored.Email != learner.Email || stored.Phone != learner.Phone || stored.Province != learner.Province)
                throw new ValidationException("A follow-up preserves learner identity. Start a new learner profile to capture different personal details.");
        }

        AdvisingValidation.Marks(input.Marks, Subjects());
        // An imported or deliberately incomplete draft still receives More Information Needed from the engine.
        AdvisingValidation.Interests(input.Responses, Questions(), requireComplete: false);
        if (!Faculties().Any(f => f.Id == input.PreferredFacultyId))
            throw new ValidationException("Select a preferred faculty.");
    }
}
