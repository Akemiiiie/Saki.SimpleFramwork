using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 更新作业信息实体
    /// </summary>
    public class ExamsUpdateInputDto: ExamsAddInputDto
    {
        /// <summary>
        /// 作业主键
        /// </summary>
        public required string Id { get; set; }
    }
}
