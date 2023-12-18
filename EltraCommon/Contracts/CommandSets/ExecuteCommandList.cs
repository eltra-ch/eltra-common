using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    /// <summary>
    /// ExecuteCommand
    /// </summary>
    [DataContract]
    public class ExecuteCommandList
    {
        #region Private fields

        private List<ExecuteCommand> _items;

        #endregion

        #region Constructors

        /// <summary>
        /// ExecuteCommand
        /// </summary>
        public ExecuteCommandList()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ExecuteCommandList";

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
        public List<ExecuteCommand> Items
        {
            get => _items ?? (_items = new List<ExecuteCommand>());
            set => _items = value;
        }

        #endregion
    }
}
