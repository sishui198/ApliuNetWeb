using Apliu.Database.Core;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Apliu.Database.SqlServer
{
    [DatabaseType(DatabaseType.SqlServer)]
    public sealed class SqlServerContext : DbContext
    {
        public SqlServerContext(string ip, string port, string dbName, string userName, string password)
            : base(new SqlServerSession(ip, port, dbName, userName, password), new SqlServerParameter())
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
                using (DbConnection dbConnection = new SqlConnection(this.DbSession.ConnectionString))
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();
                    
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(dbConnection as SqlConnection))
                    {
                        sqlBulkCopy.BatchSize = 2000;
                        sqlBulkCopy.BulkCopyTimeout = timeout;
                        sqlBulkCopy.DestinationTableName = dataTable.TableName;
                        //sqlBulkCopy.SqlRowsCopied += SqlBulkCopy_SqlRowsCopied;
                        sqlBulkCopy.WriteToServer(dataTable);
                        affected = dataTable.Rows.Count;
                        sqlBulkCopy.Close();
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
            return new SqlDataAdapter(dbCommand as SqlCommand);
        }
    }
}
