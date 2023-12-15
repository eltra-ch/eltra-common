using EltraCommon.Logger;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    /// <summary>
    /// DeviceCommandSet
    /// </summary>
    [DataContract]
    public class DeviceCommandList
    {
        #region Private fields

        private List<DeviceCommand> _commands;

        #endregion

        #region Constructors

        /// <summary>
        /// DeviceCommandSet
        /// </summary>
        public DeviceCommandList()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "DeviceCommandList";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        /// <summary>
        /// Commands list
        /// </summary>
        [DataMember]
        public List<DeviceCommand> Commands => _commands ?? (_commands = new List<DeviceCommand>());

        #endregion

        #region Methods

        /// <summary>
        /// AddCommand
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool AddCommand(DeviceCommand command)
        {
            bool result = false;

            if (!CommandExists(command))
            {
                Commands.Add(command);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// CommandExists
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool CommandExists(DeviceCommand command)
        {
            return FindCommandByName(command.Name) != null;
        }

        /// <summary>
        /// FindCommandByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DeviceCommand FindCommandByName(string name)
        {
            DeviceCommand result = null;

            if (!string.IsNullOrEmpty(name))
            {
                foreach (var command in Commands)
                {
                    if (command.Name.ToLower() == name.ToLower())
                    {
                        result = command;
                        break;
                    }
                }
            }
            else
            {
                MsgLogger.WriteError($"{GetType().Name} - FindCommandByName", $"name not specified!");
            }

            return result;
        }

        #endregion
    }
}
