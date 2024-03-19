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
                    while (dataReader.Read())
                    {
                        var allergen = new Allergen()
                        {
                            id = dataReader.GetInt32("id"),
                            category_name = dataReader.GetString("category_name")
                        };
                        allergens.Add(allergen);
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
            const string sqlClear = @"DELETE FROM allergenedb.user_allergens WHERE user_id=@user_id;";
            const string sqlInsert = @"INSERT INTO allergenedb.user_allergens (user_id, category_id) VALUES (@user_id, @category_id);";
            
            try
            {
                connection.Open();
                using (var command = new MySqlCommand(sqlClear, connection))
                {
                    command.Parameters.AddWithValue("@user_id", user_id);
                    command.ExecuteNonQuery();
                }
                
                int rowsAffected = 0;
                foreach (var allergen in allergens)
                {
                    using (var command = new MySqlCommand(sqlInsert, connection))
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

    public IEnumerable<int> GetUsersAllergens(int userId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"SELECT * FROM allergenedb.user_allergens WHERE user_id=@userId;";
            
            try
            {
                var allergens = new List<int>();
                
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    var dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var userAllergen = new
                        {
                            category_id = dataReader.GetInt32("category_id"),
                        };
                        
                        allergens.Add(userAllergen.category_id);
                    }
                    dataReader.Close();
                }
                connection.Close();

                return allergens;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when getting your allergens", e);
            }
        }
    }

    public List<AllergenWithCategoryDto> CheckForAllergy(List<string> ingredientlist, List<int> userIsAllergicTo)
    {
        List<AllergenWithCategoryDto> finalResult = new List<AllergenWithCategoryDto>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                // Check if ingredients match allergen names
                foreach (string ingredient in ingredientlist)
                {
                    var query = @"SELECT a.allergen_name, c.id, c.category_name 
                              FROM allergenedb.allergens a
                              INNER JOIN allergenedb.categories c ON a.category_id = c.id
                              WHERE a.allergen_name = @ingredient";
                    var result = connection.Query<AllergenWithCategoryDto>(query, new { ingredient });
                    
                    // Filter allergens based on user's allergies
                    foreach (var allergenDto in result)
                    {
                        if (userIsAllergicTo.Contains(allergenDto.id))
                        {
                            finalResult.Add(allergenDto);
                        }
                    }

                }

        
                return finalResult;

            }
            catch (Exception e)
            {
                throw new SqlNullValueException("Could not find the allergenes", e);
            }
        }
    }
}

public class AllergenWithCategoryDto
{
    public string allergen_name { get; set; }
    public int id { get; set; }

    public string category_name { get; set; }
}