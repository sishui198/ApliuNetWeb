﻿using Apliu.Database.Core;
using Apliu.Standard.ORM;
using Apliu.Tools.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace ApliuCoreWeb.Models
{
    /// <summary>
    /// 使用Apliu.Standard.ORM进行对象/数据库映射
    /// </summary>
    public class ORM
    {
        private DataAccess DataAccess;
        public ORM(DataAccess dataAccess) { this.DataAccess = dataAccess; }

        /// <summary>
        /// 插入IModelORM 对象到数据库
        /// </summary>
        /// <param name="modelORM"></param>
        /// <returns></returns>
        public bool Insert(IModelORM modelORM)
        {
            return DataAccess.PostData(modelORM.GetInsertSql());
        }

        /// <summary>
        /// 从数据库删除IModelORM 对象
        /// </summary>
        /// <param name="modelORM"></param>
        /// <returns></returns>
        public bool Delete(IModelORM modelORM)
        {
            return DataAccess.PostData(modelORM.GetDeleteSql());
        }

        /// <summary>
        /// 在数据库中更新IModelORM 对象
        /// </summary>
        /// <param name="modelORM"></param>
        /// <returns></returns>
        public bool Update(IModelORM modelORM)
        {
            return DataAccess.PostData(modelORM.GetUpdateSql());
        }
    }
    public class DataAccess
    {
        private static Dictionary<string, DataAccess> _Instance = new Dictionary<string, DataAccess>() { };
        /// <summary>
        /// 静态锁确保不重复
        /// </summary>
        private static readonly Object objectInstanceLock = new Object();
        /// <summary>
        /// 获取默认的数据库链接对象 key=Default 从配置文件中读取
        /// </summary>
        public static DataAccess Instance
        {
            get
            {
                if (_Instance.ContainsKey("Default"))
                {
                    return _Instance["Default"];
                }
                else
                {
                    lock (objectInstanceLock)
                    {
                        if (_Instance.ContainsKey("Default")) return _Instance["Default"];
                        else
                        {
                            LoadDataAccess("Default", ConfigurationJson.DatabaseType, SecurityHelper.DESDecrypt(ConfigurationJson.DatabaseIp, ConfigurationJson.AllEncodingAESKey), "", ConfigurationJson.DatabaseName, ConfigurationJson.DatabaseUserName, SecurityHelper.DESDecrypt(ConfigurationJson.DatabasePassword, ConfigurationJson.AllEncodingAESKey));
                            return _Instance["Default"];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定Key的数据库链接对象，再使用前需要进行Load，如果找不到则返回Null
        /// </summary>
        public static ReadOnlyDictionary<string, DataAccess> InstanceKey
        {
            get
            {
                return new ReadOnlyDictionary<string, DataAccess>(_Instance);
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DbType => DbHelper.DbSession.Type;

        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private IDbContext DbHelper;

        /// <summary>
        /// IModelORM 对象/数据库映射
        /// </summary>
        public ORM ORM;

        public DataAccess(string databaseType, string ip, string port, string dbName, string userName, string password)
        {
            ORM = new ORM(this);
            DbHelper = DbContext.CreateDbContext((DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType), ip, port, dbName, userName, password);
        }

        private static readonly Object objectLoadDataAccessLock = new Object();
        /// <summary>
        /// 初始化数据库连接通道
        /// </summary>
        public static void LoadDataAccess(string instanceKey, string databaseType, string ip, string port, string dbName, string userName, string password)
        {
            try
            {
                if (!_Instance.ContainsKey(instanceKey))
                {
                    lock (objectLoadDataAccessLock)
                    {
                        if (!_Instance.ContainsKey(instanceKey))
                        {
                            DataAccess dataAccess = new DataAccess(databaseType, ip, port, dbName, userName, password);
                            //dataAccess.dbHelper.InitializtionConnection();
                            _Instance.Add(instanceKey, dataAccess);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogAsync("加载数据库配置失败，详情：" + ex.Message);
            }
        }

        /// <summary>
        /// 执行SQL语句查询数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>结果集</returns>
        public DataRow GetDataRow(string Sql)
        {
            DataRow dataRow = null;
            DataSet dataSet = GetData(Sql);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0) dataRow = dataSet.Tables[0].Rows[0];
            return dataRow;
        }

        /// <summary>
        /// 执行SQL语句查询数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>结果集</returns>
        public DataTable GetDataTable(string Sql)
        {
            DataTable dataTable = null;
            DataSet dataSet = GetData(Sql);
            if (dataSet != null && dataSet.Tables.Count > 0) dataTable = dataSet.Tables[0];
            return dataTable;
        }

        /// <summary>
        /// 执行SQL语句查询数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>结果集</returns>
        public DataSet GetData(string Sql)
        {
            return GetDataExecute(Sql, 30);
        }

        /// <summary>
        /// 执行SQL语句更新数据库
        /// </summary>
        /// <param name="Sql">Sql语句</param>
        /// <returns>受影响的行数</returns>
        public bool PostData(string Sql)
        {
            int val = PostDataExecute(Sql, 30);
            if (val >= 0) return true;
            else return false;
        }

        /// <summary>
        /// 测试用例
        /// </summary>
        [Obsolete]
        public void TestCeshi()
        {
            //string guid = Guid.NewGuid().ToString().ToLower();
            //string sql = "insert into Test values(@name1,@name2,@name3);";
            //SqlParameter[] myprams = {
            //            MakeParam("@name1" , SqlDbType.VarChar.ToString() , 50 ,guid) as SqlParameter ,
            //            MakeParam("@name2" , SqlDbType.VarChar.ToString() , 50 ,"CeshiPrams") as SqlParameter,
            //            MakeParam("@name3" , SqlDbType.VarChar.ToString() , 50 ,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) as SqlParameter
            //        };
            //int p1 = PostDataExecute(CommandType.Text, sql, 30, myprams);

            //bool p2 = PostData("update Test set Msg=Msg+'Ceshi' where ID='" + guid + "';");

            //DataSet ds = GetData("select * from Test;");
        }

        /// <summary>
        /// 执行Transact-SQL语句或存储过程，并返回查询结果
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="commandText">Sql语句或存储过程</param>
        /// <param name="commandTimeout">语句执行的超时时间</param>
        /// <param name="commandParameters">语句参数</param>
        /// <returns>返回结果集</returns>
        public DataSet GetDataExecute(string commandText, int commandTimeout)
        {
            DataSet dsData = null;
            try
            {
                dsData = DbHelper.Query(commandText, commandTimeout).DataSet;
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("数据库查询失败，Sql：" + commandText + "，详情：" + ex.Message);
                Logger.WriteLogWeb("StackTrace：" + ex.StackTrace);
            }
            return dsData;
        }

        /// <summary>
        /// 执行 Transact-SQL 语句并返回受影响的行数
        /// </summary>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <param name="commandText">Sql语句或存储过程</param>
        /// <param name="commandTimeout">语句执行的超时时间</param>
        /// <param name="commandParameters">语句参数 </param>
        /// <returns>返回受影响的行数</returns>
        public int PostDataExecute(string commandText, int commandTimeout)
        {
            int result = -1;
            try
            {
                result = DbHelper.Execute(commandText, commandTimeout);
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("数据库更新失败，Sql：" + commandText + "，详情：" + ex.Message);
                Logger.WriteLogWeb("StackTrace：" + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// 批量插入DataTable到数据库，并返回受影响的行数
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int InsertTableAsync(DataTable dataTable, Int32 timeout)
        {
            try
            {
                return DbHelper.BulkCopy(dataTable, timeout);
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb($"数据库批量插入失败，TableName：{dataTable?.TableName}，Count：{dataTable?.Rows?.Count}，详情：" + ex.Message);
                Logger.WriteLogWeb("StackTrace：" + ex.StackTrace);
                return -1;
            }
        }

        /// <summary>
        /// 开启数据库事务
        /// </summary>
        /// <param name="seconds">事务超时时间 单位秒</param>
        public void BeginTransaction(int seconds)
        {
            DbHelper.BeginTrans(seconds);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            DbHelper.Commit();
        }

        /// <summary>
        /// 撤销事务
        /// </summary>
        public void Rollback()
        {
            DbHelper.Dispose();
        }
    }
}