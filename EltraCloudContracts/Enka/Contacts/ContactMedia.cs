using System;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Contacts
{
    [DataContract]
    public class ContactMedia
    {
        [DataMember]
        public string ContactUuid { get; set; }
        [DataMember]
        public Medias.Media Media {get; set; }
        [DataMember]
        public ContactMediaStatus Status { get; set; }
        [DataMember]
        public ContactMediaType Type { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
    }
}
