using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Apliu.Logger
{
    class Logger : ILogger
    {
        private readonly log4net.ILog _logger;
        private readonly ConcurrentQueue<Action> _queue;
        private readonly Thread _thread;

        internal Logger(log4net.ILog logger)
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

        void ILogger.Debug(object message)
        {
            this.Enqueue(() => this._logger.Debug(message));
        }

        void ILogger.Debug(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Debug(message, exception));
        }

        void ILogger.DebugFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.DebugFormat(format, args));
        }

        void ILogger.Info(object message)
        {
            this.Enqueue(() => this._logger.Info(message));
        }

        void ILogger.Info(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Info(message, exception));
        }

        void ILogger.InfoFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.InfoFormat(format, args));
        }

        void ILogger.Warn(object message)
        {
            this.Enqueue(() => this._logger.Warn(message));
        }

        void ILogger.Warn(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Warn(message, exception));
        }

        void ILogger.WarnFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.WarnFormat(format, args));
        }

        void ILogger.Error(object message)
        {
            this.Enqueue(() => this._logger.Error(message));
        }

        void ILogger.Error(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Error(message, exception));
        }

        void ILogger.ErrorFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.ErrorFormat(format, args));
        }

        void ILogger.Fatal(object message)
        {
            this.Enqueue(() => this._logger.Fatal(message));
        }

        void ILogger.Fatal(object message, Exception exception)
        {
            this.Enqueue(() => this._logger.Fatal(message, exception));
        }

        void ILogger.FatalFormat(string format, params object[] args)
        {
            this.Enqueue(() => this._logger.FatalFormat(format, args));
        }
    }
}
