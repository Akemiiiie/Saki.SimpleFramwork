using Microsoft.AspNetCore.Mvc;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using Saki.AutoFac.AutofacRegister;
using Saki.Framework.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.BaseTemplate.BaseControllers
{
    /// <summary>
    /// 动态api扩展基础控制器
    /// </summary>
    [DynamicWebApi]
    public class BaseController:Controller,IDynamicWebApi, IScopeDependency
    {
        /// <summary>
        /// 返回成功，无数据
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public IActionResult Success<T>(T data)
        {
            return Ok(data);
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult Fail(string message, int code = 1000, object data = null)
        {
            return BadRequest(new ApiResponse<object>() { Message = message, Code = code });
        }
    }
}
