using System;
using System.Runtime.Serialization;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using EltraCommon.Logger;
using System.Text;
using EltraCommon.Converters;
using System.Collections.Generic;
using System.ComponentModel;

#pragma warning disable 1591, S3897, S4035

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters
{
    [DataContract]
    public class ParameterValue : IEqualityComparer<ParameterValue>
    {
        private string _value;

        public ParameterValue()
        {
        }

        public ParameterValue(byte[] data)
        {
            Value = Convert.ToBase64String(data, Base64FormattingOptions.None);
        }

        public ParameterValue(ParameterValue parameterValue)
        {
            Value = parameterValue.Value;
        }

        public ParameterValue(TypeCode type, string defaultValue)
        {
            SetDefaultValue(type, defaultValue);
        }

        public ParameterValue(DataType type, string defaultValue)
        {
            SetDefaultValue(type, defaultValue);
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "ParameterValue";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

        [DataMember]
        public string Value
        {
            get => _value ?? (_value = string.Empty);
            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        [DataMember]
        public DateTime Modified { get; set; }

        public bool Equals(ParameterValue other)
        {
            return other != null && other.Value == Value;
        }

        public ParameterValue Clone()
        {
            return new ParameterValue(this);
        }

        public bool SetDefaultValue(DataType dataType, string defaultValue)
        {
            var result = SetDefaultValue(dataType.Type, defaultValue);

            return result;
        }

        private bool SetDefaultValue(TypeCode type, string defaultValue)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(defaultValue))
            {
                if (type == TypeCode.String)
                {
                    Value = Convert.ToBase64String(Encoding.Unicode.GetBytes(defaultValue));
                }
                else
                {
                    if (defaultValue.StartsWith("0x"))
                    {
                        switch (type)
                        {
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                Value = Convert.ToBase64String(new byte[] { Convert.ToByte(defaultValue.Substring(2), 16) }, Base64FormattingOptions.None);
                                break;
                            case TypeCode.UInt16:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToUInt16(defaultValue.Substring(2), 16)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.UInt32:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToUInt32(defaultValue.Substring(2), 16)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.UInt64:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToUInt64(defaultValue.Substring(2), 16)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Int16:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt16(defaultValue.Substring(2), 16)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Int32:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt32(defaultValue.Substring(2), 16)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Int64:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt64(defaultValue.Substring(2), 16)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Object:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt64(defaultValue.Substring(2), 16)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Boolean:
                                result = false;
                                break;
                            default:
                                result = false;
                                break;
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                Value = Convert.ToBase64String(new byte[] { Convert.ToByte(defaultValue) }, Base64FormattingOptions.None);
                                break;
                            case TypeCode.UInt16:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToUInt16(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.UInt32:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToUInt32(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.UInt64:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToUInt64(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Int16:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt16(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Int32:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt32(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Int64:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt64(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Object:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToInt64(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Double:
                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToDouble(defaultValue)), Base64FormattingOptions.None);
                                break;
                            case TypeCode.Boolean:

                                if (defaultValue == "0")
                                {
                                    defaultValue = "false";
                                }
                                else if (defaultValue == "1")
                                {
                                    defaultValue = "true";
                                }

                                Value = Convert.ToBase64String(BitConverter.GetBytes(Convert.ToBoolean(defaultValue)), Base64FormattingOptions.None);
                                break;
                            default:
                                result = false;
                                break;
                        }

                    }
                }
            }

            return result;
        }

        public bool SetValue<T>(T value)
        {
            bool result = true;

            if (value != null)
            {
                if (typeof(T) == typeof(bool))
                {
                    var byteArray = BitConverter.GetBytes((bool)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
                {
                    var byteArray = new byte[] { (byte)(object)value };

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(char))
                {
                    var byteArray = BitConverter.GetBytes((char)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(short))
                {
                    var byteArray = BitConverter.GetBytes((short)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(int))
                {
                    var byteArray = BitConverter.GetBytes((int)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(long))
                {
                    var byteArray = BitConverter.GetBytes((Int64)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(ushort))
                {
                    var byteArray = BitConverter.GetBytes((ushort)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(uint))
                {
                    var byteArray = BitConverter.GetBytes((uint)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(ulong))
                {
                    var byteArray = BitConverter.GetBytes((ulong)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(double))
                {
                    var byteArray = BitConverter.GetBytes((double)(object)value);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(string))
                {
                    string s = (string)(object)value;

                    var byteArray = Encoding.Unicode.GetBytes(s);

                    Value = Convert.ToBase64String(byteArray);
                }
                else if (typeof(T) == typeof(byte[]))
                {
                    byte[] s = (byte[])(object)value;

                    Value = Convert.ToBase64String(s);
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    DateTime dt = (DateTime)(object)value;

                    Value = Convert.ToBase64String(BitConverter.GetBytes(dt.Ticks));
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        public bool GetValue<T>(ref T value)
        {
            bool result = false;

            try
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    if (typeof(T) == typeof(bool))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T)(object)(byteArray[0] > 0);
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(byte))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T)(object)byteArray[0];
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(sbyte))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length >= sizeof(sbyte))
                        {
                            sbyte[] signed = Array.ConvertAll(byteArray, b => unchecked((sbyte)b));
                            value = (T)(object)signed[0];
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(char))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(char))
                        {
                            value = (T)(object)BitConverter.ToChar(byteArray, 0);
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(short))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(short))
                        {
                            value = (T)(object)BitConverter.ToInt16(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(char))
                        {
                            value = (T)(object)(short)BitConverter.ToChar(byteArray, 0);
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(int))
                        {
                            value = (T)(object)BitConverter.ToInt32(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(short))
                        {
                            value = (T)(object)(int)BitConverter.ToInt16(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(char))
                        {
                            value = (T)(object)(int)BitConverter.ToChar(byteArray, 0);
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(long))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(long))
                        {
                            value = (T)(object)BitConverter.ToInt64(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(int))
                        {
                            value = (T)(object)(long)BitConverter.ToInt32(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(short))
                        {
                            value = (T)(object)(long)BitConverter.ToInt16(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(char))
                        {
                            value = (T)(object)(long)BitConverter.ToChar(byteArray, 0);
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(ushort))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(ushort))
                        {
                            value = (T)(object)BitConverter.ToUInt16(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(sbyte))
                        {
                            value = (T)(object)(ushort)byteArray[0];
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(uint))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(uint))
                        {
                            value = (T)(object)BitConverter.ToUInt32(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(ushort))
                        {
                            value = (T)(object)(uint)BitConverter.ToUInt16(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(sbyte))
                        {
                            value = (T)(object)(uint)byteArray[0];
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(ulong))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(ulong))
                        {
                            value = (T)(object)BitConverter.ToUInt64(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(uint))
                        {
                            value = (T)(object)(ulong)BitConverter.ToUInt32(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(ushort))
                        {
                            value = (T)(object)(ulong)BitConverter.ToUInt16(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(sbyte))
                        {
                            value = (T)(object)(ulong)byteArray[0];
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(double))
                        {
                            value = (T)(object)BitConverter.ToDouble(byteArray, 0);
                            result = true;
                        }
                        else if (byteArray.Length == sizeof(float))
                        {
                            value = (T)(object)BitConverter.ToSingle(byteArray, 0);
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        var bytesArray = Base64Converter.AllocateBase64Buffer(Value);

                        if (Base64Converter.TryFromBase64String(Value, bytesArray, out int bytesWritten))
                        {
                            value = (T)(object)Encoding.Unicode.GetString(bytesArray.ToArray());
                            result = true;
                        }
                        else
                        {
                            value = (T)(object)Value;
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(byte[]))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T)(object)byteArray;
                            result = true;
                        }
                    }
                    else if (typeof(T) == typeof(DateTime))
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length == sizeof(long))
                        {
                            long dateData = BitConverter.ToInt64(byteArray, 0);

                            var val = DateTime.FromBinary(dateData);

                            value = (T)(object)val;

                            result = true;
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            catch(Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - GetValue", e);
            }
                        
            return result;
        }

        protected virtual void OnValueChanged()
        {
            Modified = DateTime.Now;
        }

        public bool Equals(ParameterValue x, ParameterValue y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(ParameterValue obj)
        {
            return obj.GetHashCode();
        }
    }
}
