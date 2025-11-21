using LT.Framework.Exam.Domain.Entities;
using Saki.AutoFac.AutofacRegister;
using Saki.IRepositoryTemplate.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Domain.IRepositories
{
    /// <summary>
    /// 默认继承自IBaseRepository，可以在此扩展自定义方法
    /// </summary>
    public interface IExamRecordRepository : IBaseRepository<ExamRecordEntity>, ITransitDependency
    {
        /// <summary>
        /// 保存一次考试是整体数据，采用事务
        /// </summary>
        /// <returns></returns>
        Task<bool> TranCompletedExam(ExamRecordEntity examRecord, List<ExamAnswerEntity> examAnswers);
    }
}
