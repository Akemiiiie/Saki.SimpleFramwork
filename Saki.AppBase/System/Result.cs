using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase
{
    /// <summary>
    /// api通用返回结果
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static object Success(object data = null, string message = "操作成功") =>
            new { success = true, code = 0, message, data };

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Failure(string message, int code = 1000, object data = null) =>
            new { success = false, code, message, data };
    }
}
