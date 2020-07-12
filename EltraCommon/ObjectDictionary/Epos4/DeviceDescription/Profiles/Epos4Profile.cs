﻿using EltraCommon.Contracts.Node;
using EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles.Device.DataRecorder;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles;

namespace EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles
{
    class Epos4Profile : XddProfile
    {
        #region Constructors

        public Epos4Profile(EltraDeviceNode device)
            : base(device)
        {
            ProfileBody = new Epos4ProfileBody(device);
        }

        #endregion

        #region Properties

        public DataRecorderList DataRecorderList => (ProfileBody as Epos4ProfileBody).DataRecorderList;

        #endregion
    }
}
