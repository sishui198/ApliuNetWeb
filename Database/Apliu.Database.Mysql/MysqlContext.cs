using Apliu.Database.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.IO;


namespace Apliu.Database.Mysql
{
    [DbType(DbType.Mysql)]
    public sealed class MysqlContext : DbContext
    {
        public MysqlContext(string ip, string port, string dbName, string userName, string password)
            : base(new MysqlSession(ip, port, dbName, userName, password), new MysqlParameter())
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
                using (DbConnection dbConnection = new MySqlConnection(this.DbSession.ConnectionString))
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    //TODO：需要更换临时目录地址
                    string tempFilePath = Path.GetTempFileName();
                    File.WriteAllText(tempFilePath, dataTable.ToCsv());

                    MySqlBulkLoader mySqlBulkLoader = new MySqlBulkLoader(dbConnection as MySqlConnection);
                    mySqlBulkLoader.FieldTerminator = ",";
                    mySqlBulkLoader.FieldQuotationCharacter = '"';
                    mySqlBulkLoader.EscapeCharacter = '"';
                    mySqlBulkLoader.LineTerminator = "\r\n";
                    mySqlBulkLoader.FileName = tempFilePath;
                    mySqlBulkLoader.NumberOfLinesToSkip = 0;
                    mySqlBulkLoader.Timeout = timeout;
                    mySqlBulkLoader.TableName = dataTable.TableName;
                    affected = mySqlBulkLoader.Load();

                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
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
            return new MySqlDataAdapter(dbCommand as MySqlCommand);
        }
    }
}
