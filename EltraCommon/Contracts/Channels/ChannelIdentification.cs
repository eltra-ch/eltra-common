using EltraCommon.Contracts.Users;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    [DataContract]
    public class ChannelIdentification
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public UserData UserData { get; set; }
    }
}
