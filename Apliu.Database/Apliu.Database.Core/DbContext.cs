using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Apliu.Database.Core.Extensions;

namespace Apliu.Database.Core
{
    public abstract class DbContext : IDbContext
    {
        /// <summary>
        /// 执行脚本超时时间
        /// </summary>
        public const int COMMAND_TIMEOUT = 30;

        private static readonly IReadOnlyDictionary<DatabaseType, Type> _dbContextTypes;
        static DbContext()
        {
            var dbTypes = new Dictionary<DatabaseType, Type>();
            string dbDllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePaths = Directory.GetFiles(dbDllPath, "Apliu.Database.*.dll", SearchOption.TopDirectoryOnly);
            var itype = typeof(IDbContext);
            foreach (var filePath in filePaths)
            {
                var assembly = Assembly.LoadFile(filePath);
                foreach (var type in assembly.GetTypes())
                {
                    if (itype.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                    {
                        var dbAttr = type.GetCustomAttribute<DatabaseTypeAttribute>();
                        if (dbAttr != null)
                        {
                            dbTypes.Add(dbAttr.DbType, type);
                        }
                    }
                }
            }
            _dbContextTypes = new ReadOnlyDictionary<DatabaseType, Type>(dbTypes);
        }

        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据库上下文</returns>
        public static IDbContext CreateDbContext(DatabaseType dbType, string ip, string port, string dbName, string userName, string password)
        {
            IDbContext dbContext = null;
            Type contextType;
            if (_dbContextTypes.TryGetValue(dbType, out contextType))
            {
                dbContext = Activator.CreateInstance(contextType, ip, port, dbName, userName, password) as IDbContext;
            }
            return dbContext;
        }

        private readonly IDbSession _dbSession;
        public IDbSession DbSession { get { return this._dbSession; } }

        private readonly IDbParameter _dbParameter;
        public IDbParameter DbParameter { get { return this._dbParameter; } }

        protected DbContext(IDbSession dbSession, IDbParameter dbParameter)
        {
            this._dbSession = dbSession;
            this._dbParameter = dbParameter;
        }

        #region 数据库事务相关处理
        /// <summary>
        /// 数据库事务范围
        /// </summary>
        private TransactionScope transactionScope = null;

        /// <summary>
        /// 开启数据库事务
        /// </summary>
        /// <param name="seconds">事务超时时间 单位秒</param>
        public void BeginTrans(int seconds)
        {
            TransactionOptions transactionOption = new TransactionOptions
            {
                //设置事务隔离级别
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                // 设置事务超时时间
                Timeout = new TimeSpan(0, 0, seconds)
            };

            transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOption);

            //当时间超过之后，主动注销该事务
            Task.Factory.StartNew(() => { Thread.Sleep(seconds * 1000); Dispose(); });
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (transactionScope != null)
            {
                transactionScope.Complete();
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        /// <summary>
        /// 撤销事务
        /// </summary>
        public void Rollback()
        {
            if (transactionScope != null)
            {
                transactionScope.Dispose();
                transactionScope = null;
            }
        }
        #endregion

        public int Execute(string sql, object parameters = null)
        {
            return Execute(sql, parameters.GetPropertyValues());
        }

        public int Execute(string sql, IDictionary<string, object> parameters)
        {
            return Execute(sql, COMMAND_TIMEOUT, parameters);
        }

        public int Execute(string sql, int timeout, object parameters = null)
        {
            return Execute(sql, timeout, parameters.GetPropertyValues());
        }

        public int Execute(string sql, int timeout, IDictionary<string, object> parameters)
        {
            var ps = this.DbParameter.MakeParam(parameters);
            return Execute(sql, timeout, CommandType.Text, ps);
        }

        public virtual int Execute(string text, int timeout, CommandType type, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("CommandText");
            }
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("CommandTimeout");
            }
            if (!this.IsSupportTableDirect() && type == CommandType.TableDirect)
            {
                throw new ArgumentOutOfRangeException("CommandType");
            }

            int affected = -1;
            try
            {
                using (DbConnection dbConnection = this.DbSession.CreateConnection())
                {
                    if (dbConnection.State != ConnectionState.Open)
                        dbConnection.Open();

                    using (DbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = text;
                        dbCommand.CommandTimeout = timeout;
                        dbCommand.CommandType = type;
                        if (parameters != null)
                        {
                            foreach (DbParameter parm in parameters)
                                dbCommand.Parameters.Add(parm);
                        }
                        affected = dbCommand.ExecuteNonQuery();
                        dbConnection.Close();
                    }
                }
            }
            catch
            {
                throw;
            }

            return affected;
        }

        public T[] Query<T>(string sql, object parameters = null)
            where T : class, new()
        {
            return Query<T>(sql, parameters.GetPropertyValues());
        }

        public T[] Query<T>(string sql, IDictionary<string, object> parameters)
            where T : class, new()
        {
            return Query<T>(sql, COMMAND_TIMEOUT, parameters);
        }

        public T[] Query<T>(string sql, int timeout, object parameters = null)
            where T : class, new()
        {
            return Query<T>(sql, timeout, parameters.GetPropertyValues());
        }

        public T[] Query<T>(string sql, int timeout, IDictionary<string, object> parameters)
            where T : class, new()
        {
            var ps = this.DbParameter.MakeParam(parameters);
            return Query<T>(sql, timeout, CommandType.Text, ps);
        }

        public T[] Query<T>(string text, int timeout, CommandType type, params DbParameter[] parameters)
            where T : class, new()
        {
            var ds = Query(text, timeout, type, parameters);
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            else
            {
                return ds.Tables[0].ToArray<T>();
            }
        }

        public DataTable Query(string sql, object parameters = null)
        {
            return Query(sql, parameters.GetPropertyValues());
        }

        public DataTable Query(string sql, IDictionary<string, object> parameters)
        {
            return Query(sql, COMMAND_TIMEOUT, parameters);
        }

        public DataTable Query(string sql, int timeout, object parameters = null)
        {
            return Query(sql, timeout, parameters.GetPropertyValues());
        }

        public DataTable Query(string sql, int timeout, IDictionary<string, object> parameters)
        {
            var ps = this.DbParameter.MakeParam(parameters);
            var ds = Query(sql, timeout, CommandType.Text, ps);
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            else return ds.Tables[0];
        }

        public virtual DataSet Query(string text, int timeout, CommandType type, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("CommandText");
            }
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("CommandTimeout");
            }
            if (!this.IsSupportTableDirect() && type == CommandType.TableDirect)
            {
                throw new ArgumentOutOfRangeException("CommandType");
            }

            DataSet dsData = null;
            try
            {
                using (DbConnection dbConnection = this.DbSession.CreateConnection())
                {
                    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
                    using (DbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = text;
                        dbCommand.CommandTimeout = timeout;
                        dbCommand.CommandType = type;
                        if (parameters != null)
                        {
                            //sqlCommand.Parameters.Clear();
                            foreach (DbParameter parm in parameters)
                                dbCommand.Parameters.Add(parm);
                        }
                        DbDataAdapter da = this.CreateDataAdapter(dbCommand);
                        dsData = new DataSet();
                        da.Fill(dsData);
                    }
                }
            }
            catch
            {
                throw;
            }

            return dsData;
        }

        public int BulkCopy<T>(string tableName, IEnumerable<T> source)
            where T : class
        {
            return BulkCopy(tableName, source, COMMAND_TIMEOUT);
        }

        public int BulkCopy<T>(string tableName, IEnumerable<T> source, int timeout)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException("Source");
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("TableName");
            }
            if (timeout < 0)
            {
                throw new ArgumentOutOfRangeException("Timeout");
            }
            var dataTable = source.ToDataTable(tableName);
            return BulkCopy(dataTable, timeout);
        }

        public int BulkCopy(DataTable source)
        {
            return BulkCopy(source, COMMAND_TIMEOUT);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public abstract int BulkCopy(DataTable dataTable, int timeout);

        protected abstract DbDataAdapter CreateDataAdapter(DbCommand dbCommand);

        /// <summary>
        /// 是否支持TableDirect CommandType模式, 目前应该仅当OleDb Framework平台才支持
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsSupportTableDirect()
        {
            return false;
        }

        public void Dispose()
        {
        }
    }
}
