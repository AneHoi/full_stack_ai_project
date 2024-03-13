using MySql.Data.MySqlClient;

namespace infrastructure.repositories;

public class MySQLRepo
{
        
    private readonly string _connectionString;

    public MySQLRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    
    public void Test()
    {
        using (var connection = new MySqlConnection(_connectionString))
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