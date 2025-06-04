using Dapper;
using Microsoft.Data.SqlClient;
//using Azure.TrendsAndInsights.Configuration;

namespace Azure.TrendsAndInsights.Data
{
    //https://www.learndapper.com/database-providers
    public class DatabaseManager
    {
        private static string ENV_KEY_SQL_CONNECTION = "AZUREUPDATESIMPORTER_SQL_CONNECTION_STRING";
        internal static string _connectionString;

        //internal static DatabaseSettings CONFIG = ConfigurationManager.GetConfiguration().Database;

        static DatabaseManager()
        {
            //ENV_KEY_SQL_CONNECTION = CONFIG.EnvironmentVariableName;
            _connectionString = GetConnectionString();
        }

        public static string GetConnectionString()
        {
            var con = Environment.GetEnvironmentVariable(ENV_KEY_SQL_CONNECTION);

            if (string.IsNullOrEmpty(con))
            {
                throw new ApplicationException($"Environment variable [{ENV_KEY_SQL_CONNECTION}] was nof found.");
            }

            return con;
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static async Task<string> GetSqlVersionAsync()
        {
            using (var connection = CreateConnection())
            {
                var query = @$"
                    SELECT @@VERSION
                    ";

                var entities = await connection.QuerySingleAsync<string>(query);
                return entities;
            }
        }
    }
}
