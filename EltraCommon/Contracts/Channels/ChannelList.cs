using System.Collections.Generic;
using System.ComponentModel;
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
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ChannelList";

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
        public List<Channel> Items
        {
            get => _items ?? (_items = new List<Channel>());
            set => _items = value;
        }

        #endregion
    }
}
