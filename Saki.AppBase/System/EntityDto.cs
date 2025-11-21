using Saki.BaseTemplate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class EntityDto<TKey> where TKey : struct
    {
        public EntityDto()
        {

        }

        /// <summary>
        /// 包含Id的构造
        /// </summary>
        /// <param name="id"></param>
        public EntityDto(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        public virtual TKey? Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTimeOffset? CreatedTime { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        public virtual BooleanEnum? IsDeleted { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTimeOffset? UpdatedTime { get; set; }
    }
}
