using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using System.Xml;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common
{
    public class XddValueEntry
    {
        private readonly DataType _dataType;

        public XddValueEntry(DataType dataType)
        {
            _dataType = dataType;
        }

        public XddValue Value { get; set; }

        public XddParamIdRef ParamIdRef { get; set; }

        public bool Parse(XmlNode node)
        {
            bool result = true;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "value")
                {
                    var value = new XddValue(_dataType);

                    if (!value.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    Value = value;
                }
                else if (childNode.Name == "paramIDRef")
                {
                    var paramIdRef = new XddParamIdRef();

                    if (!paramIdRef.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    ParamIdRef = paramIdRef;
                }
            }

            return result;
        }
    }
}
