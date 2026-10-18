using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class ReferenceDataDAL : DB
    {
        public ReferenceDataDAL(string databaseName = DatabaseConnection.DefaultDatabase) : base(databaseName)
        {
        }

        public List<LookupItem> GetLookup(string key)
        {
            string[] parts = key.Split(':');
            string table = parts[0];
            if (table is not ("Faculty" or "Subject" or "InterestCategory" or "DegreeProgramme" or "FundingOption"))
            {
                throw new ValidationException("Invalid lookup.");
            }

            using (SqlConnection c = Open())
            {
                using (SqlCommand cmd = Command(c, $"SELECT Id,Name FROM {table}" + (parts.Length == 1 ? " WHERE Active=1" : "") + " ORDER BY Name"))
                {
                    DataTable tableData = ReadData(cmd);
                    List<LookupItem> result = new List<LookupItem>();
                    foreach (DataRow row in tableData.Rows)
                    {
                        LookupItem item = new LookupItem();
                        item.Id = Convert.ToInt32(row["Id"]);
                        item.Name = Convert.ToString(row["Name"]) ?? "";
                        result.Add(item);
                    }

                    return result;
                }
            }
        }

        public List<InterestQuestion> GetQuestions(bool activeOnly = true)
        {
            using (SqlConnection c = Open())
            {
                using (SqlCommand cmd = Command(c, "SELECT q.Id,q.CategoryId,i.Name,q.Text FROM InterestQuestion q JOIN InterestCategory i ON i.Id=q.CategoryId" + (activeOnly ? " WHERE q.Active=1 AND i.Active=1" : "") + " ORDER BY q.Id"))
                {
                    DataTable tableData = ReadData(cmd);
                    List<InterestQuestion> result = new List<InterestQuestion>();
                    foreach (DataRow row in tableData.Rows)
                    {
                        InterestQuestion question = new InterestQuestion();
                        question.Id = Convert.ToInt32(row["Id"]);
                        question.CategoryId = Convert.ToInt32(row["CategoryId"]);
                        question.Category = Convert.ToString(row["Name"]) ?? "";
                        question.Text = Convert.ToString(row["Text"]) ?? "";
                        result.Add(question);
                    }

                    return result;
                }
            }
        }

        public List<ReferenceRow> GetReference(string key)
        {
            string sql;
            if (key == "UserAccount")
            {
                sql = "SELECT Id,Username,Role,Active,FailedAttempts,LockedUntilUtc FROM UserAccount ORDER BY Username";
            }
            else if (key == "AuditEvent")
            {
                sql = "SELECT TOP (500) a.Id,COALESCE(u.Username,'Learner / setup') AS Actor,a.Action,a.Detail,a.CreatedUtc FROM AuditEvent a LEFT JOIN UserAccount u ON u.Id=a.UserId ORDER BY a.Id DESC";
            }
            else
            {
                List<string> columns = new List<string>();
                foreach (ReferenceField field in ReferenceCatalog.Get(key).Fields)
                {
                    columns.Add("[" + field.Name + "]");
                }

                sql = "SELECT Id," + string.Join(",", columns) + " FROM [" + key + "] ORDER BY Id";
            }

            using (SqlConnection c = Open())
            {
                using (SqlCommand cmd = Command(c, sql))
                {
                    DataTable tableData = ReadData(cmd);
                    List<ReferenceRow> result = new List<ReferenceRow>();
                    foreach (DataRow row in tableData.Rows)
                    {
                        Dictionary<string, object?> values = new Dictionary<string, object?>();
                        for (int i = 1; i < tableData.Columns.Count; i++)
                            values[tableData.Columns[i].ColumnName] = row.IsNull(i) ? null : ReadReferenceValue(row, i);
                        result.Add(new ReferenceRow {
                            Id = Convert.ToInt32(row[0]),
                            Values = values
                        });
                    }

                    return result;
                }
            }
        }

        public int SaveReference(string key, int id, IReadOnlyDictionary<string, object?> values)
        {

            ReferenceDefinition definition = ReferenceCatalog.Get(key);
            List<string> columns = new List<string>();
            List<string> parameters = new List<string>();
            List<string> assignments = new List<string>();
            List<object?> arguments = new List<object?>();
            int index = 0;
            foreach (ReferenceField field in definition.Fields)
            {
                object? value = values[field.Name];
                if (field.Name == "ClosingDate" && value is string text)
                {
                    value = DateOnly.ParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToDateTime(TimeOnly.MinValue);
                }

                columns.Add("[" + field.Name + "]");
                parameters.Add("@p" + index);
                assignments.Add("[" + field.Name + "]=@p" + index);
                arguments.Add(value);
                index++;
            }

            arguments.Add(id);
            string sql;
            if (id == 0)
            {
                sql = "INSERT INTO [" + key + "](" + string.Join(",", columns) + ") OUTPUT INSERTED.Id VALUES(" + string.Join(",", parameters) + ")";
            }
            else
            {
                sql = "UPDATE [" + key + "] SET " + string.Join(",", assignments) + " WHERE Id=@p" + index;
            }

            try
            {
                using (SqlConnection connection = Open())
                {
                    using (SqlCommand command = Command(connection, sql, null, arguments.ToArray()))
                    {
                        if (id == 0)
                        {
                            return Convert.ToInt32(command.ExecuteScalar());
                        }

                        if (command.ExecuteNonQuery() != 1)
                        {
                            throw new ValidationException("Record not found. Refresh and try again.");
                        }

                        return id;
                    }
                }
            }
            catch (SqlException ex)when (ex.Number is 2601 or 2627 or 547 or 515 or 2628)
            {
                throw new ValidationException("Duplicate record, invalid reference or value too long. Check required values and linked records.");
            }
        }

        private static object? ReadReferenceValue(DataRow row, int index)
        {
            if (row.IsNull(index))
            {
                return null;
            }

            object value = row[index];
            if (value is bool flag)
            {
                return flag ? 1 : 0;
            }

            if (value is DateTime date)
            {
                return row.Table.Columns[index].ColumnName == "ClosingDate" ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : Utc(date).ToString("O", CultureInfo.InvariantCulture);
            }

            return value;
        }
    }
}
