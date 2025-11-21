using Saki.ModelTemplate.Bases;
using Saki.SqlSugar.Bases;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saki.BaseTemplate.Enums;

namespace LT.Framework.Domain.Entities
{
    /// <summary>
    /// 实体层
    /// </summary>
    [SugarTable("sys_Users")]
    public class UsersEntity : Entity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "User_Id")]
        public override Guid Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(ColumnName = "User_Name")]
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [SugarColumn(ColumnName = "User_Account")]
        public string Account { get; set; }

        /// <summary>
        /// 用户性别
        /// </summary>
        [SugarColumn(ColumnName = "User_Sex", IsNullable = true)]
        public int? UserSex { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [SugarColumn(ColumnName = "IdCard", IsNullable = true)]
        public string? IdCard { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [SugarColumn(ColumnName = "Age", IsNullable = true)]
        public int? Age { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [SugarColumn(ColumnName = "PhoneNumber", IsNullable = true)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(ColumnName = "Address", IsNullable = true)]
        public string? Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [SugarColumn(ColumnName = "Email", IsNullable = true)]
        public string? Email { get; set; }

        /// <summary>
        /// 加密密码
        /// </summary>
        [SugarColumn(ColumnName = "Password", IsNullable = true)]
        public string Password { get; set; }

        /// <summary>
        /// 盐值
        /// </summary>
        [SugarColumn(ColumnName = "Salt", IsNullable = true)]
        public string Salt { get; set; }

        public UsersEntity()
        {
        }

        /// <summary>
        /// 构建默认用户信息
        /// </summary>
        public UsersEntity(string name, string userId)
        {
            this.Name = name;
            this.Account = Math.Round(new Random().NextDouble(), 9).ToString().Replace("0.", "");
            //生成盐值
            this.Salt = Math.Round(new Random().NextDouble(), 6).ToString().Replace("0.", "");
            this.IsDeleted = BooleanEnum.NO;
            this.CreatedTime = DateTime.Now;
            this.CreatedBy = userId;
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="password"></param>
        public void setPassword(string password)
        {
            this.Password = password;
        }
    }
}
