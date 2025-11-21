using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase.Enums
{
    /// <summary>
    /// 错误码枚举
    /// </summary>
    public enum ErrorEnum
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = 1000,
        /// <summary>
        /// 未授权访问
        /// </summary>
        [Description("未授权访问")]
        UnauthorizedAccess = 1002,
    }
}
