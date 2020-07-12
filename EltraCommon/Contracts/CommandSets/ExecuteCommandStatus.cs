using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    [DataContract]
    public class ExecuteCommandStatus
    {
        public ExecuteCommandStatus()
        {
        }

        public ExecuteCommandStatus(string sessionUuid, ExecuteCommand execCommand)
        {
            SessionUuid = sessionUuid;
            NodeId = execCommand.NodeId;
            CommandUuid = execCommand.CommandUuid;
            
            var command = execCommand.Command;

            if (command != null)
            {
                if (!string.IsNullOrEmpty(command.Uuid))
                {
                    CommandUuid = command.Uuid;
                }

                CommandName = command.Name;
            }
        }

        [DataMember]
        public string CommandUuid { get; set; }

        [DataMember]
        public string SessionUuid { get; set; }

        [DataMember]
        public int NodeId { get; set; }

        [DataMember]
        public string CommandName { get; set; }

        [DataMember]
        public ExecCommandStatus Status { get; set; }

        [DataMember]
        public ExecCommandCommStatus CommStatus { get; set; }

        [DataMember]
        public DateTime Modified { get; set; }
    }
}
