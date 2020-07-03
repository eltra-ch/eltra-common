using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters
{
    [DataContract]
    public class ParameterSet
    {
        private List<Parameter> _parameterList;

        [DataMember]
        public List<Parameter> ParameterList
        {
            get => _parameterList ?? (_parameterList = new List<Parameter>());
            set => _parameterList = value;
        }

        public int Count
        {
            get => ParameterList.Count;
        }

        public void Add(Parameter parameter)
        {
            ParameterList.Add(parameter);
        }

        public Parameter Get(int index)
        {
            Parameter result = null;
            if(Count > index)
            {
                result = ParameterList[index];
            }

            return result;
        }
    }
}