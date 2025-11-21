using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Framework.Exam.Domain.Shared.Enum
{
    /// <summary>
    /// 题目类型
    /// </summary>
    public enum ExamType
    {
        /// <summary>
        /// 单选题
        /// </summary>
        [Description("单选题")]
        SingleChoice = 0,

        /// <summary>
        /// 多选题
        /// </summary>
        [Description("多选题")]
        MultipleChoice = 1,

        /// <summary>
        /// 判断题
        /// </summary>
        [Description("判断题")]
        TrueFalse = 2,

        /// <summary>
        /// 填空题
        /// </summary>
        [Description("填空题")]
        FillInBlank = 3,

        /// <summary>
        /// 简答题
        /// </summary>
        [Description("简答题")]
        ShortAnswer = 4
    }

    /// <summary>
    /// 作业文件类型
    /// </summary>
    public enum ExamFileType 
    {
        /// <summary>
        /// 背景框架附件
        /// </summary>
        [Description("背景框架附件")]
        BGFile = 0,
        /// <summary>
        /// 答案附件
        /// </summary>
        [Description("答案附件")]
        AnswersFile = 1,
    }

    /// <summary>
    /// 作业状态
    /// </summary>
    public enum ExamStatus 
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        NotStarted = 0,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Started = 1,
        /// <summary>
        /// 已结束
        /// </summary>
        [Description("已结束")]
        Ended = 2
    }
}
