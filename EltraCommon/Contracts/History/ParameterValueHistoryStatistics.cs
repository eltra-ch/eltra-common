﻿using System;
using System.ComponentModel;
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
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ParameterValueHistoryStatistics";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

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
