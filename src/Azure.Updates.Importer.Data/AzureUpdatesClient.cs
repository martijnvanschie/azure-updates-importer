using Azure.TrendsAndInsights.Data;
using Azure.Updates.Importer.Data.DataModels;
using Dapper;

namespace Azure.Updates.Importer.Data
{

    public class AzureUpdatesClient
    {
        private static string TABLE_SCHEMA = "dbo";
        private static string TABLE_NAME = "AzureUpdates";

        private string TableName
        {
            get => $"[{TABLE_SCHEMA}].[{TABLE_NAME}]";
        }

        public async Task<IEnumerable<AzureUpdateEntity>> SelectAllAsync()
        {
            using (var connection = DatabaseManager.CreateConnection())
            {
                var query = @$"
                    SELECT * FROM [{TABLE_SCHEMA}].[{TABLE_NAME}]
                    ";

                var entities = await connection.QueryAsync<AzureUpdateEntity>(query);
                return entities;
            }
        }

        public async Task<int> SelectCountAsync()
        {
            using (var connection = DatabaseManager.CreateConnection())
            {
                var query = @$"
                    SELECT COUNT(*) FROM [{TABLE_SCHEMA}].[{TABLE_NAME}]
                    ";

                var count = await connection.ExecuteScalarAsync<int>(query);
                return count;
            }
        }

        public async Task<int> InsertAzureUpdateSP(AzureUpdateEntity update)
        {
            using (var connection = DatabaseManager.CreateConnection())
            {
                var parameters = new
                {
                    update.Id,
                    update.Title,
                    update.Description,
                    update.Url,
                    update.DatePublished
                };

                var result = await connection.ExecuteAsync(
                    "usp_InsertAzureUpdate",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<int> InsertAzureUpdateCategory(AzureUpdateEntity update, CategoryEntity category)
        {
            using (var connection = DatabaseManager.CreateConnection())
            {
                var parameters = new
                {
                    AzureUpdateId = update.Id,
                    CategoryId = category.Id
                };

                var result = await connection.ExecuteAsync(
                    "usp_InsertAzureUpdateCategory",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
