using System;
using System.Data.Common;


namespace Apliu.Database.Core
{
    public interface IDbSession : IDisposable
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        DatabaseType Type { get; }
        string Ip { get; }
        string Port { get; }
        string DbName { get; }
        string UserName { get; }
        string Password { get; }
        /// <summary>
        /// 最小连接数
        /// </summary>
        int MinPool { get; }
        /// <summary>
        /// 最大连接数
        /// </summary>
        int MaxPool { get; }
        /// <summary>
        /// 连接等待时间（秒）
        /// </summary>
        int ConnectionTimeout { get; }
        /// <summary>
        /// 连接的生命周期（秒）
        /// </summary>
        int ConnectionLifetime { get; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectionString { get; }
        void ResetMinPool(int minPool);
        void ResetMaxPool(int maxPool);
        void ResetConnectionTimeout(int connectionTimeout);
        void ResetConnectionLifetime(int connectionLifetime);
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        DbConnection CreateConnection();
    }
}
