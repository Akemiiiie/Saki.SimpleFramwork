using Saki.AutoFac.AutofacRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Domain.Shared.Ibll
{
    public interface ICurrentUser: ITransitDependency
    {
        string UserId { get; }
        string UserName { get; }
        string Role { get; }
    }
}
