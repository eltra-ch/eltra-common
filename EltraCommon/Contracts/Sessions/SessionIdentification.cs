using EltraCommon.Contracts.Users;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class SessionIdentification
    {
        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public int NodeId { get; set; }
        [DataMember]
        public UserAuthData AuthData { get; set; }
    }
}
