using Saki.AutoFac.AutofacRegister;
using Saki.Framework.BaseRepository.BaseEntitys;
using Saki.Framework.IBaseRepository.System;
using Saki.RepositoryTemplate.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.BaseRepository.System
{
    public class RequestLogRepository : BaseRepository<RequestLogEntity>, IRequestLogRepository, IScopeDependency
    {
        private ISugarQueryable<RequestLogEntity> qeryable;

        public RequestLogRepository(ISqlSugarClient db) : base(db)
        {
            qeryable = base.AsQueryable();
        }
    }
}
