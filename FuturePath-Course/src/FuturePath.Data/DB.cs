using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class DB
    {
        private readonly DatabaseConnection database;
        public string DatabaseName
        {
            get { return database.DatabaseName; }
        }

        public string ConnectionString
        {
            get { return database.ConnectionString; }
        }

        public DB(string databaseName = DatabaseConnection.DefaultDatabase)
        {
            database = new DatabaseConnection(databaseName);
        }

        protected SqlConnection Open()
        {
            SqlConnection connection = database.CreateConnection();
            connection.Open();
            return connection;
        }

        protected static SqlCommand Command(SqlConnection connection, string sql, SqlTransaction? transaction = null, params object? [] values)
        {
            SqlCommand command = new SqlCommand(sql, connection, transaction);
            for (int index = 0; index < values.Length; index++)
            {
                object value = values[index] ?? DBNull.Value;
                SqlParameter parameter = command.Parameters.AddWithValue("@p" + index, value);
                if (value is DateTime)
                {
                    parameter.SqlDbType = SqlDbType.DateTime2;
                }

                if (value is decimal)
                {
                    parameter.SqlDbType = SqlDbType.Decimal;
                    parameter.Precision = 7;
                    parameter.Scale = 2;
                }
            }

            return command;
        }

        protected static int Execute(SqlConnection connection, string sql, SqlTransaction? transaction = null, params object? [] values)
        {
            using (SqlCommand command = Command(connection, sql, transaction, values))
            {
                return command.ExecuteNonQuery();
            }
        }

        protected static DateTime Utc(DateTime value)
        {
            return DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        protected static string Lines(string value)
        {
            return value.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }

        public void Initialize()
        {

            try
            {
                int schemaVersion;
                using (SqlConnection connection = Open())
                {
                    using (SqlCommand command = Command(connection, "SELECT MAX(Version) FROM dbo.SchemaVersion"))
                    {
                        object version = command.ExecuteScalar();
                        if (version == null || version == DBNull.Value || (Convert.ToInt32(version) < 1 || Convert.ToInt32(version) > 3))
                        {
                            throw new ValidationException("The database schema version is not supported. Use Prepare demonstration database in the application.");
                        }
                        schemaVersion = Convert.ToInt32(version);
                    }
                }
                if (schemaVersion < 3)
                {
                    DatabaseSetup.Upgrade(DatabaseName);
                }
            }
            catch (SqlException ex)when (ex.Number is 4060 or 208 or -2 or 2 or 26 or 50)
            {
                throw new ValidationException("Cannot open the Future Path LocalDB database. Install SQL Server Express LocalDB through Visual Studio Installer, then use Prepare demonstration database in the app. " + "Expected database: " + DatabaseName + ". Existing data is not reset.");
            }
        }

        protected static DataTable ReadData(SqlCommand command)
        {
            DataSet dsMain = new DataSet();
            using (SqlDataAdapter daMain = new SqlDataAdapter(command))
            {
                daMain.Fill(dsMain, "Records");
            }

            return dsMain.Tables["Records"]!;
        }
    }
}
