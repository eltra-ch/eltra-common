using EltraCommon.Helpers;
using EltraCommon.Logger.Formatter;
using System;
using System.Diagnostics;
using System.IO;

namespace EltraCommon.Logger.Output
{
    class FileLogOutput : LogOutput, ILogOutput 
    {
        #region Private fields
                
        private readonly ILogOutput _fallback;
        private readonly string defaultFilePrefix = "log";
        private readonly ILogFormatter _formatter;

        private bool _unauthorizedAccess;        
        private string _logFilePrefix;

        #endregion

        #region Constructors

        public FileLogOutput()
        {
            _formatter = new DefaultLogFormatter();
        }

        public FileLogOutput(ILogOutput fallback)
        {
            _fallback = fallback;
            _formatter = new DefaultLogFormatter();
        }

        #endregion

        #region Properties

        public string LogPath { get; set; }
        
        public string LogFilePrefix
        {
            get => _logFilePrefix ?? (_logFilePrefix = defaultFilePrefix);
            set => _logFilePrefix = value;
        }
        
        public string Name => "File";

        public ILogFormatter Formatter { get => _formatter; }

        #endregion

        #region Methods

        private bool CreateDefaultLogPath(Environment.SpecialFolder folder)
        {
            const string method = "CreateDefaultLogPath";
            bool result = false;

            try
            {
                var logPath = Environment.GetFolderPath(folder);

                result = CreateDefaultLogPath(logPath);
            }
            catch (Exception e)
            {
                _fallback?.Write($"{GetType().Name} - {method}", LogMsgType.Exception, e.Message);
            }

            return result;
        }

        private bool CreateDefaultLogPath(string basePath)
        {
            bool result = false;
            const string method = "CreateDefaultLogPath";

            try
            {
                var processName = AppHelper.GetProcessFileName(false);
                var eltraLogPath = Path.Combine(basePath, "eltra", processName);

                if (!Directory.Exists(eltraLogPath))
                {
                    Directory.CreateDirectory(eltraLogPath);
                }

                LogPath = eltraLogPath;

                result = true;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"{GetType().Name} - {method}, {LogMsgType.Exception}, unauthorized");
            }
            catch (Exception e)
            {
                _fallback?.Write($"{GetType().Name} - {method}", LogMsgType.Exception, e.Message);
            }

            return result;
        }

        private bool ValidateLogPath()
        {
            bool result = false;
            const string method = "ValidateLogPath";

            try
            {
                if (string.IsNullOrEmpty(LogPath))
                {
                    result = CreateDefaultLogPath(Environment.SpecialFolder.LocalApplicationData);

                    if (!result)
                    {
                        result = CreateDefaultLogPath(Environment.SpecialFolder.MyDocuments);
                    }

                    if (!result)
                    {
                        result = CreateDefaultLogPath(Environment.SpecialFolder.InternetCache);
                    }

                    if (!result)
                    {
                        result = CreateDefaultLogPath(Path.GetTempPath());
                    }
                }
                else if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                    
                    result = true;
                }
                else
                {
                    result = true;
                }
            }
            catch(Exception e)
            {
                _fallback?.Write($"{GetType().Name} - {method}", LogMsgType.Exception, e.Message);
            }

            return result;
        }

        public void Write(string source, LogMsgType type, string msg, bool newLine)
        {
            string formattedMsg = Formatter.Format(source, type, msg);

            if(!string.IsNullOrEmpty(formattedMsg))
            {
                if (!_unauthorizedAccess)
                {
                    if (ValidateLogPath())
                    {
                        var currentProcess = Process.GetCurrentProcess();

                        string errorLogPath = Path.Combine(LogPath, $"{LogFilePrefix}_{currentProcess.Id}.log");

                        Lock();

                        try
                        {
                            if (!formattedMsg.EndsWith(Environment.NewLine))
                            {
                                formattedMsg += Environment.NewLine;
                            }

                            File.AppendAllText(errorLogPath, formattedMsg);
                        }
                        catch (UnauthorizedAccessException e)
                        {
                            _unauthorizedAccess = true;

                            _fallback?.Write(source, LogMsgType.Exception, e.Message);
                        }
                        catch (Exception e)
                        {
                            _fallback?.Write(source, LogMsgType.Exception, e.Message);
                        }

                        Unlock();
                    }
                    else
                    {
                        _fallback.Write($"{GetType().Name} - Write", LogMsgType.Error, "log path validation failed!");
                    }
                }
            }
        }

        #endregion
    }
}
