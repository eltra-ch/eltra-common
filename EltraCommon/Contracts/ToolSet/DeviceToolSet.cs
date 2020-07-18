using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.ToolSet
{
    [DataContract]
    public class DeviceToolSet
    {
        #region Private fields

        private List<DeviceTool> _tools;

        #endregion

        #region Properties

        [DataMember]
        public List<DeviceTool> Tools => _tools ?? (_tools = new List<DeviceTool>());

        #endregion

        #region Methods

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

        public bool ToolExists(DeviceTool tool)
        {
            return FindToolById(tool.Id) != null;
        }

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
