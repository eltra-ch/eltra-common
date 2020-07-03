using System.Xml;

using EltraCommon.ObjectDictionary.Common.DeviceDescription.Common;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Templates;

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Templates
{
    public class XddFlags : Flags
    {
        private readonly XmlNode _source;

        public XddFlags(XmlNode source)
        {
            _source = source;
        }
        
        #region Methods

        public bool Parse()
        {
            bool result = false;

            UniqueId = _source.Attributes["uniqueID"].InnerText;
            Name = _source.Attributes["name"].InnerText;
            
            foreach (XmlNode childNode in _source.ChildNodes)
            {
                if (childNode.Name == "Access")
                {
                    switch (childNode.InnerXml)
                    {
                        case "ro":
                            Access = AccessMode.ReadOnly;
                            break;
                        case "rw":
                            Access = AccessMode.ReadWrite;
                            break;
                        case "wo":
                            Access = AccessMode.WriteOnly;
                            break;
                    }

                    //TODO access WriteRestrictions att

                    result = true;    
                }
                else if (childNode.Name == "PdoMapping")
                {
                    //TODO flags PdoMapping
                }
                else if (childNode.Name == "Backup")
                {
                    if(int.TryParse(childNode.InnerXml, out int backup))
                    {
                        Backup = backup;
                    }
                }
                else if (childNode.Name == "Setting")
                {
                    if(int.TryParse(childNode.InnerXml, out int setting))
                    {
                        Setting = setting;
                    }
                }
                else if (childNode.Name == "Fieldbus1")
                {
                }
                else if (childNode.Name == "Fieldbus2")
                {
                }
            }

            return result;
        }

        #endregion
    }
}
