using Autofac;
using Examination.Middleware;
using LT.Framework.Application;
using LT.Framework.Application.Services;
using LT.Framework.DbCore.Repositories;
using LT.Framework.Exam.Application.Services;
using LT.Framework.Exam.DbCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Saki.AutoFac.AutofacRegister;
using Saki.Framework.BaseRepository.System;
using Saki.IRepositoryTemplate.Base;
using Saki.Logging;

namespace Examination.Startups;

internal class AutofacRegisterModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // 获取所有控制器类型并使用属性注入
        var controllersTypeAssembly = typeof(Program).Assembly.GetExportedTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
        builder.RegisterTypes(controllersTypeAssembly).PropertiesAutowired(new AutowiredPropertySelector());

        // 批量自动注入,把需要注入层的程序集传参数,注入Service层的类
        // 基础模块
        builder.BatchAutowired(typeof(IBaseRepository<>).Assembly);
        builder.BatchAutowired(typeof(RequestLogMiddleware).Assembly);
        builder.BatchAutowired(typeof(RequestLogRepository).Assembly);
        builder.BatchAutowired(typeof(CurrentUser).Assembly);
        // 系统模块
        builder.BatchAutowired(typeof(UsersRepository).Assembly);
        builder.BatchAutowired(typeof(UserService).Assembly);
        builder.BatchAutowired(typeof(NLogLogger).Assembly);
        // 自定义模块
        builder.BatchAutowired(typeof(ExamRepository).Assembly);
        builder.BatchAutowired(typeof(ExamService).Assembly);
    }
}