using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 考试答卷提交-整体提交
    /// </summary>
    public class ExamAnswerCommitInputDto: ExamRecordAddInputDto
    {
        /// <summary>
        /// 答卷列表
        /// </summary>
        public List<ExamAnswerAddInputDto> AnswerList { get; set; }
    }
}
