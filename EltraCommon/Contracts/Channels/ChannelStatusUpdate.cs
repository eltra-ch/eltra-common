using EltraCommon.Contracts.Users;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    [DataContract]
    public class ChannelStatusUpdate
    {
        [DataMember]
        public UserData UserData { get; set; }

        [DataMember]
        public string ChannelId { get; set; }

        [DataMember]
        public ChannelStatus Status { get; set; }
    }
}
