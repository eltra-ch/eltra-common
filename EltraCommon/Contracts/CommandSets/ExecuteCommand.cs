using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    [DataContract]
    public class ExecuteCommand
    {
        private DeviceCommand _command;

        [DataMember]
        public string CommandId { get; set; }

        [DataMember]
        public string SourceChannelId { get; set; }

        [DataMember]
        public string TargetChannelId { get; set; }

        [DataMember]
        public int NodeId { get; set; }

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

        [DataMember]
        public DateTime Modified { get; set; }
    }
}
