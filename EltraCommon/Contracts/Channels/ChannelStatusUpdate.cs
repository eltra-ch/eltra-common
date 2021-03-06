﻿using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// Channel status update
    /// </summary>
    [DataContract]
    public class ChannelStatusUpdate
    {
        /// <summary>
        /// ChannelStatusUpdate
        /// </summary>
        public ChannelStatusUpdate()
        {
            Header = DefaultHeader;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "ACF5";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// Channel id
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// Channel status
        /// </summary>
        [DataMember]
        public ChannelStatus Status { get; set; }

        /// <summary>
        /// Local host
        /// </summary>
        [DataMember]
        public string LocalHost { get; set; }
    }
}
