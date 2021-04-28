using Apliu.Database.Model;
using MySql.Data.MySqlClient;
using System.Data.Common;


namespace Apliu.Database.Mysql
{
    public sealed class MysqlSession : DbSession
    {
        public const string DEFAULT_PORT = "3306";

        /// <summary>
        /// 数据库会话信息
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionString">连接字符串</param>
        public MysqlSession(string ip, string port, string dbName, string userName, string password)
            : base(DbType.Mysql, ip, port, dbName, userName, password)
        {
        }

        public override DbConnection CreateConnection()
        {
            return new MySqlConnection(this.ConnectionString);
        }

        protected override string CreateConnectionString()
        {
            var port = string.IsNullOrEmpty(this.Port) ? DEFAULT_PORT : this.Port;
            string dbString = string.Format("Server={0};Port={1};Database={2};User ID={3};Password={4};", this.Ip, port, this.DbName, this.UserName, this.Password);
            dbString = dbString + "minpoolsize=" + this.MinPool + ";"
                           + "maxpoolsize=" + this.MaxPool + ";"
                           + "connectiontimeout=" + this.ConnectionTimeout + ";"
                           + "connectionlifetime=" + this.ConnectionLifetime + ";"
                           + "pooling=True;SslMode = none;";

            return dbString;
        }
    }
}
