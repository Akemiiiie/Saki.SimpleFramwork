using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Saki.AutoFac.AutofacRegister
{
    /// <summary>
    /// 新增依赖注入特性类 - 检测属性是否包含指定注释以进行注入
    /// 废弃
    /// </summary>
    public class AutowiredPropertySelector:IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.CustomAttributes.Any(s => s.AttributeType == typeof(AutowiredPropertyAttribute));
        }
    }
}
