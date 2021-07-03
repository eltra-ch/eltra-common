using EltraCommon.Helpers;
using EltraCommon.Logger.Config;
using EltraCommon.Logger.Output;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

#pragma warning disable 1591

namespace EltraCommon.Logger
{
    public class EltraLogger : IEltraLogger
    {
        #region Private fields

        private const string ConfigFileName = "logconfig.json";

        private readonly List<ILogOutput> _logOutputs;
        private LoggerConfiguration _loggerConfiguration;
        private string _configPath;

        #endregion

        #region Constructors

        public EltraLogger()
        {
            _logOutputs = new List<ILogOutput>();
            _loggerConfiguration = new LoggerConfiguration();

            AddOutput(new ConsoleLogOutput());
            AddOutput(new DebugLogOutput());
            AddOutput(new FileLogOutput(new ConsoleLogOutput()));
        }

        #endregion

        #region Properties

        protected string ConfigPath 
        { 
            get
            {
                if (string.IsNullOrEmpty(_configPath))
                {
                    if(CreateLogConfigurationPath(out string path))
                    {
                        _configPath = path;
                    }
                }

                return _configPath;
            }
        }

        protected string ConfigFilePath => Path.Combine(ConfigPath, ConfigFileName);

        public List<string> FilterOutSources => _loggerConfiguration.FilterOutSources;

        public string Types
        {
            get => _loggerConfiguration.Types;
            set => _loggerConfiguration.Types = value;
        }

        public string Outputs
        {
            get => _loggerConfiguration.Outputs;
            set => _loggerConfiguration.Outputs = value;
        }

        public string DefaultTypeRange => _loggerConfiguration.DefaultTypeRange;
       
        public string TypeRange
        {
            get
            {
                string result = string.Empty;

                result += LogTypeHelper.TypeToString(LogMsgType.Debug);
                result += ";";
                result += LogTypeHelper.TypeToString(LogMsgType.Error);
                result += ";";
                result += LogTypeHelper.TypeToString(LogMsgType.Exception);
                result += ";";
                result += LogTypeHelper.TypeToString(LogMsgType.Info);
                result += ";";
                result += LogTypeHelper.TypeToString(LogMsgType.Timing);
                result += ";";
                result += LogTypeHelper.TypeToString(LogMsgType.Workflow);

                return result;
            }
        }

        public string DefaultOutputRange => _loggerConfiguration.DefaultOutputRange;

        public string OutputRange
        {
            get
            {
                string result = string.Empty;

                foreach(var output in _logOutputs)
                {
                    result += output.Name;
                    result += ";";
                }
                
                return result;
            }
        }
        
        public bool NewLine { get; set; }

        #endregion

        #region Methods

        public void AddOutput(ILogOutput output)
        {
            _logOutputs.Add(output);
        }

        public ILogOutput GetOutput(string name)
        {
            ILogOutput result = null;

            foreach (var output in _logOutputs)
            {
                if(output.Name == name)
                {
                    result = output;
                    break;
                }
            }

            return result;
        }

        public void WriteFlow(string source, string msg)
        {
            LogMsg(source, LogMsgType.Workflow, msg);
        }

        public void Debug(string source, string msg)
        {
            LogMsg(source, LogMsgType.Debug, msg);
        }

        public void Error(string source, string msg)
        {
            LogMsg(source, LogMsgType.Error, msg);
        }

        public void Exception(string source, Exception e)
        {
            Exception(source, e.Message);
        }

        public void Exception(string source, Exception e, int retryCount)
        {
            Exception(source, $" [retry={retryCount}] '{e.Message}'");
        }

        public void Exception(string source, string msg)
        {
            LogMsg(source, LogMsgType.Exception, msg);
        }

        public void Info(string source, string msg)
        {
            LogMsg(source, LogMsgType.Info, msg);
        }

        public void Warning(string source, string msg)
        {
            LogMsg(source, LogMsgType.Warning, msg);
        }

        public Stopwatch BeginTimeMeasure()
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            return stopWatch;
        }

        public void EndTimeMeasure(string source, Stopwatch stopWatch, string msg)
        {
            if (IsLogTypeActive(LogMsgType.Timing))
            {
                if (stopWatch != null)
                {
                    stopWatch.Stop();

                    if (!string.IsNullOrEmpty(msg))
                    {
                        LogMsg(source, LogMsgType.Timing, $" '{msg}' [time={stopWatch.ElapsedMilliseconds} ms]");                        
                    }
                }
            }
        }
                
        public void WriteLine(string source, LogMsgType type, string msg)
        {
            LogMsg(source, type, msg);
        }

        public void WriteLine(string source, string msg)
        {
            LogMsg(source, LogMsgType.Info, msg);
        }

        private List<ILogOutput> GetLogOutputs()
        {
            var logOutputs = new List<ILogOutput>();

            foreach (var output in _logOutputs)
            {
                if(IsLogOutputActive(output.Name))
                {
                    logOutputs.Add(output);
                }
            }
            
            return logOutputs;
        }

        private void LogMsg(string source, LogMsgType type, string msg, bool newLine = true)
        {
            UpdateConfiguration();

            if (IsLogTypeActive(type) && !string.IsNullOrEmpty(msg) && !IsSourceFilteredOut(source))
            {
                foreach (var output in GetLogOutputs())
                {
                    output.Write(source, type, msg, newLine);
                }
            }
        }

        private bool CreateLogConfigurationPath(out string path)
        {
            bool result = false;
            
            path = string.Empty;

            try
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var processName = AppHelper.GetProcessFileName(false);
                var configPath = Path.Combine(appDataPath, "eltra", processName, "config");

                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }

                path = configPath;
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetType().Name} - CreateLogConfigurationPath, {LogMsgType.Exception}, {e.Message}");
            }

            return result;
        }

        private void UpdateConfiguration()
        {
            const double UpdateIntervalInSec = 1;

            try
            {
                if (_loggerConfiguration != null)
                {
                    var lastAccessed = _loggerConfiguration.Accessed;
                    var lastDiff = DateTime.Now - lastAccessed;

                    if (lastDiff.TotalSeconds >= UpdateIntervalInSec)
                    {
                        try
                        {
                            if (File.Exists(ConfigFilePath))
                            {
                                UpdateExistingConfiguration();
                            }
                            else
                            {
                                CreateNewConfiguration();
                            }
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.Print($"{GetType().Name} - UpdateConfiguration, {LogMsgType.Exception}, {e.Message}");
                        }

                        _loggerConfiguration.Accessed = DateTime.Now;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{GetType().Name} - UpdateConfiguration, ERROR: processing log configuration failed, reason = '{e.Message}'");
            }
        }

        private void CreateNewConfiguration()
        {
            try
            {
                if (_loggerConfiguration != null)
                {
                    var content = JsonSerializer.Serialize(_loggerConfiguration);
                    var hashCode = CryptHelpers.ToMD5(content);

                    _loggerConfiguration.HashCode = hashCode;

                    File.WriteAllText(ConfigFilePath, content);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetType().Name} - CreateNewConfiguration - ERROR: Create log configuration failed!, reason = '{e.Message}'");
            }
        }

        private void UpdateExistingConfiguration()
        {
            try
            {
                var content = File.ReadAllText(ConfigFilePath);
                var hashCode = CryptHelpers.ToMD5(content);

                var loggerConfiguration = JsonSerializer.Deserialize<LoggerConfiguration>(content);

                if (loggerConfiguration != null && _loggerConfiguration.HashCode != hashCode)
                {
                    _loggerConfiguration = loggerConfiguration;

                    _loggerConfiguration.HashCode = hashCode;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"ERROR: Update log configuration failed!, reason = '{e.Message}'");
            }
        }

        private bool IsSourceFilteredOut(string source)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(source))
            {
                foreach (var filterOutSource in FilterOutSources)
                {
                    if (source.Contains(filterOutSource))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private bool IsLogTypeActive(LogMsgType type)
        {
            bool result = false;
            string typeAsString = LogTypeHelper.TypeToString(type);

            if (Types.Contains("*") || Types.ToLower().Contains(typeAsString.ToLower()))
            {
                result = true;
            }

            return result;
        }

        private bool IsLogOutputActive(string outputName)
        {
            bool result = false;
            
            if (Outputs.Contains("*") || Outputs.ToLower().Contains(outputName.ToLower()))
            {
                result = true;
            }

            return result;
        }
        
        #endregion
    }
}
