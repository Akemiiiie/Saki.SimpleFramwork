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
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string TraceId { get; set; } = Activity.Current?.Id ?? Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> Success(T data, string msg = "success") => new()
        {
            Code = 0,
            Message = msg,
            Data = data
        };

        public static ApiResponse<T> Fail(int code, string msg) => new()
        {
            Code = code,
            Message = msg,
            Data = default
        };
    }
}
