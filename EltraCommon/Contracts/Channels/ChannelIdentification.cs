using EltraCommon.Contracts.Users;
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
        /// User authorization data
        /// </summary>
        [DataMember]
        public UserData UserData { get; set; }
    }
}
