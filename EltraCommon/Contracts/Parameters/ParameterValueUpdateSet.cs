using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Parameters
{
    /// <summary>
    /// ParameterValueUpdateSet
    /// </summary>
    [DataContract]
    public class ParameterValueUpdateSet
    {
        private List<ParameterValueUpdate> _items;

        /// <summary>
        /// ParameterValueUpdateSet
        /// </summary>
        public ParameterValueUpdateSet()
        {
            Header = DefaultHeader;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AC12";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        [DataMember]
        public List<ParameterValueUpdate> Items
        {
            get => _items ?? (_items = new List<ParameterValueUpdate>());
            set => _items = value;
        }

        /// <summary>
        /// Count
        /// </summary>
        public int Count
        {
            get => Items.Count;
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="parameter"></param>
        public void Add(ParameterValueUpdate parameter)
        {
            Items.Add(parameter);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ParameterValueUpdate Get(int index)
        {
            ParameterValueUpdate result = null;
            if (Count > index)
            {
                result = Items[index];
            }

            return result;
        }
    }
}