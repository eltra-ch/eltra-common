using System.Collections.Generic;
using System.Runtime.Serialization;

using EltraCommon.Contracts.Devices;
using EltraCommon.Contracts.Node;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class EltraDeviceNodeList
    {
        #region Private fields

        private List<EltraDeviceNode> _deviceNodeList;
        
        #endregion

        #region Properties

        [DataMember]
        public Session Session { get; set; }

        [DataMember]
        public List<EltraDeviceNode> DeviceNodeList
        {
            get => _deviceNodeList ?? (_deviceNodeList = new List<EltraDeviceNode>());
            set => _deviceNodeList = value;
        }

        public int DevicesCount => DeviceNodeList.Count;

        #endregion

        #region Methods

        public bool AddDevice(EltraDeviceNode deviceNode)
        {
            bool result = false;

            if (!DeviceExists(deviceNode) && Session != null)
            {
                deviceNode.SessionUuid = Session.Uuid;

                DeviceNodeList.Add(deviceNode);

                result = true;
            }

            return result;
        }

        public EltraDevice FindDevice(int nodeId)
        {
            EltraDevice result = null;

            foreach (var deviceNode in DeviceNodeList)
            {
                if (deviceNode.NodeId == nodeId)
                {
                    result = deviceNode;
                    break;
                }
            }

            return result;
        }

        public void RemoveDevice(EltraDeviceNode device)
        {
            foreach (var sessionDevice in DeviceNodeList)
            {
                if (sessionDevice == device)
                {
                    DeviceNodeList.Remove(sessionDevice);
                    break;
                }
            }
        }

        private bool DeviceExists(EltraDeviceNode device)
        {
            bool result = false;

            if (device?.Identification != null)
            {
                result = FindDevice(device.NodeId) != null;
            }
            
            return result;
        }

        #endregion
    }
}
