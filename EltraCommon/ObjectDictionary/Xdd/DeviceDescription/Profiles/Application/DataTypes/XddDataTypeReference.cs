using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.DataTypes;

namespace EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.DataTypes
{
    class XddDataTypeReference : DataTypeReference
    {
        public string UniqueId { get; set; }

        public string Name { get; set; }
        
        public override bool Parse()
        {
            return false;
        }
    }
}
