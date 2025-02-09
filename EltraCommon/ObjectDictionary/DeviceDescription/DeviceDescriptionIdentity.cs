﻿using System.ComponentModel;
using System.Runtime.Serialization;

namespace EltraCommon.ObjectDictionary.DeviceDescription
{
    /// <summary>
    /// Device Description Identity
    /// </summary>
    [DataContract]
    public class DeviceDescriptionIdentity
    {
        /// <summary>
        /// DeviceDescriptionIdentity
        /// </summary>
        public DeviceDescriptionIdentity()
        {
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "DeviceDescriptionIdentity";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// Hash code encoding algorithm
        /// </summary>
        [DataMember]
        public string Encoding { get; set; }

        /// <summary>
        /// Hash code content
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
