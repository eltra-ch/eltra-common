using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace EltraCommon.Contracts.Devices
{
    /// <summary>
    /// EltraDevice
    /// </summary>
    [DataContract]
    public class EltraDeviceList
    {
        #region Private fields

        private List<EltraDevice> _items;

        #endregion

        #region Constructors

        /// <summary>
        /// EltraDeviceList
        /// </summary>
        public EltraDeviceList()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "EltraDeviceList";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// Items
        /// </summary>
        [DataMember]
        public List<EltraDevice> Items
        {
            get => _items ?? (_items = new List<EltraDevice>());
            set => _items = value;
        }

        #endregion
    }
}
