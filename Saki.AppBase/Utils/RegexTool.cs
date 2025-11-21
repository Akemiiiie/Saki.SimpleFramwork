using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase.Utils
{
    public static class RegexTool
    {
        /// <summary>
        /// 将通配符模式转换为正则表达式模式
        /// </summary>
        /// <param name="pattern">通配符模式</param>
        /// <returns>正则表达式模式</returns>
        private static string WildcardToRegex(string pattern)
        {
            // 将所有非特殊字符转义
            string regexPattern = Regex.Escape(pattern);
            // 替换通配符 "*" 为 .* (表示任意字符, 包括空字符, 任意次数)
            regexPattern = regexPattern.Replace(@"\*", ".*");
            // 替换通配符 "?" 为 . (表示任意单个字符)
            regexPattern = regexPattern.Replace(@"\?", ".");
            return $"^{regexPattern}$"; // 添加锚点表示整个字符串匹配
        }

        /// <summary>
        /// 使用通配符模式匹配文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="wildcardPattern">通配符模式</param>
        /// <returns>是否匹配</returns>
        public static bool MatchesWildcardPattern(string filename, string wildcardPattern)
        {
            string regexPattern = WildcardToRegex(wildcardPattern);
            Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(filename);
        }
    }
}
