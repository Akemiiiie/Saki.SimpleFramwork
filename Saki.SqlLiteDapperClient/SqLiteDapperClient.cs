using Saki.Logging;

public class SqliteDapperClient : BaseDapperDbClient
{
    public SqliteDapperClient(IDbContext context, ILoggerService logger)
        : base(context, logger)
    {
    }
}