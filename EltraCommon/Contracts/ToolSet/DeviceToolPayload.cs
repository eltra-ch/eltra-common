using System;
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
            Modified = DateTime.MinValue;
            Created = DateTime.MinValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Channel Id
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// Node Id
        /// </summary>
        [DataMember]
        public int NodeId { get; set; }

        /// <summary>
        /// Tool Id
        /// </summary>
        [DataMember]
        public string ToolId { get; set; }

        /// <summary>
        /// UniqueId
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// FileName
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [DataMember]
        public string Content { get; set; }

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

        /// <summary>
        /// Mode
        /// </summary>
        [DataMember]
        public DeviceToolPayloadMode Mode { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }

        #endregion
    }
}