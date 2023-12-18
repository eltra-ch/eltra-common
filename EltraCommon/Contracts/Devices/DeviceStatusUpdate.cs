using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Devices
{
    /// <summary>
    /// DeviceStatus
    /// </summary>
    [DataContract]
    public class DeviceStatusUpdate
    {
        #region Constructors
        
        /// <summary>
        /// DeviceStatusUpdate
        /// </summary>
        public DeviceStatusUpdate()
        {
        }

        #endregion

        #region Properties
        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "DeviceStatusUpdate";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public DeviceStatus Status { get; set;}

        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }

        #endregion
    }
}
