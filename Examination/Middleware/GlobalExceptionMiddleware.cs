using Saki.Framework.AppBase;
using Saki.Logging;

namespace Examination.Middleware
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public GlobalExceptionMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 实际处理方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.Error("Global exception captured.",ex);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = ApiResponse<string>.Fail(500, ex.Message);
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
