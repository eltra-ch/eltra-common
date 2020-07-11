using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    [DataContract]
    public class ExecuteCommand
    {
        private DeviceCommand _command;

        [DataMember]
        public string CommandUuid { get; set; }

        [DataMember]
        public string SourceSessionUuid { get; set; }

        [DataMember]
        public string TargetSessionUuid { get; set; }

        [DataMember]
        public ulong SerialNumber { get; set; }

        [DataMember]
        public DeviceCommand Command
        {
            get => _command;

            set
            {
                _command = value;
                
                if (_command != null)
                {
                    if (!string.IsNullOrEmpty(_command.Uuid))
                    {
                        CommandUuid = _command.Uuid;
                    }

                    var device = _command.SessionDevice?.Device;
                    if (device != null)
                    {
                        var identification = device.Identification;

                        SerialNumber = identification.SerialNumber;
                    }
                }
            }
        }

        [DataMember]
        public DateTime Modified { get; set; }
    }
}
