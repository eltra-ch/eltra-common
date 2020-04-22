using System;
using System.Runtime.Serialization;

namespace EltraCloudContracts.Enka.Contacts
{
    [DataContract]
    public class Contact
    {
        #region Properties

        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Phone { get; set; }        
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Notice { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
        [DataMember]
        public DateTime Created { get; set; }

        #endregion

        #region Methods

        public void RemovePrivatData()
        {
            Uuid = string.Empty;

            LastName = string.Empty;
            Notice = string.Empty;
            Phone = string.Empty;
            Street = string.Empty;
            PostalCode = string.Empty;

            Latitude = 0;
            Longitude = 0;
        }

        #endregion
    }
}
