using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Devices
{
    /// <summary>
    /// DeviceVersion
    /// </summary>
    [DataContract]
    public class DeviceVersion
    {
        /// <summary>
        /// HardwareVersion
        /// </summary>
        [DataMember]
        public ushort HardwareVersion { get; set; }
        /// <summary>
        /// SoftwareVersion
        /// </summary>
        [DataMember]
        public ushort SoftwareVersion { get; set; }
        /// <summary>
        /// ApplicationNumber
        /// </summary>
        [DataMember]
        public ushort ApplicationNumber { get; set; }
        /// <summary>
        /// ApplicationVersion
        /// </summary>
        [DataMember]
        public ushort ApplicationVersion { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"0x{HardwareVersion:X2} 0x{SoftwareVersion:X2} 0x{ApplicationNumber:X2} 0x{ApplicationVersion:X2}";
        }
    }
}
