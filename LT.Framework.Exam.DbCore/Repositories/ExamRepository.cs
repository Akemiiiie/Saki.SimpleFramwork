using LT.Framework.Exam.Domain.Entities;
using LT.Framework.Exam.Domain.IRepositories;
using Saki.RepositoryTemplate.Base;
using SqlSugar;

namespace LT.Framework.Exam.DbCore.Repositories
{
    /// <summary>
    /// 考试记录仓储
    /// </summary>
    public class ExamRepository : BaseRepository<ExamEntity>, IExamRepository
    {
        private ISugarQueryable<ExamEntity> qeryable;

        public ExamRepository(ISqlSugarClient db) : base(db)
        {
            qeryable = base.AsQueryable();
        }
    }
}
