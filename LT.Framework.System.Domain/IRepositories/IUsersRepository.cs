using LT.Framework.Application.Contract.Dtos.User;
using LT.Framework.Domain.Entities;
using Saki.AppBase;
using Saki.AutoFac.AutofacRegister;
using Saki.IRepositoryTemplate.Base;

namespace Saki.IRepositoryTemplate.Users
{
    /// <summary>
    /// 仓储接口
    /// 可以在原有仓储基类上增加自定义扩展接口
    /// </summary>
    public interface IUsersRepository: IBaseRepository<UsersEntity>, ITransitDependency
    {
        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<UsersEntity>> GetList(UserDtoInput query);

        /// <summary>
        /// 多条件分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<UsersEntity>> GetPageList(UserDtoInput query, PageParam page);
            
        /// <summary>
        /// 根据账号获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<UsersEntity> GetUserByAccount(string account);
        
    }
}
