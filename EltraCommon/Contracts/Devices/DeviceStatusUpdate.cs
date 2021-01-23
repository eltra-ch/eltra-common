using System;
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
            Header = DefaultHeader;
        }

        #endregion

        #region Properties
        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AKD2";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

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
