﻿using System;
using System.Runtime.Serialization;
using System.Text;
using EltraCommon.Logger;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters.Events;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Templates;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.DataTypes;
using System.Collections.Generic;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters;
using System.Xml;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Units;
using EltraCommon.Contracts.Devices;
using System.Threading.Tasks;
using EltraCommon.Contracts.History;
using EltraCommon.Converters;
using EltraCommon.Contracts.Parameters;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters
{
    [DataContract]
    public class Parameter : ParameterBase
    {
        #region Private fields

        private const string DefaultDiscriminator = "Parameter";

        private ParameterValue _defaultValue;
        private ParameterValue _actualValue;
        private List<XddAllowedValues> _allowedValues;

        #endregion

        #region Constructors

        public Parameter()
            : base()
        {
            Discriminator = DefaultDiscriminator;
            DateTimeFormat = "dd MMM HH:mm:ss";
        }

        public Parameter(EltraDevice device, XmlNode source)
            : base(device, source)
        {
            Discriminator = DefaultDiscriminator;
            DateTimeFormat = "dd MMM HH:mm:ss";
        }

        #endregion

        #region Properties

        [DataMember]
        public byte SubIndex { get; set; }

        [DataMember]
        public string DisplayFormat { get; set; }

        [DataMember]
        public DataType DataType { get; set; }

        [DataMember]
        public Flags Flags { get; set; }

        [DataMember]
        public ParameterValue DefaultValue
        {
            get => _defaultValue ?? (_defaultValue = new ParameterValue());
            set { _defaultValue = value; }
        }

        [DataMember]
        public ParameterValue ActualValue
        {
            get => _actualValue ?? (_actualValue = new ParameterValue());
            set
            {
                var oldValue = ActualValue.Clone();

                if (!value.Equals(ActualValue))
                {
                    _actualValue = value;

                    OnParameterChanged(new ParameterChangedEventArgs(this, oldValue, value));
                }
            }
        }

        [DataMember]
        public string DateTimeFormat { get; set; }

        [DataMember]
        public List<XddAllowedValues> AllowedValues => _allowedValues ?? (_allowedValues = new List<XddAllowedValues>());

        [DataMember]
        public Unit Unit { get; set; }

        #endregion

        #region Events

        public event EventHandler<ParameterChangedEventArgs> ParameterChanged;

        public event EventHandler<ParameterWrittenEventArgs> ParameterWritten;

        #endregion

        #region Event handler
        protected virtual void OnParameterChanged(ParameterChangedEventArgs e)
        {
            ParameterChanged?.Invoke(this, e);
        }

        protected virtual void OnParameterWritten(ParameterWrittenEventArgs e)
        {
            ParameterWritten?.Invoke(this, e);
        }

        #endregion

        #region Methods

        public override bool Parse()
        {
            bool result = base.Parse();

            if (result)
            {
                try
                {
                    if (Source.Attributes != null)
                    {
                        UniqueId = Source.Attributes["uniqueID"].InnerXml;

                        foreach (XmlNode childNode in Source.ChildNodes)
                        {
                            if (childNode.Name == "subIndex")
                            {
                                var val = childNode.InnerText;
                                if (val.StartsWith("0x"))
                                {
                                    SubIndex = Convert.ToByte(val.Substring(2), 16);
                                }
                            }
                            else if (childNode.Name == "displayFormat")
                            {
                                DisplayFormat = childNode.InnerText.Trim();
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

        private bool IsParameterValueValid(ParameterValue newValue)
        {
            bool result = false;

            if(newValue != null && !string.IsNullOrEmpty(newValue.Value))
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(newValue.Value);

                if (Base64Converter.TryFromBase64String(newValue.Value, bytesArray, out int bytesWritten))
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Boolean:
                            {
                                result = bytesWritten >= sizeof(Boolean);
                            }
                            break;
                        case TypeCode.Byte:
                            {
                                result = bytesWritten >= sizeof(Byte);
                            }
                            break;
                        case TypeCode.SByte:
                            {
                                result = bytesWritten >= sizeof(SByte);
                            }
                            break;
                        case TypeCode.Int16:
                            {
                                result = bytesWritten >= sizeof(Int16);
                            }
                            break;
                        case TypeCode.Int32:
                            {
                                result = bytesWritten >= sizeof(Int32);
                            }
                            break;
                        case TypeCode.Int64:
                            {
                                result = bytesWritten >= sizeof(Int64);
                            }
                            break;
                        case TypeCode.UInt16:
                            {
                                result = bytesWritten >= sizeof(UInt16);
                            }
                            break;
                        case TypeCode.UInt32:
                            {
                                result = bytesWritten >= sizeof(UInt32);
                            }
                            break;
                        case TypeCode.UInt64:
                            {
                                result = bytesWritten >= sizeof(UInt64);
                            }
                            break;
                        case TypeCode.Double:
                            {
                                result = bytesWritten >= sizeof(Double);
                            }
                            break;
                        case TypeCode.String:
                            {
                                result = bytesWritten > 0;
                            }
                            break;
                        case TypeCode.DateTime:
                            {
                                result = (bytesWritten >= sizeof(Int64));
                            }
                            break;
                        case TypeCode.Object:
                            {
                                result = bytesWritten > 0;
                            }
                            break;
                    }
                }
            }
            else
            {
                if(DataType.Type == TypeCode.String)
                {
                    result = true;
                }
                else if (DataType.Type == TypeCode.Object)
                {
                    result = true;
                }
                else if (DataType.Type == TypeCode.DateTime)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool SetValue(byte[] data)
        {
            bool result = false;

            if (data != null && (data.Length > 0 || DataType.Type == TypeCode.Object || DataType.Type == TypeCode.String || DataType.Type == TypeCode.DateTime))
            {
                result = SetValue(new ParameterValue(data));
            }

            return result;
        }

        public bool SetValue(ParameterValue newValue)
        {
            bool result = false;

            if (newValue != null && IsParameterValueValid(newValue))
            {
                ActualValue = newValue;

                result = true;
            }

            return result;
        }

        public bool SetValue<T>(T value)
        {
            bool result = false;
            var parameterValue = new ParameterValue();

            if (IsValueInRange(value) && parameterValue.SetValue(value))
            {
                ActualValue = parameterValue;
                result = true;
            }

            return result;
        }

        public bool SetValueAsString(string text)
        {
            bool result = false;

            switch (DataType.Type)
            {
                case TypeCode.Boolean:
                    {
                        if (bool.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.SByte:
                    {
                        if (sbyte.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.Byte:
                    {
                        if (byte.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.Char:
                    {
                        if (char.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.Int16:
                    {
                        if (Int16.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.Int32:
                    {
                        if (Int32.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.Int64:
                    {
                        if (Int64.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.UInt16:
                    {
                        if (UInt16.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.UInt32:
                    {
                        if (UInt32.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.UInt64:
                    {
                        if (UInt64.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.String:
                    {
                        result = SetValue(text);
                    }
                    break;
                case TypeCode.Double:
                    {
                        if (double.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
                case TypeCode.DateTime:
                    {
                        if (DateTime.TryParse(text, out var value))
                        {
                            result = SetValue(value);
                        }
                    }
                    break;
            }

            return result;
        }

        public string GetValueAsString()
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(ActualValue.Value))
            {
                switch (DataType.Type)
                {
                    case TypeCode.Boolean:
                        {
                            if (GetValue(out bool value))
                            {
                                result = $"{value}";
                            }
                        } break;
                    case TypeCode.SByte:
                        {
                            if (GetValue(out sbyte value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X1}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            if (GetValue(out byte value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X1}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.Char:
                        {
                            if (GetValue(out char value))
                            {
                                result = $"{value}";
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            if (GetValue(out Int16 value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X2}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.Int32:
                        {
                            if (GetValue(out Int32 value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X4}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            if (GetValue(out Int64 value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X8}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            if (GetValue(out UInt16 value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X2}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            if (GetValue(out UInt32 value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X4}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            if (GetValue(out UInt64 value))
                            {
                                result = DisplayFormat == "hex" ? $"0x{value:X8}" : $"{value}";
                            }
                        }
                        break;
                    case TypeCode.Object:
                        {
                            result = "Domain";
                        }
                        break;
                    case TypeCode.String:
                        {
                            if (GetValue(out string value))
                            {
                                result = value;
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            if (GetValue(out double value))
                            {
                                //TODO decimal places
                                result = $"{value}";
                            }
                        }
                        break;
                    case TypeCode.DateTime:
                        {
                            if (GetValue(out DateTime value))
                            {
                                result = value.ToString(DateTimeFormat);
                            }
                        }
                        break;
                }
            }

            return result;
        }

        private bool GetReferenceLabel<T>(T value, out string refLabel)
        {
            bool result = false;

            refLabel = string.Empty;

            if (DataType != null && DataType.Reference is XddEnumDataTypeReference enumReference &&
                    enumReference.GetValueLabel(value, out var label))
            {
                refLabel = label;
                result = true;
            }

            return result;
        }

        private bool GetValue<T>(ParameterValue parameterValue, out T value)
        {
            bool result = false;

            value = default;

            if (!string.IsNullOrEmpty(ActualValue.Value))
            {
                switch (DataType.Type)
                {
                    case TypeCode.Boolean:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0 && typeof(T) == typeof(bool))
                            {
                                value = (T)(object)(byteArray[0] > 0);
                                result = true;
                            }
                        }
                        break;
                    case TypeCode.SByte:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                sbyte[] signed = Array.ConvertAll(byteArray, b => unchecked((sbyte)b));

                                if (GetReferenceLabel(signed[0], out string label) && typeof(T) == typeof(string))
                                {
                                    value = (T)(object)label;
                                    result = true;
                                }
                                else if(typeof(T) == typeof(sbyte))
                                {
                                    value = (T)(object)signed[0];
                                    result = true;
                                }
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                if (GetReferenceLabel(byteArray[0], out string label) && typeof(T) == typeof(string))
                                {
                                    value = (T)(object)label;
                                    result = true;
                                }
                                else if(typeof(T) == typeof(byte))
                                {
                                    value = (T)(object)byteArray[0];
                                    result = true;
                                }
                            }
                        }
                        break;
                    case TypeCode.Char:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0 && typeof(T) == typeof(char))
                            {
                                value = (T)(object)BitConverter.ToChar(byteArray, 0);
                                result = true;
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                if (GetReferenceLabel(BitConverter.ToInt16(byteArray, 0), out string label) && typeof(T) == typeof(string))
                                {
                                    value = (T)(object)label;
                                    result = true;
                                }
                                else if(typeof(T) == typeof(Int16))
                                {
                                    value = (T)(object)BitConverter.ToInt16(byteArray, 0);
                                    result = true;
                                }
                            }
                        }
                        break;
                    case TypeCode.Int32:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                if (GetReferenceLabel(BitConverter.ToInt32(byteArray, 0), out string label) && typeof(T) == typeof(string))
                                {
                                    value = (T)(object)label;
                                    result = true;
                                }
                                else if (typeof(T) == typeof(Int32))
                                {
                                    value = (T)(object)BitConverter.ToInt32(byteArray, 0);
                                    result = true;
                                }
                            }
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0 && typeof(T) == typeof(Int64))
                            {
                                value = (T)(object)BitConverter.ToInt64(byteArray, 0);
                                result = true;
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                if (GetReferenceLabel(BitConverter.ToUInt16(byteArray, 0), out string label) && typeof(T) == typeof(string))
                                {
                                    value = (T)(object)label;
                                    result = true;
                                }
                                else if (typeof(T) == typeof(UInt16))
                                {
                                    value = (T)(object)BitConverter.ToUInt16(byteArray, 0);
                                    result = true;
                                }
                            }
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                if (GetReferenceLabel(BitConverter.ToUInt32(byteArray, 0), out string label) && typeof(T) == typeof(string))
                                {
                                    value = (T)(object)label;
                                    result = true;
                                }
                                else if(typeof(T) == typeof(UInt16))
                                {
                                    value = (T)(object)BitConverter.ToUInt32(byteArray, 0);
                                    result = true;
                                }
                            }
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0 && typeof(T) == typeof(UInt64))
                            {
                                value = (T)(object)BitConverter.ToUInt64(byteArray, 0);
                                result = true;
                            }
                        }
                        break;
                    case TypeCode.Object:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                value = (T)(object)byteArray;
                                result = true;
                            }
                        }
                        break;
                    case TypeCode.String:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0)
                            {
                                value = (T)(object)Encoding.Unicode.GetString(byteArray);
                                result = true;
                            }
                        }
                        break;
                    case TypeCode.Double:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0 && typeof(T) == typeof(double))
                            {
                                var doubleValue = BitConverter.ToDouble(byteArray, 0);

                                if (Unit != null && Unit.DecimalPlaces > 0)
                                {
                                    doubleValue = System.Math.Round(doubleValue, Unit.DecimalPlaces);
                                }

                                value = (T)(object)doubleValue;

                                result = true;
                            }
                        }
                        break;
                    case TypeCode.DateTime:
                        {
                            var byteArray = Convert.FromBase64String(parameterValue.Value);
                            if (byteArray.Length > 0 && typeof(T) == typeof(DateTime))
                            {
                                long dateData = BitConverter.ToInt64(byteArray, 0);

                                var val = DateTime.FromBinary(dateData);

                                value = (T)(object)val;

                                result = true;
                            }
                        }
                        break;
                }
            }
            else
            {
                if (DataType.Type == TypeCode.Object)
                {
                    value = (T)(object)new byte[DataType.SizeInBytes];
                    result = true;
                }
                else if( DataType.Type == TypeCode.String)
                {
                    value = (T)(object)string.Empty;
                    result = true;
                }
                else if (DataType.Type == TypeCode.DateTime)
                {
                    value = (T)(object)DateTime.MinValue;
                    result = true;
                }
            }

            return result;
        }

        public bool GetDefaultValue<T>(out T value)
        {
            var result = GetValue(DefaultValue, out value);

            return result;
        }

        public bool GetValue<T>(out T value)
        {
            var result = GetValue(ActualValue, out value);

            return result;
        }

        public bool GetValue(out byte value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.Byte)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten >= sizeof(byte))
                {
                    value = bytesArray[0];
                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out sbyte value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.SByte)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten >= sizeof(sbyte))
                {
                    value = (sbyte)bytesArray[0];
                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out Int16 value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.Int16)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten == sizeof(Int16))
                {
                    value = BitConverter.ToInt16(bytesArray.ToArray(), 0);

                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out Int32 value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.Int32)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten == sizeof(Int32))
                {
                    value = BitConverter.ToInt32(bytesArray.ToArray(), 0);

                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out Int64 value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.Int64)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten == sizeof(Int64))
                {
                    value = BitConverter.ToInt64(bytesArray.ToArray(), 0);

                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out UInt16 value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.UInt16)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten == sizeof(UInt16))
                {
                    value = BitConverter.ToUInt16(bytesArray.ToArray(), 0);

                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out UInt32 value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.UInt32)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten == sizeof(UInt32))
                {
                    value = BitConverter.ToUInt32(bytesArray.ToArray(), 0);

                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out UInt64 value)
        {
            bool result = false;
            value = 0;

            if (DataType.Type == TypeCode.UInt64)
            {
                var bytesArray = Base64Converter.AllocateBase64Buffer(ActualValue.Value);
                if (Base64Converter.TryFromBase64String(ActualValue.Value, bytesArray, out int bytesWritten) && bytesWritten == sizeof(UInt64))
                {
                    value = BitConverter.ToUInt64(bytesArray.ToArray(), 0);
                    result = true;
                }
            }

            return result;
        }

        public bool GetValue(out byte[] value)
        {
            bool result = false;

            value = null;

            try
            {
                string actualValue = ActualValue.Value;
                var bytesArray = Base64Converter.AllocateBase64Buffer(actualValue);

                if (!string.IsNullOrEmpty(actualValue))
                {
                    if (Base64Converter.TryFromBase64String(actualValue, bytesArray, out int bytesWritten) && bytesWritten > 0)
                    {
                        value = bytesArray.ToArray();
                        result = true;
                    }
                }
                else
                {
                    if(DataType.Type == TypeCode.String || 
                       DataType.Type == TypeCode.Object)
                    {
                        if(DataType.SizeInBytes > 0)
                        {
                            value = new byte[DataType.SizeInBytes];
                            result = true;
                        }
                        else if (DataType.SizeInBits > 0)
                        {
                            int sizeInBytes = (int)(double)(DataType.SizeInBits / 8);
                            value = new byte[sizeInBytes];
                            result = true;
                        }
                        else
                        {
                            const int sizeInBytes = 255;
                            value = new byte[sizeInBytes];
                            result = true;
                        }
                    }
                    else if (DataType.Type == TypeCode.DateTime)
                    {
                        DateTime dt = DateTime.MinValue;
                        var dtBytes = BitConverter.GetBytes(dt.Ticks);

                        value = new byte[dtBytes.Length];
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - GetValue", e);
            }

            return result;
        }

        public bool GetStructVariableValue<T>(string variableId, out T value)
        {
            bool result = false;

            value = default;

            try
            {
                if (DataType.Reference is XddStructDataType structDataType)
                {
                    result = structDataType.GetStructVariableValue(variableId, ActualValue, out value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public bool GetRange<T>(ref T minValue, ref T maxValue)
        {
            bool result = false;
            Type rangeType = typeof(T);

            if (rangeType == typeof(byte))
            {
                byte minVal = byte.MinValue;
                byte maxVal = byte.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(sbyte))
            {
                sbyte minVal = sbyte.MinValue;
                sbyte maxVal = sbyte.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(short))
            {
                short minVal = short.MinValue;
                short maxVal = short.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(ushort))
            {
                ushort minVal = ushort.MinValue;
                ushort maxVal = ushort.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(int))
            {
                int minVal = int.MinValue;
                int maxVal = int.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(uint))
            {
                uint minVal = uint.MinValue;
                uint maxVal = uint.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(long))
            {
                long minVal = long.MinValue;
                long maxVal = long.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(ulong))
            {
                ulong minVal = ulong.MinValue;
                ulong maxVal = ulong.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }
            else if (rangeType == typeof(double))
            {
                double minVal = double.MinValue;
                double maxVal = double.MaxValue;

                foreach (var allowedValue in AllowedValues)
                {
                    allowedValue.LimitRange(ref minVal, ref maxVal);
                }

                minValue = (T)Convert.ChangeType(minVal, typeof(T));
                maxValue = (T)Convert.ChangeType(maxVal, typeof(T));

                result = true;
            }

            return result;
        }

        public void GetEnumRange(ref List<long> enumRange)
        {
            foreach (var allowedValue in AllowedValues)
            {
                allowedValue.GetEnumRange(ref enumRange);
            }
        }
        public void GetEnumRange(ref List<string> enumRange)
        {
            foreach (var allowedValue in AllowedValues)
            {
                allowedValue.GetEnumRange(ref enumRange);
            }
        }

        public void GetValueRange(ref List<long> valueRange)
        {
            foreach (var allowedValue in AllowedValues)
            {
                allowedValue.GetValueRange(ref valueRange);
            }
        }

        private bool IsValueInRange<T>(T value)
        {
            bool result = true;

            if (DataType != null && value != null)
            {
                switch (DataType.Type)
                {
                    case TypeCode.Byte:
                        {
                            byte minValue = byte.MinValue;
                            byte maxValue = byte.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                byte typedValue = (byte)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        }
                        break;
                    case TypeCode.SByte:
                        {
                            sbyte minValue = sbyte.MinValue;
                            sbyte maxValue = sbyte.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                sbyte typedValue = (sbyte)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        }
                        break;
                    case TypeCode.Int16:
                        {
                            short minValue = short.MinValue;
                            short maxValue = short.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                short typedValue = (short)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            ushort minValue = ushort.MinValue;
                            ushort maxValue = ushort.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                ushort typedValue = (ushort)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        }
                        break;

                    case TypeCode.Int32:
                        {
                            int minValue = int.MinValue;
                            int maxValue = int.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                int typedValue = (int)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        } break;
                    case TypeCode.UInt32:
                        {
                            uint minValue = uint.MinValue;
                            uint maxValue = uint.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                uint typedValue = (uint)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        } break;
                    case TypeCode.Int64:
                        {
                            long minValue = long.MinValue;
                            long maxValue = long.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                long typedValue = (long)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        } break;
                    case TypeCode.UInt64:
                        {
                            ulong minValue = ulong.MinValue;
                            ulong maxValue = ulong.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                ulong typedValue = (ulong)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        } break;
                    case TypeCode.Double:
                        {
                            double minValue = double.MinValue;
                            double maxValue = double.MaxValue;

                            if (GetRange(ref minValue, ref maxValue))
                            {
                                double typedValue = (double)(object)value;

                                if (typedValue > maxValue || typedValue < minValue)
                                {
                                    result = false;
                                }
                            }
                        } break;

                }
            }
            else if(DataType != null && value == null)
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> Write()
        {
            bool result = false;
            var claudConnector = Device?.CloudConnector;

            if (claudConnector != null)
            {
                result = await claudConnector.WriteParameter(Device, this);

                OnParameterWritten(new ParameterWrittenEventArgs(this, result));
            }

            return result;
        }

        public async Task<List<ParameterValue>> GetValueHistory(DateTime from, DateTime to)
        {
            var result = new List<ParameterValue>();
            var claudConnector = Device?.CloudConnector;

            if (claudConnector != null)
            {
                result = await claudConnector.GetParameterValueHistory(Device, Index, SubIndex, from, to);
            }

            return result;
        }

        public async Task<ParameterValueHistoryStatistics> GetValueHistoryStatistics(DateTime from, DateTime to)
        {
            ParameterValueHistoryStatistics result = null;
            var claudConnector = Device?.CloudConnector;

            if (claudConnector != null)
            {
                result = await claudConnector.GetParameterValueHistoryStatistics(Device, UniqueId, from, to);
            }

            return result;
        }

        /// <summary>
        /// Write parameter value on the eltra server / master only method 
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public async Task<bool> SetParameterValue(ParameterValue parameterValue)
        {
            bool result = false;

            if (Device != null && Device.SearchParameter(UniqueId) is Parameter parameter)
            {
                var claudConnector = Device.CloudConnector;

                if (claudConnector != null)
                {
                    result = await claudConnector.SetParameterValue(Device, parameter.Index, parameter.SubIndex, parameterValue);

                    OnParameterWritten(new ParameterWrittenEventArgs(parameter, result));
                }
            }

            return result;
        }

        public async Task<ParameterValue> ReadValue()
        {
            ParameterValue result = null;
            var claudConnector = Device?.CloudConnector;

            if (claudConnector != null)
            {
                var actualValue = await claudConnector.GetParameterValue(Device, Index, SubIndex);

                if(actualValue != null)
                {
                    ActualValue = actualValue;

                    result = actualValue;
                }
            }

            return result;
        }

        public async Task<ParameterValue> UpdateValue()
        {
            ParameterValue result = null;
            ParameterValue actualValue = await ReadValue();
            
            if (actualValue != null)
            {
                ActualValue = actualValue;

                result = actualValue;
            }

            return result;
        }

        public bool AutoUpdate(ParameterUpdatePriority priority = ParameterUpdatePriority.Low, bool waitForResult = false)
        {
            bool result = false;
            var connector = Device?.CloudConnector;

            if (connector != null)
            {
                result = connector.RegisterParameterUpdate(Device, UniqueId, priority, waitForResult);
            }

            return result;
        }

        public bool StopUpdate(ParameterUpdatePriority priority = ParameterUpdatePriority.Low, bool waitForResult = false)
        {
            bool result = false;
            var connector = Device?.CloudConnector;

            if (connector != null)
            {
                result = connector.UnregisterParameterUpdate(Device, UniqueId, priority, waitForResult);
            }

            return result;
        }

        #endregion
    }
}
