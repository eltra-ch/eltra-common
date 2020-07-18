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

        public ExecuteCommandStatus(string channelId, ExecuteCommand execCommand)
        {
            ChannelId = channelId;
            NodeId = execCommand.NodeId;
            CommandId = execCommand.CommandId;
            
            var command = execCommand.Command;

            if (command != null)
            {
                if (!string.IsNullOrEmpty(command.Id))
                {
                    CommandId = command.Id;
                }

                CommandName = command.Name;
            }
        }

        [DataMember]
        public string CommandId { get; set; }

        [DataMember]
        public string ChannelId { get; set; }

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
