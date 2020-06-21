using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EltraCloudContracts.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters
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
            get => _parameterList.Count;
        }

        public void Add(Parameter parameter)
        {
            ParameterList.Add(parameter);
        }
    }
}