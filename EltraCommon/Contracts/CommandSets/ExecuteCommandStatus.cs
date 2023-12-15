using System;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    /// <summary>
    /// ExecuteCommandStatus
    /// </summary>
    [DataContract]
    public class ExecuteCommandStatus
    {
        /// <summary>
        /// ExecuteCommandStatus
        /// </summary>
        public ExecuteCommandStatus()
        {
        }

        /// <summary>
        /// ExecuteCommandStatus
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="execCommand"></param>
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

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ExecuteCommandStatus";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// CommandId
        /// </summary>
        [DataMember]
        public string CommandId { get; set; }

        /// <summary>
        /// ChannelId
        /// </summary>
        [DataMember]
        public string ChannelId { get; set; }

        /// <summary>
        /// NodeId
        /// </summary>
        [DataMember]
        public int NodeId { get; set; }

        /// <summary>
        /// CommandName
        /// </summary>
        [DataMember]
        public string CommandName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public ExecCommandStatus Status { get; set; }

        /// <summary>
        /// CommStatus
        /// </summary>
        [DataMember]
        public ExecCommandCommStatus CommStatus { get; set; }

        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }
    }
}
