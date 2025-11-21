using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.Framework.Mapster
{
        /// <summary>
    /// Mapster 全局映射配置类。
    /// 用于集中注册项目中所有自定义的对象映射规则，
    /// 避免映射逻辑分散在各处，提升可维护性与可测试性。
    /// </summary>
    public static class MapsterConfig
    {
        /// <summary>
        /// 注册所有自定义的 Mapster 映射配置
        /// 此方法应在应用程序启动时（如 Program.cs）调用一次
        /// </summary>
        public static void Register()
        {
            // 额外的映射配置注册可以在这里添加

        }
    }
}
