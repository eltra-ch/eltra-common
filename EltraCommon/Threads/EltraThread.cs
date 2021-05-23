using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EltraCommon.Logger;

namespace EltraCommon.Threads
{
    /// <summary>
    /// EltraThread
    /// </summary>
    public class EltraThread
    {
        #region Private fields

        private readonly ManualResetEvent _stopRequestEvent;
        private readonly ManualResetEvent _running;
        private readonly Thread _workingThread;
        private object _lock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// EltraThread
        /// </summary>
        public EltraThread()
        {
            _stopRequestEvent = new ManualResetEvent(false);
            _running = new ManualResetEvent(false);
            _workingThread = new Thread(Run);
        }

        #endregion

        #region Properties

        /// <summary>
        /// IsRunning
        /// </summary>
        public bool IsRunning => _running.WaitOne(0);

        #endregion

        #region Methods

        /// <summary>
        /// SetRunning
        /// </summary>
        protected void SetRunning()
        {
            lock (_lock)
            {
                _running.Set();
            }
        }

        /// <summary>
        /// SetStopped
        /// </summary>
        protected void SetStopped()
        {
            _running.Reset();
        }

        /// <summary>
        /// RequestStop
        /// </summary>
        protected void RequestStop()
        {
            _stopRequestEvent?.Set();
        }

        /// <summary>
        /// ShouldRun
        /// </summary>
        /// <returns></returns>
        protected bool ShouldRun()
        {
            return !_stopRequestEvent.WaitOne(0);
        }

        /// <summary>
        /// Wait
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected bool Wait(int timeout = int.MaxValue)
        {
            return _stopRequestEvent.WaitOne(timeout);
        }
        
        /// <summary>
        /// Stop
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            const int minWaitTimeMs = 10;
            
            bool result = false;
            int maxWaitTime = (int)TimeSpan.FromSeconds(60).TotalMilliseconds;

            if (IsRunning && ShouldRun())
            {                
                lock (_lock)
                {
                    RequestStop();

                    var timeout = new Stopwatch();

                    timeout.Start();

                    while (_running.WaitOne(minWaitTimeMs) && timeout.ElapsedMilliseconds < maxWaitTime)
                    {
                        Thread.Sleep(minWaitTimeMs);
                    }

                    if (timeout.ElapsedMilliseconds > maxWaitTime)
                    {
                        MsgLogger.WriteError($"{GetType().Name}::Stop", "stop thread timeout");
                    }
                    else
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        private void Run()
        {
            const int minDelay = 1;
            const int maxErrorCount = 100;

            SetRunning();

            int errorCount = 0;

            while (ShouldRun())
            {
                Task.Run(async ()=> {
                    
                    try
                    {
                        await Execute();

                        errorCount = 0;
                    }
                    catch (Exception e)
                    {
                        MsgLogger.Exception($"{GetType().Name} - Run", e);
                        errorCount++;
                    }

                    await Task.Delay(minDelay);

                    if(errorCount > maxErrorCount)
                    {
                        MsgLogger.WriteError($"{GetType().Name} - Run", $"Stop processing, max error count {maxErrorCount} reached!");

                        RequestStop();
                    }

                }).Wait();
            }

            SetStopped();
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <returns></returns>
        protected virtual Task Execute()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Start
        /// </summary>
        public virtual void Start()
        {
            int minWaitTime = 1;

            if (!IsRunning)
            {
                _workingThread.Start();

                while (!IsRunning)
                {
                    Thread.Sleep(minWaitTime);
                }
            }            
        }

        /// <summary>
        /// StartAsync
        /// </summary>
        /// <returns></returns>
        public Task StartAsync()
        {
            Task result = null;

            if (!IsRunning)
            {
                result = Task.Run(() =>
                {
                    Run();
                });
            }

            return result;
        }

        /// <summary>
        /// Restart
        /// </summary>
        public void Restart()
        {
            Stop();

            Start();
        }

        #endregion
    }
}
