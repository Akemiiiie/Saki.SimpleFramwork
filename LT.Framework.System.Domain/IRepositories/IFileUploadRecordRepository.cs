using LT.Framework.Domain.Entities;
using Saki.AutoFac.AutofacRegister;
using Saki.IRepositoryTemplate.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Domain.IRepositories
{
    /// <summary>
    /// 默认继承自IBaseRepository，可以在此扩展自定义方法
    /// </summary>
    public interface IFileUploadRecordRepository:IBaseRepository<FileUploadRecordEntity>, ITransitDependency
    {

    }
}
