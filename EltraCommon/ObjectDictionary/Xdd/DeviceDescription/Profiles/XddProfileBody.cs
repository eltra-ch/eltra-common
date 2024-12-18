﻿using System.Xml;
using EltraCommon.Contracts.Devices;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Device;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Identity;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles
{
    public class XddProfileBody
    {
        #region Private fields

        private XddDeviceIdentity _deviceIdentity;
        private XddApplicationProcess _applicationProcess;
        private XddDeviceManager _deviceManager;
        private readonly EltraDevice _device;

        #endregion

        #region Constructors

        public XddProfileBody(EltraDevice device)
        {
            _device = device;
        }

        #endregion

        #region Properties

        public XddApplicationProcess ApplicationProcess => _applicationProcess ?? (_applicationProcess = new XddApplicationProcess(_device, DeviceManager));

        public XddDeviceManager DeviceManager => _deviceManager ?? (_deviceManager = new XddDeviceManager());

        public XddDeviceIdentity DeviceIdentity => _deviceIdentity ?? (_deviceIdentity = new XddDeviceIdentity(_device));

        public XddParameterList ParameterList => ApplicationProcess.ParameterList;

        #endregion

        #region Methods

        public virtual bool Parse(XmlNode profileBodyNode)
        {
            bool result = true;

            foreach (XmlNode childNode in profileBodyNode.ChildNodes)
            {
                if (childNode.Name == "DeviceIdentity")
                {
                    if (!DeviceIdentity.Parse(childNode))
                    {
                        result = false;
                        break;
                    }
                }
                else if (childNode.Name == "ApplicationProcess")
                {
                    if (!ApplicationProcess.Parse(childNode))
                    {
                        result = false;
                        break;
                    }
                }
                else if (childNode.Name == "DeviceManager" && !DeviceManager.Parse(childNode))
                {
                    result = false;
                    break;
                }
            }

            DeviceManager.ResolveParameterReferences(ApplicationProcess.ParameterList);

            return result;
        }

        #endregion
    }
}
