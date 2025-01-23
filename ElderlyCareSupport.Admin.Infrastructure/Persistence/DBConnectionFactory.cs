using System.Data;
using Microsoft.Data.SqlClient;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}