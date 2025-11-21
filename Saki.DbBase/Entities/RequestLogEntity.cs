using Saki.IDbBase.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.BaseRepository.BaseEntitys
{
    /// <summary>
    /// 请求日志表，用于记录系统中所有 Web API 请求的详细信息。
    /// </summary>
    [SugarTable("sys_RequestLog")]
    public class RequestLogEntity: BaseEntity
    {
        /// <summary>
        /// 主键 Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 请求的唯一标识符（用于链路追踪）
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string TraceId { get; set; }

        /// <summary>
        /// 请求的 API 路径
        /// </summary>
        [SugarColumn(Length = 512)]
        public string RequestPath { get; set; }

        /// <summary>
        /// HTTP 请求方法（GET、POST、PUT、DELETE 等）
        /// </summary>
        [SugarColumn(Length = 10)]
        public string Method { get; set; }

        /// <summary>
        /// 请求头部信息（JSON 格式）
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string Headers { get; set; }

        /// <summary>
        /// 请求参数或 Body 内容（JSON 格式）
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string RequestBody { get; set; }

        /// <summary>
        /// 响应内容（仅保留必要信息，避免存大文件）
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string ResponseBody { get; set; }

        /// <summary>
        /// 请求发起人 IP 地址
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string IPAddress { get; set; }

        /// <summary>
        /// 所属控制器或类名
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string ControllerName { get; set; }

        /// <summary>
        /// 调用的方法名
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string ActionName { get; set; }

        /// <summary>
        /// 请求是否成功（true=成功，false=失败）
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息（仅在失败时记录）
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 请求耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime? ResponseTime { get; set; }

        /// <summary>
        /// 用户 Id（可选，用于记录操作人）
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string UserName { get; set; }
    }
}
