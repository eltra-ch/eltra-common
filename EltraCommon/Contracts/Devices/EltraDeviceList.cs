using System.Runtime.Serialization;
using System.Collections.Generic;

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
            Header = DefaultHeader;
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "ALR8";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

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
