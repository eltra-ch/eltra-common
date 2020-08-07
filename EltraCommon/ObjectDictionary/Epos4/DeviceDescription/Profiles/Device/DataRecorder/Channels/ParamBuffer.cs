using System.Xml;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles.Device.DataRecorder.Channels
{
    public class ParamBuffer
    {
        #region Properties

        public XddParamIdRef ParamIdRef { get; set; }
        public Parameter Parameter { get; set; }
        
        #endregion

        #region Methods

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
            }

            return result;
        }

        public void Resolve(XddParameterList parameterList)
        {
            Parameter = parameterList.FindParameter(ParamIdRef.UniqueIdRef) as Parameter;
        }

        #endregion


    }
}
