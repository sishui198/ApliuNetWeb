using System;

namespace Apliu.Database.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DatabaseTypeAttribute : Attribute
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DbType { get; private set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        public DatabaseTypeAttribute(DatabaseType dbType)
        {
            this.DbType = dbType;
        }
    }
}
