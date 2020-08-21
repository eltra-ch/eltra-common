using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    /// <summary>
    /// ExecuteCommand
    /// </summary>
    [DataContract]
    public class ExecuteCommand
    {
        private DeviceCommand _command;

        /// <summary>
        /// CommandId
        /// </summary>
        [DataMember]
        public string CommandId { get; set; }

        /// <summary>
        /// SourceChannelId
        /// </summary>
        [DataMember]
        public string SourceChannelId { get; set; }

        /// <summary>
        /// TargetChannelId
        /// </summary>
        [DataMember]
        public string TargetChannelId { get; set; }

        /// <summary>
        /// NodeId
        /// </summary>
        [DataMember]
        public int NodeId { get; set; }

        /// <summary>
        /// Command
        /// </summary>
        [DataMember]
        public DeviceCommand Command
        {
            get => _command;

            set
            {
                _command = value;
                
                if (_command != null)
                {
                    if (!string.IsNullOrEmpty(_command.Id))
                    {
                        CommandId = _command.Id;
                    }

                    var device = _command.Device;
                    if (device != null)
                    {
                        NodeId = device.NodeId;
                    }
                }
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }
    }
}
