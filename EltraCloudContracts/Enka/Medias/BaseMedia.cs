using EltraCloudContracts.Enka.Contacts;
using System;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Medias
{
    [DataContract]
    public class BaseMedia
    {
        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public Media Media { get; set; }
        [DataMember]
        public MediaStatus Status { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
    }
}