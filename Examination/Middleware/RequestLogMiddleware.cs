using Microsoft.AspNetCore.Mvc.Controllers;
using Saki.Framework.BaseRepository.BaseEntitys;
using Saki.Framework.IBaseRepository.System;
using Saki.Logging;
using SqlSugar;
using System.Diagnostics;
using System.Text;

namespace Examination.Middleware
{
    /// <summary>
    /// 全局请求日志中间件，记录所有 HTTP 请求及响应。
    /// </summary>
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;
        private readonly IRequestLogRepository _repository;

        public RequestLogMiddleware(RequestDelegate next, ILoggerService logger, IRequestLogRepository repository)
        {
            _next = next;
            _logger = logger;
            _repository = repository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var log = new RequestLogEntity
            {
                TraceId = context.TraceIdentifier,
                RequestTime = DateTime.Now,
                RequestPath = context.Request.Path,
                Method = context.Request.Method,
                IPAddress = context.Connection.RemoteIpAddress?.ToString(),
                Headers = SerializeHeaders(context.Request.Headers),
                ControllerName = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName,
                ActionName = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ActionName
            };

            // 读取请求体
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                log.RequestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            // 替换响应流，防止直接写入
            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
                stopwatch.Stop();
                log.IsSuccess = context.Response.StatusCode < 400;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                log.IsSuccess = false;
                log.ErrorMessage = ex.Message;
                _logger.Error($"请求异常: {log.RequestPath}", ex);

                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { message = "系统异常", Code=500 });
                }
            }
            finally
            {
                log.ResponseTime = DateTime.Now;
                log.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                // 读取响应内容
                responseBody.Position = 0;
                using (var reader = new StreamReader(responseBody, Encoding.UTF8, leaveOpen: true))
                {
                    var respText = await reader.ReadToEndAsync();
                    log.ResponseBody = respText.Length > 2000 ? respText[..2000] + "..." : respText;
                }
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);
                // 异步写入日志，异常不影响主流程
                try
                {
                    await _repository.Add(log);
                }
                catch (Exception dbEx)
                {
                    _logger.Error($"插入请求日志失败: {log.RequestPath}", dbEx);
                }
                context.Response.Body = originalBodyStream;
                responseBody.Dispose();
            }
        }

        private static string SerializeHeaders(IHeaderDictionary headers)
        {
            return string.Join("; ", headers.Select(h => $"{h.Key}:{h.Value}"));
        }
    }
}
