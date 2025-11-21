using Microsoft.EntityFrameworkCore;
using Saki.Logging;

public class SqlServerEfClient : BaseEfCoreDbClient<SqlServerEFDbContext>
{
    public SqlServerEfClient(SqlServerEFDbContext db, ILoggerService logger)
        : base(db, logger)
    { 
    }
}