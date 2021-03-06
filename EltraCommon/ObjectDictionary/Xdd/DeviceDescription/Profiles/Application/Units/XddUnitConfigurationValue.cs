﻿using System.Xml;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Units
{
    public class XddUnitConfigurationValue
    {
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
                    var valueEntry = new XddValueEntry();

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
