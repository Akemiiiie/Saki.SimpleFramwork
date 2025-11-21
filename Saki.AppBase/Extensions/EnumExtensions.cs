using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}