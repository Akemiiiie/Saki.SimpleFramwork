using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Saki.Framework.AppBase;
using Saki.Framework.AppBase.Extensions;

namespace Examination.Filter
{
    /// <summary>
    /// 过滤器：统一API响应格式
    /// </summary>
    public class ApiResponseFilter : IAsyncResultFilter
    {
        /// <summary>
        /// 接口响应过滤器--统一API响应格式
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var path = context.HttpContext.Request.Path.Value;
            // 必须跳过这些 HTML 页，不然你会把 HTML 包成 JSON
            if (path.StartsWith("/swagger") || path.StartsWith("/profiler"))
            {
                await next();
                return;
            }
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is not null &&
                            objectResult.Value.GetType().IsGenericType &&
                            objectResult.Value.GetType().GetGenericTypeDefinition() == typeof(ApiResponse<>))
                {
                    var dataProp = objectResult.Value.GetType().GetProperty("Data");
                    var dataVal = dataProp.GetValue(objectResult.Value);
                    // 返回的是空数据，也就是说仅通用的响应体
                    // 一般来说是封装后的错误信息
                    if (dataVal == null)
                    {
                        // 则直接返回已经封装的内容
                        context.Result = new ObjectResult(objectResult.Value);
                    }
                    else 
                    {
                        var newData = DateTimeExtensions.AddExtensions(dataVal);
                        context.Result = new ObjectResult(ApiResponse<object>.Success(newData))
                        {
                            StatusCode = objectResult.StatusCode
                        };
                    }
                    await next();
                    return;
                }
                // 非 ApiResponse，包装一下
                var wrappedData = DateTimeExtensions.AddExtensions(objectResult.Value);
                context.Result = new ObjectResult(ApiResponse<object>.Success(wrappedData))
                {
                    StatusCode = objectResult.StatusCode
                };
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(ApiResponse<string>.Success(null));
            }
            else if (context.Result is StatusCodeResult statusResult && statusResult.StatusCode >= 400)
            {
                context.Result = new ObjectResult(ApiResponse<string>.Fail(statusResult.StatusCode, "error"));
            }
            await next();
        }
    }
}
