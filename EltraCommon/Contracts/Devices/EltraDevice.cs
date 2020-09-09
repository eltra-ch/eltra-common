using System;
using System.Runtime.Serialization;
using EltraCommon.Logger;
using EltraCommon.Contracts.CommandSets;
using EltraCommon.ObjectDictionary.Common;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.DeviceDescription.Factory;
using EltraCommon.ObjectDictionary.Factory;
using EltraCommon.Contracts.ToolSet;
using EltraCommon.ObjectDictionary.Common.DeviceDescription;
using EltraCommon.ObjectDictionary.DeviceDescription.Events;
using EltraCommon.ObjectDictionary.DeviceDescription;
using System.Threading.Tasks;
using EltraCommon.Contracts.Interfaces;
using System.Collections.Generic;

namespace EltraCommon.Contracts.Devices
{
    /// <summary>
    /// EltraDevice
    /// </summary>
    [DataContract]
    public class EltraDevice
    {
        #region Private fields

        private DeviceVersion _version;
        private DeviceCommandSet _commandSet;
        private DeviceToolSet _toolSet;
        private DeviceIdentification _deviceIdentification;
        private DeviceStatus _status;
        private IDeviceDescription _deviceDescription;

        #endregion

        #region Constructors

        /// <summary>
        /// EltraDevice
        /// </summary>
        public EltraDevice()
        {
            Modified = DateTime.Now;
            Created = DateTime.Now;
        }

        #endregion

        #region Interfaces

        /// <summary>
        /// Connector
        /// </summary>
        [IgnoreDataMember]
        public ICloudConnector CloudConnector { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// StatusChanged
        /// </summary>
        public event EventHandler StatusChanged;

        /// <summary>
        /// OnStatusChanged
        /// </summary>
        protected virtual void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Events handling

        private void OnDeviceDescriptionFileStateChanged(object sender, DeviceDescriptionFileEventArgs e)
        {
            if (e.State == DeviceDescriptionState.Read)
            {
                Status = DeviceStatus.DescriptionAvailable;

                if (!CreateDeviceDescription(e.DeviceDescriptionFile))
                {
                    MsgLogger.WriteError($"{GetType().Name} - OnDeviceDescriptionFileStateChanged", $"create device description failed!");
                }
            }
            else if (e.State == DeviceDescriptionState.Failed)
            {
                MsgLogger.WriteError($"{GetType().Name} - OnDeviceDescriptionFileStateChanged", $"device description read failed!, reason = '{e?.Exception?.Message}'");
            }
        }

        private void OnDeviceDescriptionChanged()
        {
            if (DeviceDescription != null)
            {
                if (!CreateObjectDictionary())
                {
                    MsgLogger.WriteError($"{GetType().Name} - OnDeviceDescriptionFileStateChanged", $"create object dictionary failed!");
                }
                else
                {
                    Name = DeviceDescription?.Profile?.ProfileBody?.DeviceIdentity?.ProductName;
                    ProductPicture = DeviceDescription?.Profile?.ProfileBody?.DeviceIdentity?.ProductPicture;
                }
            }
        }

        #endregion

        #region Properties

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
        /// Family
        /// </summary>
        [DataMember]
        public string Family { get; set; }

        /// <summary>
        /// Identification
        /// </summary>
        [DataMember]
        public DeviceIdentification Identification
        {
            get => _deviceIdentification ?? (_deviceIdentification = new DeviceIdentification());
            set => _deviceIdentification = value;
        }

        /// <summary>
        /// Version
        /// </summary>
        [DataMember]
        public DeviceVersion Version
        {
            get => _version ?? (_version = new DeviceVersion());
            set => _version = value;
        }

        /// <summary>
        /// CommandSet
        /// </summary>
        [DataMember]
        public DeviceCommandSet CommandSet => _commandSet ?? (_commandSet = new DeviceCommandSet());

        /// <summary>
        /// ToolSet
        /// </summary>
        [DataMember]
        public DeviceToolSet ToolSet => _toolSet ?? (_toolSet = new DeviceToolSet());

        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public DeviceStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnStatusChanged();
                }
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }

        /// <summary>
        /// ObjectDictionary - optional
        /// </summary>
        [IgnoreDataMember]
        public DeviceObjectDictionary ObjectDictionary { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// DeviceDescription - optional
        /// </summary>
        [IgnoreDataMember]
        public IDeviceDescription DeviceDescription
        {
            get => _deviceDescription;
            set 
            {
                if (_deviceDescription != value)
                {
                    _deviceDescription = value;
                    OnDeviceDescriptionChanged();
                }
            }
        }

        /// <summary>
        /// ProductPicture - optional
        /// </summary>
        [IgnoreDataMember]
        public string ProductPicture { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Add command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool AddCommand(DeviceCommand command)
        {
            bool result = CommandSet.AddCommand(command);

            return result;
        }

        /// <summary>
        /// FindCommand
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DeviceCommand FindCommand(DeviceCommand command)
        {
            var result = CommandSet.FindCommandByName(command.Name);

            return result;
        }

        /// <summary>
        /// FindCommand
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public DeviceCommand FindCommand(string commandName)
        {
            DeviceCommand result = null;

            try
            {
                result = CommandSet.FindCommandByName(commandName);
            }
            catch (Exception e)
            {
                MsgLogger.Exception("Device - FindCommand", e);
            }
            
            return result;
        }

        /// <summary>
        /// AddTool
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public bool AddTool(DeviceTool tool)
        {
            tool.Device = this;

            bool result = ToolSet.AddTool(tool);

            return result;
        }

        /// <summary>
        /// FindTool
        /// </summary>
        /// <param name="toolId"></param>
        /// <returns></returns>
        public DeviceTool FindTool(string toolId)
        {
            DeviceTool result = null;

            try
            {
                result = ToolSet.FindToolById(toolId);
            }
            catch (Exception e)
            {
                MsgLogger.Exception("Device - FindTool", e);
            }

            return result;
        }

        /// <summary>
        /// FindTool
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public DeviceTool FindTool(DeviceTool tool)
        {
            var result = ToolSet.FindToolById(tool.Id);

            return result;
        }

        /// <summary>
        /// RunAsync
        /// </summary>
        public virtual void RunAsync()
        {
        }

        /// <summary>
        /// ReadDeviceDescriptionFile
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> ReadDeviceDescriptionFile()
        {
            var deviceDescriptionFile = DeviceDescriptionFactory.CreateDeviceDescriptionFile(this);

            return await ReadDeviceDescriptionFile(deviceDescriptionFile);
        }

        /// <summary>
        /// ReadDeviceDescriptionFile
        /// </summary>
        /// <param name="deviceDescriptionFile"></param>
        /// <returns></returns>
        public async Task<bool> ReadDeviceDescriptionFile(DeviceDescriptionFile deviceDescriptionFile)
        {
            bool result = false;

            if (deviceDescriptionFile != null)
            {
                deviceDescriptionFile.StateChanged += OnDeviceDescriptionFileStateChanged;

                result = await deviceDescriptionFile.Read();
            }

            return result;
        }

        private bool CreateObjectDictionary()
        {
            bool result = false;

            lock (this)
            {
                if (Status != DeviceStatus.Ready || ObjectDictionary == null)
                {
                    if(Status == DeviceStatus.Ready && ObjectDictionary == null)
                    {
                        Status = DeviceStatus.DescriptionAvailable;
                    }

                    ObjectDictionary = ObjectDictionaryFactory.CreateObjectDictionary(this);

                    if (ObjectDictionary != null)
                    {
                        if (ObjectDictionary.Open())
                        {
                            Status = DeviceStatus.Ready;
                            result = true;
                        }
                        else
                        {
                            MsgLogger.WriteError("Device - CreateObjectDictionary", "Cannot open object dictionary!");
                            ObjectDictionary = null;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// SearchParameter
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public ParameterBase SearchParameter(string uniqueId)
        {
            ParameterBase result = null;

            if (ObjectDictionary!=null)
            {
                result = ObjectDictionary.SearchParameter(uniqueId);
            }

            return result;
        }

        /// <summary>
        /// SearchParameter
        /// </summary>
        /// <param name="index"></param>
        /// <param name="subIndex"></param>
        /// <returns></returns>
        public ParameterBase SearchParameter(ushort index, byte subIndex)
        {
            ParameterBase result = null;

            if (ObjectDictionary != null)
            {
                result = ObjectDictionary.SearchParameter(index, subIndex);
            }

            return result;
        }

        /// <summary>
        /// CreateDeviceDescription
        /// </summary>
        /// <param name="deviceDescriptionFile"></param>
        /// <returns></returns>
        public virtual bool CreateDeviceDescription(DeviceDescriptionFile deviceDescriptionFile)
        {
            bool result = false;
            var content = deviceDescriptionFile?.Content;

            if (content != null)
            {
                var deviceDescription = DeviceDescriptionFactory.CreateDeviceDescription(this, deviceDescriptionFile);

                if (deviceDescription.Parse())
                {
                    DeviceDescription = deviceDescription;

                    result = true;
                }
                else
                {
                    MsgLogger.WriteError($"{GetType().Name} - CreateDeviceDescription", "Parsing device description failed!");
                }
            }
            else
            {
                MsgLogger.WriteError($"{GetType().Name} - CreateDeviceDescription", "Content is empty!");
            }

            return result;
        }

        /// <summary>
        /// Get commands supported by device.
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceCommand>> GetCommands()
        {
            var result = new List<DeviceCommand>();

            if (CloudConnector!=null)
            {
                result = await CloudConnector.GetDeviceCommands(this);
            }

            return result;
        }

        /// <summary>
        /// Get device command.
        /// </summary>
        /// <param name="commandName">Command name.</param>
        /// <returns>DeviceCommand</returns>
        public async Task<DeviceCommand> GetCommand(string commandName)
        {
            DeviceCommand result = null;

            if (CloudConnector != null)
            {
                result = await CloudConnector.GetDeviceCommand(this, commandName);
            }

            return result;
        }

        /// <summary>
        /// Get parameter by index, subindex.
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="subIndex">Subindex</param>
        /// <returns></returns>
        public async Task<Parameter> GetParameter(ushort index, byte subIndex)
        {
            Parameter result = null;

            if (CloudConnector != null)
            {
                result = await CloudConnector.GetParameter(this, index, subIndex);
            }

            return result;
        }

        /// <summary>
        /// Get parameter by uniqueid.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public async Task<Parameter> GetParameter(string uniqueId)
        {
            Parameter result = null;

            if (CloudConnector != null)
            {
                var parameterBase = SearchParameter(uniqueId);

                if (parameterBase is Parameter parameter)
                {
                    result = await CloudConnector.GetParameter(this, parameter.Index, parameter.SubIndex);
                }
                else if (parameterBase != null)
                {
                    result = await CloudConnector.GetParameter(this, parameterBase.Index, 0x00);
                }
            }

            return result;
        }

        #endregion
    }
}
