using Dapper;
using infrastructure.datamodels;
using Npgsql;

namespace infrastructure.repositories;

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public User Create(string username, int tlfnumber, string email)
    {
        const string sql = $@"
INSERT INTO allergenedb.users (username, tlfnumber, email)
VALUES (@username, @tlfnumber, @email)
RETURNING
    id as {nameof(User.id)},
    username as {nameof(User.username)},
    tlfnumber as {nameof(User.tlfnumber)},
    email as {nameof(User.email)};
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new { username, tlfnumber, email });
    }

    /**
     * Returns the user, with the given ID.
     */
    public User? GetById(int id)
    {
        const string sql = $@"
SELECT
    id as {nameof(User.id)},
    username as {nameof(User.username)},
    tlfnumber as {nameof(User.tlfnumber)},
    email as {nameof(User.email)}
FROM allergenedb.users
WHERE id = @id;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<User>(sql, new { id });
    }

    public IEnumerable<User> GetAll()
    {
        const string sql = $@"
SELECT
    id as {nameof(User.id)},
    username as {nameof(User.username)},
    tlfnumber as {nameof(User.tlfnumber)}
    email as {nameof(User.email)}
FROM taxapp.users
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<User>(sql);
        }
    }
}