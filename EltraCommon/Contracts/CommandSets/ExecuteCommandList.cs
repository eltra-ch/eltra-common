using System.Collections.Generic;
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
            Header = DefaultHeader;
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AOE8";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

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
