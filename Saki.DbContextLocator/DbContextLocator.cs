using Saki.Framework.AppBase.ConfigerOptions;

public static class DbContextLocator
{
    private static Dictionary<(DbType, OrmType), Func<string, IDbClient>> _map = new();

    public static void Configure(DbType type, OrmType orm, Func<string, IDbClient> factory)
        => _map[(type, orm)] = factory;

    public static IDbClient GetClient(DbType type, OrmType orm, string connStr)
        => _map.TryGetValue((type, orm), out var factory) ? factory(connStr) : throw new NotImplementedException();
}
