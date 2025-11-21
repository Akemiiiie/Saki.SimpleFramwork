using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 返回单个学生的作业详情
    /// </summary>
    public class ExamDetailOutputDto
    {
        /// <summary>
        /// 考试信息
        /// </summary>
        public ExamsAddOutputDto ExamInfo { get; set; }

        /// <summary>
        /// 题目列表以及对应答案
        /// </summary>
        public List<ExamQuestionOutputDto> Questions { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public float TotalScore
        { 
            get 
            {
                return Questions?.Sum(t => t.Answer?.Score ?? 0) ?? 0;
            }
        }
    }
}
