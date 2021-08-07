using EltraCommon.ObjectDictionary.Common.DeviceDescription.Common;
using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Templates
{
    [DataContract]
    public class Flags
    {
        #region Properties

        [DataMember]
        public AccessMode Access { get; set; }

        [DataMember]
        public string UniqueId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Backup { get; set; }
        
        [DataMember]
        public int Setting { get; set; }

        [DataMember]
        public int Volatile { get; set; }

        #endregion
    }
}
