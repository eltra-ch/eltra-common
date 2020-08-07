using EltraCommon.Contracts.Users;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// Channel status update
    /// </summary>
    [DataContract]
    public class ChannelStatusUpdate
    {
        /// <summary>
        /// User authorization data
        /// </summary>
        [DataMember]
        public UserData UserData { get; set; }

        /// <summary>
        /// Channel id
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// Channel status
        /// </summary>
        [DataMember]
        public ChannelStatus Status { get; set; }
    }
}
