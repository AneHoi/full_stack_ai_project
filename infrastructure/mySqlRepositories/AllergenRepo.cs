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

    public List<string> recieveIngredients(List<string> ingredientlist, List<int> userIsAllergicTo)
    {
        List<string> categoryId = new List<string>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                // Check if ingredients match allergen names
                foreach (string ingredient in ingredientlist)
                {
                    var query = @"SELECT allergen_name 
                              FROM allergenedb.allergens a
                              INNER JOIN allergenedb.categories c ON a.category_id = c.id
                              WHERE allergen_name = @ingredient AND category_name IN @categories";
                    var matchingAllergens = connection.Query<string>(query, new { ingredient, categories = userIsAllergicTo });

                    if (matchingAllergens.Any())
                    {
                        categoryId.Add(matchingAllergens.ToString());
                        // Handle allergic reaction
                        Console.WriteLine($"Allergic reaction detected for ingredient: {ingredient}");
                    }
                }

                return categoryId;
            }
            catch (Exception e)
            {
                throw new SqlNullValueException("Could not find the allergenes", e);
            }
            
        }
    }
}