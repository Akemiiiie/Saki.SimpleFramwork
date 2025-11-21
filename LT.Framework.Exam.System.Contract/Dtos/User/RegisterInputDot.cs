using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Application.Contract.Dtos.User
{
    /// <summary>
    /// 用户注册输入参数
    /// </summary>
    public class RegisterInputDot
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
