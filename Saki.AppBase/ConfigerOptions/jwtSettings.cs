using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase.ConfigerOptions
{
    /// <summary>
    /// 简单Jwt授权配置
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// 自定义秘钥
        /// </summary>
        public static string SecretKey { get; set; }

        /// <summary>
        /// 发起者
        /// </summary>
        public static string Issuer { get; set; }

        /// <summary>
        /// 令牌接收者
        /// </summary>
        public static string Audience { get; set; }

        /// <summary>
        /// cookie名称
        /// </summary>
        public static string CookieName { get; set; }
    }
}
