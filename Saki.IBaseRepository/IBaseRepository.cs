using Saki.AutoFac.AutofacRegister;
using Saki.Framework.AppBase;
using SqlSugar;

namespace Saki.IRepositoryTemplate.Base
{
    /// <summary>
    /// 仓储基类接口,其他接口继承该接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> : IScopeDependency, ISimpleClient<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Add(TEntity model);

        /// <summary>
        /// 批量写入实体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> AddRange(List<TEntity> list);

        /// <summary>
        /// 大大大批量写入实体数据-同时写入1000或以上
        /// </summary>
        /// <param name="list">数据集</param>
        /// <param name="batchCount">每次插入的行数</param>
        /// <returns></returns>
        Task<bool> BatchInsert(List<TEntity> list, int batchCount);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> Update(TEntity model);

        /// <summary>
        /// 更新实体数据 - 仅更新非空字段
        /// 数据少可以用, 大量更新性能一般
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> UpdateNotNull(TEntity model);


        /// <summary>
        /// 更新实体数据 - 仅更新非空字段
        /// 数据少可以用, 大量更新性能一般
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> UpdateNotNull(List<TEntity> model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIds(object[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        Task<TEntity> QueryById(object objId);

        /// <summary>
        /// 分页查询基类
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <param name="query"></param>
        /// <param name="conditionBuilder"></param>
        /// <returns></returns>
        Task<PageResult<TEntity>> QueryPageAsync<TQuery>(
            TQuery query,
            Action<ISugarQueryable<TEntity>, TQuery> conditionBuilder)
            where TQuery : PageQuery;

        /// <summary>
        /// 分页查询（支持条件 + Lambda 排序）
        /// </summary>
        /// <typeparam name="TQuery">查询入参</typeparam>
        /// <param name="query">分页和条件参数</param>
        /// <param name="conditionBuilder">条件构造委托</param>
        /// <param name="orderByBuilder">排序构造委托</param>
        /// <returns></returns>
        Task<PageResult<TEntity>> QueryPageAsync<TQuery>(
            TQuery query,
            Action<ISugarQueryable<TEntity>, TQuery> conditionBuilder,
            Action<ISugarQueryable<TEntity>> orderByBuilder = null)
            where TQuery : PageQuery;

    }
}
