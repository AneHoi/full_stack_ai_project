using System.Data.SqlTypes;
using Dapper;
using infrastructure.datamodels;
using MySql.Data.MySqlClient;

namespace infrastructure.mySqlRepositories;

public class PasswordHashRepo
{
             
    private readonly string _connectionString;

    public PasswordHashRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    
    public void Create(int userId, string hash, string salt, string algorithm)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
INSERT INTO allergenedb.password_hash (user_id, hash, salt, algorithm)
VALUES (@userId, @hash, @salt, @algorithm)";

            try
            {
                connection.Open();
                connection.Execute(sql, new { userId, hash, salt, algorithm });
            }
            catch (Exception ex)
            {
                throw new SqlTypeException("Could not create user", ex);
            }
        }
    }

    public PasswordHash GetByEmail(string email)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            const string sql = @"
            SELECT 
                ph.user_id as UserId,
                ph.hash as Hash,
                ph.salt as Salt,
                ph.algorithm as Algorithm
            FROM allergenedb.password_hash ph
            JOIN allergenedb.users u ON ph.user_id = u.id
            WHERE u.email = @email;
        ";

            try
            {
                connection.Open();
                return connection.QuerySingle<PasswordHash>(sql, new { email });
            }
            catch (Exception ex)
            {
                throw new SqlNullValueException("Could not find hash, from userEmail", ex);
            }
        }
    }
}