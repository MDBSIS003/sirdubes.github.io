using System.Globalization;
using FuturePath.Business;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FuturePath.Data;
/// <summary>ADO.NET operations for the existing Future Path schema. Services enforce access before calling this store.</summary>
public sealed class SqlServerStore : IFuturePathStore
{
    private readonly DatabaseConnection database;
    public string DatabaseName => database.DatabaseName;
    public string ConnectionString => database.ConnectionString;

    public SqlServerStore(string databaseName = DatabaseConnection.DefaultDatabase)
    {
        database = new DatabaseConnection(databaseName);
    }

    private SqlConnection Open()
    {
        var connection = database.CreateConnection();
        connection.Open();
        return connection;
    }

    private static SqlCommand Command(SqlConnection connection, string sql, SqlTransaction? transaction = null, params object? [] values)
    {
        var command = new SqlCommand(sql, connection, transaction);
        for (int index = 0; index < values.Length; index++)
        {
            object value = values[index] ?? DBNull.Value;
            var parameter = command.Parameters.AddWithValue("@p" + index, value);
            if (value is DateTime)
                parameter.SqlDbType = SqlDbType.DateTime2;
            if (value is decimal)
            {
                parameter.SqlDbType = SqlDbType.Decimal;
                parameter.Precision = 7;
                parameter.Scale = 2;
            }
        }

        return command;
    }

    private static int Execute(SqlConnection connection, string sql, SqlTransaction? transaction = null, params object? [] values)
    {
        using var command = Command(connection, sql, transaction, values);
        return command.ExecuteNonQuery();
    }

    private static DateTime Utc(DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc);
    private static string Lines(string value) => value.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
    public void Initialize()
    {
        // Startup verifies an existing installation. Creating/seeding a database is an explicit setup action.
        try
        {
            using var connection = Open();
            using var command = Command(connection, "SELECT MAX(Version) FROM dbo.SchemaVersion");
            object? version = command.ExecuteScalar();
            if (version == null || version == DBNull.Value || Convert.ToInt32(version) != 1)
                throw new ValidationException("The database schema version is not supported. See docs/Database-Setup.md.");
        }
        catch (SqlException ex)when (ex.Number is 4060 or 208 or -2 or 2 or 26 or 50)
        {
            throw new ValidationException("Cannot open the Future Path LocalDB database. Install SQL Server Express LocalDB through Visual Studio Installer, then use Prepare demonstration database in the app. " + "Expected database: " + DatabaseName + ". Existing data is not reset.");
        }
    }

    public List<LookupItem> GetLookup(string key)
    {
        var parts = key.Split(':');
        var table = parts[0];
        if (table is not ("Faculty" or "Subject" or "InterestCategory" or "DegreeProgramme" or "FundingOption"))
            throw new ValidationException("Invalid lookup.");
        using var c = Open();
        using var cmd = Command(c, $"SELECT Id,Name FROM {table}" + (parts.Length == 1 ? " WHERE Active=1" : "") + " ORDER BY Name");
        using var r = cmd.ExecuteReader();
        var result = new List<LookupItem>();
        while (r.Read())
            result.Add(new LookupItem { Id = r.GetInt32(0), Name = r.GetString(1) });
        return result;
    }

    public List<DegreeProgramme> GetProgrammes(bool activeOnly)
    {
        using var c = Open();
        var result = new List<DegreeProgramme>();
        using (var cmd = Command(c, "SELECT d.Id,d.FacultyId,f.Name,d.Name,d.Description,d.Active,d.MinimumAverage FROM DegreeProgramme d JOIN Faculty f ON f.Id=d.FacultyId" + (activeOnly ? " WHERE d.Active=1 AND f.Active=1" : "") + " ORDER BY d.Name"))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                result.Add(new DegreeProgramme { Id = r.GetInt32(0), FacultyId = r.GetInt32(1), Faculty = r.GetString(2), Name = r.GetString(3), Description = r.GetString(4), Active = r.GetBoolean(5), MinimumAverage = r.GetDecimal(6) });
        foreach (var p in result)
        {
            using (var cmd = Command(c, "SELECT q.SubjectId,s.Name,q.MinimumMark,q.Mandatory,q.AlternativeGroup,q.Note FROM ProgrammeRequirement q JOIN Subject s ON s.Id=q.SubjectId WHERE q.DegreeId=@p0 AND q.Active=1 ORDER BY q.Id", null, p.Id))
            using (var r = cmd.ExecuteReader())
                while (r.Read())
                    p.Requirements.Add(new ProgrammeRequirement { SubjectId = r.GetInt32(0), Subject = r.GetString(1), MinimumMark = r.GetDecimal(2), Mandatory = r.GetBoolean(3), AlternativeGroup = r.GetInt32(4), Note = r.GetString(5) });
            using (var cmd = Command(c, "SELECT CategoryId,Weight FROM DegreeInterestMapping WHERE DegreeId=@p0 AND Active=1 ORDER BY CategoryId", null, p.Id))
            using (var r = cmd.ExecuteReader())
                while (r.Read())
                    p.Interests.Add(new InterestWeight { CategoryId = r.GetInt32(0), Weight = r.GetDecimal(1) });
        }

        return result;
    }

    public List<InterestQuestion> GetQuestions(bool activeOnly = true)
    {
        using var c = Open();
        using var cmd = Command(c, "SELECT q.Id,q.CategoryId,i.Name,q.Text FROM InterestQuestion q JOIN InterestCategory i ON i.Id=q.CategoryId" + (activeOnly ? " WHERE q.Active=1 AND i.Active=1" : "") + " ORDER BY q.Id");
        using var r = cmd.ExecuteReader();
        var result = new List<InterestQuestion>();
        while (r.Read())
            result.Add(new InterestQuestion { Id = r.GetInt32(0), CategoryId = r.GetInt32(1), Category = r.GetString(2), Text = r.GetString(3) });
        return result;
    }

    public List<FundingOption> GetFunding(int degreeId, DateOnly today)
    {
        using var c = Open();
        // Explicit programme links narrow an award; faculty-wide options have no active programme links.
        using var cmd = Command(c, """
            SELECT f.Id,f.Name,f.Type,f.Eligibility,f.Documents,f.ClosingDate,f.Contact,f.Sample
            FROM FundingOption f JOIN DegreeProgramme d ON d.Id=@p0 JOIN Faculty fa ON fa.Id=d.FacultyId
            WHERE f.Active=1 AND d.Active=1 AND fa.Active=1 AND f.ClosingDate>=@p1 AND (
              EXISTS(SELECT 1 FROM FundingProgramme fp WHERE fp.FundingId=f.Id AND fp.DegreeId=d.Id AND fp.Active=1)
              OR (NOT EXISTS(SELECT 1 FROM FundingProgramme fp WHERE fp.FundingId=f.Id AND fp.Active=1)
                  AND EXISTS(SELECT 1 FROM FundingFaculty ff WHERE ff.FundingId=f.Id AND ff.FacultyId=d.FacultyId AND ff.Active=1)))
            ORDER BY f.ClosingDate,f.Name
            """, null, degreeId, today.ToDateTime(TimeOnly.MinValue));
        using var r = cmd.ExecuteReader();
        var result = new List<FundingOption>();
        while (r.Read())
            result.Add(new FundingOption { Id = r.GetInt32(0), Name = r.GetString(1), Type = r.GetString(2), Eligibility = r.GetString(3), Documents = r.GetString(4), ClosingDate = DateOnly.FromDateTime(r.GetDateTime(5)), Contact = r.GetString(6), Sample = r.GetBoolean(7) });
        return result;
    }

    public LearnerProfile? GetLearner(int id)
    {
        using var c = Open();
        using var cmd = Command(c, "SELECT Id,Name,GradeId,School,Province,Email,Phone FROM Learner WHERE Id=@p0", null, id);
        using var r = cmd.ExecuteReader();
        return r.Read() ? new LearnerProfile
        {
            Id = r.GetInt32(0),
            Name = r.GetString(1),
            Grade = r.GetInt32(2),
            School = r.GetString(3),
            Province = r.GetString(4),
            Email = r.GetString(5),
            Phone = r.GetString(6)
        }

        : null;
    }

    public int SaveSession(AdvisorySession session, int? actorId = null)
    {
        using var c = Open();
        using var tx = c.BeginTransaction();
        var input = session.Input;
        var l = input.Learner;
        var learnerId = l.Id;
        if (learnerId == 0)
        {
            using var learnerCommand = Command(c, "INSERT INTO Learner(Name,GradeId,School,Province,Email,Phone) OUTPUT INSERTED.Id VALUES(@p0,@p1,@p2,@p3,@p4,@p5)", tx, l.Name, l.Grade, l.School, l.Province, l.Email, l.Phone);
            learnerId = Convert.ToInt32(learnerCommand.ExecuteScalar());
        }

        using var sessionCommand = Command(c, "INSERT INTO AdvisorySession(LearnerId,GradeId,DegreeId,PreferredFacultyId,CreatedUtc,SubjectsComplete,FundingRequested,RuleVersion) OUTPUT INSERTED.Id VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)", tx, learnerId, l.Grade, input.DegreeId, input.PreferredFacultyId, session.CreatedUtc, input.SubjectsComplete, input.FundingRequested, session.RuleVersion);
        int id = Convert.ToInt32(sessionCommand.ExecuteScalar());
        foreach (var mark in input.Marks)
            Execute(c, "INSERT INTO LearnerSubject VALUES(@p0,@p1,@p2)", tx, id, mark.SubjectId, mark.Mark);
        foreach (var answer in input.Responses)
            Execute(c, "INSERT INTO LearnerInterestResponse VALUES(@p0,@p1,@p2)", tx, id, answer.QuestionId, answer.Score);
        SaveResults(c, tx, id, session);
        Execute(c, "INSERT INTO AuditEvent(UserId,Action,Detail,CreatedUtc) VALUES(@p0,@p1,@p2,@p3)", tx, actorId, "Advisory session created", "Session " + id, session.CreatedUtc);
        tx.Commit();
        l.Id = learnerId;
        return id;
    }

    private static void SaveResults(SqlConnection c, SqlTransaction tx, int id, AdvisorySession session)
    {
        var rank = 0;
        foreach (var r in new[]
        {
            session.Selected
        }.Concat(session.Alternatives))
            Execute(c, "INSERT INTO RecommendationResult VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", tx, id, r.DegreeId, r.Outcome.ToString(), r.SubjectScore, r.MarkScore, r.InterestScore, r.PreferenceScore, r.OverallScore, r.Explanation, rank++);
        foreach (var category in session.InterestScores)
            Execute(c, "INSERT INTO SessionInterestScore SELECT @p0,Id,@p2 FROM InterestCategory WHERE Name=@p1", tx, id, category.Key, category.Value);
    }

    public AdvisorySession? GetSession(int id)
    {
        using var c = Open();
        AdvisoryInput input;
        DateTime created;
        string version;
        using (var cmd = Command(c, "SELECT LearnerId,GradeId,DegreeId,PreferredFacultyId,CreatedUtc,SubjectsComplete,FundingRequested,RuleVersion FROM AdvisorySession WHERE Id=@p0", null, id))
        using (var r = cmd.ExecuteReader())
        {
            if (!r.Read())
                return null;
            input = new AdvisoryInput
            {
                Learner = GetLearner(r.GetInt32(0))!,
                DegreeId = r.GetInt32(2),
                PreferredFacultyId = r.GetInt32(3),
                SubjectsComplete = r.GetBoolean(5),
                FundingRequested = r.GetBoolean(6)
            };
            input.Learner.Grade = r.GetInt32(1);
            created = Utc(r.GetDateTime(4));
            version = r.GetString(7);
        }

        using (var cmd = Command(c, "SELECT m.SubjectId,s.Name,m.Mark FROM LearnerSubject m JOIN Subject s ON s.Id=m.SubjectId WHERE m.SessionId=@p0 ORDER BY s.Name", null, id))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                input.Marks.Add(new SubjectMark { SubjectId = r.GetInt32(0), SubjectName = r.GetString(1), Mark = r.GetDecimal(2) });
        using (var cmd = Command(c, "SELECT QuestionId,Score FROM LearnerInterestResponse WHERE SessionId=@p0", null, id))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                input.Responses.Add(new InterestResponse { QuestionId = r.GetInt32(0), Score = r.GetInt32(1) });
        var results = new List<Recommendation>();
        using (var cmd = Command(c, "SELECT r.DegreeId,d.Name,f.Name,r.Outcome,r.SubjectScore,r.MarkScore,r.InterestScore,r.PreferenceScore,r.OverallScore,r.Explanation FROM RecommendationResult r JOIN DegreeProgramme d ON d.Id=r.DegreeId JOIN Faculty f ON f.Id=d.FacultyId WHERE r.SessionId=@p0 ORDER BY r.[Rank]", null, id))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                results.Add(new Recommendation { DegreeId = r.GetInt32(0), Programme = r.GetString(1), Faculty = r.GetString(2), Outcome = Enum.Parse<AdvisoryOutcome>(r.GetString(3)), SubjectScore = r.GetDecimal(4), MarkScore = r.GetDecimal(5), InterestScore = r.GetDecimal(6), PreferenceScore = r.GetDecimal(7), OverallScore = r.GetDecimal(8), Explanation = Lines(r.GetString(9)) });
        var interests = new Dictionary<string, decimal>();
        using (var cmd = Command(c, "SELECT i.Name,s.Score FROM SessionInterestScore s JOIN InterestCategory i ON i.Id=s.CategoryId WHERE s.SessionId=@p0", null, id))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                interests[r.GetString(0)] = r.GetDecimal(1);
        return new AdvisorySession
        {
            Id = id,
            CreatedUtc = created,
            RuleVersion = version,
            Input = input,
            Selected = results.FirstOrDefault() ?? new Recommendation(),
            Alternatives = results.Skip(1).ToList(),
            InterestScores = interests
        };
    }

    public List<SessionSummary> FindSessions(DateOnly from, DateOnly to, string search)
    {
        using var c = Open();
        using var cmd = Command(c, """
            SELECT s.Id,l.Id,l.Name,s.GradeId,s.CreatedUtc,d.Name,f.Name,r.Outcome,r.OverallScore,s.FundingRequested,r.Explanation
            FROM AdvisorySession s JOIN Learner l ON l.Id=s.LearnerId JOIN DegreeProgramme d ON d.Id=s.DegreeId
            JOIN Faculty f ON f.Id=d.FacultyId JOIN RecommendationResult r ON r.SessionId=s.Id AND r.[Rank]=0
            WHERE CAST(s.CreatedUtc AS date)>=@p0 AND CAST(s.CreatedUtc AS date)<=@p1
              AND (CHARINDEX(@p2,l.Name)>0 OR CHARINDEX(@p2,d.Name)>0 OR CONVERT(nvarchar(20),s.Id)=@p2)
            ORDER BY s.CreatedUtc DESC,s.Id DESC
            """, null, from.ToDateTime(TimeOnly.MinValue), to.ToDateTime(TimeOnly.MinValue), search);
        using var r = cmd.ExecuteReader();
        var result = new List<SessionSummary>();
        while (r.Read())
            result.Add(new SessionSummary { Id = r.GetInt32(0), LearnerId = r.GetInt32(1), Learner = r.GetString(2).Length == 0 ? "Anonymous learner" : r.GetString(2), Grade = r.GetInt32(3), CreatedUtc = Utc(r.GetDateTime(4)), Programme = r.GetString(5), Faculty = r.GetString(6), Outcome = new Recommendation { Outcome = Enum.Parse<AdvisoryOutcome>(r.GetString(7)) }.OutcomeText, Score = r.GetDecimal(8), FundingRequested = r.GetBoolean(9), Explanation = Lines(r.GetString(10)) });
        return result;
    }

    private static UserAccount ReadUser(SqlDataReader r) => new()
    {
        Id = r.GetInt32(0),
        Username = r.GetString(1),
        PasswordHash = r.GetString(2),
        Role = Enum.Parse<UserRole>(r.GetString(3)),
        Active = r.GetBoolean(4),
        FailedAttempts = r.GetInt32(5),
        LockedUntilUtc = r.IsDBNull(6) ? null : Utc(r.GetDateTime(6))
    };
    public UserAccount? FindUser(string username)
    {
        using var c = Open();
        using var cmd = Command(c, "SELECT * FROM UserAccount WHERE Username=@p0", null, username);
        using var r = cmd.ExecuteReader();
        return r.Read() ? ReadUser(r) : null;
    }

    public UserAccount? GetUser(int id)
    {
        using var c = Open();
        using var cmd = Command(c, "SELECT * FROM UserAccount WHERE Id=@p0", null, id);
        using var r = cmd.ExecuteReader();
        return r.Read() ? ReadUser(r) : null;
    }

    public void RecordLogin(int userId, bool success, DateTime now)
    {
        using var c = Open();
        Execute(c, """
            UPDATE UserAccount SET FailedAttempts=CASE WHEN @p1=1 OR LockedUntilUtc<=@p2 THEN 0 ELSE FailedAttempts END WHERE Id=@p0;
            UPDATE UserAccount SET FailedAttempts=CASE WHEN @p1=1 THEN 0 ELSE FailedAttempts+1 END,
             LockedUntilUtc=CASE WHEN @p1=1 THEN NULL WHEN FailedAttempts>=4 THEN @p3 ELSE NULL END WHERE Id=@p0;
            """, null, userId, success, now, now.AddMinutes(5));
    }

    public void SavePassword(int userId, string hash)
    {
        using var c = Open();
        Execute(c, "UPDATE UserAccount SET PasswordHash=@p1,FailedAttempts=0,LockedUntilUtc=NULL WHERE Id=@p0", null, userId, hash);
    }

    public void SaveUser(int id, string username, UserRole role, bool active, string? passwordHash)
    {
        using var c = Open();
        try
        {
            if (id == 0)
                Execute(c, "INSERT INTO UserAccount(Username,PasswordHash,Role,Active) VALUES(@p0,@p1,@p2,@p3)", null, username, passwordHash, role.ToString(), active);
            else if (Execute(c, "UPDATE UserAccount SET Username=@p0,Role=@p1,Active=@p2,PasswordHash=COALESCE(@p3,PasswordHash),FailedAttempts=0,LockedUntilUtc=NULL WHERE Id=@p4", null, username, role.ToString(), active, passwordHash, id) == 0)
                throw new ValidationException("Account not found.");
        }
        catch (SqlException ex)when (ex.Number is 2601 or 2627 or 547 or 515 or 2628)
        {
            throw new ValidationException("An account with that username already exists, or the account values are invalid.");
        }
    }

    public List<ReferenceRow> GetReference(string key)
    {
        var sql = key switch
        {
            "UserAccount" => "SELECT Id,Username,Role,Active,FailedAttempts,LockedUntilUtc FROM UserAccount ORDER BY Username",
            "AuditEvent" => "SELECT TOP (500) a.Id,COALESCE(u.Username,'Learner / setup') AS Actor,a.Action,a.Detail,a.CreatedUtc FROM AuditEvent a LEFT JOIN UserAccount u ON u.Id=a.UserId ORDER BY a.Id DESC",
            _ => $"SELECT Id,{string.Join(',', ReferenceCatalog.Get(key).Fields.Select(f => "[" + f.Name + "]"))} FROM {key} ORDER BY Id"};
        using var c = Open();
        using var cmd = Command(c, sql);
        using var r = cmd.ExecuteReader();
        var result = new List<ReferenceRow>();
        while (r.Read())
        {
            var values = new Dictionary<string, object?>();
            for (var i = 1; i < r.FieldCount; i++)
                values[r.GetName(i)] = r.IsDBNull(i) ? null : ReadReferenceValue(r, i);
            result.Add(new ReferenceRow { Id = r.GetInt32(0), Values = values });
        }

        return result;
    }

    public int SaveReference(string key, int id, IReadOnlyDictionary<string, object?> values)
    {
        var fields = ReferenceCatalog.Get(key).Fields.Select(field => field.Name).ToArray();
        var arguments = fields.Select(field =>
        {
            object? value = values[field];
            if (field == "ClosingDate" && value is string text)
                return (object)DateOnly.ParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToDateTime(TimeOnly.MinValue);
            return value;
        }).Append(id).ToArray();
        string columns = string.Join(",", fields.Select(field => "[" + field + "]"));
        string parameters = string.Join(",", fields.Select((field, index) => "@p" + index));
        string assignments = string.Join(",", fields.Select((field, index) => "[" + field + "]=@p" + index));
        string sql = id == 0 ? $"INSERT INTO [{key}]({columns}) OUTPUT INSERTED.Id VALUES({parameters})" : $"UPDATE [{key}] SET {assignments} WHERE Id=@p{fields.Length}";
        try
        {
            using var connection = Open();
            using var command = Command(connection, sql, null, arguments);
            if (id == 0)
                return Convert.ToInt32(command.ExecuteScalar());
            if (command.ExecuteNonQuery() != 1)
                throw new ValidationException("Record not found. Refresh and try again.");
            return id;
        }
        catch (SqlException ex)when (ex.Number is 2601 or 2627 or 547 or 515 or 2628)
        {
            throw new ValidationException("Duplicate record, invalid reference or value too long. Check required values and linked records.");
        }
    }

    private static object? ReadReferenceValue(SqlDataReader reader, int index)
    {
        if (reader.IsDBNull(index))
            return null;
        object value = reader.GetValue(index);
        if (value is bool flag)
            return flag ? 1 : 0;
        if (value is DateTime date)
            return reader.GetName(index) == "ClosingDate" ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : Utc(date).ToString("O", CultureInfo.InvariantCulture);
        return value;
    }

    public void Audit(int? userId, string action, string detail, DateTime now)
    {
        using var c = Open();
        Execute(c, "INSERT INTO AuditEvent(UserId,Action,Detail,CreatedUtc) VALUES(@p0,@p1,@p2,@p3)", null, userId, action, detail, now);
    }

    public void RecordExport(int sessionId, string path, DateTime now)
    {
        using var c = Open();
        Execute(c, "INSERT INTO ExportReport(SessionId,Path,GeneratedUtc) VALUES(@p0,@p1,@p2)", null, sessionId, path, now);
    }

    public void Backup(string path)
    {
        path = Path.GetFullPath(path);
        if (!Path.GetExtension(path).Equals(".bak", StringComparison.OrdinalIgnoreCase))
            throw new ValidationException("Choose a new SQL Server backup filename ending in .bak.");
        if (File.Exists(path))
            throw new ValidationException("Choose a new backup filename to preserve existing backups.");
        using var connection = Open();
        using var command = Command(connection, "BACKUP DATABASE [" + DatabaseName + "] TO DISK=@p0 WITH COPY_ONLY,CHECKSUM; RESTORE VERIFYONLY FROM DISK=@p0 WITH CHECKSUM;", null, path);
        command.CommandTimeout = 120;
        command.ExecuteNonQuery();
    }
}
