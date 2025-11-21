using LT.Framework.Exam.Domain.Entities;
using LT.Framework.Exam.Domain.IRepositories;
using Saki.RepositoryTemplate.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.DbCore.Repositories
{
    /// <summary>
    /// 考试答题记录仓储
    /// </summary>
    public class ExamAnswerRepository : BaseRepository<ExamAnswerEntity>, IExamAnswerRepository
    {
        private ISugarQueryable<ExamAnswerEntity> qeryable;

        public ExamAnswerRepository(ISqlSugarClient db) : base(db)
        {
            qeryable = base.AsQueryable();
        }
    }
}
