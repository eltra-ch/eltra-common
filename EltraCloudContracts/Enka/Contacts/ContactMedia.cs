using EltraCloudContracts.Enka.Medias;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Contacts
{
    [DataContract]
    public class ContactMedia : BaseMedia
    {
        [DataMember]
        public ContactMediaType Type { get; set; }
    }
}
