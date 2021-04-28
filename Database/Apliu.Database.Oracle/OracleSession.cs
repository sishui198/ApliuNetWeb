using Apliu.Database.Model;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

namespace Apliu.Database.Oracle
{
    public sealed class OracleSession : DbSession
    {
        public const string DEFAULT_PORT = "1521";

        /// <summary>
        /// 数据库会话信息
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionString">连接字符串</param>
        public OracleSession(string ip, string port, string dbName, string userName, string password)
            : base(DbType.Oracle, ip, port, dbName, userName, password)
        {
        }

        public override DbConnection CreateConnection()
        {
            return new OracleConnection(this.ConnectionString);
        }

        protected override string CreateConnectionString()
        {
            var port = string.IsNullOrEmpty(this.Port) ? DEFAULT_PORT : this.Port;
            string dbString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};", this.Ip, port, this.DbName, this.UserName, this.Password);
            dbString = dbString + "MIN POOL SIZE=" + this.MinPool + ";"
                                + "MAX POOL SIZE=" + this.MaxPool + ";"
                                + "CONNECTION TIMEOUT=" + this.ConnectionTimeout + ";"
                                + "CONNECTION LIFETIME=" + this.ConnectionLifetime + ";"
                                + "POOLING=True;";

            return dbString;
        }
    }
}
