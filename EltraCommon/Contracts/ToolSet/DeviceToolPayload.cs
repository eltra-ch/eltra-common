using System.Runtime.Serialization;

namespace EltraCommon.Contracts.ToolSet
{
    /// <summary>
    /// DeviceToolPayload
    /// </summary>
    [DataContract]
    public class DeviceToolPayload
    {
        #region Constructors
        /// <summary>
        /// DeviceToolPayload
        /// </summary>
        public DeviceToolPayload()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// FileName
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// HashCode
        /// </summary>
        [DataMember]
        public string HashCode { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        #endregion
    }
}