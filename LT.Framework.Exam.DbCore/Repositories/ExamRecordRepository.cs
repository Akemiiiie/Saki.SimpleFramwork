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
    public class ExamRecordRepository : BaseRepository<ExamRecordEntity>, IExamRecordRepository
    {
        private ISugarQueryable<ExamRecordEntity> qeryable;
        private ISqlSugarClient sqlSugarClient;

        public ExamRecordRepository(ISqlSugarClient db) : base(db)
        {
            qeryable = base.AsQueryable();
            sqlSugarClient = db;
        }

        /// <summary>
        /// 保存一次考试是整体数据，采用事务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TranCompletedExam(ExamRecordEntity examRecord, List<ExamAnswerEntity> examAnswers)
        {
            try
            {
                await sqlSugarClient.Ado.BeginTranAsync();
                await sqlSugarClient.Insertable(examRecord).ExecuteCommandAsync();
                await sqlSugarClient.Insertable(examAnswers).ExecuteCommandAsync();
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
