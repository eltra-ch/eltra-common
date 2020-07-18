using EltraCommon.Contracts.Devices;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.ToolSet
{
    [DataContract]
    public class DeviceTool
    {
        #region Constructors

        public DeviceTool()
        {
        }

        public DeviceTool(EltraDevice device)
        {
            Device = device;
        }

        #endregion

        #region Properties

        [DataMember]
        public string Id { get; set; }

        [IgnoreDataMember]
        public EltraDevice Device { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DeviceToolStatus Status { get; set; }

        #endregion
    }
}