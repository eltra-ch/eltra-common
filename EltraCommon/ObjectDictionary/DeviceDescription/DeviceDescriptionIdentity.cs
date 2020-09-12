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
