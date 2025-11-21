using Saki.IDbBase.Entities;
using Saki.ModelTemplate.Bases;
using Saki.SqlSugar.Bases;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saki.BaseTemplate.Enums;

namespace LT.Framework.Exam.Domain.Entities
{
    /// <summary>
    /// 考试记录表
    /// </summary>
    [SugarTable("Exam_Records")]
    public class ExamRecordEntity:DefaultEntity<Guid>
    {
        /// <summary>
        /// 考试Id
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string ExamId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string StudentId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? SubmitTime { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public double? Score { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string FileId { get; set; }

        /// <summary>
        /// 构造默认数据
        /// </summary>
        public void Create(string userId) 
        {
            this.Id = Guid.NewGuid();
            this.CreatedTime = DateTime.Now;
            this.CreatedBy = userId;
            this.StartTime = DateTime.Now;
        }

        /// <summary>
        /// 构造默认数据
        /// </summary>
        public void Update(string userId)
        {
            this.UpdatedBy = userId;
            this.UpdatedTime = DateTime.Now;
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void Completed(string userId)
        {
            this.Update(userId);
            this.SubmitTime = DateTime.Now;
        }

        /// <summary>
        /// 作废
        /// </summary>
        public void Cancelled(string userId)
        {
            this.IsDeleted = BooleanEnum.YES;
            this.Update(userId);
        }
    }
}
