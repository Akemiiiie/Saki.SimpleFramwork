using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Saki.Framework.AppBase.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 递归为对象中所有 DateTime/DateTime? 或 DateTimeOffset/DateTimeOffset? 属性添加等值时间戳
        /// </summary>
        public static object AddExtensions(object value)
        {
            if (value == null) return null;

            return value switch
            {
                DateTime dt => CreateDateTimeObj(dt),
                DateTimeOffset dto => CreateDateTimeOffsetObj(dto),
                IEnumerable list when !(value is string) => ProcessList(list),
                IDictionary dict => ProcessDictionary(dict),
                //Enum e => CreateEnumObj(e),
                _ when IsClass(value) => ProcessObject(value),
                _ => value
            };
        }

        private static object CreateDateTimeObj(DateTime? dt)
        {
            dt ??= new DateTime(1970, 1, 1);
            // 处理传入的时间就是0的情况
            if (dt == DateTime.MinValue)
            {
                return new
                {
                    DateTime = dt,
                    DateTime_ts = (long?)null // 返回 null 表示 Unix 时间戳无效
                };
            }
            return new
               {
                   DateTime = dt,
                   DateTime_ts = new DateTimeOffset(dt.Value).ToUnixTimeMilliseconds()
               };
        }

        private static object CreateDateTimeOffsetObj(DateTimeOffset? dto)
        {
            dto ??= DateTimeOffset.MinValue;
            if (dto == DateTimeOffset.MinValue)
            {
                return new
                {
                    DateTime = dto,
                    DateTime_ts = (long?)null // 返回 null 表示 Unix 时间戳无效
                };
            }
            return new
            {
                DateTimeOffset = dto,
                DateTimeOffset_ts = dto.Value.ToUnixTimeSeconds()
            };
        }

        private static List<object> ProcessList(IEnumerable list)
        {
            var newList = new List<object>();
            foreach (var item in list)
                newList.Add(AddExtensions(item));
            return newList;
        }

        private static Dictionary<object, object> ProcessDictionary(IDictionary dict)
        {
            var newDict = new Dictionary<object, object>();
            foreach (DictionaryEntry kv in dict)
                newDict[kv.Key] = AddExtensions(kv.Value);
            return newDict;
        }

        private static Dictionary<string, object> ProcessObject(object obj)
        {
            var type = obj.GetType();
            var result = new Dictionary<string, object>();
            foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var val = prop.GetValue(obj);
                result[prop.Name] = AddExtensions(val);
            }
            return result;
        }

        //private static object CreateEnumObj(Enum e) => new
        //{
        //    Value = Convert.ToInt32(e),
        //    Name = e.ToString(),
        //    Description = e.GetDescription()
        //};

        private static bool IsClass(object obj) => obj.GetType().IsClass && obj is not string;
    }
}

