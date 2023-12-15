using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    /// <summary>
    /// ExecuteCommandStatusSet
    /// </summary>
    [DataContract]
    public class ExecuteCommandStatusList
    {
        private List<ExecuteCommandStatus> _items;

        /// <summary>
        /// ExecuteCommandStatus
        /// </summary>
        public ExecuteCommandStatusList()
        {
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ExecuteCommandStatusList";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// Items
        /// </summary>
        [DataMember]
        public List<ExecuteCommandStatus> Items 
        { 
            get => _items ?? (_items = new List<ExecuteCommandStatus>()); 
            set => _items = value; 
        }
    }
}
