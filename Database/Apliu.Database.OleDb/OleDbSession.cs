using Apliu.Database.Model;
using System;
using System.Data.Common;
using System.Data.OleDb;

namespace Apliu.Database.OleDb
{
    public sealed class OleDbSession : DbSession
    {
        public const string DEFAULT_PROVIDER = "Microsoft.Jet.OleDB.12.0";

        /// <summary>
        /// 数据库会话信息
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionString">连接字符串</param>
        public OleDbSession(string ip, string port, string dbName, string userName, string password)
            : base(DbType.OleDb, ip, port, dbName, userName, password)
        {
#if NET20 || NET35 || NET40 || NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48 || NET49

#else
            throw new NotSupportedException("必须是.NET Framework框架");
#endif
        }

        public override DbConnection CreateConnection()
        {
            return new OleDbConnection(this.ConnectionString);
        }

        protected override string CreateConnectionString()
        {
            var provider = string.IsNullOrEmpty(this.Ip) ? DEFAULT_PROVIDER : this.Ip;
            string dbString = string.Format("Provider={0};Data Source={1};", provider, this.DbName);
            if (!string.IsNullOrWhiteSpace(this.UserName))
            {
                dbString = dbString + string.Format("UserId ={0};Password={1};", this.UserName, this.Password);
            }
            return dbString;
        }
    }
}
