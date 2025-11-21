using LT.Framework.Exam.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    public class ExamQuestionUpdateInputDto
    {
        /// <summary>
        /// 题目的主键Id
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// 考试Id
        /// </summary>
        public string ExamId { get; set; }

        /// <summary>
        /// 题号
        /// </summary>
        public int QuestionNumber { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 题目类型
        /// </summary>
        public ExamType? Type { get; set; }

        /// <summary>
        /// 分数 - 不设置得分的话不统计总分
        /// </summary>
        public decimal? Score { get; set; }

        /// <summary>
        /// 内容-富文本形式的题目主体
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 正确答案，多个空格以；分割
        /// </summary>
        public string CorrectAnswer { get; set; }
    }
}
