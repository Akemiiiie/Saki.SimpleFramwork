using LT.Framework.Domain.Entities;
using LT.Framework.Domain.IRepositories;
using Saki.RepositoryTemplate.Base;
using SqlSugar;

namespace LT.Framework.DbCore.Repositories
{
    /// <summary>
    /// 文件上传记录仓储
    /// </summary>
    public class FileUploadRecordRepository : BaseRepository<FileUploadRecordEntity>, IFileUploadRecordRepository
    {
        private ISugarQueryable<FileUploadRecordEntity> qeryable;

        public FileUploadRecordRepository(ISqlSugarClient db) : base(db)
        {
            qeryable = base.AsQueryable();
        }
    }
}
