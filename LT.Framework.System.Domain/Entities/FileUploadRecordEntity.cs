using Saki.BaseTemplate.Enums;
using Saki.ModelTemplate.Bases;
using Saki.SqlSugar.Bases;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Domain.Entities
{
    /// <summary>
    /// 表示系统中的文件上传记录。
    /// </summary>
    [SugarTable("FileUploadRecords")]
    public class FileUploadRecordEntity : DefaultEntity<Guid>
    {
        /// <summary>
        /// 获取或设置文件上传记录的唯一标识符。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "File_Id")]
        public override Guid Id { get; set; }

        /// <summary>
        /// 获取或设置与文件上传关联的用户ID。
        /// </summary>
        [SugarColumn(IsNullable = false, ColumnName = "User_Id")]
        public string UserId { get; set; }

        /// <summary>
        /// 获取或设置上传文件的文件名。
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 255)]
        public string FileName { get; set; }

        /// <summary>
        /// 获取或设置文件在服务器上的存储路径。
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 255)]
        public string FilePath { get; set; }

        /// <summary>
        /// 获取或设置文件的线上路径。
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 255)]
        public string OnlinePath { get; set; }

        /// <summary>
        /// 获取或设置上传文件的大小（字节）。
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public long FileSize { get; set; }

        /// <summary>
        /// 获取或设置上传文件的MIME类型。
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 100)]
        public string FileType { get; set; }

        /// <summary>
        /// 获取或设置文件的描述信息。
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="userId"></param>
        public void Create(string userId) 
        {
            this.CreatedBy = userId;
            this.CreatedTime = DateTime.Now;
            this.IsDeleted = BooleanEnum.NO;
        }
    }
}
