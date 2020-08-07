using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Parameters
{
    /// <summary>
    /// ParameterUniqueIdValuePair
    /// </summary>
    [DataContract]
    public class ParameterUniqueIdValuePair
    {
        /// <summary>
        /// UniqueId
        /// </summary>
        [DataMember]
        public string UniqueId { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public ParameterValue Value { get; set; }
    }
}
