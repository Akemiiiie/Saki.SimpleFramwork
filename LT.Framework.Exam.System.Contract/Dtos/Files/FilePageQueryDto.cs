using Saki.Framework.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Contract.Dtos.Files
{
    /// <summary>
    /// 文件分页查询DTO
    /// </summary>
    public class FilePageQueryDto : PageQuery
    {
        public string FileName { get; set; }

        public string FileType { get; set; }
    }
}
