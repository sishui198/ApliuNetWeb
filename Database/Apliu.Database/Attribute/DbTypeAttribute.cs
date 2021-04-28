using System;

namespace Apliu.Database
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DbTypeAttribute : Attribute
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DbType { get; private set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        public DbTypeAttribute(DbType dbType)
        {
            this.DbType = dbType;
        }
    }
}
