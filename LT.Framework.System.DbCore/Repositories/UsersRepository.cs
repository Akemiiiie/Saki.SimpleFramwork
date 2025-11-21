using LT.Framework.Application.Contract.Dtos.User;
using LT.Framework.Domain.Entities;
using Saki.AppBase;
using Saki.IRepositoryTemplate.Users;
using Saki.RepositoryTemplate.Base;
using SqlSugar;

namespace LT.Framework.DbCore.Repositories;

public class UsersRepository : BaseRepository<UsersEntity>, IUsersRepository
{
    private ISugarQueryable<UsersEntity> qeryable;

    public UsersRepository(ISqlSugarClient db) : base(db)
    {
        qeryable = base.AsQueryable();
    }

    /// <summary>
    /// 多条件查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<List<UsersEntity>> GetList(UserDtoInput query)
    {
        var res = await qeryable
            .WhereIF(query.Id!=null, t => t.Id.Equals(query.Id))
            .WhereIF(!string.IsNullOrEmpty(query.UserName), t => t.Name.Equals(query.UserName))
            .WhereIF(!string.IsNullOrEmpty(query.Account), t => t.Account.Equals(query.Account))
            .ToListAsync();
        return res;
    }

    /// <summary>
    /// 多条件分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<List<UsersEntity>> GetPageList(UserDtoInput query, PageParam page)
    {
        var res = await qeryable.Where(t => t.Id.Equals(query.Id))
            .ToPageListAsync(page.PageIndex, page.PageSize);
        return res;
    }

    /// <summary>
    /// 根据账号获取用户信息
    /// </summary>
    /// <returns></returns>
    public async Task<UsersEntity> GetUserByAccount(string logstr)
    {
        var user = await qeryable.Where(t => t.PhoneNumber == logstr || t.Account == logstr || t.Name == logstr)
            .FirstAsync();
        return user;
    }
}