using EltraCommon.Contracts.Devices;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.ToolSet
{
    /// <summary>
    /// DeviceTool
    /// </summary>
    [DataContract]
    public class DeviceTool
    {
        #region Private fields

        private List<DeviceToolPayload> _payloadList;

        #endregion

        #region Constructors
        /// <summary>
        /// DeviceTool
        /// </summary>
        public DeviceTool()
        {
        }

        /// <summary>
        /// DeviceTool
        /// </summary>
        /// <param name="device"></param>
        public DeviceTool(EltraDevice device)
        {
            Device = device;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Device - optional
        /// </summary>
        [IgnoreDataMember]
        public EltraDevice Device { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public DeviceToolStatus Status { get; set; }

        /// <summary>
        /// PayloadSet
        /// </summary>
        [DataMember]
        public List<DeviceToolPayload> PayloadSet
        { 
            get => _payloadList ?? (_payloadList = new List<DeviceToolPayload>());
        }

        #endregion

        #region Methods

        internal void AddPayload(DeviceToolPayload payload)
        {
            PayloadSet.Add(payload);
        }

        #endregion
    }
}