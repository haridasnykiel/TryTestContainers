using System.Data;

namespace Weather.API.Repository.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}