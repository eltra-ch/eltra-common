using System.Runtime.Serialization;

using EltraCommon.Contracts.Devices;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class SessionDevice
    {
        #region Properties

        [DataMember]
        public string SessionUuid { get; set; }

        [DataMember]
        public EltraDevice Device { get; set; }

        [DataMember]
        public int NodeId { get; set; }

        #endregion
    }
}
