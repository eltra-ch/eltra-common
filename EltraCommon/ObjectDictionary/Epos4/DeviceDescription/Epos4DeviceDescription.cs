using System.Collections.Generic;
using EltraCommon.Contracts.Node;
using EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles;
using EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles.Device.DataRecorder;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Epos4.DeviceDescription
{
    public class Epos4DeviceDescription : XddDeviceDescription
    {
        #region Constructors

        public Epos4DeviceDescription(EltraDeviceNode device)
            : base(device)
        {
            Profile = new Epos4Profile(device);
        }

        #endregion

        #region Properties

        public List<DataRecorder> DataRecorders { get; set; }

        #endregion

        #region Methods

        public override bool Parse()
        {
            bool result = base.Parse();

            if (result)
            {
                if (Profile is Epos4Profile epos4Profile)
                {
                    DataRecorders = epos4Profile.DataRecorderList.DataRecorders;
                }
            }

            return result;
        }

        #endregion
    }
}
