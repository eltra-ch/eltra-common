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
        /// ChannelIdentification
        /// </summary>
        public ChannelIdentification()
        {
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ChannelIdentification";

        /// <summary>
        /// Discriminator
        /// </summary>
        [DataMember]
        public string Discriminator { get; set; } = DefaultDiscriminator;
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
