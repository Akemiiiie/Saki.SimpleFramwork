using Microsoft.AspNetCore.Authorization;
using Saki.BaseTemplate.BaseControllers;
using StackExchange.Profiling;
using Microsoft.AspNetCore.Mvc;

namespace Examination.Controllers
{
    [Authorize]
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    public class HealthController : BaseController
    {
        /// <summary>
        /// 健康检查接口
        /// </summary>
        /// <returns></returns>
        public string CheckHealth()
        {
            return "Service is healthy.";
        }

        [HttpGet]
        public IActionResult GetSwaggerJs()
        {
            var html = MiniProfiler.Current.RenderIncludes(this.HttpContext);
            return Ok(html.Value);
        }
    }
}
