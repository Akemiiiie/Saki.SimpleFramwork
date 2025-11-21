using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// 自定义响应码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public string TraceId { get; set; } = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 封装的便捷成功响应
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResponse<T> Success(T data, string msg = "success") => new()
        {
            Code = 0,
            Message = msg,
            Data = data
        };

        /// <summary>
        /// 封装的编写失败硬响应
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResponse<T> Fail(int code, string msg) => new()
        {
            Code = code,
            Message = msg,
            Data = default
        };
    }
}
