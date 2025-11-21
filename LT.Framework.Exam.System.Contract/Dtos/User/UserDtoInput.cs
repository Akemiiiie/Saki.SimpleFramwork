using Saki.Framework.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Application.Contract.Dtos.User
{
    /// <summary>
    /// 模拟UserDto
    /// </summary>
    public class UserDtoInput:EntityDto<Guid>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// hash后的密码
        /// </summary>
        public string PasswordHash { get; set; } // 存储哈希后的密码

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
    }
}
