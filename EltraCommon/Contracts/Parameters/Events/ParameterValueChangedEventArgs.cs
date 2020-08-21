using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using System;

namespace EltraCommon.Contracts.Parameters.Events
{
    /// <summary>
    /// ParameterValueChangedEventArgs
    /// </summary>
    public class ParameterValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// ParameterValueChangedEventArgs
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="index"></param>
        /// <param name="subIndex"></param>
        /// <param name="parameterValue"></param>
        public ParameterValueChangedEventArgs(int nodeId, ushort index, byte subIndex, ParameterValue parameterValue)
        {
            NodeId = nodeId;
            Index = index;
            SubIndex = subIndex;
            ParameterValue = parameterValue;
        }

        /// <summary>
        /// NodeId
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// Index
        /// </summary>
        public ushort Index { get; set; }
        /// <summary>
        /// SubIndex
        /// </summary>
        public byte SubIndex { get; set; }
        /// <summary>
        /// ParameterValue
        /// </summary>
        public ParameterValue ParameterValue { get; set; }
    }
}
