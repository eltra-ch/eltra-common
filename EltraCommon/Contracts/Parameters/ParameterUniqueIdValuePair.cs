using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Parameters
{
    [DataContract]
    public class ParameterUniqueIdValuePair
    {
        [DataMember]
        public string UniqueId { get; set; }
        
        [DataMember]
        public ParameterValue Value { get; set; }
    }
}
