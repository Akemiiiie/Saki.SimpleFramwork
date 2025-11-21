using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

public class SqlServerEFDbContext : DbContext, IDbContext
{
    public SqlServerEFDbContext(DbContextOptions<SqlServerEFDbContext> options) : base(options) { }

    public IDbConnection CreateConnection()
    {
        throw new NotImplementedException();
    }
}
