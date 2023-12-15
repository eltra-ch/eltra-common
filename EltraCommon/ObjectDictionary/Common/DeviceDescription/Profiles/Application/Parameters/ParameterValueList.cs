using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters
{
    /// <summary>
    /// ParameterValueList
    /// </summary>
    [DataContract]
    public class ParameterValueList
    {
        private List<ParameterValue> _items;

        /// <summary>
        /// ParameterValueList
        /// </summary>
        public ParameterValueList()
        {
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ParameterValueList";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// Items
        /// </summary>
        [DataMember]
        public List<ParameterValue> Items
        {
            get => _items ?? (_items = new List<ParameterValue>());
            set => _items = value;
        }
    }
}
