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
            Header = DefaultHeader;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AIP5";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

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
