using System.Data;
using System.Text.RegularExpressions;
using FuturePathStudentAdvisingSystem.BusinessLogic;
using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Models;
using FuturePathStudentAdvisingSystem.Security;
using Microsoft.Data.SqlClient;

int passed = 0, failed = 0, skipped = 0;
void Check(string name, Action action)
{ try { action(); passed++; Console.WriteLine("PASS " + name); } catch (Exception e) { failed++; Console.WriteLine("FAIL " + name + ": " + e.Message); } }
void Assert(bool condition, string message = "Assertion failed") { if (!condition) throw new Exception(message); }
void Throws<T>(Action action) where T : Exception
{ try { action(); } catch (T) { return; } throw new Exception("Expected " + typeof(T).Name); }

Check("PBKDF2 unique salts, correct password, wrong password, malformed hashes", () =>
{ var a = PasswordHasher.Hash("Student-demo-2026!"); var b = PasswordHasher.Hash("Student-demo-2026!"); Assert(a != b); Assert(PasswordHasher.Verify("Student-demo-2026!", a)); Assert(!PasswordHasher.Verify("student-demo-2026!", a)); Assert(!PasswordHasher.Verify("anything", "plaintext")); Assert(!PasswordHasher.Verify("anything", "PBKDF2-SHA256$600000$bad$bad")); });
Check("Password length bounds", () => { Throws<ArgumentException>(() => PasswordHasher.Hash("short")); Throws<ArgumentException>(() => PasswordHasher.Hash(new string('x', 129))); });
Check("Guest cannot access staff screens or protected operations", () =>
{ AppSession.End(); Throws<UnauthorizedAccessException>(() => AccessPolicy.RequirePage("Advisor")); Throws<UnauthorizedAccessException>(() => new AdminDAL().List("Subject")); Throws<UnauthorizedAccessException>(() => new AdvisingDAL().SearchLearners("")); Throws<UnauthorizedAccessException>(() => new UserAccountDAL().SaveProfile(new() { UserID = 1 })); });
Check("Learner ownership and role separation", () =>
{ AppSession.Start(42, Roles.Learner, "Test"); AccessPolicy.RequireLearner(42); Throws<UnauthorizedAccessException>(() => AccessPolicy.RequireLearner(43)); Throws<UnauthorizedAccessException>(() => AccessPolicy.RequirePage("Administrator")); AppSession.Start(8, Roles.Advisor, "Advisor"); AccessPolicy.RequirePage("Advisor"); Throws<UnauthorizedAccessException>(() => AccessPolicy.RequirePage("Administrator")); AppSession.End(); Assert(!AppSession.IsLoggedIn && AppSession.Role == Roles.Guest); });
Check("Profile validation accepts valid data and rejects invalid fields", () =>
{ var p = new LearnerProfile { FirstName = " Test ", LastName = "Learner", Email = "test@example.invalid", SchoolName = "School", Province = "Gauteng", GradeLevelID = 2, Phone = "+27 71 123 4567" }; Validation.Profile(p); Assert(p.FirstName == "Test"); p.Email = "bad"; Throws<ArgumentException>(() => Validation.Profile(p)); p.Email = "test@example.invalid"; p.Province = "unknown"; Throws<ArgumentException>(() => Validation.Profile(p)); p.Province = "Gauteng"; p.GradeLevelID = null; Throws<ArgumentException>(() => Validation.Profile(p)); });
Check("Mark boundaries and duplicate prevention", () =>
{ Validation.Marks([new(1, "English", 0), new(2, "Maths", 100)]); Throws<ArgumentException>(() => Validation.Marks([new(1, "English", -1)])); Throws<ArgumentException>(() => Validation.Marks([new(1, "English", 101)])); Throws<ArgumentException>(() => Validation.Marks([new(1, "English", 60), new(1, "English", 80)])); });
Check("Questionnaire requires all valid unique responses", () =>
{ List<InterestQuestion> q = [new(1, 1, "Question"), new(2, 2, "Question2")]; Validation.Responses([new(1, 1), new(2, 5)], q); Throws<ArgumentException>(() => Validation.Responses([new(1, 3)], q)); Throws<ArgumentException>(() => Validation.Responses([new(1, 3), new(2, 6)], q)); Throws<ArgumentException>(() => Validation.Responses([new(1, 3), new(1, 4)], q)); });
var degree = new DegreeProgramme { DegreeID = 1, FacultyID = 1, FacultyName = "Science", ProgrammeName = "Test degree", IsActive = true };
List<InterestQuestion> questions = [new(1, 1, "Enjoy science")]; List<InterestMapping> mappings = [new(1, 1, 1)];
List<ProgrammeRequirement> requirements = [new(1, 1, 1, "English", 60, true, null, ""), new(2, 1, 3, "Mathematics", 70, true, null, "")];
AdvisingRecord Complete(decimal maths = 75, decimal interest = 5) => new() { DegreeID = 1, FacultyID = 1, SubjectsComplete = true, Marks = [new(1, "English", 70), new(3, "Mathematics", maths)], Responses = [new(1, interest)] };
Recommendation Assess(AdvisingRecord s, List<ProgrammeRequirement>? r = null) => new RecommendationService().Rank(s, [degree], r ?? requirements, questions, mappings).Single();
Check("Meets exact minimums with high interest => Strong Match and 100 score", () => { var s = Complete(70); s.Marks[0] = new(1, "English", 60); var r = Assess(s); Assert(r.Outcome == "Strong Match" && r.AcademicEligibility == "Eligible" && r.OverallScore == 100); });
Check("Below minimum => Possible Match, explicitly academically ineligible", () => { var r = Assess(Complete(68)); Assert(r.Outcome == "Possible Match" && r.AcademicEligibility == "Not eligible" && r.Explanation.Contains("gap 2") && r.OverallScore == 99.57m); });
Check("Missing mandatory mathematics cannot be replaced by Mathematical Literacy", () => { var s = Complete(); s.Marks = [new(1, "English", 82), new(4, "Mathematical Literacy", 90)]; Assert(Assess(s).Outcome == "Not Currently Eligible"); });
Check("Incomplete subject capture or questionnaire => More Information Needed", () => { var s = Complete(); s.SubjectsComplete = false; Assert(Assess(s).Outcome == "More Information Needed"); s.SubjectsComplete = true; s.Responses = []; Assert(Assess(s).Outcome == "More Information Needed"); });
Check("Weak interests cannot imply academic ineligibility", () => { var r = Assess(Complete(80, 1)); Assert(r.Outcome == "Possible Match" && r.AcademicEligibility == "Eligible" && r.InterestScore == 0 && r.OverallScore == 70); });
Check("Explicit OR groups accept Mathematical Literacy; absent optional subject is not a gate", () =>
{ List<ProgrammeRequirement> r = [new(1, 1, 1, "English", 60, true, null, ""), new(2, 1, 3, "Maths", 50, false, "Numeracy", ""), new(3, 1, 4, "Math Literacy", 60, false, "Numeracy", ""), new(4, 1, 12, "CAT", 50, false, null, "")]; var s = Complete(); s.Marks = [new(1, "English", 80), new(4, "Math Literacy", 65)]; Assert(Assess(s, r).Outcome == "Strong Match"); s.Marks.RemoveAt(1); Assert(Assess(s, r).Outcome == "Not Currently Eligible"); });
Check("Missing programme rules or mappings never gives false positive", () => { Assert(Assess(Complete(), []).Outcome == "More Information Needed"); Assert(new RecommendationService().Rank(Complete(), [degree], requirements, questions, []).Single().Outcome == "More Information Needed"); });
Check("Rank eligible programmes first, exclude inactive, mark alternatives", () =>
{ var second = new DegreeProgramme { DegreeID = 2, ProgrammeName = "Alternative", FacultyID = 1, IsActive = true }; var inactive = new DegreeProgramme { DegreeID = 3, IsActive = false }; var rules = requirements.Concat(requirements.Select(r => r with { DegreeID = 2, MinimumMark = 50 })).ToList(); var result = new RecommendationService().Rank(Complete(65), [degree, second, inactive], rules, questions, [new(1, 1, 1), new(2, 1, 1)]); Assert(result.Count == 2 && result[0].DegreeID == 2 && result[0].IsAlternative && !result[1].IsAlternative); });
Check("SQL identifiers are allowlisted", () => { Throws<ArgumentException>(() => AdminDAL.Definition("Subject]; DROP TABLE Subject;--")); });


var fixture = System.Text.Json.JsonSerializer.Deserialize<SampleFixture>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "sample-fixtures.json")))!;
var scenarioNames = new[] { "Gadija Jacobs", "Aadiel-Raboo Magwa", "Labeeqah Rademeyer", "Siyabonga Dickson", "Derek Vin Diesel" };
var expectedOutcomes = new[] { "Strong Match", "Not Currently Eligible", "Possible Match", "Strong Match", "Not Currently Eligible" };
foreach (var session in fixture.Sessions.Take(5))
    Check("Case study: " + scenarioNames[session.SessionID - 1], () =>
    { var r = new RecommendationService().Rank(session, fixture.Degrees, fixture.Requirements, fixture.Questions, fixture.Mappings).Single(x => x.DegreeID == session.DegreeID); Assert(r.Outcome == expectedOutcomes[session.SessionID - 1], r.Outcome); });
Check("Management validation rejects empty names and invalid minimums", () =>
{ var d = AdminDAL.Definition("Subject"); Throws<ArgumentException>(() => ReferenceDataValidation.Validate(d, new Dictionary<string, object?> { { "SubjectName", " " } })); var requirement = AdminDAL.Definition("ProgrammeRequirement"); var values = new Dictionary<string, object?> { { "DegreeID", 1 }, { "SubjectID", 1 }, { "MinimumMark", 101m }, { "IsMandatory", true }, { "AlternativeGroup", null }, { "AdvisoryNote", null } }; Throws<ArgumentException>(() => ReferenceDataValidation.Validate(requirement, values)); values["MinimumMark"] = 60m; Assert(ReferenceDataValidation.Validate(requirement, values).Count == 6); });
Check("All active sample programmes have rules and interest mappings", () => { foreach (var d in fixture.Degrees) { Assert(fixture.Requirements.Any(r => r.DegreeID == d.DegreeID)); Assert(fixture.Mappings.Any(r => r.DegreeID == d.DegreeID)); } });
Check("All canonical SQL scripts parse as SQL Server T-SQL", () =>
{
    string path = Path.Combine(Directory.GetCurrentDirectory(), "Database");
    Assert(Directory.Exists(path), "Run tests from solution root.");
    foreach (var file in Directory.GetFiles(path, "*.sql"))
    {
        var parser = new Microsoft.SqlServer.TransactSql.ScriptDom.TSql160Parser(true);
        using var reader = new StringReader(File.ReadAllText(file)); parser.Parse(reader, out var errors);
        Assert(errors.Count == 0, Path.GetFileName(file) + ": " + string.Join("; ", errors.Select(e => $"line {e.Line}: {e.Message}")));
    }
});
Check("DAL raw SQL query grammar", () =>
{
    int parsed = 0;
    foreach (var file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "FuturePathStudentAdvisingSystem", "DataAccess"), "*.cs"))
        foreach (Match match in Regex.Matches(File.ReadAllText(file), "\"\"\"([\\s\\S]*?)\"\"\""))
        {
            var parser = new Microsoft.SqlServer.TransactSql.ScriptDom.TSql160Parser(true);
            using var reader = new StringReader(match.Groups[1].Value); parser.Parse(reader, out var errors); parsed++;
            Assert(errors.Count == 0, Path.GetFileName(file) + ": " + string.Join("; ", errors.Select(e => e.Message)));
        }
    Assert(parsed >= 10, "Expected at least ten actual SQL queries to be parsed.");
});
if (args.Contains("--emit-sample-sql"))
{
    var text = new System.Text.StringBuilder("-- Generated with the production RecommendationService and canonical sample fixtures. Empty-database seed only.\n");
    string Quote(string value) => "'" + value.Replace("'", "''") + "'";
    foreach (var session in fixture.Sessions)
    {
        text.AppendLine($"UPDATE dbo.AdvisorySession SET SubjectsComplete={(session.SubjectsComplete ? 1 : 0)},FundingRequested={(session.FundingRequested ? 1 : 0)},IsFinalized=1 WHERE SessionID={session.SessionID};");
        foreach (var response in session.Responses.Where(r => r.QuestionID >= 9)) text.AppendLine($"INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES({session.SessionID},{response.QuestionID},{response.Score.ToString(System.Globalization.CultureInfo.InvariantCulture)});");
        foreach (var result in new RecommendationService().Rank(session, fixture.Degrees, fixture.Requirements, fixture.Questions, fixture.Mappings))
            text.AppendLine($"INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative) VALUES({session.SessionID},{result.DegreeID},{Quote(result.Outcome)},{result.OverallScore.ToString(System.Globalization.CultureInfo.InvariantCulture)},{Quote(result.Explanation)},{(result.IsAlternative ? 1 : 0)});");
    }
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "tools", "sample-assessments.sql"), text.ToString());
}

if (!args.Contains("--database")) { skipped++; Console.WriteLine("SKIP LocalDB integration group: use --database on Windows; creates and removes a dedicated temporary test database."); }
else
{
    if (!OperatingSystem.IsWindows()) { failed++; Console.WriteLine("FAIL --database requires Windows SQL Server LocalDB."); }
    else RunDatabaseTests();
}
Console.WriteLine($"RESULT: {passed} passed, {failed} failed, {skipped} skipped groups");
return failed == 0 ? 0 : 1;

void RunDatabaseTests()
{
    string catalog = "FuturePath_IntegrationTests_" + Guid.NewGuid().ToString("N");
    string? old = Environment.GetEnvironmentVariable("FUTUREPATH_CONNECTION_STRING");
    var master = new SqlConnectionStringBuilder { DataSource = @"(localdb)\MSSQLLocalDB", InitialCatalog = "master", IntegratedSecurity = true, TrustServerCertificate = true };
    void Execute(string sql, string connection) { using var c = new SqlConnection(connection); c.Open(); using var cmd = new SqlCommand(sql, c) { CommandTimeout = 120 }; cmd.ExecuteNonQuery(); }
    try
    {
        Execute($"CREATE DATABASE [{catalog}]", master.ConnectionString);
        var target = new SqlConnectionStringBuilder(master.ConnectionString) { InitialCatalog = catalog };
        Environment.SetEnvironmentVariable("FUTUREPATH_CONNECTION_STRING", target.ConnectionString);
        string root = Directory.GetCurrentDirectory(); if (!Directory.Exists(Path.Combine(root, "Database"))) throw new Exception("Run from solution root.");
        void Script(string name)
        {
            string script = File.ReadAllText(Path.Combine(root, "Database", name)).Replace("FuturePathAdvisingSystem", catalog);
            foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                if (!string.IsNullOrWhiteSpace(batch)) Execute(batch, target.ConnectionString);
        }
        foreach (var file in new[] { "02_CreateTables.sql", "05_UpgradeIntegration.sql", "03_InsertSampleData.sql", "04_SetupDemoStaffAccounts.sql", "06_NormaliseSampleRules.sql" }) Script(file);
        Check("SQL safe setup repeatability preserves records", () =>
        { foreach (var file in new[] { "02_CreateTables.sql", "05_UpgradeIntegration.sql", "03_InsertSampleData.sql", "04_SetupDemoStaffAccounts.sql", "06_NormaliseSampleRules.sql" }) Script(file); Assert(new DegreeProgrammeDAL().GetActiveDegreeProgrammes().Count == 10); });
        var users = new UserAccountDAL();
        bool Login(string u, string p, string role) { bool ok = users.TryLogin(u, p, role, out int id, out string name); if (ok) AppSession.Start(id, role, name); return ok; }
        Check("SQL hashed staff login and incorrect password", () => { Assert(!Login("admin_test", "wrong", Roles.Admin)); Assert(Login("admin_test", "DemoAdmin2026!", Roles.Admin)); });
        int learner = 0;
        Check("SQL registration and profile update preserve one account", () =>
        { learner = users.CreateStudentAccount("Test", "Learner", "integration@example.invalid", "School", "Gauteng", "Integration-demo-2026!"); Assert(Login("integration@example.invalid", "Integration-demo-2026!", Roles.Learner)); users.SaveProfile(new() { UserID = learner, FirstName = "Changed", LastName = "Learner", Email = "integration@example.invalid", SchoolName = "Changed school", Province = "Gauteng", GradeLevelID = 2, Phone = "0712345678" }); Assert(users.GetLearnerProfile(learner)?.SchoolName == "Changed school"); Throws<SqlException>(() => users.CreateStudentAccount("Test", "Learner", "integration@example.invalid", "School", "Gauteng", "Integration-demo-2026!")); });
        int session = 0; var advising = new AdvisingDAL();
        Check("SQL session, marks CRUD and questionnaire persistence", () =>
        { Assert(Login("integration@example.invalid", "Integration-demo-2026!", Roles.Learner)); session = advising.CreateSession(learner, 2, 1); advising.SaveMarks(session, [new(1, "English", 72), new(3, "Mathematics", 70)], true); advising.SaveMarks(session, [new(1, "English", 80), new(3, "Mathematics", 75), new(7, "Accounting", 90)], true); advising.SaveMarks(session, [new(1, "English", 80), new(3, "Mathematics", 75)], true); advising.SaveResponses(session, advising.Questions().Select(q => new InterestResponse(q.QuestionID, 5)).ToList()); var loaded = advising.Load(session); Assert(loaded.Marks.Count == 2 && loaded.Marks.Single(m => m.SubjectID == 1).Mark == 80 && loaded.Responses.Count == advising.Questions().Count); });
        Check("SQL users cannot read other learner records", () => Throws<UnauthorizedAccessException>(() => users.GetLearnerProfile(1)));
        Check("SQL compute, persist, reload and freeze recommendations", () =>
        { var loaded = advising.Load(session); var results = AdvisingWorkflow.Calculate(loaded); advising.SaveRecommendations(session, results, true); Assert(advising.Load(session).IsFinalized); Assert(advising.StoredRecommendations(session).Rows.Count == 10); Throws<InvalidOperationException>(() => advising.SaveMarks(session, [new(1, "English", 90)], true)); advising.RecordExport(session); Assert(new ReportService().Summary(advising.Load(session)).Contains("Changed school")); });
        Check("SQL faculty/degree funding excludes inactive rows", () => { var f = advising.Funding(degreeID: 1); Assert(f.Count > 0 && f.All(x => x.FacultyName == "Commerce")); });
        Check("SQL admin CRUD and self-disable protection", () =>
        { Assert(Login("admin_test", "DemoAdmin2026!", Roles.Admin)); var admin = new AdminDAL(); admin.Save("Subject", null, new Dictionary<string, object?> { { "SubjectName", "Integration test subject" } }); var row = admin.List("Subject").AsEnumerable().Single(r => (string)r["SubjectName"] == "Integration test subject"); int id = (int)row["SubjectID"]; admin.Save("Subject", id, new Dictionary<string, object?> { { "SubjectName", "Updated integration subject" } }); Assert(admin.List("Subject").AsEnumerable().Any(r => (string)r["SubjectName"] == "Updated integration subject")); admin.Delete("Subject", id); Assert(!admin.List("Subject").AsEnumerable().Any(r => (int)r["SubjectID"] == id)); Throws<ArgumentException>(() => admin.Delete("UserAccount", AppSession.UserID!.Value)); });
        Check("SQL advisor assignment limits search and detail", () =>
        {
            Assert(Login("advisor_test", "DemoAdvisor2026!", Roles.Advisor)); Assert(advising.SearchLearners("integration").Rows.Count == 0); Throws<UnauthorizedAccessException>(() => advising.Load(session)); int advisor = AppSession.UserID!.Value;
            Assert(Login("admin_test", "DemoAdmin2026!", Roles.Admin)); var admin = new AdminDAL(); var r = admin.List("UserAccount").AsEnumerable().Single(x => (int)x["UserID"] == learner); var values = AdminDAL.Definition("UserAccount").Fields.ToDictionary(f => f.Name, f => r.IsNull(f.Name) ? null : r[f.Name]); values["AdvisorUserID"] = advisor; admin.Save("UserAccount", learner, values);
            Assert(Login("advisor_test", "DemoAdvisor2026!", Roles.Advisor)); Assert(advising.Load(session).UserID == learner); advising.AddNote(session, "Review subject gaps with learner."); Assert(advising.Notes(session).Rows.Count == 1); Assert(new ReportDAL().EligibilityActivity().Rows.Count > 0); Assert(new ReportDAL().Exceptions().Rows.Count == 0);
        });
        Check("SQL supplied five learner scenarios match case study", () =>
        { Assert(Login("admin_test", "DemoAdmin2026!", Roles.Admin)); var expected = new[] { "Strong Match", "Not Currently Eligible", "Possible Match", "Strong Match", "Not Currently Eligible" }; for (int i = 1; i <= 5; i++) { var s = advising.Load(i); s.SubjectsComplete = true; var qs = advising.Questions(); foreach (var q in qs) if (!s.Responses.Any(r => r.QuestionID == q.QuestionID)) s.Responses.Add(new(q.QuestionID, 4)); Assert(AdvisingWorkflow.Calculate(s).Single(r => r.DegreeID == s.DegreeID).Outcome == expected[i - 1], "Case study learner " + i); } });
    }
    catch (Exception ex) { failed++; Console.WriteLine("FAIL SQL integration setup: " + ex.Message); }
    finally
    {
        AppSession.End(); Environment.SetEnvironmentVariable("FUTUREPATH_CONNECTION_STRING", old); SqlConnection.ClearAllPools();
        try { Execute($"IF DB_ID(N'{catalog}') IS NOT NULL BEGIN ALTER DATABASE [{catalog}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{catalog}]; END", master.ConnectionString); }
        catch (Exception ex) { failed++; Console.WriteLine("FAIL temporary test database cleanup: " + ex.Message); }
    }
}

public class SampleFixture
{
    public List<DegreeProgramme> Degrees { get; set; } = [];
    public List<ProgrammeRequirement> Requirements { get; set; } = [];
    public List<InterestQuestion> Questions { get; set; } = [];
    public List<InterestMapping> Mappings { get; set; } = [];
    public List<AdvisingRecord> Sessions { get; set; } = [];
}
