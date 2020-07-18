using System.Collections.Generic;

using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription
{
    public abstract class IDeviceDescription
    {
        public List<ParameterBase> Parameters { get; set; }

        public string DataSource { get; set; }

        public abstract bool Parse();
    }
}
