using Apliu.Database.Model;
using System.Data.Common;
using System.Data.SqlClient;

namespace Apliu.Database.SqlServer
{
    public sealed class SqlServerSession : DbSession
    {
        public const string DEFAULT_PORT = "1433";

        /// <summary>
        /// 数据库会话信息
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerSession(string ip, string port, string dbName, string userName, string password)
            : base(DbType.SqlServer, ip, port, dbName, userName, password)
        {
        }

        public override DbConnection CreateConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        protected override string CreateConnectionString()
        {
            var port = string.IsNullOrEmpty(this.Port) ? DEFAULT_PORT : this.Port;
            string dbString = string.Format("Data Source={0},{1};Initial Catalog={2};User ID={3};Password={4};", this.Ip, port, this.DbName, this.UserName, this.Password);
            dbString = dbString + "Min Pool Size=" + MinPool + ";"
                                + "Max Pool Size=" + MaxPool + ";"
                                + "Connect Timeout=" + this.ConnectionTimeout + ";"
                                + "Connection Lifetime=" + this.ConnectionLifetime + ";"
                                + "Pooling =True;";

            return dbString;
        }
    }
}
