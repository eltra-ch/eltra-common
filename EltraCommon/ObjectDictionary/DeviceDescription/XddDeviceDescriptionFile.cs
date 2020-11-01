using System;
using System.Xml;
using EltraCommon.Logger;
using EltraCommon.Contracts.Devices;
using System.Collections.Generic;
using EltraCommon.Contracts.ToolSet;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.DeviceDescription
{
    public class XddDeviceDescriptionFile : DeviceDescriptionFile
    {
        #region Private fields

        private List<DeviceTool> _deviceTools;

        #endregion

        #region Constructors

        public XddDeviceDescriptionFile(EltraDevice device) 
            : base(device)
        {            
        }

        #endregion

        #region Properties

        public List<DeviceTool> DeviceTools 
        { 
            get => _deviceTools ?? (_deviceTools = new List<DeviceTool>());
            set => _deviceTools = value;
        }

        #endregion

        #region Methods

        private XmlElement GetRootNode()
        {
            XmlElement result = null;

            try
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    var doc = new XmlDocument();

                    doc.LoadXml(Content);

                    result = doc.DocumentElement;
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - GetRootNode", e);
            }

            return result;
        }

        protected override bool ReadDeviceTools()
        {
            bool result = true;

            try
            {
                if (Device != null)
                {
                    var rootNode = GetRootNode();

                    var toolsNode = rootNode?.SelectNodes("Profile/ProfileBody/DeviceInteraction/DeviceTools/DeviceTool");

                    if (toolsNode != null)
                    {
                        foreach (XmlNode toolNode in toolsNode)
                        {
                            XmlAttribute uuidAttribute = null;

                            foreach(XmlAttribute att in toolNode.Attributes)
                            {
                                if(att.Name == "Uuid")
                                {
                                    uuidAttribute = att;
                                    break;
                                }
                                else if(att.Name == "uniqueID")
                                {
                                    uuidAttribute = att;
                                    break;
                                }
                            }

                            if(uuidAttribute==null)
                            {
                                throw new Exception("device tool uniqueID attribute not specified!");
                            }

                            var deviceTool = Device.FindTool(uuidAttribute.InnerXml);

                            if (deviceTool == null)
                            {
                                deviceTool = new DeviceTool();

                                Device.AddTool(deviceTool);
                            }
                            
                            var nameAttribute = toolNode.Attributes["name"];

                            if(nameAttribute == null)
                            {
                                nameAttribute = toolNode.Attributes["Name"];
                            }

                            var statusAttribute = toolNode.Attributes["status"];

                            if(statusAttribute == null)
                            {
                                statusAttribute = toolNode.Attributes["Status"];
                            }

                            AddDeviceTool(toolNode, deviceTool, uuidAttribute, nameAttribute, statusAttribute);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadDeviceTools", e);
                
                result = false;
            }

            return result;
        }

        private void AddDeviceTool(XmlNode toolNode, DeviceTool deviceTool, XmlAttribute uuidAttribute, XmlAttribute nameAttribute, XmlAttribute statusAttribute)
        {
            if (uuidAttribute != null)
            {
                deviceTool.Id = uuidAttribute.InnerText;

                if (nameAttribute != null)
                {
                    deviceTool.Name = nameAttribute.InnerText;

                    if (statusAttribute != null)
                    {
                        var statusText = statusAttribute.InnerText.ToLower();

                        switch (statusText)
                        {
                            case "enabled": deviceTool.Status = DeviceToolStatus.Enabled; break;
                            case "disabled": deviceTool.Status = DeviceToolStatus.Disabled; break;
                        }

                        foreach(XmlElement childNode in toolNode.ChildNodes)
                        {
                            if(childNode.Name == "Payload")
                            {
                                AddDeviceToolPayload(childNode, deviceTool);
                            }
                        }

                        DeviceTools.Add(deviceTool);
                    }
                }
            }
        }

        private void AddDeviceToolPayload(XmlElement payloadNode, DeviceTool deviceTool)
        {
            var fileNameAttribute = payloadNode.Attributes["fileName"];
            var hashCodeAttribute = payloadNode.Attributes["hashCode"];
            var uniqueIdAttribute = payloadNode.Attributes["uniqueID"];
            var versionAttribute = payloadNode.Attributes["version"];
            var modeAttribute = payloadNode.Attributes["mode"];

            if (fileNameAttribute==null)
            {
                fileNameAttribute = payloadNode.Attributes["FileName"];
            }

            if(hashCodeAttribute==null)
            {
                hashCodeAttribute = payloadNode.Attributes["HashCode"];
            }

            if (versionAttribute == null)
            {
                versionAttribute = payloadNode.Attributes["Version"];
            }

            if (uniqueIdAttribute != null && fileNameAttribute != null && hashCodeAttribute != null && versionAttribute != null)
            {
                var payload = new DeviceToolPayload() { Modified = DateTime.Now, Created = DateTime.Now };
                
                payload.Id = uniqueIdAttribute.InnerXml;
                payload.FileName = fileNameAttribute.InnerXml;
                payload.HashCode = hashCodeAttribute.InnerXml;
                payload.Version = versionAttribute.InnerXml;

                if(modeAttribute!=null)
                {
                    payload.Mode = modeAttribute.InnerXml;
                }

                deviceTool.AddPayload(payload);
            }
        }

        protected override void ReadProductName()
        {
            try
            {
                if (Device != null)
                {
                    var rootNode = GetRootNode();

                    var productNameNode = rootNode?.SelectSingleNode("Profile/ProfileBody/DeviceIdentity/productName");

                    if (productNameNode != null)
                    {
                        ProductName = productNameNode.InnerText;

                        Device.Identification.Name = ProductName;
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadProductName", e);
            }
        }

        protected override bool ReadDeviceVersion()
        {
            bool result = false;

            try
            {
                if (Device != null)
                {
                    var rootNode = GetRootNode();

                    var versionNodes = rootNode?.SelectNodes("Profile/ProfileBody/DeviceIdentity/version");

                    if (versionNodes != null)
                    {
                        var version = new DeviceVersion();

                        foreach (XmlNode versionNode in versionNodes)
                        {
                            var versionTypeAttribute = versionNode.Attributes["versionType"];

                            if (versionTypeAttribute != null)
                            {
                                switch (versionTypeAttribute.InnerText)
                                {
                                    case "SW": version.SoftwareVersion = Convert.ToUInt16(versionNode.InnerText, 16); break;
                                    case "HW": version.HardwareVersion = Convert.ToUInt16(versionNode.InnerText, 16); break;
                                    case "APPNB": version.ApplicationNumber = Convert.ToUInt16(versionNode.InnerText, 16); break;
                                    case "APPVER": version.ApplicationVersion = Convert.ToUInt16(versionNode.InnerText, 16); break;
                                }
                            }
                        }

                        Device.Version = version;

                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadDeviceVersion", e);
            }

            return result;
        }

        #endregion
    }
}
