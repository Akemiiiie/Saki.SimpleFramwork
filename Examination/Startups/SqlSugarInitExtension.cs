using LT.Framework.Domain.Entities;
using LT.Framework.Exam.Domain.Entities;
using Saki.Framework.BaseRepository.BaseEntitys;
using SqlSugar;

namespace Examination.Startups
{
    /// <summary>
    /// 初始化数据库
    /// </summary>
    public static class SqlSugarInitExtension
    {
        public static void InitDefaultDb(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
            var defaultDb = new DefaultDb(db);
            defaultDb.CreateDefaultDb();
        }
    }

    /// <summary>
    /// 数据库默认初始化类
    /// </summary>
    public class DefaultDb
    {
        private readonly ISqlSugarClient _client;

        public DefaultDb(ISqlSugarClient client)
        {
            _client = client;
        }

        /// <summary>
        /// 自动建库建表
        /// </summary>
        public void CreateDefaultDb()
        {
            try
            {
                _client.DbMaintenance.CreateDatabase();
                // 系统模块
                _client.CodeFirst.InitTables(typeof(UsersEntity));
                _client.CodeFirst.InitTables(typeof(RequestLogEntity));
                _client.CodeFirst.InitTables(typeof(FileUploadRecordEntity));
                // 自定义表
                _client.CodeFirst.InitTables(typeof(ExamEntity));
                _client.CodeFirst.InitTables(typeof(ExamQuestionEntity));
                _client.CodeFirst.InitTables(typeof(ExamRecordEntity));
                _client.CodeFirst.InitTables(typeof(ExamAnswerEntity));
                Console.WriteLine("✅ 数据库与表结构初始化完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 数据库初始化失败：{ex.Message}");
            }
        }
    }
}
