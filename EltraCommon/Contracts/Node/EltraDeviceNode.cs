using System.Runtime.Serialization;

using EltraCommon.Contracts.Devices;

namespace EltraCommon.Contracts.Node
{
    [DataContract]
    public class EltraDeviceNode : EltraDevice
    {
        #region Properties

        [DataMember]
        public string SessionUuid { get; set; }

        [DataMember]
        public int NodeId { get; set; }

        #endregion
    }
}
