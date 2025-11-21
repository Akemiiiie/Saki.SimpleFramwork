using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 申请考试
    /// </summary>
    public class ExamRecordAddInputDto
    {
        /// <summary>
        /// 参加的考试Id
        /// </summary>
        public required string ExamId { get; set; }

        /// <summary>
        /// 考号
        /// </summary>
        public required string StudentId { get; set; }
    }
}
