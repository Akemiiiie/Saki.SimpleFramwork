using System.Data;
using System.Data.Common;

public interface IDbContext
{
    IDbConnection CreateConnection();
}