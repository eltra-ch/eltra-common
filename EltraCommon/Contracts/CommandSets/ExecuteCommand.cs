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
        #region Private fields

        private DeviceCommand _command;

        #endregion

        #region Constructors

        /// <summary>
        /// ExecuteCommand
        /// </summary>
        public ExecuteCommand()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ExecuteCommand";

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
        /// SourceChannelId
        /// </summary>
        [DataMember]
        public string SourceChannelId { get; set; }

        /// <summary>
        /// SourceLoginName
        /// </summary>
        [DataMember]
        public string SourceLoginName { get; set; }

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
        
        #endregion
    }
}
