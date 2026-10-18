using System.Globalization;
using System.Text;
using System.Text.Json;
using FuturePath.Business;
using FuturePath.Fixtures;

string root = Directory.GetCurrentDirectory();
var fixture = new ReferenceFixture(Path.Combine(root, "validation", "reference", "sample-data.json"));
var sql = new StringBuilder("USE FuturePathStudentAdvisingSystem;\nGO\nSET XACT_ABORT ON;\nBEGIN TRANSACTION;\n");
string[] order = ["GradeLevel", "Subject", "Faculty", "InterestCategory", "DegreeProgramme", "InterestQuestion", "ProgrammeRequirement", "DegreeInterestMapping", "FundingOption", "FundingFaculty", "FundingProgramme", "Learner", "AdvisorySession", "LearnerSubject", "LearnerInterestResponse"];
sql.AppendLine("-- Explicit sample setup, not application startup. A nonempty database is left untouched.");
sql.AppendLine("IF " + string.Join(" OR ", order.Concat(["UserAccount", "RecommendationResult", "SessionInterestScore", "ExportReport", "AuditEvent"]).Select(t => $"EXISTS(SELECT 1 FROM dbo.[{t}] WITH (UPDLOCK,HOLDLOCK))")));
sql.AppendLine("BEGIN PRINT 'Sample seed skipped: existing records were preserved.'; COMMIT TRANSACTION; RETURN; END;");
string Quote(string value) => "N'" + value.Replace("'", "''") + "'";
string Value(JsonElement value, string column)
{
    if (value.ValueKind == JsonValueKind.Null)
        return "NULL";
    if (value.ValueKind != JsonValueKind.String)
        return value.GetRawText();
    string text = value.GetString()!;
    if (column.EndsWith("Utc", StringComparison.Ordinal))
    {
        DateTime utc = DateTime.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToUniversalTime();
        return "CONVERT(datetime2(7)," + Quote(utc.ToString("yyyy-MM-ddTHH:mm:ss.fffffff", CultureInfo.InvariantCulture)) + ",126)";
    }

    if (column == "ClosingDate")
        return "CONVERT(date," + Quote(text) + ",23)";
    return Quote(text);
}

foreach (string table in order)
{
    var rows = fixture.Rows(table);
    if (rows.Count == 0)
        continue;
    bool identity = table != "GradeLevel" && rows[0].TryGetProperty("Id", out _);
    if (identity)
        sql.AppendLine($"SET IDENTITY_INSERT dbo.[{table}] ON;");
    foreach (var row in rows)
    {
        var fields = row.EnumerateObject().ToList();
        sql.AppendLine($"INSERT dbo.[{table}]({string.Join(",", fields.Select(f => "[" + f.Name + "]"))}) VALUES({string.Join(",", fields.Select(f => Value(f.Value, f.Name)))});");
    }

    if (identity)
        sql.AppendLine($"SET IDENTITY_INSERT dbo.[{table}] OFF;");
}

var engine = new RecommendationEngine();
foreach (var row in fixture.Rows("AdvisorySession"))
{
    int id = ReferenceFixture.Number(row, "Id");
    DateTime created = DateTime.Parse(ReferenceFixture.Text(row, "CreatedUtc"), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    var assessment = engine.Assess(fixture.Input(id), fixture.Programmes(), fixture.Questions(), created);
    int rank = 0;
    foreach (var result in new[]
    {
        assessment.Selected
    }.Concat(assessment.Alternatives))
    {
        string Dec(decimal value) => value.ToString(CultureInfo.InvariantCulture);
        sql.AppendLine($"INSERT dbo.RecommendationResult(SessionId,DegreeId,Outcome,SubjectScore,MarkScore,InterestScore,PreferenceScore,OverallScore,Explanation,[Rank]) VALUES({id},{result.DegreeId},{Quote(result.Outcome.ToString())},{Dec(result.SubjectScore)},{Dec(result.MarkScore)},{Dec(result.InterestScore)},{Dec(result.PreferenceScore)},{Dec(result.OverallScore)},{Quote(result.Explanation)},{rank++});");
    }

    foreach (var category in assessment.InterestScores)
        sql.AppendLine($"INSERT dbo.SessionInterestScore(SessionId,CategoryId,Score) SELECT {id},Id,{category.Value.ToString(CultureInfo.InvariantCulture)} FROM dbo.InterestCategory WHERE Name={Quote(category.Key)};");
}

// These public credentials belong only to explicitly requested fictional demonstration databases.
var hasher = new PasswordHasher();
foreach (var account in new[]
{
    ("admin_test", "FuturePathAdmin2026!", "Administrator"),
    ("advisor_test", "FuturePathAdvisor2026!", "Advisor")
}

)
    sql.AppendLine($"INSERT dbo.UserAccount(Username,PasswordHash,Role,Active) VALUES({Quote(account.Item1)},{Quote(hasher.Hash(account.Item2))},{Quote(account.Item3)},1);");
sql.AppendLine("COMMIT TRANSACTION;\nGO");
File.WriteAllText(Path.Combine(root, "Database", "03_SeedData.sql"), sql.ToString(), new UTF8Encoding(false));
Console.WriteLine("Generated LocalDB seed using the production rules and unchanged reference inputs. No database was opened.");
