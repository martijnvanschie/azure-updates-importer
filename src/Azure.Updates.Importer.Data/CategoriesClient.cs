using Azure.TrendsAndInsights.Data;
using Azure.Updates.Importer.Data.DataModels;
using Dapper;

namespace Azure.Updates.Importer.Data
{
    public class CategoriesClient
    {
        private static string TABLE_SCHEMA = "dbo";
        private static string TABLE_NAME = "Categories";

        private string TableName
        {
            get => $"[{TABLE_SCHEMA}].[{TABLE_NAME}]";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Returns -1 if the entry already exists, else the IDENTITY created for this category</returns>
        public async Task<int> InsertAzureUpdateSP(CategoryEntity category)
        {
            using (var connection = DatabaseManager.CreateConnection())
            {
                var parameters = new
                {
                    category.Title
                };

                var result = await connection.QuerySingleAsync<int>(
                    "usp_InsertCategory",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
