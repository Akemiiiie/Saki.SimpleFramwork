using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.AutoFac.AutofacRegister
{
    [AttributeUsage(AttributeTargets.Class)]
    // 为类型增加注入标识
    public class AutowiredPropertyAttribute : Attribute
    {

    }
}
