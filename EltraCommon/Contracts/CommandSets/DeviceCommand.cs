using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using EltraCommon.Logger;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using EltraCommon.Contracts.Devices;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EltraCommon.Contracts.CommandSets
{
    /// <summary>
    /// DeviceCommand
    /// </summary>
    [DataContract]
    public class DeviceCommand
    {
        /// <summary>
        /// Default command timeout in miliseconds
        /// </summary>
        public const int DefaultTimeout = 10000;

        #region Private fields

        private List<DeviceCommandParameter> _parameters;

        #endregion

        #region Constructors

        /// <summary>
        /// DeviceCommand
        /// </summary>
        public DeviceCommand()
        {
            Header = DefaultHeader;
            Timeout = DefaultTimeout;
        }

        /// <summary>
        /// DeviceCommand
        /// </summary>
        /// <param name="device">{EltraDevice}</param>
        public DeviceCommand(EltraDevice device)
        {
            Header = DefaultHeader;
            Timeout = DefaultTimeout;
            Device = device;
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "ARP9";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// Command Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        
        /// <summary>
        /// Device - optional
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
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
        public ExecCommandStatus Status { get; set; }

        /// <summary>
        /// Timeout
        /// </summary>
        [DataMember]
        public int Timeout { get; set; }

        /// <summary>
        /// Parameter list
        /// </summary>
        [DataMember]
        public List<DeviceCommandParameter> Parameters
        {
            get => _parameters ?? (_parameters = new List<DeviceCommandParameter>());
            set => _parameters = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="sourceChannelId"></param>
        /// <param name="sourceLoginName"></param>
        /// <returns></returns>
        public virtual bool Execute(string sourceChannelId, string sourceLoginName)
        {
            return false;
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <returns>DeviceCommand</returns>
        public async Task<DeviceCommand> Execute()
        {
            DeviceCommand result = null;
            var connector = Device?.CloudConnector;

            if (connector != null)
            {
                result = await connector.ExecuteCommand(this);
            }

            return result;
        }

        /// <summary>
        /// Add parameter to command
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool AddParameter(DeviceCommandParameter parameter)
        {
            bool result = false;

            if (!ParameterExists(parameter))
            {
                Parameters.Add(parameter);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Add parameter to command
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="typeCode"></param>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        public bool AddParameter(string parameterName, TypeCode typeCode, ParameterType parameterType = ParameterType.In)
        {
            bool result = false;
            var parameter = new DeviceCommandParameter
                {Name = parameterName, Type = parameterType, DataType = new DataType {Type = typeCode}};

            if (!ParameterExists(parameter))
            {
                Parameters.Add(parameter);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// SetParameterValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        public bool SetParameterValue<T>(string parameterName, T parameterValue, ParameterType parameterType = ParameterType.In)
        {
            bool result = false;

            var parameter = FindParameterByName(Parameters, parameterName);

            if (parameter != null)
            {
                result = parameter.SetValue(parameterValue);
            }
            else
            {
                parameter = new DeviceCommandParameter { Name = parameterName };

                if (parameter.SetValue(parameterValue))
                {
                    result = AddParameter(parameter);
                }
            }

            return result;
        }

        /// <summary>
        /// ParameterExists
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool ParameterExists(DeviceCommandParameter parameter)
        {
            return FindParameterByName(Parameters, parameter.Name) != null;
        }

        private DeviceCommandParameter FindParameterByName(List<DeviceCommandParameter> parameters, string name)
        {
            DeviceCommandParameter result = null;

            foreach (var parameter in parameters)
            {
                if (parameter.Name.ToLower() == name.ToLower())
                {
                    result = parameter;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// GetParameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public DeviceCommandParameter GetParameter(string parameterName)
        {
            DeviceCommandParameter result = null;

            var parameter = FindParameterByName(Parameters, parameterName);

            if (parameter != null)
            {
                result = parameter;
            }
            
            return result;
        }

        /// <summary>
        /// GetParameterValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public bool GetParameterValue<T>(string parameterName, ref T parameterValue)
        {
            bool result = false;

            var parameter = FindParameterByName(Parameters, parameterName);

            if (parameter != null)
            {
                result = parameter.GetValue(ref parameterValue);
            }

            return result;
        }
        
        /// <summary>
        /// Sync command
        /// </summary>
        /// <param name="command"></param>
        public void Sync(DeviceCommand command)
        {
            try
            {
                if (command != null)
                {
                    if (command.Device != null)
                    {
                        Device = command.Device;
                    }

                    Parameters = command.Parameters;
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception("DeviceCommand - Sync", e);
            }
        }

        /// <summary>
        /// SetParameterDataType
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public bool SetParameterDataType(string parameterName, DataType dataType)
        {
            bool result = false;

            var parameter = FindParameterByName(Parameters, parameterName);

            if (parameter != null)
            {
                result = parameter.SetDataType(dataType);
            }

            return result;
        }

        /// <summary>
        /// SetParameterDataType
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public bool SetParameterDataType(string parameterName, TypeCode typeCode)
        {
            bool result = false;

            var parameter = FindParameterByName(Parameters, parameterName);

            if (parameter != null)
            {
                result = parameter.SetDataType(typeCode);
            }

            return result;
        }

        /// <summary>
        /// GetParameterDataType
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public bool GetParameterDataType(string parameterName, out TypeCode typeCode)
        {
            bool result = false;
            var parameter = FindParameterByName(Parameters, parameterName);

            typeCode = TypeCode.Object;

            if (parameter != null)
            {
                result = parameter.GetDataType(out typeCode);
            }

            return result;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public virtual DeviceCommand Clone()
        {
            Clone(out DeviceCommand result);
            
            return result;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clone"></param>
        /// <returns></returns>
        protected bool Clone<T>(out T clone)
        {
            bool result = false;
            
            clone = default;

            try
            {
                var serializer = new DataContractSerializer(typeof(T));
            
                using (Stream memoryStream = new MemoryStream())
                {
                    serializer.WriteObject(memoryStream, this);

                    memoryStream.Position = 0;

                    var deserializer = new DataContractSerializer(typeof(T));

                    clone = (T)deserializer.ReadObject(memoryStream);

                    if (clone is DeviceCommand deviceCommand)
                    {
                        deviceCommand.Device = Device;
                    }

                    result = true;
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception("DeviceCommand - Clone", e);    
            }
            
            return result;
        }

        #endregion
    }
}
