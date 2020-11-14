using System;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters.Events
{
    public class ParameterWrittenEventArgs : EventArgs
    {
        public ParameterWrittenEventArgs(Parameter parameter, bool result)
        {
            Parameter = parameter;

            Result = result;
        }
        
        public Parameter Parameter { get; }
        public bool Result { get; }
    }
}
