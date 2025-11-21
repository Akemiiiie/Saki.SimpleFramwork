using LT.Framework.Exam.Domain.Shared.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    /// <summary>
    /// 上传作业附件
    /// </summary>
    public class UploadExamUploadDto
    {
        /// <summary>
        /// 作业Id
        /// </summary>
        public string ExamId { get; set; }

        /// <summary>
        /// 文件类型
        /// 0:背景框架附件,1答案附件
        /// </summary>
        public ExamFileType FileType { get; set; }

        /// <summary>
        /// 上传的文件
        /// </summary>
        public IFormFile File { get; set; }
    }
}
