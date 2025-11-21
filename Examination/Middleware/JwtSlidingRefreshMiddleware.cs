using LT.Framework.Application.Contract.Dtos.User;
using LT.Framework.Application.Services;
using Saki.Framework.AppBase.ConfigerOptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Examination.Middleware
{
    /// <summary>
    /// 滑动刷新 JWT Token 中间件
    /// </summary>
    public class JwtSlidingRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtService _jwtService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="next"></param>
        /// <param name="jwtService"></param>
        public JwtSlidingRefreshMiddleware(RequestDelegate next, JwtService jwtService)
        {
            _next = next;
            _jwtService = jwtService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies[JwtSettings.CookieName];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                try
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var expireTime = jwtToken.ValidTo;
                    var timeLeft = expireTime - DateTime.UtcNow;
                    if (timeLeft < TimeSpan.FromHours(2)) // 剩余时间小于 2 小时则刷新
                    {
                        // 提取用户信息
                        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                        var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                        var account = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                        if (userId != null)
                        {
                            var userDto = new UserDtoOutput
                            {
                                Id = Guid.Parse(userId),
                                Name = userName,
                                Account = account
                            };
                            // 生成新的 Token
                            var newToken = _jwtService.GenerateToken(userDto, expireHours: 24);
                            // 写入 Cookie
                            context.Response.Cookies.Append(JwtSettings.CookieName, newToken, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.None,
                                Expires = DateTimeOffset.UtcNow.AddHours(24)
                            });
                        }
                    }
                }
                catch
                {
                    // 无效 Token，直接忽略
                }
            }
            await _next(context);
        }
    }
}
