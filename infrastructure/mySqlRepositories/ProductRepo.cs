using System.Data.SqlTypes;
using ConsoleApp1.JsonFileExtractor;
using MySql.Data.MySqlClient;

namespace infrastructure.mySqlRepositories;

public class ProductRepo
{
                
    private readonly string _connectionString;

    public ProductRepo(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void SaveProductInfo(List<ProductInfo> productInfos)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
INSERT INTO allergenedb.products (barcode, language, name, productName, declaration)
VALUES (@barcode, @language, @name, @productName, @declaration);";

            try
            {
                connection.Open();
                foreach (var productInfo in productInfos)
                {
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@barcode", productInfo.barCode);
                        command.Parameters.AddWithValue("@language", productInfo.language);
                        command.Parameters.AddWithValue("@name", productInfo.name);
                        command.Parameters.AddWithValue("@productName", productInfo.productName);
                        command.Parameters.AddWithValue("@declaration", productInfo.declaration);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                throw new SqlTypeException(" read balances from group", ex);
            }
        }
    }
    
}