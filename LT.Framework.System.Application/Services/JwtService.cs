using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Saki.AutoFac.AutofacRegister;
using Saki.Framework.AppBase.ConfigerOptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LT.Framework.Application.Services
{
    /// <summary>
    /// JWT服务，不对外暴露
    /// </summary>
    public class JwtService: ITransitDependency
    {
        /// <summary>
        /// 为用户生成Token，可指定有效期
        /// </summary>
        /// <param name="user"></param>
        /// <param name="expireHours">默认24小时</param>
        /// <returns></returns>
        public string GenerateToken(Contract.Dtos.User.UserDtoOutput user, int expireHours = 24)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Sub, user.Account),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
