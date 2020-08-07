using System.Runtime.Serialization;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;

namespace EltraCommon.Contracts.Parameters
{
    /// <summary>
    /// ParameterUpdate
    /// </summary>
    [DataContract]
    public class ParameterUpdate
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
        public Parameter Parameter { get; set; }
}
}
