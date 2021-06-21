﻿using EltraCommon.Helpers;
using EltraCommon.Logger.Output;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#pragma warning disable 1591

namespace EltraCommon.Logger
{
    public class EltraLogger : IEltraLogger
    {
        #region Private fields
                
        private readonly List<ILogOutput> _logOutputs;
        private string _types;
        private List<string> _filterOutSources;
        private string _outputs;

        #endregion

        #region Constructors

        public EltraLogger()
        {
            _types = DefaultTypeRange;
                        
            _logOutputs = new List<ILogOutput>();

            AddOutput(new ConsoleLogOutput());
            AddOutput(new DebugLogOutput());
            AddOutput(new FileLogOutput(new ConsoleLogOutput()));
        }

        #endregion

        #region Properties

        public List<string> FilterOutSources => _filterOutSources ?? (_filterOutSources = new List<string>());

        public string Types
        {
            get => _types ?? (_types = DefaultTypeRange);
            set => _types = value;
        }

        public string Outputs
        {
            get => _outputs ?? (_outputs = DefaultOutputRange);
            set => _outputs = value;
        }

        public string DefaultTypeRange
        {
            get
            {
                string result = string.Empty;

                result += LogTypeHelper.TypeToString(LogMsgType.Error);
                result += ";";
                result += LogTypeHelper.TypeToString(LogMsgType.Exception);
                
                return result;
            }
        }

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

        public string DefaultOutputRange
        {
            get
            {
                var result = "Console";

                return result;
            }
        }

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
            if (IsLogTypeActive(type) && !string.IsNullOrEmpty(msg) && !IsSourceFilteredOut(source))
            {
                foreach (var output in GetLogOutputs())
                {
                    output.Write(source, type, msg, newLine);
                }
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
