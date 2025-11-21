using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Application.Contract.Dtos
{
    public class UploadExamRecordFileDto
    {
        /// <summary>
        /// 答题记录的Id
        /// </summary>
        [Required]
        public string RecordId { get; set; }

        /// <summary>
        /// 上传附件
        /// </summary>
        [Required]
        public IFormFile File { get; set; }
    }
}
