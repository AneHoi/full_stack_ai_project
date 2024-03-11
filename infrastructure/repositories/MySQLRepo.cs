using MySql.Data.MySqlClient;

namespace infrastructure.repositories;

public class MySQLRepo
{
    public void Test()
    {
        string connectionString = "Server=127.0.0.1;Port=63306;Database=Blog;Uid=root;Pwd=example;";

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
                Console.WriteLine("Stacktrace: " + ex.StackTrace);
            }
        }
    }
}