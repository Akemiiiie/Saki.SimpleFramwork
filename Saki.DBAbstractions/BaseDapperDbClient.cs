using Dapper;                     // 必须
using Saki.IDbBase.Entities;      // IEntity / BaseEntity
using Saki.Logging;
using System.Data.Common;

public abstract class BaseDapperDbClient : IDbClient
{
    private readonly IDbContext _context;
    protected readonly ILoggerService _logger;
    protected BaseDapperDbClient(IDbContext context, ILoggerService logger) => _context = context;

    protected DbConnection GetConnection() => (DbConnection)_context.CreateConnection();

    public virtual async Task<int> ExecuteAsync(string sql, object parameters = null)
    {
        using var conn = GetConnection();
        await conn.OpenAsync();
        int result = await conn.ExecuteAsync(sql, parameters);
        _logger.Info($"执行 SQL 成功: {sql}, 受影响行数: {result}");
        return result;
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        where T : class, IEntity
    {
        using var conn = GetConnection();
        await conn.OpenAsync();
        var result = await conn.QueryAsync<T>(sql, parameters);
        _logger.Info($"查询 SQL 成功: {sql}, 返回 {result.AsList().Count} 条记录");
        return result;
    }

    public virtual async Task<IEnumerable<dynamic>> QueryDynamicAsync(string sql, object parameters = null)
    {
        using var conn = GetConnection();
        await conn.OpenAsync();
        var result = await conn.QueryAsync(sql, parameters);
        _logger.Info($"查询动态 SQL 成功: {sql}, 返回 {result.AsList().Count} 条记录");
        return result;
    }

    /// <summary>
    /// 执行单条或多条 SQL（自动拆分分号），可选事务
    /// </summary>
    public virtual async Task<int> ExecuteSqlBatchAsync(string sql, bool useTransaction = true)
    {
        if (string.IsNullOrWhiteSpace(sql)) return 0;

        var statements = sql.Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrEmpty(s));

        int affected = 0;
        using var conn = GetConnection();
        await conn.OpenAsync();

        using var tran = useTransaction ? conn.BeginTransaction() : null;

        foreach (var stmt in statements)
        {
            affected += await conn.ExecuteAsync(stmt, transaction: tran);
        }

        tran?.Commit();
        return affected;
    }

    /// <summary>
    /// 查询动态结果，支持多条 SELECT（返回 List 每条 SQL 的结果）
    /// </summary>
    public virtual async Task<List<IEnumerable<dynamic>>> QueryDynamicBatchAsync(string sql)
    {
        if (string.IsNullOrWhiteSpace(sql)) return new List<IEnumerable<dynamic>>();

        var statements = sql.Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrEmpty(s));

        var results = new List<IEnumerable<dynamic>>();

        using var conn = GetConnection();
        await conn.OpenAsync();

        foreach (var stmt in statements)
        {
            var res = await conn.QueryAsync(stmt);
            results.Add(res);
        }

        return results;
    }

    public virtual Task EnsureCreatedAsync() => Task.CompletedTask;

}
