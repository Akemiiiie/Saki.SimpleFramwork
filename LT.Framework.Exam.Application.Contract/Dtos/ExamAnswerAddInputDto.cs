using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    public class ExamAnswerAddInputDto
    {
        /// <summary>
        /// 题目Id
        /// </summary>
        public required string QuestionId { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public required string StudentAnswer { get; set; }
    }
}
