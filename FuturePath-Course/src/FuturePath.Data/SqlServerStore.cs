using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class SqlServerStore : DB, IFuturePathStore
    {
        private readonly ReferenceDataDAL referenceDataDAL;
        private readonly DegreeProgrammeDAL degreeProgrammeDAL;
        private readonly FundingDAL fundingDAL;
        private readonly AdvisorySessionDAL advisorySessionDAL;
        private readonly UserAccountDAL userAccountDAL;
        private readonly ReportDAL reportDAL;
        public SqlServerStore(string databaseName = DatabaseConnection.DefaultDatabase) : base(databaseName)
        {
            referenceDataDAL = new ReferenceDataDAL(databaseName);
            degreeProgrammeDAL = new DegreeProgrammeDAL(databaseName);
            fundingDAL = new FundingDAL(databaseName);
            advisorySessionDAL = new AdvisorySessionDAL(databaseName);
            userAccountDAL = new UserAccountDAL(databaseName);
            reportDAL = new ReportDAL(databaseName);
        }

        public List<LookupItem> GetLookup(string key)
        {
            return referenceDataDAL.GetLookup(key);
        }

        public List<InterestQuestion> GetQuestions(bool activeOnly = true)
        {
            return referenceDataDAL.GetQuestions(activeOnly);
        }

        public List<ReferenceRow> GetReference(string key)
        {
            return referenceDataDAL.GetReference(key);
        }

        public int SaveReference(string key, int id, IReadOnlyDictionary<string, object?> values)
        {
            return referenceDataDAL.SaveReference(key, id, values);
        }

        public List<DegreeProgramme> GetProgrammes(bool activeOnly)
        {
            return degreeProgrammeDAL.GetProgrammes(activeOnly);
        }

        public List<FundingOption> GetFunding(int degreeId, DateOnly today)
        {
            return fundingDAL.GetFunding(degreeId, today);
        }

        public LearnerProfile? GetLearner(int id)
        {
            return advisorySessionDAL.GetLearner(id);
        }

        public int SaveSession(AdvisorySession session, int? actorId = null)
        {
            return advisorySessionDAL.SaveSession(session, actorId);
        }

        public AdvisorySession? GetSession(int id)
        {
            return advisorySessionDAL.GetSession(id);
        }

        public List<SessionSummary> FindSessions(DateOnly from, DateOnly to, string search)
        {
            return advisorySessionDAL.FindSessions(from, to, search);
        }

        public UserAccount? FindUser(string username)
        {
            return userAccountDAL.FindUser(username);
        }

        public UserAccount? GetUser(int id)
        {
            return userAccountDAL.GetUser(id);
        }

        public void RecordLogin(int userId, bool success, DateTime now)
        {
            userAccountDAL.RecordLogin(userId, success, now);
        }

        public void SavePassword(int userId, string hash)
        {
            userAccountDAL.SavePassword(userId, hash);
        }

        public void SaveUser(int id, string username, UserRole role, bool active, string? passwordHash)
        {
            userAccountDAL.SaveUser(id, username, role, active, passwordHash);
        }

        public void Audit(int? userId, string action, string detail, DateTime now)
        {
            reportDAL.Audit(userId, action, detail, now);
        }

        public void RecordExport(int sessionId, string path, DateTime now)
        {
            reportDAL.RecordExport(sessionId, path, now);
        }

        public void Backup(string path)
        {
            reportDAL.Backup(path);
        }
    }
}
