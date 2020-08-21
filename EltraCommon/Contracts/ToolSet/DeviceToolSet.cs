using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.ToolSet
{
    /// <summary>
    /// DeviceToolSet
    /// </summary>
    [DataContract]
    public class DeviceToolSet
    {
        #region Private fields

        private List<DeviceTool> _tools;

        #endregion

        #region Properties

        /// <summary>
        /// Tools
        /// </summary>
        [DataMember]
        public List<DeviceTool> Tools => _tools ?? (_tools = new List<DeviceTool>());

        #endregion

        #region Methods

        /// <summary>
        /// AddTool
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public bool AddTool(DeviceTool tool)
        {
            bool result = false;

            if (!ToolExists(tool))
            {
                Tools.Add(tool);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// ToolExists
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public bool ToolExists(DeviceTool tool)
        {
            return FindToolById(tool.Id) != null;
        }

        /// <summary>
        /// FindToolById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeviceTool FindToolById(string id)
        {
            DeviceTool result = null;

            if(!string.IsNullOrEmpty(id))
            {
                foreach (var command in Tools)
                {
                    if (command.Id.ToLower() == id.ToLower())
                    {
                        result = command;
                        break;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
