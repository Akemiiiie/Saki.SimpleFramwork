using Saki.IDbBase.Entities;

public interface IDbClient
{
    Task<IEnumerable<dynamic>> QueryDynamicAsync(string sql, object parameters = null);

    Task<int> ExecuteAsync(string sql, object parameters = null);

    Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null) where T : class, IEntity;

    Task EnsureCreatedAsync();

    /// <summary>
    /// 执行单条或多条 SQL（自动拆分分号），可选事务
    /// </summary>
    Task<int> ExecuteSqlBatchAsync(string sql, bool useTransaction = true);

    /// <summary>
    /// 查询动态结果，支持多条 SELECT（返回 List 每条 SQL 的结果）
    /// </summary>
    Task<List<IEnumerable<dynamic>>> QueryDynamicBatchAsync(string sql);
}
