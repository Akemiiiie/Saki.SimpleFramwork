using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase
{
    /// <summary>
    /// 分页响应基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T>
    {
        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> PageList { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        public PageResult() { }

        public PageResult(List<T> data, int total, int pageIndex, int pageSize)
        {
            PageList = data;
            TotalCount = total;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }

}
