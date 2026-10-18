using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
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
        void SaveSecurity(int userId, string username, string? passwordHash, DateTime now);
        void SaveUser(int id, string username, UserRole role, bool active, string? passwordHash);
        List<ReferenceRow> GetReference(string key);
        int SaveReference(string key, int id, IReadOnlyDictionary<string, object?> values);
        void Audit(int? userId, string action, string detail, DateTime now);
        void RecordExport(int sessionId, string path, DateTime now);
        void Backup(string path);
    }
}
