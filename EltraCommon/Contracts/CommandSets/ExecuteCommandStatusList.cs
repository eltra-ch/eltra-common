using System;
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
            Header = DefaultHeader;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AHJ7";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

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
