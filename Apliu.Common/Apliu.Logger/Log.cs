using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"config/log4net.config", Watch = true)]
namespace Apliu.Logger
{
    public class Log : ILog
    {
        private readonly static ILog DEFAULT;
        public static ILog Default { get { return DEFAULT; } }

        static Log()
        {
            var file = Path.Combine(AppContext.BaseDirectory, "config", "log4net.config");
            if (!File.Exists(file))
            {
                Console.WriteLine("未找到log4net配置文件：" + file);
                Debug.WriteLine("未找到log4net配置文件：" + file);
                throw new FileNotFoundException("未找到log4net配置文件", file);
            }

            //var conf = File.ReadAllText(file);
            //XmlDocument xml = new XmlDocument();
            //try
            //{
            //    xml.LoadXml(conf);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("log4net配置文件格式错误", ex);
            //}

            var repository = log4net.LogManager.CreateRepository("NET5Repository");
            //log4net.Config.BasicConfigurator.Configure(repository);
            //log4net.Config.XmlConfigurator.Configure(xml.DocumentElement);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(repository, new FileInfo(file));
            DEFAULT = new Log(log4net.LogManager.GetLogger(repository.Name, AppDomain.CurrentDomain.FriendlyName));
        }

        private readonly log4net.ILog _logger;
        private readonly ConcurrentQueue<Action> _queue;
        private readonly Thread _thread;

        private Log(log4net.ILog logger)
        {
            this._logger = logger;
            this._queue = new ConcurrentQueue<Action>();
            this._thread = new Thread(this.OnWriting);
            this._thread.IsBackground = true;
            this._thread.Start();
        }

        private void Enqueue(Action action)
        {
            //Debug模式直接实时打印日志, 避免调试的时候暂停每次都是记录日志线程
#if DEBUG
            action.Invoke();
            return;
#else
            this._queue.Enqueue(action);
#endif
        }

        private void OnWriting()
        {
#if DEBUG
            //Debug模式直接实时打印日志, 避免调试的时候暂停每次都是记录日志线程
            return;
#endif
            while (true)
            {
                while (!this._queue.IsEmpty)
                {
                    Action action = null;
                    if (this._queue.TryDequeue(out action))
                    {
                        action();
                    }

                    Thread.Sleep(10);
                }
                Thread.Sleep(1000);
            }

        }

        void ILog.Debug(object message)
        {
            this.Enqueue(() => this._logger.Debug(message));
        }

        void ILog.Debug(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Debug(message, exception));
        }

        void ILog.DebugFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.DebugFormat(format, args));
        }

        void ILog.Info(object message)
        {
            this.Enqueue(() => this._logger.Info(message));
        }

        void ILog.Info(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Info(message, exception));
        }

        void ILog.InfoFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.InfoFormat(format, args));
        }

        void ILog.Warn(object message)
        {
            this.Enqueue(() => this._logger.Warn(message));
        }

        void ILog.Warn(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Warn(message, exception));
        }

        void ILog.WarnFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.WarnFormat(format, args));
        }

        void ILog.Error(object message)
        {
            this.Enqueue(() => this._logger.Error(message));
        }

        void ILog.Error(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Error(message, exception));
        }

        void ILog.ErrorFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.ErrorFormat(format, args));
        }

        void ILog.Fatal(object message)
        {
            this.Enqueue(() => this._logger.Fatal(message));
        }

        void ILog.Fatal(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Fatal(message, exception));
        }

        void ILog.FatalFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.FatalFormat(format, args));
        }
    }
}
