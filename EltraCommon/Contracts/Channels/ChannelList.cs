using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Channels
{
    /// <summary>
    /// Channel
    /// </summary>
    [DataContract]
    public class ChannelList
    {
        #region Private fields

        private List<Channel> _items;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Channel
        /// </summary>
        public ChannelList()
        {
            Header = DefaultHeader;
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AJF9";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        [DataMember]
        public List<Channel> Items
        {
            get => _items ?? (_items = new List<Channel>());
            set => _items = value;
        }

        #endregion
    }
}
