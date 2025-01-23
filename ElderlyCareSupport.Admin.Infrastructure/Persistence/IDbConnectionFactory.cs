using System.Data;

namespace ElderlyCareSupport.Admin.Infrastructure.Persistence;

public interface IDbConnectionFactory 
{
    IDbConnection GetConnection();
}