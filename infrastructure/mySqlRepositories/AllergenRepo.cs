using System.Data.SqlTypes;
using ConsoleApp1.JsonFileExtractor;
using Dapper;
using infrastructure.datamodels;
using MySql.Data.MySqlClient;

namespace infrastructure.mySqlRepositories;

public class AllergenRepo
{
    private readonly string _connectionString;

    public AllergenRepo(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void CreateCategoriesFromJsonList(List<string> categoryNames)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
INSERT INTO allergenedb.categories (category_name)
VALUES (@category);
SELECT LAST_INSERT_ID() as id;";

            try
            {
                connection.Open();
                foreach (string categoryName in categoryNames)
                {
                    connection.Execute(sql, new { category = categoryName });
                }
            }
            catch (Exception ex)
            {
                throw new SqlTypeException("Failed to create categories", ex);
            }
        }
    }

    public void CreateAllergens(AllergenData allergenData)
    {
        int categoryId = GetCategoryId(allergenData.Category);

        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
INSERT INTO allergenedb.allergens (allergen_name, category_id)
VALUES (@allergenName, @categoryId);";

            try
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@allergenName", allergenData.Allergen);
                    command.Parameters.AddWithValue("@categoryId", categoryId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Failed to create allergen: " + ex.Message);
            }
        }
    }
    
    private int GetCategoryId(string categoryName)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
SELECT id FROM allergenedb.categories WHERE category_name = @categoryName;";

            try
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@categoryName", categoryName);
                    var categoryId = command.ExecuteScalar();
                    if (categoryId != null)
                    {
                        return Convert.ToInt32(categoryId);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Failed to retrieve category ID: " + ex.Message);
            }
        }

        // If category not found, return -1 or throw an exception based on your requirement
        return -1;
    }

    public IEnumerable<Allergen> GetAllergenCategories()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"SELECT * FROM allergenedb.categories;";
            
            try
            {
                var allergens = new List<Allergen>();
                
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    var dataReader = command.ExecuteReader();
                    int i = 0;
                    while (dataReader.Read())
                    {
                        var allergen = new Allergen()
                        {
                            id = dataReader.GetInt32("id"),
                            category_name = dataReader.GetString("category_name")
                        };
                        allergens.Add(allergen);
                        i++;
                    }
                    dataReader.Close();
                }
                connection.Close();

                return allergens;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when getting allergen categories", e);
            }
        }
    }

    public bool SaveAllergens(IEnumerable<int> allergens, int user_id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"INSERT INTO allergenedb.user_allergens (user_id, category_id) VALUES (@user_id, @category_id);";
            
            try
            {
                int rowsAffected = 0;
                
                connection.Open();
                foreach (var allergen in allergens)
                {
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@category_id", allergen);
                        var row = command.ExecuteNonQuery();
                        rowsAffected += row;
                    }
                }
                
                connection.Close();
                
                return allergens.Count() == rowsAffected;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when saving your profile's allergens", e);
            }
        }
    }
}