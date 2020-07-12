using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Sessions
{
    [DataContract]
    public class EltraDeviceNodeSet
    {
        private List<EltraDeviceNodeList> _deviceNodeList;

        [DataMember]
        public List<EltraDeviceNodeList> DeviceNodeList
        {
            get => _deviceNodeList ?? (_deviceNodeList = new List<EltraDeviceNodeList>());
        }

        public void Add(EltraDeviceNodeList sessionDevices)
        {
            DeviceNodeList.Add(sessionDevices);
        }
    }
}
