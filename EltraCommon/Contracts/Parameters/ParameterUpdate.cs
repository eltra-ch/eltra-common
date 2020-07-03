using System.Runtime.Serialization;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;

namespace EltraCommon.Contracts.Parameters
{
    [DataContract]
    public class ParameterUpdate
    {
        [DataMember]
        public string SessionUuid { get; set; }
        [DataMember]
        public ulong SerialNumber { get; set; }
        [DataMember]
        public Parameter Parameter { get; set; }
}
}
