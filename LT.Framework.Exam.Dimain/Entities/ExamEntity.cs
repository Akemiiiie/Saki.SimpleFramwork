using Saki.BaseTemplate.Enums;
using Saki.SqlSugar.Bases;
using SqlSugar;

namespace LT.Framework.Exam.Domain.Entities
{
    [SugarTable("Exams")]
    public class ExamEntity : DefaultEntity<Guid>
    {
        /// <summary>
        /// 考试标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 考试开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 考试结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(ColumnDescription = "描述", IsNullable = true)]
        public string? Description { get; set; }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            this.IsDeleted = BooleanEnum.YES;
        }

        /// <summary>
        /// 背景框架附件Id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string BGFileId { get; set; }

        /// <summary>
        /// 答案附件Id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string AnswerFileId { get; set; }
    }
}
