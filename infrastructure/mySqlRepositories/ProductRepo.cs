namespace infrastructure.mySqlRepositories;

public class ProductRepo
{
                
    private readonly string _connectionString;

    public ProductRepo(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    
}