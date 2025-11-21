using Saki.ModelTemplate.Bases;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Domain.Entities
{
    [SugarTable("Exam_Questions")]
    public class ExamQuestionEntity : BaseEntity<Guid>
    {
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
        public string Type { get; set; }

        /// <summary>
        /// 分数 - 不设置得分的话不统计总分
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public double Score { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NVARCHAR(MAX)")]
        public string Content { get; set; }

        /// <summary>
        /// 正确答案
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR(MAX)")]
        public string CorrectAnswer { get; set; }

        /// <summary>
        /// 构造默认数据
        /// </summary>
        public void Create() 
        {
        
        }

        /// <summary>
        /// 构造默认数据
        /// </summary>
        public void Update()
        {

        }
    }
}
