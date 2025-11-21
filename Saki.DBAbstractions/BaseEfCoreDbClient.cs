using Microsoft.EntityFrameworkCore;
using Saki.IDbBase.Entities;
using Saki.Logging;

public abstract class BaseEfCoreDbClient<TContext> : IDbClient
    where TContext : DbContext
{
    protected readonly TContext _db;
    protected readonly ILoggerService _logger;

    protected BaseEfCoreDbClient(TContext db, ILoggerService logger)
    {
        _db = db;
        _logger = logger;
    }

    public virtual async Task<int> ExecuteAsync(string sql, object parameters = null)
    {
        var efParams = parameters is null ? Array.Empty<object>() : new[] { parameters };
        int result = await _db.Database.ExecuteSqlRawAsync(sql, efParams);
        _logger.Info($"执行 SQL 成功: {sql}, 受影响行数: {result}");
        return result;
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        where T : class, IEntity
    {
        var efParams = parameters is null ? Array.Empty<object>() : new[] { parameters };
        var result = await _db.Set<T>().FromSqlRaw(sql, efParams).ToListAsync();
        _logger.Info($"查询 SQL 成功: {sql}, 返回 {result.Count} 条记录");
        return result;
    }

    public virtual async Task<IEnumerable<dynamic>> QueryDynamicAsync(string sql, object parameters = null)
    {
        var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        var reader = await cmd.ExecuteReaderAsync();
        var list = new List<dynamic>();
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
                row[reader.GetName(i)] = reader.GetValue(i);
            list.Add(row);
        }

        _logger.Info($"查询动态 SQL 成功: {sql}, 返回 {list.Count} 条记录");
        return list;
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
        var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();

        using var tran = useTransaction ? conn.BeginTransaction() : null;

        foreach (var stmt in statements)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = stmt;
            if (useTransaction) cmd.Transaction = tran;
            affected += await cmd.ExecuteNonQueryAsync();
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
        var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();

        foreach (var stmt in statements)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = stmt;

            var reader = await cmd.ExecuteReaderAsync();
            var list = new List<dynamic>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);
                list.Add(row);
            }

            results.Add(list);
            await reader.CloseAsync();
        }

        return results;
    }

    public virtual Task EnsureCreatedAsync()
    {
        _db.Database.EnsureCreated();
        return Task.CompletedTask;
    }
}
