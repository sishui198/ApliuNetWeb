using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apliu.Database.Model;
using Oracle.ManagedDataAccess.Client;

namespace Apliu.Database.Oracle
{
    public class OracleDbParameter : DbParameter
    {
        private readonly ReadOnlyDictionary<Type, OracleDbType> _dbFieldTypeMapping;

        public OracleDbParameter()
            : base(DbType.Oracle)
        {
            var dic = new Dictionary<Type, OracleDbType>()
            {
                { typeof(byte), OracleDbType.Byte },
                { typeof(short), OracleDbType.Int16 },
                { typeof(int), OracleDbType.Int32 },
                { typeof(long), OracleDbType.Int64 },
                { typeof(float), OracleDbType.BinaryFloat },
                { typeof(double), OracleDbType.BinaryDouble },
                { typeof(decimal), OracleDbType.Decimal },
                { typeof(bool), OracleDbType.Boolean },
                { typeof(DateTime), OracleDbType.Date },
                { typeof(string), OracleDbType.Varchar2 },
                { typeof(Enum), OracleDbType.Int32 },
            };
            this._dbFieldTypeMapping = new ReadOnlyDictionary<Type, OracleDbType>(dic);
        }

        public override int? GetDbFieldType(Type type)
        {
            int? fieldType = null;
            OracleDbType t;
            if (this._dbFieldTypeMapping.TryGetValue(type, out t))
            {
                fieldType = (int)t;
            }
            return fieldType;
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, object value)
        {
            return new OracleParameter(parameterName, value);
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, int dbFieldType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            return new OracleParameter(parameterName, (OracleDbType)dbFieldType, size, direction, isNullable, precision, scale, sourceColumn, sourceVersion, value);
        }
    }
}
