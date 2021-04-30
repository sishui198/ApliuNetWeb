using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Apliu.Logger
{
    public class Log
    {
        private readonly static ILogger DEFAULT;
        public static ILogger Default { get { return DEFAULT; } }

        static Log()
        {
            var file = Path.Combine("Config", "log4net.config");
            if (!File.Exists(file))
                throw new FileNotFoundException("未找到log4net配置文件");

            var conf = File.ReadAllText(file);
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.LoadXml(conf);
            }
            catch (Exception ex)
            {
                throw new Exception("log4net配置文件格式错误", ex);
            }


            var loggerRepository = log4net.LogManager.CreateRepository("NETRepositoryObject");
            log4net.Config.XmlConfigurator.Configure(xml.DocumentElement);
            DEFAULT = new Logger(log4net.LogManager.GetLogger(loggerRepository.Name, typeof(object)));
        }

        private Log() { }
    }
}
