using System.Runtime.Serialization;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;

namespace EltraCommon.Contracts.Parameters
{
    /// <summary>
    /// ParameterValueUpdate
    /// </summary>
    [DataContract]
    public class ParameterValueUpdate
    {
        /// <summary>
        /// ChannelId
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }
        /// <summary>
        /// NodeId
        /// </summary>
        [DataMember]
        public int NodeId { get; set; }
        /// <summary>
        /// Parameter
        /// </summary>
        [DataMember]
        public ParameterValue ParameterValue { get; set; }
}
}
