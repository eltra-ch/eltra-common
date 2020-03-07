using System.Runtime.Serialization;

namespace EltraCloudContracts.Contracts.Sessions
{
    [DataContract]
    public class SessionStatusUpdate
    {
        [DataMember]
        public string SessionUuid { get; set; }

        [DataMember]
        public SessionStatus Status { get; set; }
    }
}
