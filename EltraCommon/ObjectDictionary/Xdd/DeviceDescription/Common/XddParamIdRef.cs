using System.Xml;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common
{
    public class XddParamIdRef
    {
        public string UniqueIdRef { get; set; }
        
        public bool Parse(XmlNode childNode)
        {
            bool result = false;
            var uniqueIdAttribute = childNode.Attributes?["uniqueIDRef"];

            if (uniqueIdAttribute != null)
            {
                UniqueIdRef = uniqueIdAttribute.InnerXml;

                result = true;
            }

            return result;
        }
    }
}
