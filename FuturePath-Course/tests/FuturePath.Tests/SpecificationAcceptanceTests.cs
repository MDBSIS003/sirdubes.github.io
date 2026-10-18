using System.Text.Json;
using FuturePath.Business;
using FuturePath.Fixtures;

namespace FuturePath.Tests;

public sealed class SpecificationAcceptanceTests
{
    private static readonly DateTime Now = new(2026, 10, 18, 12, 0, 0, DateTimeKind.Utc);
    private readonly ReferenceFixture fixture = new(Path.Combine(AppContext.BaseDirectory, "sample-data.json"));
    private FixtureStore Store() => new(fixture);
    private AdvisoryInput Input()
    {
        AdvisoryInput input = fixture.Input(1);
        input.Learner.Id = 0;
        input.Learner.Email = "learner@example.com";
        input.Learner.Phone = "0712345678";
        input.Marks.Add(new SubjectMark(2, "Afrikaans", 70));
        return input;
    }
    private static AdvisorySessionController Controller(FixtureStore store, AccessSession? access = null) => new(store, access ?? new(), new TestClock { UtcNow = Now });
    [Theory]
    [InlineData("Name")]
    [InlineData("School")]
    [InlineData("Province")]
    [InlineData("Email")]
    [InlineData("Phone")]
    public void EveryEmptyOrWhitespaceProfileFieldStopsPreviewAndPersistence(string field)
    {
        foreach (string missing in new[] { "", "   " })
        {
            var store = Store();
            var input = Input();
            typeof(LearnerProfile).GetProperty(field)!.SetValue(input.Learner, missing);
            Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
            Assert.Equal(0, store.SaveCalls);
        }
    }
    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("10")]
    [InlineData("13")]
    public void InvalidGradesCannotBeSaved(string grade)
    {
        var store = Store(); var input = Input(); input.Learner.Grade = int.Parse(grade);
        Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
        Assert.Equal(0, store.SaveCalls);
    }
    [Theory]
    [InlineData(11)]
    [InlineData(12)]
    public void CompleteValidProfileWorksForBothGrades(int grade)
    {
        var input = Input(); input.Learner.Grade = grade;
        AdvisingValidation.ProfileCapture(input.Learner);
    }
    [Theory]
    [InlineData("1234")]
    [InlineData("---")]
    [InlineData("Learner123")]
    [InlineData("<script>")]
    public void InvalidNamesAreRejected(string name)
    {
        var input = Input(); input.Learner.Name = name;
        Assert.Throws<ValidationException>(() => AdvisingValidation.ProfileCapture(input.Learner));
    }
    [Theory]
    [InlineData("QA O'Neil")]
    [InlineData("QA O’Neil")]
    [InlineData("Thandi Nkosi")]
    [InlineData("Anne-Marie Dlamini")]
    public void ValidNamesAreAccepted(string name)
    {
        var input = Input(); input.Learner.Name = name;
        AdvisingValidation.ProfileCapture(input.Learner);
    }
    [Theory]
    [InlineData("bad email")]
    [InlineData("x@")]
    [InlineData("Learner <learner@example.com>")]
    public void InvalidRequiredEmailStopsSave(string email)
    {
        var input = Input(); input.Learner.Email = email;
        Assert.Throws<ValidationException>(() => Controller(Store()).Assess(input));
    }
    [Theory]
    [InlineData("+++++++")]
    [InlineData("-------")]
    [InlineData("123456")]
    [InlineData("12 34 56")]
    [InlineData("abc1234567")]
    public void PhoneRequiresRealDigitsAndValidFormatting(string phone)
    {
        var input = Input(); input.Learner.Phone = phone;
        Assert.Throws<ValidationException>(() => AdvisingValidation.ProfileCapture(input.Learner));
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(6)]
    public void FewerThanSevenSubjectsCannotReachSave(int count)
    {
        var store = Store(); var input = Input(); input.Marks.RemoveRange(count, input.Marks.Count - count);
        Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
        Assert.Equal(0, store.SaveCalls);
    }
    [Fact]
    public void SevenAndEightDifferentSubjectsAreAccepted()
    {
        var input = Input(); var store = Store();
        Assert.Equal(0, Controller(store).Preview(input).Id);
        input.Marks.Add(new SubjectMark(4, "Mathematical Literacy", 70));
        Assert.Equal(0, Controller(store).Preview(input).Id);
        Assert.Equal(0, store.SaveCalls);
    }
    [Theory]
    [InlineData(-0.01)]
    [InlineData(100.01)]
    public void MarksOutsideRangeDoNotReachStore(decimal mark)
    {
        var input = Input(); var store = Store(); input.Marks[0].Mark = mark;
        Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
        Assert.Equal(0, store.SaveCalls);
    }
    [Fact]
    public void DuplicateSubjectsAndUnconfirmedListsAreRejected()
    {
        var input = Input(); var store = Store();
        input.Marks.Add(new SubjectMark(input.Marks[0].SubjectId, "Duplicate", 70));
        Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
        input.Marks.RemoveAt(input.Marks.Count - 1);
        input.SubjectsComplete = false;
        Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
        Assert.Equal(0, store.SaveCalls);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(7)]
    public void MissingQuestionnaireAnswersNeverSave(int count)
    {
        var store = Store(); var input = Input(); input.Responses.RemoveRange(count, input.Responses.Count - count);
        Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
        Assert.Equal(0, store.SaveCalls);
    }
    [Fact]
    public void BadScoresAndDuplicateQuestionAnswersNeverSave()
    {
        foreach (int score in new[] { 0, 6 })
        {
            var input = Input(); input.Responses[0].Score = score;
            Assert.Throws<ValidationException>(() => Controller(Store()).Assess(input));
        }
        var duplicate = Input(); duplicate.Responses.Add(duplicate.Responses[0]);
        Assert.Throws<ValidationException>(() => Controller(Store()).Assess(duplicate));
    }
    [Fact]
    public void DegreeOutsideSelectedFacultyCannotSave()
    {
        var store = Store(); var input = Input(); input.PreferredFacultyId = 3;
        Assert.Throws<ValidationException>(() => Controller(store).Assess(input));
        Assert.Equal(0, store.SaveCalls);
    }
    [Fact]
    public void AllCompletedInputsAndResultsReachPersistenceAndStaffRetrieval()
    {
        var store = Store(); var access = new AccessSession(); var advising = Controller(store, access);
        var input = Input(); input.FundingRequested = true;
        int id = advising.Assess(input).Id;
        input.Learner.Name = "Changed draft";
        input.Marks.Clear(); input.Responses.Clear();
        var loaded = advising.Load(id);
        Assert.Equal("Gadija Jacobs", loaded.Input.Learner.Name);
        Assert.Equal("learner@example.com", loaded.Input.Learner.Email);
        Assert.Equal("0712345678", loaded.Input.Learner.Phone);
        Assert.Equal(7, loaded.Input.Marks.Count);
        Assert.Equal(8, loaded.Input.Responses.Count);
        Assert.Equal(8, loaded.InterestScores.Count);
        Assert.True(loaded.Input.FundingRequested);
        Assert.True(loaded.Input.SubjectsComplete);
        Assert.Equal(1, loaded.Input.DegreeId);
        Assert.NotEmpty(loaded.Alternatives);
        Assert.Equal(1, store.SaveCalls);
        access.SignOut();
        Assert.Throws<AccessDeniedException>(() => advising.Load(id));
        new AuthenticationController(store, access, new TestClock()).Login("advisor_test", "FuturePathAdvisor2026!");
        var report = new ReportController(store, access, new TestClock()).Summary(id).Content;
        Assert.Contains("learner@example.com", report);
        Assert.Contains("0712345678", report);
        Assert.Contains("QUESTIONNAIRE RESPONSES", report);
        Assert.DoesNotContain("No external AI", report);
    }
    [Theory]
    [InlineData("advisor_test", "FuturePathAdvisor2026!", UserRole.Advisor)]
    [InlineData("admin_test", "FuturePathAdmin2026!", UserRole.Administrator)]
    public void SeedCredentialsAuthenticateToTheirStoredRoles(string name, string password, UserRole role)
    {
        var store = Store(); var access = new AccessSession();
        new AuthenticationController(store, access, new TestClock()).Login(" " + name + " ", password);
        Assert.Equal(role, access.Role);
        access.RequireStaff();
        if (role == UserRole.Advisor) Assert.Throws<AccessDeniedException>(() => access.RequireAdmin());
        else access.RequireAdmin();
    }
    [Theory]
    [InlineData("advisor_test", "wrong")]
    [InlineData("unknown", "FuturePathAdvisor2026!")]
    [InlineData("' OR 1=1--", "FuturePathAdvisor2026!")]
    public void WrongCredentialsAndSqlTextDoNotAuthenticate(string name, string password)
    {
        var access = new AccessSession();
        Assert.Throws<ValidationException>(() => new AuthenticationController(Store(), access, new TestClock()).Login(name, password));
        Assert.False(access.IsStaff);
    }
    [Fact]
    public void InactiveAndLockedStaffCannotSignIn()
    {
        var store = Store(); var access = new AccessSession(); var auth = new AuthenticationController(store, access, new TestClock { UtcNow = Now });
        store.Users[0].Active = false;
        Assert.Throws<ValidationException>(() => auth.Login(store.Users[0].Username, "FuturePathAdmin2026!"));
        store.Users[1].LockedUntilUtc = Now.AddMinutes(5);
        Assert.Throws<ValidationException>(() => auth.Login(store.Users[1].Username, "FuturePathAdvisor2026!"));
        Assert.False(access.IsStaff);
    }
    [Fact]
    public void ExceptionReportIncludesEverySpecifiedFieldAndGroupsByOutcome()
    {
        var store = Store(); var access = new AccessSession();
        new AuthenticationController(store, access, new TestClock()).Login("advisor_test", "FuturePathAdvisor2026!");
        var report = new ReportController(store, access, new TestClock()).Activity(new(2026,9,1), new(2026,9,30), true).Content;
        Assert.Contains("Sessions: 3", report);
        foreach (string field in new[] { "Faculty: Science", "Interest alignment:", "Alternative degree recommendations:", "Required: Mathematics", "Aadiel-Raboo Magwa", "Derek Vin Diesel", "Yusuf Patel" }) Assert.Contains(field, report);
        Assert.DoesNotContain("Gadija Jacobs", report);
        Assert.True(report.IndexOf("Aadiel-Raboo", StringComparison.Ordinal) < report.IndexOf("Derek", StringComparison.Ordinal));
        Assert.True(report.IndexOf("Derek", StringComparison.Ordinal) < report.IndexOf("Yusuf", StringComparison.Ordinal));
        Assert.DoesNotContain("No external AI", report);
    }
    [Fact]
    public void InitialActivityReportHasExactSeededCountsAndFilters()
    {
        var store = Store(); var access = new AccessSession();
        new AuthenticationController(store, access, new TestClock()).Login("advisor_test", "FuturePathAdvisor2026!");
        var reports = new ReportController(store, access, new TestClock());
        string text = reports.Activity(new(2026,9,1), new(2026,9,30), false).Content;
        foreach (string value in new[] { "Sessions: 10", "Distinct learners: 10", "Strong Match: 3", "Possible Match: 4", "Not Currently Eligible: 2", "More Information Needed: 1", "Funding requested: 4" }) Assert.Contains(value,text);
        Assert.Contains("Sessions: 1", reports.Activity(new(2026,9,1), new(2026,9,30), false, "", 3, 4, "Not Currently Eligible").Content);
        Assert.Throws<ValidationException>(() => reports.Activity(new(2026,10,1), new(2026,9,1), false));
    }
    [Fact]
    public void ValidHtmlAndTextExportsAreWrittenAndInvalidPathsHaveNoSuccessMetadata()
    {
        var store = Store(); var access = new AccessSession(); var advising = Controller(store, access);
        var input = Input(); input.Learner.School = "QA <script>alert(1)</script>";
        var session = advising.Assess(input); var reports = new ReportController(store, access, new TestClock());
        string dir = Path.Combine(Path.GetTempPath(), "FuturePathAcceptance_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var report = reports.Summary(session.Id);
            reports.Export(report, Path.Combine(dir,"summary.html"));
            reports.Export(report, Path.Combine(dir,"summary.txt"));
            Assert.Contains("&lt;script&gt;", File.ReadAllText(Path.Combine(dir,"summary.html")));
            Assert.DoesNotContain("<script>", File.ReadAllText(Path.Combine(dir,"summary.html")));
            Assert.Equal(report.Content, File.ReadAllText(Path.Combine(dir,"summary.txt")));
            Assert.Equal(2, store.ExportCalls);
            Assert.Throws<DirectoryNotFoundException>(() => reports.Export(report, Path.Combine(dir,"missing","summary.txt")));
            Assert.Equal(2, store.ExportCalls);
        }
        finally { Directory.Delete(dir,true); }
    }
    [Fact]
    public void FailedPersistenceDoesNotGrantGuestAccessOrClaimSavedId()
    {
        var store = Store(); store.FailSave = true; var access = new AccessSession();
        Assert.Throws<IOException>(() => Controller(store,access).Assess(Input()));
        Assert.Throws<AccessDeniedException>(() => access.RequireSession(11));
        Assert.Equal(10,store.Sessions.Count);
    }

    private sealed class FixtureStore : IFuturePathStore
    {
        private readonly ReferenceFixture fixture;
        public List<UserAccount> Users { get; } = new();
        public Dictionary<int, AdvisorySession> Sessions { get; } = new();
        public int SaveCalls { get; private set; }
        public int ExportCalls { get; private set; }
        public bool FailSave { get; set; }
        private static T Copy<T>(T value) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value))!;
        public FixtureStore(ReferenceFixture fixture)
        {
            this.fixture = fixture;
            string seed = File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"Database","03_SeedData.sql"));
            var users = System.Text.RegularExpressions.Regex.Matches(seed, @"INSERT dbo.UserAccount\(Username,PasswordHash,Role,Active\) VALUES\(N'([^']+)',N'([^']+)',N'([^']+)',1\)");
            foreach (System.Text.RegularExpressions.Match user in users)
                Users.Add(new UserAccount { Id=Users.Count+1, Username=user.Groups[1].Value, PasswordHash=user.Groups[2].Value, Role=Enum.Parse<UserRole>(user.Groups[3].Value), Active=true });
            Assert.Equal(2, Users.Count);
            for(int id=1;id<=10;id++)
            {
                var input=fixture.Input(id);
                var date=fixture.Rows("AdvisorySession").Single(r=>ReferenceFixture.Number(r,"Id")==id).GetProperty("CreatedUtc").GetDateTime();
                var session=new RecommendationEngine().Assess(input,fixture.Programmes(),fixture.Questions(),date);
                session.Id=id; Sessions.Add(id,session);
            }
        }
        public void Initialize() { }
        public List<LookupItem> GetLookup(string key) => fixture.Rows(key.Split(':')[0]).Where(r=>ReferenceFixture.Number(r,"Active")==1).Select(r=>new LookupItem { Id=ReferenceFixture.Number(r,"Id"),Name=ReferenceFixture.Text(r,"Name") }).ToList();
        public List<DegreeProgramme> GetProgrammes(bool activeOnly) => fixture.Programmes().Where(p=>!activeOnly || p.Active).ToList();
        public List<InterestQuestion> GetQuestions(bool activeOnly=true) => fixture.Questions();
        public List<FundingOption> GetFunding(int degreeId, DateOnly today) => new();
        public LearnerProfile? GetLearner(int id) => Sessions.Values.Select(s=>s.Input.Learner).FirstOrDefault(l=>l.Id==id) is { } learner ? Copy(learner) : null;
        public int SaveSession(AdvisorySession session,int? actorId=null)
        {
            if(FailSave) throw new IOException("Test store failure");
            SaveCalls++; int id=Sessions.Keys.Max()+1; var saved=Copy(session); saved.Id=id;
            if(saved.Input.Learner.Id==0) saved.Input.Learner.Id=id;
            Sessions.Add(id,saved); return id;
        }
        public AdvisorySession? GetSession(int id) => Sessions.TryGetValue(id,out var s) ? Copy(s) : null;
        public List<SessionSummary> FindSessions(DateOnly from,DateOnly to,string search) => Sessions.Values.Where(s=>DateOnly.FromDateTime(s.CreatedUtc)>=from && DateOnly.FromDateTime(s.CreatedUtc)<=to && (s.Input.Learner.Name.Contains(search,StringComparison.OrdinalIgnoreCase) || s.Selected.Programme.Contains(search,StringComparison.OrdinalIgnoreCase) || s.Id.ToString()==search)).Select(s=>new SessionSummary { Id=s.Id,LearnerId=s.Input.Learner.Id,Learner=s.Input.Learner.Name,Grade=s.Input.Learner.Grade,CreatedUtc=s.CreatedUtc,Programme=s.Selected.Programme,Faculty=s.Selected.Faculty,Outcome=s.Selected.OutcomeText,Score=s.Selected.OverallScore,FundingRequested=s.Input.FundingRequested,Explanation=s.Selected.Explanation,DegreeId=s.Input.DegreeId,FacultyId=s.Input.PreferredFacultyId }).ToList();
        public UserAccount? FindUser(string username) => Users.FirstOrDefault(u=>u.Username.Equals(username,StringComparison.OrdinalIgnoreCase));
        public UserAccount? GetUser(int id) => Users.FirstOrDefault(u=>u.Id==id);
        public void RecordLogin(int userId,bool success,DateTime now) { }
        public void SavePassword(int userId,string hash) => GetUser(userId)!.PasswordHash=hash;
        public void SaveUser(int id,string username,UserRole role,bool active,string? passwordHash) => throw new NotSupportedException();
        public List<ReferenceRow> GetReference(string key) => throw new NotSupportedException();
        public int SaveReference(string key,int id,IReadOnlyDictionary<string,object?> values) => throw new NotSupportedException();
        public void Audit(int? userId,string action,string detail,DateTime now) { }
        public void RecordExport(int sessionId,string path,DateTime now) => ExportCalls++;
        public void Backup(string path) => throw new NotSupportedException();
    }
}
