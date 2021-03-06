﻿using System.Xml;
using EltraCommon.Contracts.Devices;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common;

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters
{
    class XddStructuredParameter : StructuredParameter
    {
        public XddStructuredParameter(EltraDevice device, XmlNode source)
            :base(device, source)
        {   
        }
        
        #region Methods

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

        #endregion
    }
}
