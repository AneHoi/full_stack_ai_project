using System.Data.SqlTypes;
using Dapper;
using infrastructure.datamodels;
using MySql.Data.MySqlClient;

namespace infrastructure.mySqlRepositories;

public class UserRepo
{
            
    private readonly string _connectionString;

    public UserRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public User Create(string username, int tlfnumber, string email)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
INSERT INTO allergenedb.users (username, tlfnumber, email)
VALUES (@username, @tlfnumber, @email);
SELECT LAST_INSERT_ID() as id, @username as username, @tlfnumber as tlfnumber, @email as email;";

            try
            {
                connection.Open();
                return connection.QueryFirst<User>(sql, new { username, tlfnumber, email });
            }
            catch (Exception ex)
            {
                throw new SqlTypeException(" could not create user", ex);
            }
        }
    }

    public User? GetById(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
            SELECT
                id,
                username,
                tlfnumber,
                email
            FROM allergenedb.users
            WHERE id = @id;
        ";

            try
            {
                connection.Open();
                return connection.QueryFirstOrDefault<User>(sql, new { id });
            }   
            catch (Exception ex)
            {
                throw new SqlNullValueException(" read balances from group", ex);
            }
        }
    }
}