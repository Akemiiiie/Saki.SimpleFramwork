using Dm;
using Saki.AutoFac.AutofacRegister;
using Saki.Framework.AppBase;
using Saki.IRepositoryTemplate.Base;
using Saki.RepositoryTemplate.DBClients;
using SqlSugar;
using System.Linq.Expressions;

namespace Saki.RepositoryTemplate.Base;

/// <summary>
/// 基类实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class BaseRepository<TEntity> : Repository<TEntity>, IScopeDependency,
    IBaseRepository<TEntity> where TEntity : class, new()
{
    public BaseRepository(ISqlSugarClient db) : base(db)
    {
        base.Context = db;
    }

    /// <summary>
    /// 写入实体数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> Add(TEntity model)
    {
        //这里需要注意的是，如果使用了Task.Run()就会导致 sql语句日志无法记录改成下面的
        var count = await base.Context.Insertable(model).ExecuteCommandAsync();
        return count > 0;
    }

    /// <summary>
    /// 批量写入实体数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddRange(List<TEntity> list)
    {
        //这里需要注意的是，如果使用了Task.Run()就会导致 sql语句日志无法记录改成下面的
        var count = await base.Context.Insertable(list).ExecuteCommandAsync(); 
        return count > 0;
    }

    /// <summary>
    /// 大大大批量写入实体数据-同时写入1000或以上
    /// </summary>
    /// <param name="list">数据集</param>
    /// <param name="batchCount">每次插入的行数</param>
    /// <returns></returns>
    public async Task<bool> BatchInsert(List<TEntity> list,int batchCount)
    {
        //这里需要注意的是，如果使用了Task.Run()就会导致 sql语句日志无法记录改成下面的
        var count = await base.Context.Fastest<TEntity>().PageSize(batchCount).BulkCopyAsync(list);
        return count > 0;
    }

    /// <summary>
    ///根据ID删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<bool> DeleteByIds(object[] ids)
    {
        var i = await base.Context.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
        return i > 0;
    }

    /// <summary>
    /// 根据ID查询一条数据
    /// </summary>
    /// <param name="objId"></param>
    /// <returns></returns>
    public async Task<TEntity> QueryById(object objId)
    {
        return await base.Context.Queryable<TEntity>().InSingleAsync(objId);
    }

    /// <summary>
    /// 更新实体数据
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> Update(TEntity model)
    {
        //这种方式会以主键为条件
        var i = await base.Context.Updateable(model).ExecuteCommandAsync();
        return i > 0;
    }

    /// <summary>
    /// 更新实体数据 - 仅更新非空字段
    /// 数据少可以用, 大量更新性能一般
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateNotNull(TEntity model)
    {
        //这种方式会以主键为条件
        var i = await base.Context.Updateable(model).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        return i > 0;
    }

    /// <summary>
    /// 更新实体数据 - 仅更新非空字段
    /// 数据少可以用, 大量更新性能一般
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateNotNull(List<TEntity> model)
    {
        //这种方式会以主键为条件
        var i = await base.Context.Updateable(model).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        return i > 0;
    }

    /// <summary>
    /// 分页查询基类
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <param name="query"></param>
    /// <param name="conditionBuilder"></param>
    /// <returns></returns>
    public async Task<PageResult<TEntity>> QueryPageAsync<TQuery>(
        TQuery query,
        Action<ISugarQueryable<TEntity>, TQuery> conditionBuilder)
        where TQuery : PageQuery
    {
        RefAsync<int> total = 0;
        var queryable = base.Context.Queryable<TEntity>();
        // 外部通过委托定义查询条件
        conditionBuilder?.Invoke(queryable, query);
        var list = await queryable
            .ToPageListAsync(query.PageIndex, query.PageSize, total);
        return new PageResult<TEntity>(list, total, query.PageIndex, query.PageSize);
    }

    /// <summary>
    /// 分页查询（支持条件 + Lambda 排序）
    /// </summary>
    /// <typeparam name="TQuery">查询入参</typeparam>
    /// <param name="query">分页和条件参数</param>
    /// <param name="conditionBuilder">条件构造委托</param>
    /// <param name="orderByBuilder">排序构造委托</param>
    /// <returns></returns>
    public async Task<PageResult<TEntity>> QueryPageAsync<TQuery>(
        TQuery query,
        Action<ISugarQueryable<TEntity>, TQuery> conditionBuilder,
        Action<ISugarQueryable<TEntity>> orderByBuilder = null)
        where TQuery : PageQuery
    {
        RefAsync<int> total = 0;
        var queryable = base.Context.Queryable<TEntity>();

        // 构造 where 条件
        conditionBuilder?.Invoke(queryable, query);

        // 构造 order by
        if (orderByBuilder != null)
            orderByBuilder.Invoke(queryable);
        else
        {
            // 没写排序就用主键倒序
            var primaryKey = base.Context.EntityMaintenance
                .GetEntityInfo<TEntity>()
                .Columns.FirstOrDefault(c => c.IsPrimarykey)?.DbColumnName ?? "Id";
            queryable = queryable.OrderByPropertyName(primaryKey, OrderByType.Desc);
        }

        var list = await queryable.ToPageListAsync(query.PageIndex, query.PageSize, total);
        return new PageResult<TEntity>(list, total, query.PageIndex, query.PageSize);
    }

}