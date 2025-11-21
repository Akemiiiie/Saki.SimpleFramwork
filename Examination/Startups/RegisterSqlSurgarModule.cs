using Autofac;
using LT.Framework.Exam.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Saki.AutoFac.AutofacRegister;
using Saki.BaseTemplate.ConfigerOptions;
using Saki.IRepositoryTemplate.Users;
using SqlSugar;
using StackExchange.Profiling;

namespace Examination.Startups
{
    public static class RegisterSqlSurgarModule
    {
        public static IServiceCollection AddSqlSugar(this IServiceCollection services)
        {
            services.AddSingleton(_ => CreateClient());
            return services;
        }

        private static ISqlSugarClient CreateClient()
        {
            var client = new SqlSugarScope(new ConnectionConfig
            {
                ConnectionString = BaseDbConfig.DefaultConnection,
                DbType = DbType.SqlServer,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            }, db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    //注意:  这儿AOP设置不能少
                    MiniProfiler.Current.
                     CustomTiming($"ConnId:[{db.ContextID}] SQL：",
                         "【SQL语句】：" + UtilMethods.GetNativeSql(sql, pars));
                    Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
                    //Console.WriteLine(sql + "\r\n" +
                    //Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    Console.WriteLine();
                };
            });
            return client;
        }
    }
}
