﻿using System;
using System.Xml;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.DataTypes;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Templates;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Units;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Device;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Device.UserLevels;
using EltraCommon.Logger;
using EltraCommon.Contracts.Devices;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters
{
    public class XddParameter : XddParameterBase
    {
        #region Private fields

        private const string DefaultDiscriminator = "XddParameter";

        private XddUserLevelList _userLevels;

        private readonly XddTemplateList _templateList;
        private readonly XddDataTypeList _dataTypeList;
        private readonly XddDeviceManager _deviceManager;

        #endregion

        #region Constructors

        public XddParameter()
            : base()
        {
            Discriminator = DefaultDiscriminator;
            DataType = new DataType();
            Unit = new XddUnit(DataType);
        }

        public XddParameter(EltraDevice device, XmlNode source, XddDeviceManager deviceManager, XddDataTypeList dataTypeList, XddTemplateList templateList)
            : base(device, source)
        {
            Discriminator = DefaultDiscriminator;
            DataType = new DataType();

            _deviceManager = deviceManager;
            _templateList = templateList;
            _dataTypeList = dataTypeList;

            Unit = new XddUnit(DataType);
        }

        #endregion

        #region Properties
        
        public XddUserLevelList UserLevels { get => _userLevels ?? (_userLevels = new XddUserLevelList(_deviceManager)); }

        #endregion

        #region Methods

        public bool VisibleBy(string role)
        {
            return UserLevels.HasRole(role);
        }

        public bool VisibleByAny()
        {
            return UserLevels.IsRoleUndefined;
        }

        public override bool Parse()
        {
            bool result = base.Parse();

            if (result)
            {
                try
                {
                    if (Source.Attributes != null)
                    {
                        foreach (XmlNode childNode in Source.ChildNodes)
                        {
                            if (childNode.Name == "dataType" || childNode.Name == "dataTypeIDRef")
                            {
                                DataType = new XddDataType(childNode, _dataTypeList);

                                if (!DataType.Parse())
                                {
                                    result = false;
                                    break;
                                }
                            }
                            else if (childNode.Name == "flagsIDRef")
                            {
                                if (childNode.Attributes != null)
                                {
                                    var uniqueId = childNode.Attributes["uniqueIDRef"].InnerXml;

                                    Flags = _templateList.FindFlags(uniqueId);
                                }
                            }
                            else if (childNode.Name == "userLevels")
                            {
                                if (!UserLevels.Parse(childNode))
                                {
                                    result = false;
                                    break;
                                }
                            }
                            else if (childNode.Name == "defaultValue")
                            {
                                var defaultValue = new XddDefaultValue(childNode, DataType, _dataTypeList);

                                if (!defaultValue.Parse())
                                {
                                    result = false;
                                    break;
                                }

                                DefaultValue = defaultValue;
                                ActualValue = defaultValue.Clone();
                            }
                            else if (childNode.Name == "unit")
                            {
                                var unit = new XddUnit(DataType);

                                if (!unit.Parse(childNode))
                                {
                                    result = false;
                                    break;
                                }

                                Unit = unit;
                            }
                            else if (childNode.Name == "unitPhysicalQuantityIDRef")
                            {
                                if (childNode.Attributes != null)
                                {
                                    var uniqueIdRef = childNode.Attributes["uniqueIDRef"].InnerXml;

                                    Unit = new XddUnitReference { UniqueId = uniqueIdRef };
                                }
                            }
                            else if (childNode.Name == "allowedValues")
                            {
                                var allowedValues = new XddAllowedValues(_dataTypeList, DataType);

                                if (!allowedValues.Parse(childNode))
                                {
                                    result = false;
                                    break;
                                }

                                AllowedValues.Add(allowedValues);
                            }
                            else if (childNode.Name == "allowedValuesIDRef" && childNode.Attributes != null)
                            {
                                var uniqueIdRef = childNode.Attributes["uniqueIDRef"].InnerXml;

                                var allowedValues = _templateList.FindAllowedValues(uniqueIdRef);

                                if (allowedValues != null)
                                {
                                    AllowedValues.Add(allowedValues);
                                }
                            }

                            if (!result)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MsgLogger.Exception($"{GetType().Name} - Parse", e);
                }
            }
            
            return result;
        }

        #endregion
    }
}
