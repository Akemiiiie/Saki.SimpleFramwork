using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase
{
    /// <summary>
    /// 分页请求基类
    /// </summary>
    public class PageQuery
    {
        /// <summary>
        /// 页码，从1开始
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 10;
    }

}
