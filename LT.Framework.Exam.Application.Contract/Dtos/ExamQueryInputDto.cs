using Saki.Framework.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 直接返回填报数据，在看情况添加其他字段
    /// </summary>
    public class ExamQueryInputDto : PageQuery
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 关键字-模糊搜索
        /// </summary>
        public string Keyword { get; set; }

    }
}
