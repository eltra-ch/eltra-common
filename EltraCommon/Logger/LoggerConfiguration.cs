using EltraCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EltraCommon.Logger
{
    [DataContract]
    class LoggerConfiguration
    {
        #region Private fields

        private string _types;
        private List<string> _filterOutSources;
        private string _outputs;

        #endregion

        #region Constructors

        public LoggerConfiguration()
        {
            _types = DefaultTypeRange;
        }

        #endregion

        #region Properties

        [DataMember]
        public List<string> FilterOutSources
        {
            get => _filterOutSources ?? (_filterOutSources = new List<string>());
            set => _filterOutSources = value;
        }

        [DataMember]
        public string Types
        {
            get => _types ?? (_types = DefaultTypeRange);
            set => _types = value;
        }

        [DataMember]
        public string Outputs
        {
            get => _outputs ?? (_outputs = DefaultOutputRange);
            set => _outputs = value;
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

        #endregion
    }
}
