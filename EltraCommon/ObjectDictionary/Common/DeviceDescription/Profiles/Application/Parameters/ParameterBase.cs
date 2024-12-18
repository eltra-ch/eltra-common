﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Common;
using EltraCommon.Logger;
using EltraCommon.Contracts.Devices;
using System.Text.Json.Serialization;
using System.ComponentModel;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters
{
    [DataContract]
    public class ParameterBase
    {
        #region Private fields

        private List<Label> _labels;
        private List<ParameterBase> _parameters;
        private readonly XmlNode _source;

        #endregion

        #region Constructors

        public ParameterBase()
        {
        }

        public ParameterBase(EltraDevice device, XmlNode source)
        {
            _source = source;

            Device = device;
        }

        #endregion

        #region Properties

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ParameterBase";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        [IgnoreDataMember]
        [JsonIgnore]
        public EltraDevice Device { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        protected XmlNode Source => _source;

        [DataMember]
        public ushort Index { get; set; }

        [DataMember]
        protected List<Label> Labels { get => _labels ?? (_labels = new List<Label>()); }

        [DataMember]
        public string UniqueId { get; set; }

        [DataMember]
        public List<ParameterBase> Parameters => _parameters ?? (_parameters = new List<ParameterBase>());

        [DataMember]
        public string Label => GetLabel(RegionalOptions.Language);

        #endregion

        #region Methods

        public virtual bool Parse()
        {
            bool result = false;

            try
            {
                if (_source.Attributes != null)
                {
                    UniqueId = _source.Attributes["uniqueID"].InnerXml;

                    foreach (XmlNode childNode in _source.ChildNodes)
                    {
                        if (childNode.Name == "index")
                        {
                            var val = childNode.InnerText;
                            if (val.StartsWith("0x"))
                            {
                                Index = Convert.ToUInt16(val.Substring(2), 16);
                                result = true;
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

            return result;
        }
        
        public bool AddParameter(ParameterBase parameter)
        {
            bool result = false;

            if (parameter!=null)
            {
                Parameters.Add(parameter);
                result = true;
            }

            return result;
        }

        private string GetLabel(string lang)
        {
            string result = string.Empty;

            foreach (var label in Labels)
            {
                if (label.Lang == lang)
                {
                    result = label.Content;
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}
