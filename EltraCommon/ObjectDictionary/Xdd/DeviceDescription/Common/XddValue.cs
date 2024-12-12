using System;
using System.Xml;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.Logger;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Common
{
    public class XddValue
    {
        #region Private fields

        private readonly ParameterValue _actualValue;
        private readonly DataType _dataType;

        #endregion

        #region Constructors

        public XddValue()
        {
        }

        public XddValue(DataType dataType)
        {
            _dataType = dataType;
        }

        public XddValue(ParameterValue actualValue, DataType dataType)
        {
            _actualValue = actualValue;
            _dataType = dataType;

            Value = _actualValue.Value;
        }

        #endregion

        #region Properties

        public ParameterValue ActualValue => _actualValue;

        public string Value { get; set; }

        protected DataType DataType => _dataType;

        #endregion

        #region Methods

        public bool Parse(XmlNode childNode)
        {
            bool result = false;
            string valueAsString = childNode.InnerText;
            long rawValue = 0;

            if (valueAsString.StartsWith("0x"))
            {
                rawValue = Convert.ToInt64(valueAsString.Substring(2), 16);
                result = true;
            }
            else if (long.TryParse(childNode.InnerText, out var value))
            {
                rawValue = value;
                result = true;
            }

            if (result)
            {
                Value = Convert.ToBase64String(BitConverter.GetBytes(rawValue), Base64FormattingOptions.None);
            }

            return result;
        }

        public double ToDouble()
        {
            double result = double.NaN;
            
            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    if (TypeCode.Double == DataType.Type)
                    {
                        result = BitConverter.ToInt32(byteArray, 0);
                    }
                    else
                    {
                        MsgLogger.WriteError($"{GetType().Name} - ToDouble", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");                        
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToDouble", $"Cannot convert base64 value = {Value}, unknown data type!");

                    if (byteArray.Length == sizeof(double))
                    {
                        result = BitConverter.ToDouble(byteArray, 0);
                    }
                    else if (byteArray.Length == sizeof(int)) 
                    {
                        result = BitConverter.ToInt32(byteArray, 0);
                    }
                    else if(byteArray.Length == sizeof(short))
                    {
                        result = BitConverter.ToInt16(byteArray, 0);
                    }
                    
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToDouble", e);
            }

            return result;
        }

        public long ToLong()
        {
            long result = long.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.Int16:
                            result = BitConverter.ToInt16(byteArray, 0);
                            break;
                        case TypeCode.Int32:
                            result = BitConverter.ToInt32(byteArray, 0);
                            break;
                        case TypeCode.Int64:
                            result = BitConverter.ToInt64(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToLong", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToLong", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = BitConverter.ToInt64(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToLong", e);
            }
            
            return result;
        }

        public ulong ToULong()
        {
            ulong result = ulong.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.UInt16:
                            result = BitConverter.ToUInt16(byteArray, 0);
                            break;
                        case TypeCode.UInt32:
                            result = BitConverter.ToUInt32(byteArray, 0);
                            break;
                        case TypeCode.UInt64:
                            result = BitConverter.ToUInt64(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToULong", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToULong", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = BitConverter.ToUInt64(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToULong", e);
            }

            return result;
        }

        public int ToInt()
        {
            int result = int.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.Int16:
                            result = BitConverter.ToInt16(byteArray, 0);
                            break;
                        case TypeCode.Int32:
                            result = BitConverter.ToInt32(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToInt", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToInt", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = BitConverter.ToInt32(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToInt", e);
            }

            return result;
        }

        public uint ToUInt()
        {
            uint result = uint.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.UInt16:
                            result = BitConverter.ToUInt16(byteArray, 0);
                            break;
                        case TypeCode.UInt32:
                            result = BitConverter.ToUInt32(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToUInt", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToUInt", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = BitConverter.ToUInt32(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToUInt", e);
            }

            return result;
        }

        public short ToShort()
        {
            short result = short.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = (short)BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = (short)BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.Int16:
                            result = BitConverter.ToInt16(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToShort", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToShort", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = BitConverter.ToInt16(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToShort", e);
            }

            return result;
        }

        public ushort ToUShort()
        {
            ushort result = ushort.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.UInt16:
                            result = BitConverter.ToUInt16(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToUShort", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToUShort", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = BitConverter.ToUInt16(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToUShort", e);
            }

            return result;
        }

        public byte ToByte()
        {
            byte result = byte.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = (byte)BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = (byte)BitConverter.ToChar(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToByte", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToByte", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = (byte)BitConverter.ToChar(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToByte", e);
            }

            return result;
        }

        public sbyte ToSByte()
        {
            sbyte result = sbyte.MinValue;

            try
            {
                var byteArray = Convert.FromBase64String(Value);

                if (DataType != null)
                {
                    switch (DataType.Type)
                    {
                        case TypeCode.Byte:
                            result = (sbyte)BitConverter.ToChar(byteArray, 0);
                            break;
                        case TypeCode.SByte:
                            result = (sbyte)BitConverter.ToChar(byteArray, 0);
                            break;
                        default:
                            MsgLogger.WriteError($"{GetType().Name} - ToUbyte", $"Cannot convert base64 value = {Value}, to data type {DataType.Type}!");
                            break;
                    }
                }
                else
                {
                    MsgLogger.WriteWarning($"{GetType().Name} - ToUbyte", $"Cannot convert base64 value = {Value}, unknown data type!");

                    result = (sbyte)BitConverter.ToChar(byteArray, 0);
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ToUbyte", e);
            }

            return result;
        }

        #endregion
    }
}
