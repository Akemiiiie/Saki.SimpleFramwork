using Saki.BaseTemplate.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.ModelTemplate.Bases
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class BaseEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "Id")]
        public virtual TKey Id { get; set; }
    }
}
