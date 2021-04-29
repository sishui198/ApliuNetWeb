using Apliu.Database.Core;
using Apliu.Database.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;

namespace Apliu.Database.OleDb
{
    public class OleDbParameter : DbParameter
    {
        private readonly ReadOnlyDictionary<Type, OleDbType> _dbFieldTypeMapping;

        public OleDbParameter()
            : base(DatabaseType.OleDb)
        {
            var dic = new Dictionary<Type, OleDbType>()
            {
                { typeof(byte), OleDbType.TinyInt },
                { typeof(short), OleDbType.SmallInt },
                { typeof(int), OleDbType.Integer },
                { typeof(long), OleDbType.BigInt },
                { typeof(float), OleDbType.Single },
                { typeof(double), OleDbType.Double },
                { typeof(decimal), OleDbType.Decimal },
                { typeof(bool), OleDbType.Boolean },
                { typeof(DateTime), OleDbType.Date },
                { typeof(string), OleDbType.VarChar },
                { typeof(Guid), OleDbType.Guid },
                { typeof(Enum), OleDbType.Integer },
            };
            this._dbFieldTypeMapping = new ReadOnlyDictionary<Type, OleDbType>(dic);
        }

        public override int? GetDbFieldType(Type type)
        {
            int? fieldType = null;
            OleDbType t;
            if (this._dbFieldTypeMapping.TryGetValue(type, out t))
            {
                fieldType = (int)t;
            }
            return fieldType;
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, object value)
        {
            return new System.Data.OleDb.OleDbParameter(parameterName, value);
        }

        public override System.Data.Common.DbParameter MakeParam(string parameterName, int dbFieldType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            return new System.Data.OleDb.OleDbParameter(parameterName, (OleDbType)dbFieldType, size, direction, isNullable, precision, scale, sourceColumn, sourceVersion, value);
        }
    }
}
