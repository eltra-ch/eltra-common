﻿using System.Collections.Generic;
using System.Xml;

using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Device;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles.Device.DataRecorder
{
    public class DataRecorderList
    {
        #region Private fields
        
        private List<DataRecorder> _dataRecorder;

        #endregion

        #region Constructors

        public DataRecorderList()
        {
        }

        #endregion

        #region Properties

        public List<DataRecorder> DataRecorders
        {
            get => _dataRecorder ?? (_dataRecorder = new List<DataRecorder>());
        }
        
        #endregion

        #region Methods

        public bool Parse(XmlNode node)
        {
            bool result = true;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "dataRecorder")
                {
                    var dataRecorder = new DataRecorder();

                    if (!dataRecorder.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    DataRecorders.Add(dataRecorder);
                }
            }

            return result;
        }

        #endregion

        public void ResolveParameterReferences(XddParameterList parameterList)
        {
            foreach (var dataRecorder in DataRecorders)
            {
                dataRecorder.ResolveParameterReferences(parameterList);
            }
        }
    }
}
