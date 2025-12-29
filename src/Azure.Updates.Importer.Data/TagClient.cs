using Azure.TrendsAndInsights.Data;
using Azure.Updates.Importer.Data.DataModels;
using Dapper;

namespace Azure.Updates.Importer.Data
{
    public class TagsClient
    {
        private static string TABLE_SCHEMA = "dbo";
        private static string TABLE_NAME = "Tags";

        private string TableName
        {
            get => $"[{TABLE_SCHEMA}].[{TABLE_NAME}]";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>Returns the ID of the tag if it already exists, else the IDENTITY created for this tag</returns>
        public async Task<int> InsertTagSP(TagEntity tag)
        {
            using (var connection = DatabaseManager.CreateConnection())
            {
                var parameters = new
                {
                    tag.Title
                };

                var result = await connection.QuerySingleAsync<int>(
                    "usp_InsertTag",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }
    }
}