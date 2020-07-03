using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class SessionIdentification
    {
        [DataMember]
        public string Uuid { get; set; }
    }
}
