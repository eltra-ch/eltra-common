﻿using System.Collections.Generic;
using System.Xml;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Units;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Units
{
    public class XddUnit : Unit
    {
        #region Private fields

        private readonly DataType _dataType;

        private List<XddLabel> _labels;
        private XddDecimalPlaces _epos4DecimalPlaces;
        private XddMultiplier _epos4Multiplier;

        #endregion

        #region Constructors

        public XddUnit()
        {   
        }

        public XddUnit(DataType dataType)
        {
            _dataType = dataType;
        }

        #endregion

        #region Properties

        private List<XddLabel> Labels => _labels ?? (_labels = new List<XddLabel>());

        private XddMultiplier Epos4Multiplier
        {
            get => _epos4Multiplier ?? (_epos4Multiplier = new XddMultiplier());
            set => _epos4Multiplier = value;
        }

        public XddDecimalPlaces Epos4DecimalPlaces
        {
            get => _epos4DecimalPlaces ?? (_epos4DecimalPlaces = new XddDecimalPlaces());
            set => _epos4DecimalPlaces = value;
        }

        public XddUnitConfiguration Configuration { get; set; }

        #endregion

        #region Methods

        protected override double GetMultiplier()
        {
            return Epos4Multiplier.Value;
        }

        protected override int GetDecimalPlaces()
        {
            return Epos4DecimalPlaces.Value;
        }

        public bool Parse(XmlNode node)
        {
            bool result = true;
            var uniqueIdAttribute = node.Attributes["uniqueID"];

            if(uniqueIdAttribute!=null)
            {
                UniqueId = uniqueIdAttribute.InnerXml;
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
                else if (childNode.Name == "multiplier")
                {
                    var epos4Multiplier = new XddMultiplier();

                    if (!epos4Multiplier.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    Epos4Multiplier = epos4Multiplier;
                }
                else if(childNode.Name == "configuration")
                {
                    Configuration = new XddUnitConfiguration(_dataType);

                    if (!Configuration.Parse(childNode))
                    {
                        result = false;
                    }
                }
                else if(childNode.Name == "decimalPlaces")
                {
                    var epos4DecimalPlaces = new XddDecimalPlaces();

                    if (!epos4DecimalPlaces.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    Epos4DecimalPlaces = epos4DecimalPlaces;
                }

                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        protected override string GetLabel(string language)
        {
            string result = string.Empty;

            foreach (var label in Labels)
            {
                if (label.Lang == language)
                {
                    result = label.Content;
                    break;
                }
            }

            return result;
        }

        public string GetConfigurationParameterIdRef()
        {
            return Configuration.ConfigurationValue.ParamIdRef.UniqueIdRef;
        }

        public bool HasMatchingValue(Parameter parameter)
        {
            bool result = false;

            string configurationValue = Configuration.GetConfigurationValue();
            var actualValue = parameter?.ActualValue;

            if (actualValue != null && configurationValue == actualValue.Value)
            {
                result = true;
            }

            return result;
        }

        #endregion
    }
}
