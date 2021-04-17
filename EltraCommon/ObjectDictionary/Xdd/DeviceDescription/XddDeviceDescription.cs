using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using EltraCommon.Contracts.Devices;
using EltraCommon.Logger;
using EltraCommon.ObjectDictionary.Common.DeviceDescription;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription
{
    public class XddDeviceDescription : IDeviceDescription
    {
        #region Private fields

        private XddProfile _profile;
        private readonly EltraDevice _device;

        #endregion

        #region Constructors

        public XddDeviceDescription(EltraDevice device)
        {
            _device = device;
        }

        #endregion

        #region Properties

        public XddProfile Profile
        {
            get => _profile ?? (_profile = new XddProfile(_device));
            set => _profile = value;
        }
        
        public List<ParameterBase> Parameters { get; set; }

        public string DataSource { get; set; }

        #endregion

        #region Methods

        public virtual bool Parse()
        {
            bool result = false;

            try
            {
                var doc = new XmlDocument();

                if (File.Exists(DataSource))
                {
                    doc.Load(DataSource);
                    result = true;
                }
                else if(!string.IsNullOrEmpty(DataSource))
                {
                    doc.LoadXml(DataSource);
                    result = true;
                }

                if(result)
                {
                    var rootNode = doc.DocumentElement;

                    foreach (XmlNode childNode in rootNode.ChildNodes)
                    {
                        if (childNode.Name == "Profile")
                        {
                            result = Profile.Parse(childNode);
                            break;
                        }
                    }

                    if (result)
                    {
                        Parameters = Profile.ParameterList.Parameters;
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - Parse", e);

                result = false;
            }
            
            return result;
        }

        #endregion
    }
}
