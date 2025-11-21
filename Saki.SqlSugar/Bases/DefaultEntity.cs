using Saki.ModelTemplate.Bases;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saki.BaseTemplate.Enums;

namespace Saki.SqlSugar.Bases
{
    public abstract class DefaultEntity<TKey>: BaseEntity<Guid>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间")]
        public virtual DateTimeOffset? CreatedTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnDescription = "创建人")]
        public virtual string? CreatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public virtual DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [SugarColumn(ColumnDescription = "修改人", IsNullable = true)]
        public virtual string? UpdatedBy { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        [SugarColumn(ColumnDescription = "软删除标识")]
        public virtual BooleanEnum IsDeleted { get; set; } = BooleanEnum.NO;

        /// <summary>
        /// 基础实体数据构建
        /// </summary>
        public void Create(string userId)
        {
            this.CreatedBy = userId;
            this.CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 基础实体数据构建
        /// </summary>
        public void Update(string userId)
        {
            this.UpdatedBy = userId;
            this.UpdatedTime = DateTime.Now;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete(string userId)
        {
            IsDeleted = BooleanEnum.YES;
            this.Update(userId);
        }
    }

    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : DefaultEntity<Guid>
    {

    }
}
