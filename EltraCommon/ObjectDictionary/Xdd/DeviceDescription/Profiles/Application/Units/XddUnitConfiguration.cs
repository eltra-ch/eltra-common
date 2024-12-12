using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using System.Xml;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Units
{
    public class XddUnitConfiguration
    {
        private readonly DataType _dataType;

        public XddUnitConfigurationValue ConfigurationValue { get; set; }
        
        public XddUnitConfiguration(DataType dataType)
        {
            _dataType = dataType;
        }

        public bool Parse(XmlNode node)
        {
            bool result = true;

            foreach (XmlNode childNode in node)
            {
                if (childNode.Name == "configurationValue")
                {
                    var configurationValue = new XddUnitConfigurationValue(_dataType);

                    if (!configurationValue.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    ConfigurationValue = configurationValue;
                }
            }

            return result;
        }

        public string GetConfigurationValue()
        {
            string result = string.Empty;

            var valueEntry = ConfigurationValue?.ValueEntry;

            var valueEntryValue = valueEntry?.Value;

            if (valueEntryValue != null)
            {
                result = valueEntryValue.Value;
            }

            return result;
        }
    }
}
