using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

public class SqliteDapperContext : IDbContext, IDisposable
{
    private readonly string _connStr;
    public SqliteDapperContext(string connStr) => _connStr = connStr;
    public IDbConnection CreateConnection() => new SqliteConnection(_connStr);
    public void Dispose()
    {
        // 如果未来内部有需要释放的资源，可以在这里释放
        // 目前只是封装字符串，无需实际操作
    }
}
