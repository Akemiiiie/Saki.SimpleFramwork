using LT.Framework.Contract.Dtos.Files;
using LT.Framework.DbCore.Repositories;
using LT.Framework.Domain.Entities;
using LT.Framework.Domain.IRepositories;
using LT.Framework.Domain.Shared.Ibll;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Panda.DynamicWebApi.Attributes;
using Saki.AutoFac.AutofacRegister;
using Saki.BaseTemplate.BaseControllers;
using Saki.BaseTemplate.Enums;
using Saki.Framework.AppBase;

namespace LT.Framework.Application.Services
{
    /// <summary>
    /// User相关服务
    /// </summary>
    [Authorize]
    [DynamicWebApi]
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    public class FileService : BaseController, IFileService, ITransitDependency
    {
        private IFileUploadRecordRepository _uploadRepository;
        private ICurrentUser _currentUser;
        public FileService(IFileUploadRecordRepository fileUploadRecordRepository, ICurrentUser currentUser)
        {
            _uploadRepository = fileUploadRecordRepository;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 通用文件上传
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> FileUpload([FromForm] FileUploadRecordInputDto input,IFormFile file)
        {
            var res = await FileSave(input, file);
            return Success(res);
        }

        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<ApiResponse<FileUploadRecordOutputDto>> FileSave([FromForm] FileUploadRecordInputDto input, IFormFile file) 
        {
            if (file == null || file.Length == 0)
            {
                return new ApiResponse<FileUploadRecordOutputDto> { Code = 500, Message = "上传文件不能为空" };
            }
            // 保存文件到服务器
            var uniqueFileName = GenerateUniqueFileName(file.FileName);
            // 保存文件到服务器
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", Path.GetExtension(file.FileName).TrimStart('.'), DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // 创建文件上传记录
            var fileUploadRecord = new FileUploadRecordEntity
            {
                UserId = _currentUser.UserId,
                FileName = file.FileName,
                FilePath = filePath,
                OnlinePath = $"/uploads/{file.FileName}",
                FileSize = file.Length,
                FileType = file.ContentType,
                Description = input.Description
            };
            fileUploadRecord.Create(_currentUser.UserId);
            // 插入数据库
            var insertResult = await _uploadRepository.Add(fileUploadRecord);
            if (insertResult)
            {
                return new ApiResponse<FileUploadRecordOutputDto> { Code = 200, Message = "文件上传成功", Data = fileUploadRecord.Adapt<FileUploadRecordOutputDto>() };
            }
            else
            {
                return new ApiResponse<FileUploadRecordOutputDto> { Code = 500, Message = "文件上传失败" };
            }
        }

        /// <summary>
        /// 附件下载-根据Id下载指定附件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> FileDownload([FromBody] string fileId)
        {
            // 从数据库中获取文件上传记录
            var fileUploadRecord = await _uploadRepository.GetByIdAsync(fileId);
            if (fileUploadRecord == null || fileUploadRecord.IsDeleted == BooleanEnum.YES)
            {
                return NotFound("文件未找到或已被删除");
            }
            // 检查文件是否存在
            var filePath = fileUploadRecord.FilePath;
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("文件不存在于服务器");
            }
            // 读取文件并返回
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileName = fileUploadRecord.FileName;
            var contentType = fileUploadRecord.FileType;
            return File(fileStream, contentType, fileName);
        }

        /// <summary>
        /// 文件列表查询
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetFilePage(FilePageQueryDto query)
        {
            string userId = _currentUser.UserId;
            var pageData = await _uploadRepository.QueryPageAsync(query, (t, dto) =>
            {
                t.Where(t => t.UserId.Equals(userId) && t.IsDeleted == BooleanEnum.NO)
                .WhereIF(!string.IsNullOrEmpty(dto.FileName), t => t.FileName.Contains(dto.FileName))
                .WhereIF(!string.IsNullOrEmpty(dto.FileType), t => t.FileType.Equals(dto.FileType));
            });
            return Success(pageData);
        }

        /// <summary>
        /// 根据id查询附件信息
        /// 供其他程序集调用不作为API
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public async Task<FileUploadRecordOutputDto> GetFileInfo(string fileId) 
        {
            var fileUploadRecord = await _uploadRepository.GetByIdAsync(fileId);
            if (fileUploadRecord == null || fileUploadRecord.IsDeleted == BooleanEnum.YES)
            {
                return null;
            }
            return fileUploadRecord.Adapt<FileUploadRecordOutputDto>();
        }

        /// <summary>
        /// 生成唯一文件名
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <returns></returns>
        [NonAction]
        private string GenerateUniqueFileName(string originalFileName)
        {
            // 使用 GUID 生成唯一的文件名
            var extension = Path.GetExtension(originalFileName);
            var uniqueFileName = Guid.NewGuid().ToString("N") + extension;
            return uniqueFileName;
        }
    }
}
