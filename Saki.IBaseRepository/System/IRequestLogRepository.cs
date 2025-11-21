using Saki.AutoFac.AutofacRegister;
using Saki.Framework.BaseRepository.BaseEntitys;
using Saki.IRepositoryTemplate.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.IBaseRepository.System
{
    public interface IRequestLogRepository:IBaseRepository<RequestLogEntity>, IScopeDependency
    {

    }
}
