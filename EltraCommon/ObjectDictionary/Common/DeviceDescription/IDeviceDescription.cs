using System.Collections.Generic;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription
{
    public interface IDeviceDescription
    {
        List<ParameterBase> Parameters { get; set; }

        XddProfile Profile { get; set; }

        string DataSource { get; set; }

        bool Parse();
    }
}
