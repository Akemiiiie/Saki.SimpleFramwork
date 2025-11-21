using LT.Framework.Contract.Dtos.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saki.Framework.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Domain.Shared.Ibll
{
    public interface IFileService
    {
        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <returns></returns>
        Task<ApiResponse<FileUploadRecordOutputDto>> FileSave([FromForm] FileUploadRecordInputDto input, IFormFile file);

        /// <summary>
        /// 根据id查询附件信息
        /// </summary>
        /// <returns></returns>
        Task<FileUploadRecordOutputDto> GetFileInfo(string fileId);
    }
}
