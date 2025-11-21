using Saki.BaseTemplate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 考试答题记录返回Dto
    /// </summary>
    public class ExamAnswerAddOutputDto: ExamAnswerAddInputDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 答案是否正确
        /// </summary>
        public BooleanEnum IsCorrect { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public float Score { get; set; }
    }
}
