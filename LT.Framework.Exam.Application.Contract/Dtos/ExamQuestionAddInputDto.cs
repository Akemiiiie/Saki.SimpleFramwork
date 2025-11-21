using LT.Framework.Exam.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 单项题目添加
    /// </summary>
    public class ExamQuestionAddInputDto
    {
        /// <summary>
        /// 考试Id
        /// </summary>
        public required string ExamId { get; set; }

        /// <summary>
        /// 题号
        /// </summary>
        public required int QuestionNumber { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// 题目类型
        /// 0:单选题,1:多选题,2:判断题,3:填空题,4:简答题
        /// </summary>
        public required ExamType? Type { get; set; }

        /// <summary>
        /// 分数 - 不设置得分的话不统计总分
        /// </summary>
        public decimal? Score { get; set; }

        /// <summary>
        /// 内容-富文本形式的题目主体
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// 正确答案，多个空格以；分割
        /// </summary>
        public required string CorrectAnswer { get; set; }
    }
}
