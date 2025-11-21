using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    public class ExamQuestionOutputDto: ExamQuestionAddInputDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 对应的答案
        /// </summary>
        public ExamAnswerAddOutputDto? Answer { get; set; }
    }
}
