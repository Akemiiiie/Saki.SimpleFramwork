using Dm.filter;
using LT.Framework.Application.Contract.Dtos.User;
using LT.Framework.DbCore.Repositories;
using LT.Framework.Domain.Entities;
using LT.Framework.Domain.Shared.Ibll;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTaste;
using Panda.DynamicWebApi.Attributes;
using Saki.AutoFac.AutofacRegister;
using Saki.BaseTemplate.BaseControllers;
using Saki.Framework.AppBase.ConfigerOptions;
using Saki.Framework.AppBase.Utils;
using Saki.IRepositoryTemplate.Users;

namespace LT.Framework.Application.Services
{
    /// <summary>
    /// User相关服务
    /// </summary>
    [Authorize]
    [DynamicWebApi]
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    public class UserService: BaseController
    {
        private IUsersRepository _usersRepository;
        private readonly ICurrentUser _currentUser;
        private JwtService _jwtService;

        public UserService(IUsersRepository usersRepository, ICurrentUser currentUser, JwtService jwtService) 
        {
            _usersRepository = usersRepository;
            _currentUser = currentUser;
            _jwtService = jwtService;
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin(LoginDto loginDto)
        {
            var user = await _usersRepository.GetUserByAccount(loginDto.Name);
            if (user == null) 
                return Fail("用户不存在");
            string password = DESEncrypt.Encrypt(loginDto.Password, user.Salt);
            if (!string.Equals(user.Password, password)) 
                return Fail("密码错误");
            var userDto = user.Adapt<UserDtoOutput>();
            // 身份信息有效时长
            int expireHours = 24;
            var token = _jwtService.GenerateToken(userDto, expireHours: expireHours); // 默认 24 小时
            // 创建 Cookie 选项
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // 防止 JavaScript 访问 Cookie
                Secure = true, // 仅通过 HTTPS 发送 Cookie
                SameSite = SameSiteMode.None, // 设置 SameSite 属性
                Expires = DateTimeOffset.UtcNow.AddHours(expireHours) // 设置过期时间
            };
            Response.Cookies.Append(JwtSettings.CookieName, token, cookieOptions);
            return Success(userDto);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout() 
        {
            // 创建 Cookie 选项
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,  // 防止 JavaScript 访问 Cookie
                Expires = DateTime.Now,  // 设置过期时间
                Secure = true,  // 仅通过 HTTPS 发送 Cookie
                SameSite = SameSiteMode.None  // 设置 SameSite 属性
            };
            // 将 JWT 令牌写入 Cookie
            Response.Cookies.Append(JwtSettings.CookieName, "null", cookieOptions);
            return Success("Ok");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetUserInfo() 
        {
            var user = await _usersRepository.GetUserByAccount(_currentUser.UserName);
            if (user == null)
            {
                return Fail("用户不存在");
            }
            var userDto = user.Adapt<UserDtoOutput>();
            return Success(userDto);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterInputDot input)
        {
            try
            {
                var datas = await _usersRepository.GetList(new UserDtoInput() { UserName = input.Name });
                if (datas != null && datas.Count > 0)
                {
                    return Fail("用户已存在");
                }
                // 未登录时注册用户默认为系统创建
                UsersEntity register = new UsersEntity(input.Name, _currentUser.UserId ?? "System");
                string password = input.Password;
                password = DESEncrypt.Encrypt(password, register.Salt);
                register.setPassword(password);
                await _usersRepository.Add(register);
                return Success(register.Adapt<UserDtoOutput>());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
