﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EltraCommon.Contracts.Devices;
using EltraCommon.ObjectDictionary.DeviceDescription.Events;
using EltraCommon.Helpers;
using EltraCommon.Logger;
using EltraCommon.Contracts.Interfaces;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.DeviceDescription
{
    public class DeviceDescriptionFile
    {
        #region Private fields

        private string _content;

        #endregion

        #region Constructors

        public DeviceDescriptionFile(EltraDevice device)
        {
            Device = device;
        }

        #endregion

        #region Properties

        public string Url { get; set; }

        public string SourceFile { get; set; }
        
        public string Content 
        {
            get => _content;
            set
            {
                _content = value;

                OnContentChanged();
            }
        }

        public string ProductName { get; set; }

        public EltraDevice Device { get; }
        
        public string HashCode
        {
            get
            {
                return CryptHelpers.ToMD5(Content);
            }
        }

        public string Encoding
        {
            get
            {
                return "MD5";
            }
        }

        #endregion

        #region Events

        public event EventHandler<DeviceDescriptionFileEventArgs> StateChanged;

        #endregion

        #region Events handling

        protected virtual void OnDeviceDescriptionStateChanged(DeviceDescriptionFileEventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }

        private void OnContentChanged()
        {
            if (!string.IsNullOrEmpty(Content))
            {
                ReadProductName();

                if (!ReadDeviceVersion())
                {
                    MsgLogger.WriteError($"{GetType().Name} - OnContentChanged", "read device version failed!");
                }

                if (!ReadDeviceTools())
                {
                    MsgLogger.WriteError($"{GetType().Name} - OnContentChanged", "read device tools failed!");
                }

                OnDeviceDescriptionStateChanged(new DeviceDescriptionFileEventArgs { DeviceDescriptionFile = this, State = DeviceDescriptionState.Read });
            }
        }

        #endregion

        #region Methods

        private async Task<DeviceDescriptionPayload> Download(ICloudConnector connector, DeviceVersion deviceVersion)
        {
            DeviceDescriptionPayload result = null;

            try
            {
                if (connector != null)
                {
                    result = await connector.DownloadDeviceDescription(deviceVersion);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - Download", e);
            }

            return result;
        }
        
        public virtual async Task<bool> Read()
        {
            bool result = false;
            
            if (File.Exists(SourceFile))
            {
                if (string.IsNullOrEmpty(Content))
                {
                    result = ReadFile();
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = await DownloadFile();
            }

            return result;
        }

        private async Task<bool> DownloadFile()
        {
            bool result = false;
            var deviceDescription = await Download(Device.CloudConnector, Device.Version);

            if (deviceDescription != null)
            {
                Content = deviceDescription.PlainContent;

                if (!string.IsNullOrEmpty(Content))
                {
                    result = true;

                    OnDeviceDescriptionStateChanged(new DeviceDescriptionFileEventArgs { DeviceDescriptionFile = this, State = DeviceDescriptionState.Read });
                }
            }

            return result;
        }

        protected virtual void ReadProductName()
        {
        }

        protected virtual bool ReadDeviceVersion()
        {
            return false;
        }

        protected virtual bool ReadDeviceTools()
        {
            return true; //optional
        }

        private bool ReadFile()
        {
            bool result = false;

            try
            {
                if (File.Exists(SourceFile))
                {
                    Content = File.ReadAllText(SourceFile);

                    if (!string.IsNullOrEmpty(Content))
                    {
                        result = true;
                    }
                }
                else if(!string.IsNullOrEmpty(SourceFile))
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ReadFile", $"device description file '{SourceFile}'");
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ReadFile", $"device description file not spefified!");
                }
            }
            catch (Exception e)
            {
                OnDeviceDescriptionStateChanged(new DeviceDescriptionFileEventArgs { DeviceDescriptionFile = this, State = DeviceDescriptionState.Failed, Exception = e });
            }

            return result;
        }

        #endregion
    }
}
