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
    /// 考试题目仓储
    /// </summary>
    public class ExamQuestionRepository : BaseRepository<ExamQuestionEntity>, IExamQuestionRepository
    {
        private ISugarQueryable<ExamQuestionEntity> qeryable;

        private ISqlSugarClient sqlSugarClient;

        public ExamQuestionRepository(ISqlSugarClient db) : base(db)
        {
            qeryable = base.AsQueryable();
            sqlSugarClient = db;
        }

        /// <summary>
        /// 保存一次作业的整体数据，采用事务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TranCompletedExamQuestion(string examId, List<ExamQuestionEntity> questions)
        {
            try
            {
                await sqlSugarClient.Ado.BeginTranAsync();
                await sqlSugarClient.Deleteable<ExamQuestionEntity>().Where(t => t.ExamId == examId).ExecuteCommandAsync();
                await sqlSugarClient.Insertable(questions).ExecuteCommandAsync();
                await sqlSugarClient.Ado.CommitTranAsync();
                return true;
            }
            catch
            {
                await sqlSugarClient.Ado.RollbackTranAsync();
                throw;
            }
        }
    }
}
