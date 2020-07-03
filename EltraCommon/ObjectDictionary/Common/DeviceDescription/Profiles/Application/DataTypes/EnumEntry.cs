﻿using System.Collections.Generic;

using EltraCommon.ObjectDictionary.Common.DeviceDescription.Common;

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes
{
    public abstract class EnumEntry
    {
        #region Private fields

        private List<Label> _labels;

        #endregion

        #region Properties

        public long Value { get; set; }

        public List<Label> Labels => _labels ?? (_labels = new List<Label>());

        #endregion

        public abstract bool Parse();
    }
}