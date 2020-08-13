using System.Collections.Generic;
using System.Runtime.Serialization;
using EltraCommon.Contracts.Channels;
using EltraCommon.Contracts.Devices;

namespace EltraCommon.Contracts.Node
{
    /// <summary>
    /// EltraDeviceNodeList
    /// </summary>
    [DataContract]
    public class EltraDeviceNodeList
    {
        #region Private fields

        private List<EltraDevice> _deviceNodeList;

        #endregion

        #region Properties

        /// <summary>
        /// Channel
        /// </summary>
        [DataMember]
        public Channel Channel { get; set; }

        /// <summary>
        /// DeviceNodeList
        /// </summary>
        [DataMember]
        public List<EltraDevice> DeviceNodeList
        {
            get => _deviceNodeList ?? (_deviceNodeList = new List<EltraDevice>());
            set => _deviceNodeList = value;
        }

        /// <summary>
        /// DevicesCount
        /// </summary>
        public int DevicesCount => DeviceNodeList.Count;

        #endregion

        #region Methods

        /// <summary>
        /// AddDevice
        /// </summary>
        /// <param name="deviceNode"></param>
        /// <returns></returns>
        public bool AddDevice(EltraDevice deviceNode)
        {
            bool result = false;

            if (!DeviceExists(deviceNode) && Channel != null)
            {
                deviceNode.ChannelId = Channel.Id;

                DeviceNodeList.Add(deviceNode);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// FindDevice
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// RemoveDevice
        /// </summary>
        /// <param name="device"></param>
        public void RemoveDevice(EltraDevice device)
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

        private bool DeviceExists(EltraDevice device)
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
