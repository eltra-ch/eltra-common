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
        /// ParameterValueUpdate
        /// </summary>
        public ParameterValueUpdate()
        {
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ParameterValueUpdate";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Discriminator { get; set; } = DefaultDiscriminator;

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
        /// Index
        /// </summary>
        [DataMember]
        public ushort Index { get; set; }

        /// <summary>
        /// SubIndex
        /// </summary>
        [DataMember]
        public byte SubIndex { get; set; }

        /// <summary>
        /// Parameter
        /// </summary>
        [DataMember]
        public ParameterValue ParameterValue { get; set; }
}
}
