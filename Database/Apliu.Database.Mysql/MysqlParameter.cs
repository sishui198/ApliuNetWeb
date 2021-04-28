using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apliu.Database.Model;
using MySql.Data.MySqlClient;

namespace Apliu.Database.Mysql
{
    public class MysqlParameter : DbParameter
    {
        private readonly ReadOnlyDictionary<Type, MySqlDbType> _dbFieldTypeMapping;

        public MysqlParameter()
            : base(DbType.Mysql)
        {
            var dic = new Dictionary<Type, MySqlDbType>()
            {
                { typeof(byte), MySqlDbType.Byte },
                { typeof(short), MySqlDbType.Int16 },
                { typeof(int), MySqlDbType.Int32 },
                { typeof(long), MySqlDbType.Int64 },
                { typeof(float), MySqlDbType.Float },
                { typeof(double), MySqlDbType.Double },
                { typeof(decimal), MySqlDbType.Decimal },
                { typeof(bool), MySqlDbType.Bit },
                { typeof(DateTime), MySqlDbType.DateTime },
                { typeof(string), MySqlDbType.String },
                { typeof(Guid), MySqlDbType.Guid },
                { typeof(Enum), MySqlDbType.Enum },
            };
            this._dbFieldTypeMapping = new ReadOnlyDictionary<Type, MySqlDbType>(dic);
        }

        public override int? GetDbFieldType(Type type)
        {
            int? fieldType = null;
            MySqlDbType t;
            if (this._dbFieldTypeMapping.TryGetValue(type, out t))
            {
                fieldType = (int)t;
            }
            return fieldType;
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, object value)
        {
            return new MySql.Data.MySqlClient.MySqlParameter(parameterName, value);
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, int dbFieldType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            return new MySql.Data.MySqlClient.MySqlParameter(parameterName, (MySqlDbType)dbFieldType, size, direction, isNullable, precision, scale, sourceColumn, sourceVersion, value);
        }
    }
}
