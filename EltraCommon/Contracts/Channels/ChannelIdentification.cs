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
            Header = DefaultHeader;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AKR3";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }
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
