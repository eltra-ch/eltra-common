using System.Collections.Generic;
using System.Runtime.Serialization;

using EltraCommon.Contracts.Devices;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class SessionDevices
    {
        #region Private fields

        private List<SessionDevice> _sessionDeviceList;
        
        #endregion

        #region Properties

        [DataMember]
        public Session Session { get; set; }

        [DataMember]
        public List<SessionDevice> SessionDeviceList
        {
            get => _sessionDeviceList ?? (_sessionDeviceList = new List<SessionDevice>());
            set => _sessionDeviceList = value;
        }

        public int DevicesCount => SessionDeviceList.Count;

        #endregion

        #region Methods

        public bool AddDevice(EltraDevice device)
        {
            bool result = false;

            if (!DeviceExists(device) && Session != null)
            {
                var sessionDevice = new SessionDevice() { Device = device, SessionUuid = Session.Uuid };

                SessionDeviceList.Add(sessionDevice);

                result = true;
            }

            return result;
        }

        public EltraDevice FindDeviceBySerialNumber(ulong serialNumber)
        {
            EltraDevice result = null;

            foreach (var sessionDevice in SessionDeviceList)
            {
                var device = sessionDevice.Device;

                if (device.Identification.SerialNumber == serialNumber)
                {
                    result = device;
                    break;
                }
            }

            return result;
        }

        public void RemoveDevice(EltraDevice device)
        {
            foreach (var sessionDevice in SessionDeviceList)
            {
                if (sessionDevice.Device == device)
                {
                    SessionDeviceList.Remove(sessionDevice);
                    break;
                }
            }
        }

        private bool DeviceExists(EltraDevice device)
        {
            bool result = false;

            if (device?.Identification != null)
            {
                result = FindDeviceBySerialNumber(device.Identification.SerialNumber) != null;
            }
            
            return result;
        }

        #endregion
    }
}
