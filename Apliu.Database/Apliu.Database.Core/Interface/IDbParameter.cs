using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apliu.Database.Core
{
    public interface IDbParameter
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DatabaseType Type { get; }
        int? GetDbFieldType(Type type);
        DbParameter[] MakeParam(IDictionary<string, object> parameters);
        DbParameter MakeParam(string parameterName, object value);
        DbParameter MakeParam(string parameterName, int dbFieldType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value);
    }
}
