using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 考试答卷提交
    /// </summary>
    public class RecordItemAddInputDto
    {
        /// <summary>
        /// 答题记录主键
        /// </summary>
        public required string RecordId { get; set; }

        /// <summary>
        /// 答题记录列表
        /// </summary>
        public required List<ExamAnswerAddInputDto> AnswerList { get; set; }
    }
}
