using EltraCommon.Contracts.Users;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class SessionStatusUpdate
    {
        [DataMember]
        public UserAuthData AuthData { get; set; }
        
        [DataMember]
        public string SessionUuid { get; set; }

        [DataMember]
        public SessionStatus Status { get; set; }
    }
}
