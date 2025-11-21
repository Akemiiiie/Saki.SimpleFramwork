using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    public class RecordItemAddOutputDto
    {
        /// <summary>
        /// 答题记录主键
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// 答题明细
        /// </summary>
        public List<ExamAnswerAddOutputDto> ExamAnswers { get; set; }
    }
}
