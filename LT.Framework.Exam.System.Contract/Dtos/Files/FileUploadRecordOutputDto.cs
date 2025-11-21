using Saki.BaseTemplate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Contract.Dtos.Files
{
    /// <summary>
    /// 文件上传记录输出数据传输对象。
    /// </summary>
    public class FileUploadRecordOutputDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置与文件上传关联的用户ID。
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 获取或设置上传文件的文件名。
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 获取或设置文件在服务器上的存储路径。
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 获取或设置文件的线上路径。
        /// </summary>
        public string OnlinePath { get; set; }

        /// <summary>
        /// 获取或设置上传文件的大小（字节）。
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 获取或设置上传文件的MIME类型。
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 获取或设置文件上传记录的状态。
        /// </summary>
        public string RecordStatus { get; set; }

        /// <summary>
        /// 获取或设置软删除标志，1表示已删除，0表示未删除。
        /// </summary>
        public BooleanEnum IsDeleted { get; set; }

        /// <summary>
        /// 获取或设置文件上传的时间。
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 获取或设置文件的描述信息。
        /// </summary>
        public string Description { get; set; }
    }
}
