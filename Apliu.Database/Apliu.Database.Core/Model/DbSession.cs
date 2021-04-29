using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace Apliu.Database.Core.Model
{
    public abstract class DbSession : IDbSession
    {
        /// <summary>
        /// 最小连接数
        /// </summary>
        public const int MIN_POOL_COUNT = 3;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public const int MAX_POOL_COUNT = 10;
        /// <summary>
        /// 连接等待时间（秒）
        /// </summary>
        public const int CONNECTION_TIMEOUT = 10;
        /// <summary>
        /// 连接生命周期（秒）
        /// </summary>
        public const int CONNECTION_LIFETIME = 120;

        private readonly DatabaseType _type;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType Type { get { return this._type; } }

        private readonly string _ip;
        public string Ip { get { return this._ip; } }

        private readonly string _port;
        public string Port { get { return this._port; } }

        private readonly string _dbName;
        public string DbName { get { return this._dbName; } }

        private readonly string _userName;
        public string UserName { get { return this._userName; } }

        private readonly string _password;
        public string Password { get { return this._password; } }

        private int _minPool;
        /// <summary>
        /// 最小连接数
        /// </summary>
        public int MinPool { get { return this._minPool; } }

        private int _maxPool;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxPool { get { return this._maxPool; } }

        private int _connectionTimeout;
        /// <summary>
        /// 连接等待时间（秒）
        /// </summary>
        public int ConnectionTimeout { get { return this._connectionTimeout; } }

        private int _connectionLifetime;
        /// <summary>
        /// 连接的生命周期（秒）
        /// </summary>
        public int ConnectionLifetime { get { return this._connectionLifetime; } }

        private string _connectionString;
        public string ConnectionString { get { return this._connectionString; } }

        /// <summary>
        /// 数据库会话信息
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionString">连接字符串</param>
        protected DbSession(DatabaseType dbType, string ip, string port, string dbName, string userName, string password)
        {
            this._type = dbType;
            this._ip = ip;
            this._port = port;
            this._dbName = dbName;
            this._userName = userName;
            this._password = password;
            this._minPool = MIN_POOL_COUNT;
            this._maxPool = MAX_POOL_COUNT;
            this._connectionTimeout = CONNECTION_TIMEOUT;
            this._connectionLifetime = CONNECTION_LIFETIME;

            this._connectionString = this.CreateConnectionString();
        }

        public void ResetMinPool(int minPool)
        {
            this._minPool = minPool;
            this._connectionString = this.CreateConnectionString();
        }

        public void ResetMaxPool(int maxPool)
        {
            this._maxPool = maxPool;
            this._connectionString = this.CreateConnectionString();
        }

        public void ResetConnectionTimeout(int connectionTimeout)
        {
            this._connectionTimeout = connectionTimeout;
            this._connectionString = this.CreateConnectionString();
        }

        public void ResetConnectionLifetime(int connectionLifetime)
        {
            this._connectionLifetime = connectionLifetime;
            this._connectionString = this.CreateConnectionString();
        }

        /// <summary>
        /// 获取数据库连接串
        /// </summary>
        /// <param name="beginConnectionStr"></param>
        /// <returns></returns>
        protected abstract string CreateConnectionString();

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection CreateConnection();

        public void Dispose()
        {
        }
    }
}
