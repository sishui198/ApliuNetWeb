using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Apliu.Database
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// 会话信息
        /// </summary>
        IDbSession DbSession { get; }
        /// <summary>
        /// 参数构造器
        /// </summary>
        IDbParameter DbParameter { get; }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="timeout">超时时间</param>
        void BeginTrans(int timeout);
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
        T[] Query<T>(string sql, object parameters = null) where T : class, new();
        T[] Query<T>(string sql, IDictionary<string, object> parameters) where T : class, new();
        T[] Query<T>(string sql, int timeout, object parameters = null) where T : class, new();
        T[] Query<T>(string sql, int timeout, IDictionary<string, object> parameters) where T : class, new();
        T[] Query<T>(string sql, int timeout, CommandType type, params DbParameter[] parameters) where T : class, new();
        DataTable Query(string sql, object parameters = null);
        DataTable Query(string sql, IDictionary<string, object> parameters);
        DataTable Query(string sql, int timeout, object parameters = null);
        DataTable Query(string sql, int timeout, IDictionary<string, object> parameters);
        DataSet Query(string sql, int timeout, CommandType type, params DbParameter[] parameters);
        int Execute(string sql, object parameters = null);
        int Execute(string sql, IDictionary<string, object> parameters);
        int Execute(string sql, int timeout, object parameters = null);
        int Execute(string sql, int timeout, IDictionary<string, object> parameters);
        int Execute(string text, int timeout, CommandType type, params DbParameter[] parameters);
        int BulkCopy<T>(string tableName, IEnumerable<T> source) where T : class;
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        int BulkCopy<T>(string tableName, IEnumerable<T> source, int timeout) where T : class;
        int BulkCopy(DataTable source);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        int BulkCopy(DataTable source, int timeout);
    }
}
