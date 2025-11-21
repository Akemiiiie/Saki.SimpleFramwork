using Saki.Framework.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 分页查询考试相关的问题
    /// </summary>
    public class ExamQuestionPageQueryDto : PageQuery
    {
        public string ExamId { get; set; }
    }
}
