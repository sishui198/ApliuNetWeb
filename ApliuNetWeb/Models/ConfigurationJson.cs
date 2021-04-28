﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Apliu.Standard.Tools;
using System.Linq;

namespace ApliuCoreWeb.Models
{
    /// <summary>
    /// Json配置文件信息
    /// </summary>
    public class ConfigurationJson
    {
        /// <summary>
        /// 网站域名
        /// </summary>
        public static String Domain { get; set; }

        /// <summary>
        /// 静态对象锁
        /// </summary>
        private readonly static Object objectLockIsUseHttps = new Object();
        private static Boolean? _IsUseHttps;
        /// <summary>
        /// 是否开启HTTPS重定向服务
        /// </summary>
        public static Boolean IsUseHttps
        {
            get
            {
                if (_IsUseHttps == null)
                {
                    lock (objectLockIsUseHttps)
                    {
                        if (_IsUseHttps == null)
                        {
                            _IsUseHttps = GetSetting("IsUseHttps").ToBoolean();
                        }
                    }
                }
                return _IsUseHttps == true;
            }
        }

        /// <summary>
        /// 业务数据库类型: SqlServer / Oracle / MySql
        /// </summary>
        public static String DatabaseType { get; set; }
        /// <summary>
        /// 业务数据库连接字符串
        /// </summary>
        public static String DatabaseIp { get; set; }
        public static String DatabaseName { get; set; }
        public static String DatabaseUserName { get; set; }
        public static String DatabasePassword { get; set; }

        public static string AllEncodingAESKey { get; private set; }

        /// <summary>
        /// userdefined.json 中的Appsetting节点信息
        /// </summary>
        public static Appsettings Appsettings = new Appsettings();

        /// <summary>
        /// 静态对象锁
        /// </summary>
        private readonly static Object objectLockHostUrl = new Object();
        private static HostUrl _HostUrl;
        /// <summary>
        /// 网站访问URL
        /// </summary>
        public static HostUrl HostUrl
        {
            get
            {
                if (_HostUrl == null)
                {
                    lock (objectLockHostUrl)
                    {
                        if (_HostUrl == null)
                        {
                            _HostUrl = GetSetting<HostUrl>("HostUrl");
                        }
                    }
                }
                return _HostUrl;
            }
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public static void LoadConfig()
        {
            Appsettings = GetSetting<Appsettings>("Appsettings");
            Domain = GetSetting("Domain");
            AllEncodingAESKey = File.ReadAllText("Config/key.txt");
            DatabaseType = GetSetting("DatabaseType");
            DatabaseIp = GetSetting("DatabaseIp");
            DatabaseName = GetSetting("DatabaseName");
            DatabaseUserName = GetSetting("DatabaseUserName");
            DatabasePassword = GetSetting("DatabasePassword");
        }

        /// <summary>
        /// 设置并获取配置节点对象 var c =SetConfig<Cad>("Cad", (p => p.b = "123"));
        /// </summary>  
        public static T SetConfig<T>(String key, Action<T> action, String fileName = "userdefined.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(fileName, optional: true, reloadOnChange: true)
               .Build();
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .Configure<T>(action)
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }

        /// <summary>
        /// 获取配置节点对象 var result =GetSetting<Logging>("Logging");
        /// </summary>   
        public static T GetSetting<T>(String key, String fileName = "userdefined.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })
               .Build();
            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }

        /// <summary>
        /// 获取指定Json文件中节点的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static String GetSetting(String key, String fileName = "userdefined.json")
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })
               .Build();
            String value = config.GetSection(key).Value?.ToString();

            return value;
        }
    }

    public class Appsettings
    {
        public String TestDatabaseType { get; set; }
        public String TestDatabaseConnection { get; set; }
        public String TestDatabaseIp { get; set; }
        public String TestDatabaseName { get; set; }
        public String TestDatabaseUserName { get; set; }
        public String TestDatabasePassword { get; set; }

        /// <summary>
        /// Session加密密钥
        /// </summary>
        public String SessionSecurityKey { get; set; }
        /// <summary>
        /// 网站域名
        /// </summary>
        public String WxDomain { get; set; }
        /// <summary>
        /// 微信公众号Id
        /// </summary>
        public String WxAppId { get; set; }
        /// <summary>
        /// 微信公众号密钥
        /// </summary>
        public String WxAppSecret { get; set; }
        /// <summary>
        /// 微信公众号Token
        /// </summary>
        public String WxToken { get; set; }
        /// <summary>
        ///已过期，由微信公众号设置决定是否启用加密
        /// </summary>
        public String IsSecurity { get; set; }
        /// <summary>
        /// 微信公众号密文消息密钥
        /// </summary>
        public String WxEncodingAESKey { get; set; }
        /// <summary>
        /// 腾讯云应用Id
        /// </summary>
        public String TcAppId { get; set; }
        /// <summary>
        /// 腾讯云应用密钥Id
        /// </summary>
        public String TcSecretId { get; set; }
        /// <summary>
        /// 腾讯云应用密钥Key
        /// </summary>
        public String TcSecretKey { get; set; }
        /// <summary>
        /// SDK AppID是短信应用的唯一标识，调用短信API接口时需要提供该参数
        /// </summary>
        public String TcSMSAppId { get; set; }
        /// <summary>
        /// 用来校验短信发送请求合法性的密码，与SDK AppID对应
        /// </summary>
        public String TcSMSAppKey { get; set; }
    }

    public class UserConnectionString
    {
        public String SqlServer { get; set; }
        public String Oracle { get; set; }
        public String MySql { get; set; }
    }
}
