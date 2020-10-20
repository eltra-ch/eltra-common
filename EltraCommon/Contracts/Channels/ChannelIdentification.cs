using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// Channel identification
    /// </summary>
    [DataContract]
    public class ChannelIdentification
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// Node id
        /// </summary>
        [DataMember]
        public int NodeId { get; set; }
    }
}
