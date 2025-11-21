using LT.Framework.Domain.Shared.Ibll;
using Microsoft.AspNetCore.Http;
using Saki.AutoFac.AutofacRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Application
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string UserId => _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)??"";
        public string UserName => _accessor.HttpContext?.User?.Identity?.Name??"";
        public string Role => _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)??"";
    }
}
