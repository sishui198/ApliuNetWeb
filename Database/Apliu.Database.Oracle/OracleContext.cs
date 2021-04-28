using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.Common;

namespace Apliu.Database.Oracle
{
    [DbType(DbType.Oracle)]
    public sealed class OracleContext : DbContext
    {
        public OracleContext(string ip, string port, string dbName, string userName, string password)
            : base(new OracleSession(ip, port, dbName, userName, password), new OracleDbParameter())
        {
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public override int BulkCopy(DataTable dataTable, int timeout)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException("DataTable");
            }
            if (dataTable.Rows.Count == 0)
            {
                throw new ArgumentOutOfRangeException("DataTable");
            }
            if (string.IsNullOrWhiteSpace(dataTable.TableName))
            {
                throw new ArgumentNullException("TableName");
            }
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("Timeout");
            }

            int affected = -1;
            try
            {
                using (DbConnection dbConnection = new OracleConnection(this.DbSession.ConnectionString))
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    using (OracleBulkCopy bulkCopy = new OracleBulkCopy(dbConnection as OracleConnection, OracleBulkCopyOptions.UseInternalTransaction))
                    {
                        bulkCopy.BatchSize = 100000;
                        bulkCopy.BulkCopyTimeout = 260;
                        bulkCopy.DestinationTableName = dataTable.TableName;
                        //每一批次中的行数
                        bulkCopy.BatchSize = dataTable.Rows.Count;
                        bulkCopy.WriteToServer(dataTable);
                        affected = dataTable.Rows.Count;
                    }
                }
            }
            catch
            {
                throw;
            }

            return affected;
        }

        protected override DbDataAdapter CreateDataAdapter(DbCommand dbCommand)
        {
            return new OracleDataAdapter(dbCommand as OracleCommand);
        }
    }
}
