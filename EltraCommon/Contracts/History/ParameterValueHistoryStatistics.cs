using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.History
{
    /// <summary>
    /// ParameterValueHistoryInfo
    /// </summary>
    [DataContract]
    public class ParameterValueHistoryStatistics
    {
        /// <summary>
        /// ParameterValueHistoryStatistics
        /// </summary>
        public ParameterValueHistoryStatistics()
        {
            Header = DefaultHeader;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AJF2";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// EntriesCount
        /// </summary>
        [DataMember]
        public int EntriesCount { get; set; }
        /// <summary>
        /// SizeInBytes
        /// </summary>
        [DataMember]
        public int SizeInBytes { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; } 
    }
}
