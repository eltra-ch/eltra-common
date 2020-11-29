using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// ChannelBase
    /// </summary>
    [DataContract]
    public class ChannelBase
    {
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
