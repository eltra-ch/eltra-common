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

                    var deviceInteractionNode = rootNode?.SelectSingleNode("Profile/ProfileBody/DeviceInteraction");

                    if(deviceInteractionNode!=null)
                    {
                        var payloadList = new List<DeviceToolPayload>();

                        foreach (XmlNode childNode in deviceInteractionNode.ChildNodes)
                        {
                            if (childNode.Name.ToLower() == "payloadlist")
                            {
                                ReadPayloadList(childNode, ref payloadList);
                            }
                            else if (childNode.Name.ToLower() == "devicetools")
                            {
                                ReadDeviceTools(childNode);
                            }
                        }

                        ResolveDeviceToolReferences(payloadList);
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

        private bool ReadDeviceTools(XmlNode deviceToolsNode)
        {
            bool result = true;

            try
            {
                if (Device != null)
                {
                    foreach (XmlNode childNode in deviceToolsNode.ChildNodes)
                    {                        
                        if (childNode.Name.ToLower() == "devicetool")
                        {
                            ReadDeviceTool(childNode);
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

        private DeviceToolPayload SearchPayload(List<DeviceToolPayload> deviceToolPayloads, string id)
        {
            DeviceToolPayload payload = null;

            foreach (var deviceToolPayload in deviceToolPayloads)
            {
                if (deviceToolPayload.Id == id)
                {
                    payload = deviceToolPayload;
                    break;
                }
            }

            return payload;
        }

        private void ResolveDeviceToolReferences(List<DeviceToolPayload> deviceToolPayloads)
        {
            foreach(var deviceTool in DeviceTools)
            {
                foreach(var payload in deviceTool.PayloadSet)
                {
                    if(payload.Created == DateTime.MinValue)
                    {
                        var deviceToolPayload = SearchPayload(deviceToolPayloads, payload.Id);

                        if(deviceToolPayload != null)
                        {
                            CopyPayload(deviceToolPayload, payload);
                        }
                    }
                }
            }
        }

        private void CopyPayload(DeviceToolPayload source, DeviceToolPayload target)
        {
            if (target != null && source != null)
            {
                target.ChannelId = source.ChannelId;
                target.NodeId = source.NodeId;
                target.ToolId = source.ToolId;
                target.FileName = source.FileName;
                target.Content = source.Content;
                target.HashCode = source.HashCode;
                target.Version = source.Version;
                target.Mode = source.Mode;
                target.Type = source.Type;
                target.Created = source.Created;
                target.Modified = source.Modified;
            }
        }

        private void ReadPayloadList(XmlNode payloadListNode, ref List<DeviceToolPayload> payloadList)
        {
            if(payloadListNode != null)
            {
                foreach(XmlNode childNode in payloadListNode.ChildNodes)
                {
                    if(childNode.Name.ToLower() == "payload" && childNode is XmlElement payloadNode)
                    {
                        var payload = ReadDeviceToolPayload(payloadNode);

                        payloadList.Add(payload);
                    }
                }
            }
        }

        private bool ReadDeviceTool(XmlNode toolNode)
        {
            bool result = false;

            try
            {

                XmlAttribute uniqueIdAttribute = null;

                foreach (XmlAttribute att in toolNode.Attributes)
                {
                    if (att.Name.ToLower() == "uuid")
                    {
                        uniqueIdAttribute = att;
                        break;
                    }
                    else if (att.Name.ToLower() == "uniqueID")
                    {
                        uniqueIdAttribute = att;
                        break;
                    }
                }

                if (uniqueIdAttribute == null)
                {
                    throw new Exception("device tool uniqueID attribute not specified!");
                }

                var deviceTool = Device.FindTool(uniqueIdAttribute.InnerXml);

                if (deviceTool == null)
                {
                    deviceTool = new DeviceTool();

                    Device.AddTool(deviceTool);
                }

                var nameAttribute = toolNode.Attributes["name"];

                if (nameAttribute == null)
                {
                    nameAttribute = toolNode.Attributes["Name"];
                }

                var statusAttribute = toolNode.Attributes["status"];

                if (statusAttribute == null)
                {
                    statusAttribute = toolNode.Attributes["Status"];
                }

                AddDeviceTool(toolNode, deviceTool, uniqueIdAttribute, nameAttribute, statusAttribute);
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadDeviceTool", e);

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
                            if(childNode.Name.ToLower() == "payload")
                            {
                                var payload = ReadDeviceToolPayload(childNode);

                                if (payload == null)
                                {
                                    payload = ReadBareDeviceToolPayload(childNode);
                                }
                                
                                if(payload!=null)
                                { 
                                    deviceTool.AddPayload(payload);
                                }
                            }                            
                        }

                        DeviceTools.Add(deviceTool);
                    }
                }
            }
        }
        
        private DeviceToolPayload ReadDeviceToolPayload(XmlElement payloadNode)
        {
            DeviceToolPayload result = null;

            try
            {
                var payload = ReadBareDeviceToolPayload(payloadNode);

                var fileNameAttribute = payloadNode.Attributes["fileName"];
                var hashCodeAttribute = payloadNode.Attributes["hashCode"];                
                var versionAttribute = payloadNode.Attributes["version"];
                var modeAttribute = payloadNode.Attributes["mode"];
                var typeAttribute = payloadNode.Attributes["type"];

                if (fileNameAttribute == null)
                {
                    fileNameAttribute = payloadNode.Attributes["FileName"];
                }

                if (hashCodeAttribute == null)
                {
                    hashCodeAttribute = payloadNode.Attributes["HashCode"];
                }

                if (versionAttribute == null)
                {
                    versionAttribute = payloadNode.Attributes["Version"];
                }

                if (payload != null && fileNameAttribute != null &&
                    hashCodeAttribute != null && versionAttribute != null && typeAttribute != null)
                {
                    payload.FileName = fileNameAttribute.InnerXml;
                    payload.HashCode = hashCodeAttribute.InnerXml;
                    payload.Version = versionAttribute.InnerXml;
                    payload.Type = typeAttribute.InnerXml;

                    if (modeAttribute != null)
                    {
                        string mode = modeAttribute.InnerXml.ToLower();

                        switch (mode)
                        {
                            case "development":
                                payload.Mode = DeviceToolPayloadMode.Development;
                                break;
                            case "production":
                                payload.Mode = DeviceToolPayloadMode.Production;
                                break;
                            default:
                                payload.Mode = DeviceToolPayloadMode.Undefined;
                                break;
                        }
                    }
                    else
                    {
                        payload.Mode = DeviceToolPayloadMode.Production;
                    }

                    result = payload;
                }
            }
            catch(Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadDeviceToolPayload", e);
            }

            return result;
        }

        private DeviceToolPayload ReadBareDeviceToolPayload(XmlElement payloadNode)
        {
            DeviceToolPayload result = null;

            try
            {
                var uniqueIdAttribute = payloadNode.Attributes["uniqueID"];
                var uniqueIdRefAttribute = payloadNode.Attributes["uniqueIDRef"];

                if (uniqueIdAttribute != null)
                {
                    var payload = new DeviceToolPayload() { Created = DateTime.Now, Modified = DateTime.Now };

                    payload.Id = uniqueIdAttribute.InnerXml;

                    result = payload;
                }
                else if (uniqueIdRefAttribute != null)
                {
                    var payload = new DeviceToolPayload();

                    payload.Id = uniqueIdRefAttribute.InnerXml;

                    result = payload;
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadDeviceToolPayloadRef", e);
            }

            return result;
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
