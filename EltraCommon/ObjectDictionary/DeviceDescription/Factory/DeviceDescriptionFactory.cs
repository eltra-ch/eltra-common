using EltraCommon.Contracts.Devices;
using EltraCommon.Contracts.Node;
using EltraCommon.ObjectDictionary.Common.DeviceDescription;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription;
using System;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.DeviceDescription.Factory
{
    public static class DeviceDescriptionFactory
    {
        public static DeviceDescriptionFile CreateDeviceDescriptionFile(EltraDevice device)
        {
            DeviceDescriptionFile result;

            switch (device.Family)
            {
                case "EPOS2":
                    result = new Epos2DeviceDescriptionFile(device);
                    break;
                case "EPOS4":
                    result = new XddDeviceDescriptionFile(device);
                    break;
                default:
                    result = new XddDeviceDescriptionFile(device);
                    break;
            }

            return result;
        }

        public static IDeviceDescription CreateDeviceDescription(EltraDeviceNode device, DeviceDescriptionFile deviceDescriptionFile)
        {
            IDeviceDescription result = null;
            var content = deviceDescriptionFile?.Content;

            if (content != null && device != null)
            {
                switch (device.Family)
                {
                    case "EPOS2":
                        throw new NotImplementedException();
                    case "EPOS4":
                        result = new XddDeviceDescription(device) { DataSource = content };
                        break;
                    default:
                        result = new XddDeviceDescription(device) { DataSource = content };
                        break;
                }
            }

            return result;
        }
    }
}
