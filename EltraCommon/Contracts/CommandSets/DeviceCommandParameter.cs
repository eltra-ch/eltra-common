using System;
using System.Runtime.Serialization;
using System.Text;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;

namespace EltraCommon.Contracts.CommandSets
{
    /// <summary>
    /// DeviceCommandParameter
    /// </summary>
    [DataContract]
    public class DeviceCommandParameter
    {
        #region Constructors

        /// <summary>
        /// DeviceCommandParameter
        /// </summary>
        public DeviceCommandParameter()
        {
            Type = ParameterType.In;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [DataMember]
        public ParameterType Type { get; set; }

        /// <summary>
        /// DataType
        /// </summary>
        [DataMember]
        public DataType DataType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// GetValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool GetValue<T>(ref T value)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(Value))
            {
                switch (DataType.Type)
                {
                    case TypeCode.Boolean:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) (byteArray[0] > 0);
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.SByte:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            sbyte[] signed = Array.ConvertAll(byteArray, b => unchecked((sbyte) b));
                            value = (T) (object) signed[0];
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.Byte:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) byteArray[0];
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.Char:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToChar(byteArray,0);
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.Int16:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToInt16(byteArray,0);
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.Int32:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToInt32(byteArray,0);
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.Int64:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToInt64(byteArray,0);
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.UInt16:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToUInt16(byteArray,0);
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.UInt32:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToUInt32(byteArray,0);
                            result = true;
                        }
                    }
                        break;
                    case TypeCode.UInt64:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToUInt64(byteArray,0);
                            result = true;
                        }
                    } break;
                    case TypeCode.Object:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) byteArray;
                            result = true;
                        }
                    } break;
                    case TypeCode.String:
                    {
                        var byteArray = Convert.FromBase64String(Value);

                        if (byteArray.Length > 0)
                        {
                            value = (T)(object)Encoding.UTF8.GetString(byteArray);
                        }

                        result = true;
                    } break;
                    case TypeCode.Double:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            value = (T) (object) BitConverter.ToDouble(byteArray,0);
                            result = true;
                        }
                    } break;
                    case TypeCode.DateTime:
                    {
                        var byteArray = Convert.FromBase64String(Value);
                        if (byteArray.Length > 0)
                        {
                            long dateData = BitConverter.ToInt64(byteArray,0);

                            var val = DateTime.FromBinary(dateData);

                            value = (T) (object) val;

                            result = true;
                        }
                    } break;
                }
            }
            else
            {
                if (DataType.Type == TypeCode.Object)
                {
                    if (DataType.SizeInBytes > 0)
                    {
                        value = (T)(object)new byte[DataType.SizeInBytes];
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// SetValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue<T>(T value)
        {
            bool result = true;

            if (typeof(T) == typeof(bool))
            {
                DataType = new DataType { Type = TypeCode.Boolean };

                var byteArray = BitConverter.GetBytes((bool)(object)value);

                Value = Convert.ToBase64String(byteArray);

            }
            else if (typeof(T) == typeof(byte))
            {
                DataType = new DataType { Type = TypeCode.Byte };

                var byteArray = BitConverter.GetBytes((byte)(object)value);
                
                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                DataType = new DataType { Type = TypeCode.SByte };

                var byteArray = BitConverter.GetBytes((sbyte)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(char))
            {
                DataType = new DataType { Type = TypeCode.Char };

                var byteArray = BitConverter.GetBytes((char)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(Int16))
            {
                DataType = new DataType { Type = TypeCode.Int16 };

                var byteArray = BitConverter.GetBytes((Int16)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(Int32))
            {
                DataType = new DataType { Type = TypeCode.Int32 };

                var byteArray = BitConverter.GetBytes((Int32)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(Int64))
            {
                DataType = new DataType { Type = TypeCode.Int64 };

                var byteArray = BitConverter.GetBytes((Int64)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(UInt16))
            {
                DataType = new DataType { Type = TypeCode.UInt16 };

                var byteArray = BitConverter.GetBytes((UInt16)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(UInt32))
            {
                DataType = new DataType { Type = TypeCode.UInt32 };

                var byteArray = BitConverter.GetBytes((UInt32)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(UInt64))
            {
                DataType = new DataType { Type = TypeCode.UInt64 };

                var byteArray = BitConverter.GetBytes((UInt64)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(double))
            {
                DataType = new DataType { Type = TypeCode.Double };

                var byteArray = BitConverter.GetBytes((double)(object)value);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(object))
            {
                DataType = new DataType { Type = TypeCode.Object };

                var byteArray = new byte[DataType.SizeInBytes];

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(string))
            {
                string text = (string)(object)value;

                DataType = new DataType { Type = TypeCode.String, SizeInBytes = (uint)text.Length };

                var byteArray = Encoding.UTF8.GetBytes(text);

                Value = Convert.ToBase64String(byteArray);
            }
            else if (typeof(T) == typeof(byte[]))
            {
                byte[] s = (byte[])(object)value;

                DataType = new DataType { Type = TypeCode.Object, SizeInBytes = (uint)s.Length };

                Value = Convert.ToBase64String(s);
            }
            else if(typeof(T) == typeof(DateTime))
            {
                DataType = new DataType { Type = TypeCode.DateTime };
                DateTime dt = (DateTime) (object) value;
                Value = Convert.ToBase64String(BitConverter.GetBytes(dt.Ticks));
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// SetDataType
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public bool SetDataType(DataType dataType)
        {
            bool result = false;

            if (dataType != null)
            {
                DataType.Clone(dataType);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// SetDataType
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public bool SetDataType(TypeCode typeCode)
        {
            bool result = false;

            if (DataType != null)
            {
                DataType.Type = typeCode;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// GetDataType
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public bool GetDataType(out TypeCode typeCode)
        {
            bool result = false;

            typeCode = TypeCode.Object;

            if (DataType != null)
            {
                typeCode = DataType.Type;
                result = true;
            }

            return result;
        }

        #endregion
    }
}
