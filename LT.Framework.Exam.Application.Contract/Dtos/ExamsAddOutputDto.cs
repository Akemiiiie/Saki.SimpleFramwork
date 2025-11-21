using LT.Framework.Exam.Domain.Shared.Enum;
using Saki.Framework.AppBase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 直接返回填报数据，在看情况添加其他字段
    /// </summary>
    public class ExamsAddOutputDto: ExamsAddInputDto
    {
        /// <summary>
        /// 作业主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 答案文件名
        /// </summary>
        public string? AnswerFileName { get; set; }

        /// <summary>
        /// 答案文件名
        /// </summary>
        public string? BGFileName { get; set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public ExamStatus ExamStatus
        {
            get
            {
                if (DateTime.Now <= this.StartDate)
                    return ExamStatus.NotStarted;
                if (DateTime.Now >= this.EndDate)
                    return ExamStatus.Ended;
                return ExamStatus.Started;
            }
        }
    }
}
