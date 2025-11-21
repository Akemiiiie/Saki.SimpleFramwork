using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

public class SqliteEFDbContext : DbContext, IDbContext
{
    public SqliteEFDbContext(DbContextOptions<SqliteEFDbContext> options) : base(options) { }

    public IDbConnection CreateConnection()
    {
        throw new NotImplementedException();
    }
}
