﻿using System.ComponentModel;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// ChannelBase
    /// </summary>
    [DataContract]
    public class ChannelBase
    {
        private const string DefaultDiscriminator = "ChannelBase";

        /// <summary>
        /// Discriminator
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Timeout
        /// </summary>
        [DataMember]
        public uint Timeout { get; set; }

        /// <summary>
        /// Local host
        /// </summary>
        [DataMember]
        public string LocalHost { get; set; }
    }
}
