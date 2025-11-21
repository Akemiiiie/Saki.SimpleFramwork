using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saki.BaseTemplate.Enums
{
    /// <summary>
    /// 通用是/否枚举
    /// </summary>
    public enum BooleanEnum
    {
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        NO = 0,

        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        YES = 1
    }

    /// <summary>
    /// 业务类型
    /// </summary>
    public enum FileBizType 
    {
        /// <summary>
        /// 系统模块
        /// </summary>
        Base = 0,

        /// <summary>
        /// 作业模块
        /// </summary>
        Exam = 1,
    }

}
