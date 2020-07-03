using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Devices
{
    [DataContract]
    public class DeviceLock
    {
        [DataMember]
        public string AgentUuid { get; set; }

        [DataMember]
        public ulong SerialNumber { get; set; }
    }
}
