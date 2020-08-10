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
