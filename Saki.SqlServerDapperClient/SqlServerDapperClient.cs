using Saki.Logging;

public class SqlServerDapperClient : BaseDapperDbClient
{
    public SqlServerDapperClient(IDbContext context, ILoggerService logger)
        : base(context, logger)
    {
    }
}