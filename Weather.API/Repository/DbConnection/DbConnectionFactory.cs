using System.Data;
using System.Data.SqlClient;
using Weather.API.Repository.DbConnection;

namespace TryTestContiners.Repository.DbConnection;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    
    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IDbConnection Create()
    {
        return new SqlConnection(_connectionString);
    }
}