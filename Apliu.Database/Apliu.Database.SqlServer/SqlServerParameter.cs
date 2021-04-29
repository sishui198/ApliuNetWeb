using Apliu.Database.Core;
using Apliu.Database.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace Apliu.Database.SqlServer
{
    public class SqlServerParameter : DbParameter
    {
        private readonly ReadOnlyDictionary<Type, SqlDbType> _dbFieldTypeMapping;

        public SqlServerParameter()
            : base(DatabaseType.SqlServer)
        {
            var dic = new Dictionary<Type, SqlDbType>()
            {
                { typeof(byte), SqlDbType.TinyInt },
                { typeof(short), SqlDbType.SmallInt },
                { typeof(int), SqlDbType.Int },
                { typeof(long), SqlDbType.BigInt },
                { typeof(float), SqlDbType.Float },
                { typeof(double), SqlDbType.Real },
                { typeof(decimal), SqlDbType.Decimal },
                { typeof(bool), SqlDbType.Bit },
                { typeof(DateTime), SqlDbType.DateTime },
                { typeof(string), SqlDbType.VarChar },
                { typeof(Guid), SqlDbType.UniqueIdentifier },
                { typeof(Enum), SqlDbType.Int },
            };
            this._dbFieldTypeMapping = new ReadOnlyDictionary<Type, SqlDbType>(dic);
        }

        public override int? GetDbFieldType(Type type)
        {
            int? fieldType = null;
            SqlDbType t;
            if (this._dbFieldTypeMapping.TryGetValue(type, out t))
            {
                fieldType = (int)t;
            }
            return fieldType;
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, int dbFieldType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            return new SqlParameter(parameterName, (SqlDbType)dbFieldType, size, direction, isNullable, precision, scale, sourceColumn, sourceVersion, value);
        }
    }
}
