using Saki.BaseTemplate.Enums;
using Saki.Framework.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    public class ExamsAddInputDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///  考试开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 考试结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 背景框架附件Id
        /// </summary>
        public string BGFileId { get; set; }

        /// <summary>
        /// 答案附件Id
        /// </summary>
        public string AnswerFileId { get; set; }

        /// <summary>
        /// 考试描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        /// <summary>
        /// 软删除
        /// </summary>
        public BooleanEnum? IsDeleted { get; set; }
    }
}
