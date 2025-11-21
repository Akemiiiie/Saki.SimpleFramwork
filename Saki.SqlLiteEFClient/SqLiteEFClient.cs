using Microsoft.EntityFrameworkCore;
using Saki.Logging;
using SQLitePCL;

public class SqliteEfClient : BaseEfCoreDbClient<SqliteEFDbContext>
{
    public SqliteEfClient(SqliteEFDbContext db, ILoggerService logger)
        : base(db, logger)
    {
    }
}