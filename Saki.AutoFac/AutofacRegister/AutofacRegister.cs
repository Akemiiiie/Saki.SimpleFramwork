using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Saki.AutoFac.AutofacRegister
{
    /// <summary>
    /// IOC容器批量注入
    /// </summary>
    public static class AutofacRegister
    {
        /// <summary>
        /// 将指定程序集中实现含有自动扩展特性的类进行批量注入
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        public static void BatchAutowired(this ContainerBuilder builder, Assembly assembly)
        {
            var transient = typeof(ITransitDependency);
            var scoped = typeof(IScopeDependency);
            var singleton = typeof(ISingletonDependency);

            // 瞬时依赖
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.IsClass && !t.IsAbstract && transient.IsAssignableFrom(t))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // 作用域依赖
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.IsClass && !t.IsAbstract && scoped.IsAssignableFrom(t))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // 单例依赖
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.IsClass && !t.IsAbstract && singleton.IsAssignableFrom(t))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
