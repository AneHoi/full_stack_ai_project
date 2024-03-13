using Dapper;
using Newtonsoft.Json;
using Npgsql;

namespace test;

public class Helper
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;

    static Helper()
    {
        string rawConnectionString;
        string envVarKeyName = "pgconn";

        rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName)!;
        if (rawConnectionString == null)
        {
            throw new Exception($@"Unable to reach the database: There is no connection string.");
        }

        try
        {
            Uri = new Uri(rawConnectionString);
            ProperlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
                Uri.Host,
                Uri.AbsolutePath.Trim('/'),
                Uri.UserInfo.Split(':')[0],
                Uri.UserInfo.Split(':')[1],
                Uri.Port > 0 ? Uri.Port : 5432);
            DataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).Build();
            DataSource.OpenConnection().Close();
        }
        catch (Exception e)
        {
            throw new Exception($@"Unable to connect to database: Wrong connection string, or no internet.", e);
        }
    }

    public static void TriggerRebuild()
    {
        using (var conn = DataSource.OpenConnection())
        {
            try
            {
                conn.Execute(RebuildScript);
            }
            catch (Exception e)
            {
                throw new Exception($@"Unable to rebuild database", e);
            }
        }
    }

    public static string Because(object actual, object expected)
    {
        string expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
        string actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);

        return $"because we want these objects to be equivalent:\nExpected:\n{expectedJson}\nActual:\n{actualJson}";
    }

    public static string RebuildScript = $@"
        DROP SCHEMA IF EXISTS allergenedb CASCADE;
CREATE SCHEMA allergenedb;

create table if not exists allergenedb.users
(
    id          SERIAL      PRIMARY KEY,
    username    VARCHAR(50) NOT NULL,
    tlfnumber   INT,
    email       VARCHAR(50) NOT NULL UNIQUE
);

create table allergenedb.password_hash
(
    user_id     integer,
    hash        VARCHAR(350) NOT NULL,
    salt        VARCHAR(180) NOT NULL,
    algorithm   VARCHAR(12)  NOT NULL,
    FOREIGN KEY (user_id) REFERENCES allergenedb.users (id)
);

create table allergenedb.allergies(
    id          SERIAL      PRIMARY KEY,
    allergene   VARCHAR(250) NOT NULL
);

create table allergenedb.allergiesprPerson(
    user_id     integer,
    allergeneId integer,
    FOREIGN KEY (allergeneId) REFERENCES allergenedb.allergies (id)
);";
}