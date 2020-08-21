using EltraCommon.Contracts.Devices;
using EltraCommon.ObjectDictionary.Common;
using EltraCommon.ObjectDictionary.Epos4;
using EltraCommon.ObjectDictionary.Xdd;
using System;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Factory
{
    public class ObjectDictionaryFactory
    {
        public static DeviceObjectDictionary CreateObjectDictionary(EltraDevice device)
        {
            DeviceObjectDictionary result = null;

            if (device.Family == "EPOS2")
            {
                throw new NotImplementedException();
            }
            else if (device.Family == "EPOS4")
            {
                result = new Epos4ObjectDictionary(device);
            }
            else
            {
                result = new XddObjectDictionary(device);
            }

            return result;
        }        
    }
}
