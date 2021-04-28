using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Apliu.Database.OleDb
{
    [DbType(DbType.OleDb)]
    public sealed class OleDbContext : DbContext
    {
        public OleDbContext(string ip, string port, string dbName, string userName, string password)
            : base(new OleDbSession(ip, port, dbName, userName, password), new OleDbParameter())
        {
#if NET20 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET49

#else
            throw new NotSupportedException("必须是.NET Framework框架");
#endif
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override int BulkCopy(DataTable dataTable, int timeout)
        {
            throw new NotSupportedException("OleDb");
        }

        protected override DbDataAdapter CreateDataAdapter(DbCommand dbCommand)
        {
            return new OleDbDataAdapter(dbCommand as OleDbCommand);
        }

        protected override bool IsSupportTableDirect()
        {
            return true;
        }
    }
}
