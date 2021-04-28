using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Apliu.Database.Model
{
    public abstract class DbParameter : IDbParameter
    {
        private readonly DbType _dbType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DbType { get { return this._dbType; } }

        /// <summary>
        /// 数据库会话信息
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        protected DbParameter(DbType dbType)
        {
            this._dbType = dbType;
        }
        public abstract int? GetDbFieldType(Type type);
        public System.Data.Common.DbParameter[] MakeParam(IDictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return null;

            var ps = parameters.Select(u => this.MakeParam(u.Key, u.Value)).ToArray();
            return ps;
        }
        public abstract System.Data.Common.DbParameter MakeParam(string parameterName, object value);
        public abstract System.Data.Common.DbParameter MakeParam(string parameterName, int dbFieldType, int size, System.Data.ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, System.Data.DataRowVersion sourceVersion, object value);
    }
}
