using Saki.ModelTemplate.Bases;
using Saki.SqlSugar.Bases;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Domain.Entities
{
    /// <summary>
    /// 答题记录表
    /// </summary>
    [SugarTable("Exam_Answer")]
    public class ExamAnswerEntity: BaseEntity<Guid>
    {
        /// <summary>
        /// 答题记录Id
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string RecordId { get; set; }

        /// <summary>
        /// 题目Id
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string QuestionId { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnDataType = "NVARCHAR(MAX)")]
        public string StudentAnswer { get; set; }

        /// <summary>
        /// 是否正确
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsCorrect { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public double? Score { get; set; }

        /// <summary>
        /// 数据创建
        /// </summary>
        /// <param name="recordId">答卷Id</param>
        public void Create(string recordId)
        {
            this.Id = Guid.NewGuid();
            this.RecordId = recordId;
        }

        /// <summary>
        /// 数据创建
        /// </summary>
        /// <param name="recordId">答卷Id</param>
        /// <param name="questionId">题目Id</param>
        public void Create(string recordId,string questionId) 
        {
            this.RecordId = recordId;
            this.QuestionId = questionId;
        }
    }
}
