using System.Xml;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Units
{
    public class XddUnitConfigurationValue
    {
        private readonly DataType _dataType;

        public XddUnitConfigurationValue(DataType dataType)
        {
            _dataType = dataType;
        }

        public XddParamIdRef ParamIdRef { get; set; }

        public XddValueEntry ValueEntry { get; set; }

        public bool Parse(XmlNode node)
        {
            bool result = true;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "paramIDRef")
                {
                    var paramIdRef = new XddParamIdRef();

                    if (!paramIdRef.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    ParamIdRef = paramIdRef;
                }
                else if (childNode.Name == "valueEntry")
                {
                    var valueEntry = new XddValueEntry(_dataType);

                    if (!valueEntry.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    ValueEntry = valueEntry;
                }
            }

            return result;
        }
    }
}
