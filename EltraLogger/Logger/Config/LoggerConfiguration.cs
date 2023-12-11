using EltraCommon.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EltraCommon.Logger.Config
{
    [DataContract]
    internal class LoggerConfiguration
    {
        #region Private fields

        private const string ConfigFileName = "logconfig.json";
        private string _types;
        private List<string> _filterOutSources;
        private string _outputs;
        private static string _configPath;
        private bool _serializing;
        
        #endregion

        #region Constructors

        public LoggerConfiguration()
        {
            _serializing = true;
            _types = DefaultTypeRange;
        }

        public LoggerConfiguration(bool serializing)
        {
            _serializing = serializing;
            _types = DefaultTypeRange;
        }

        #endregion

        #region Events

        public event EventHandler BeforeChange;
        public event EventHandler AfterChange;
        
        #endregion

        #region Properties

        [DataMember]
        public List<string> FilterOutSources
        {
            get => _filterOutSources ?? (_filterOutSources = new List<string>());
            set
            {
                OnBeforeChange();
                
                _filterOutSources = value;
                
                OnAfterChange();
            }
        }

        [DataMember]
        public string Types
        {
            get => _types ?? (_types = DefaultTypeRange);
            set
            {
                OnBeforeChange();
                
                _types = value;

                OnAfterChange();
            }
        }

        [DataMember]
        public string Outputs
        {
            get => _outputs ?? (_outputs = DefaultOutputRange);
            set
            {
                OnBeforeChange();
                
                _outputs = value;

                OnAfterChange();
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
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

        [IgnoreDataMember]
        [JsonIgnore]
        public string DefaultOutputRange
        {
            get
            {
                var result = "Console";

                return result;
            }
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public string HashCode { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public DateTime Accessed { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        protected string ConfigFilePath => Path.Combine(ConfigPath, ConfigFileName);

        [IgnoreDataMember]
        [JsonIgnore]
        protected string ConfigPath
        {
            get
            {
                if (string.IsNullOrEmpty(_configPath))
                {
                    if (CreateLogConfigurationPath(Environment.SpecialFolder.LocalApplicationData, out string localAppDataPath))
                    {
                        _configPath = localAppDataPath;
                    }
                    else if (CreateLogConfigurationPath(Environment.SpecialFolder.MyDocuments, out string myDocsPath))
                    {
                        _configPath = myDocsPath;
                    }
                    else if (CreateLogConfigurationPath(Environment.SpecialFolder.InternetCache, out string internetCachePath))
                    {
                        _configPath = internetCachePath;
                    }
                    else if (CreateLogConfigurationPath(Environment.CurrentDirectory, out string currentPath))
                    {
                        _configPath = currentPath;
                    }
                    else
                    {
                        _configPath = Environment.CurrentDirectory;
                    }
                }

                return _configPath;
            }
        }

        #endregion

        #region Events handling

        private void OnBeforeChange()
        {
            if (!_serializing)
            {
                LoadConfiguration();

                BeforeChange?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnAfterChange()
        {
            if (!_serializing)
            {
                SaveConfiguration();

                AfterChange?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Methods

        private bool CreateLogConfigurationPath(string folderPath, out string path)
        {
            bool result = false;
            const string method = "CreateLogConfigurationPath";

            path = string.Empty;

            try
            {
                var processName = AppHelper.GetProcessFileName(false);
                var configPath = Path.Combine(folderPath, "eltra", processName, "config");

                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }

                path = configPath;
                result = true;
            }
            catch(UnauthorizedAccessException)
            {
                Console.WriteLine($"{GetType().Name} - {method}, {LogMsgType.Exception}, unauthorized");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetType().Name} - {method}, {LogMsgType.Exception}, {e.Message}");
            }

            return result;
        }

        private bool CreateLogConfigurationPath(Environment.SpecialFolder folder, out string path)
        {
            bool result = false;
            const string method = "CreateLogConfigurationPath";

            path = string.Empty;

            try
            {
                var folderPath = Environment.GetFolderPath(folder);
            
                result = CreateLogConfigurationPath(folderPath, out path);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetType().Name} - {method}, {LogMsgType.Exception}, {e.Message}");
            }

            return result;
        }

        public void Update()
        {
            const double UpdateIntervalInSec = 10;

            try
            {
                var lastAccessed = Accessed;
                var lastDiff = DateTime.Now - lastAccessed;

                if (lastDiff.TotalSeconds >= UpdateIntervalInSec)
                {
                    try
                    {
                        if (File.Exists(ConfigFilePath))
                        {
                            LoadConfiguration();
                        }
                        else
                        {
                            SaveConfiguration();
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.Print($"{GetType().Name} - UpdateConfiguration, {LogMsgType.Exception}, {e.Message}");
                    }

                    Accessed = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetType().Name} - UpdateConfiguration, ERROR: processing log configuration failed, reason = '{e.Message}'");
            }
        }

        private void SaveConfiguration()
        {
            try
            {
                var content = JsonSerializer.Serialize(this);
                var hashCode = CryptHelpers.ToMD5(content);

                HashCode = hashCode;

                File.WriteAllText(ConfigFilePath, content);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{GetType().Name} - CreateNewConfiguration - ERROR: Create log configuration failed!, reason = '{e.Message}'");
            }
        }

        private void LoadConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    var content = File.ReadAllText(ConfigFilePath);

                    if (!string.IsNullOrEmpty(content))
                    {
                        var hashCode = CryptHelpers.ToMD5(content);

                        var loggerConfiguration = JsonSerializer.Deserialize<LoggerConfiguration>(content);

                        if (loggerConfiguration != null && HashCode != hashCode)
                        {
                            Copyfrom(loggerConfiguration);

                            HashCode = hashCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Update log configuration failed!, reason = '{e.Message}'");
            }
        }

        private void Copyfrom(LoggerConfiguration configuration)
        {
            if (configuration != null)
            {
                _serializing = true;

                FilterOutSources = configuration.FilterOutSources;
                Types = configuration.Types;
                Outputs = configuration.Outputs;

                _serializing = false;
            }
        }

        #endregion
    }
}
