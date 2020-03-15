using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Contacts
{
    [DataContract]
    public class Contact
    {
        #region Properties

        [DataMember]
        [Required]
        public string FirstName { get; set; }
        [DataMember]
        [Required]
        public string LastName { get; set; }
        [DataMember]
        public string PhoneStationary { get; set; }
        [DataMember]
        public string PhoneMobile { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string HouseNumber { get; set; }
        [DataMember]
        public string FlatNumber { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Notice { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        #endregion
    }
}
