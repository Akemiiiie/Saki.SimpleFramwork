using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    public class ExamRecordAddOutputDto: ExamRecordAddInputDto
    {
        /// <summary>
        /// 开始考试的主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public double? Score { get; set; }

        /// <summary>
        /// 提交的答案
        /// </summary>
        public List<ExamAnswerAddOutputDto> ExamAnswers { get; set; }
    }
}
