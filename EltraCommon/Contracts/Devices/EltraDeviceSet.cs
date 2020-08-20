using System.Collections.Generic;
using System.Runtime.Serialization;
using EltraCommon.Contracts.Channels;

namespace EltraCommon.Contracts.Devices
{
    /// <summary>
    /// EltraDeviceSet
    /// </summary>
    [DataContract]
    public class EltraDeviceSet
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
        public List<EltraDevice> Devices
        {
            get => _deviceNodeList ?? (_deviceNodeList = new List<EltraDevice>());
            set => _deviceNodeList = value;
        }

        /// <summary>
        /// DevicesCount
        /// </summary>
        public int DevicesCount => Devices.Count;

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

                Devices.Add(deviceNode);

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

            foreach (var deviceNode in Devices)
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
            foreach (var channelDevice in Devices)
            {
                if (channelDevice == device)
                {
                    Devices.Remove(channelDevice);
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
