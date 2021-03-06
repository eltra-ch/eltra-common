﻿using System.Xml;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Units
{
    public class XddDecimalPlaces
    {
        public XddDecimalPlaces()
        {
            Value = 0;
        }

        public int Value { get; set; }

        public bool Parse(XmlNode node)
        {
            bool result = false;

            if (int.TryParse(node.InnerXml, out var value))
            {
                Value = value;
                result = true;
            }

            return result;
        }
    }
}
