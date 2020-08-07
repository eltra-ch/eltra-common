using System.Xml;
using EltraCommon.Contracts.Node;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters
{
    public class XddParameterBase : Parameter
    {
        public XddParameterBase(EltraDeviceNode device, XmlNode source)
            : base(device, source)
        {            
        }

        public override bool Parse()
        {
            bool result = base.Parse();

            if (result)
            {
                foreach (XmlNode childNode in Source.ChildNodes)
                {
                    if (childNode.Name == "label")
                    {
                        var label = new XddLabel(childNode);

                        if (label.Parse())
                        {
                            Labels.Add(label);
                        }
                    }
                }
            }

            return result;
        }
    }

}
