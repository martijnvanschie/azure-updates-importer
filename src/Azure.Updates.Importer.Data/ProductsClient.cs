using Azure.TrendsAndInsights.Data;
using Azure.Updates.Importer.Data.DataModels;
using Dapper;

namespace Azure.Updates.Importer.Data
{
    public class ProductsClient
    {
        private static string TABLE_SCHEMA = "dbo";
        private static string TABLE_NAME = "Products";

        private string TableName
        {
            get => $"[{TABLE_SCHEMA}].[{TABLE_NAME}]";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns the ID of the product if it already exists, else the IDENTITY created for this product</returns>
        public async Task<int> InsertProductSP(ProductEntity product)
        {
            using (var connection = DatabaseManager.CreateConnection())
            {
                var parameters = new
                {
                    product.Title
                };

                var result = await connection.QuerySingleAsync<int>(
                    "usp_InsertProduct",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                return result;
            }
        }
    }
}