using MySql.Data.MySqlClient;

namespace infrastructure.repositories;

public class MySQLRepo
{
    public void Test()
    {
        string connectionString = "server=localhost:3306;user=root;password=example;database=blog";

        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Perform database operations here
                Console.WriteLine("Connected to MySQL database!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}