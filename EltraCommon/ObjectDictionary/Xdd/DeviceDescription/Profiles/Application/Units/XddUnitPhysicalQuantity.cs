using System;
using System.Collections.Generic;
using System.Xml;
using EltraCommon.Logger;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Units
{
    public class XddUnitPhysicalQuantity
    {
        private List<XddLabel> _labels;
        private List<XddUnit> _units;
        private readonly XddParameterList _parameterList;

        public XddUnitPhysicalQuantity(XddParameterList parameterList)
        {
            _parameterList = parameterList;
        }

        public string UniqueId { get; set; }

        public string Type { get; set; }

        public List<XddLabel> Labels => _labels ?? (_labels = new List<XddLabel>());

        public List<XddUnit> Units => _units ?? (_units = new List<XddUnit>());

        public bool Parse(XmlNode node)
        {
            bool result = true;

            var typeAttribute = node.Attributes["type"];
            var uniqueIdAttribute = node.Attributes["uniqueID"];

            if (uniqueIdAttribute != null)
            {
                UniqueId = uniqueIdAttribute.InnerXml;
            }
            else
            {
                throw new Exception($"{GetType().Name} - missing uniqueID attribute");
            }

            if (typeAttribute != null)
            {
                Type = typeAttribute.InnerXml;
            }
            else
            {
                throw new Exception($"{GetType().Name} - missing type attribute");
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "label")
                {
                    var label = new XddLabel(childNode);

                    if (!label.Parse())
                    {
                        result = false;
                        break;
                    }
                    
                    Labels.Add(label);
                }
                else if (childNode.Name == "unit")
                {
                    var unit = new XddUnit();

                    if (!unit.Parse(childNode))
                    {
                        result = false;
                    }

                    Units.Add(unit);
                }

                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        public XddUnit GetActiveUnit()
        {
            XddUnit result = null;

            if (Units.Count == 1)
            {
                result = Units[0];
            }
            else
            {
                foreach (var unit in Units)
                {
                    string uniqueIdRef = unit.GetConfigurationParameterIdRef();

                    var parameter = _parameterList.FindParameter(uniqueIdRef) as Parameter;

                    if (unit.HasMatchingValue(parameter))
                    {
                        result = unit;
                        break;
                    }
                    else
                    {
                        MsgLogger.WriteError($"{GetType().Name} - GetActiveUnit", $"GetActiveUnit - Parameter '{uniqueIdRef}' not found!");
                    }
                }
            }

            return result;
        }
    }
}
