﻿using System;
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
        public string Name { get; set; }        
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
        public DateTime Modified { get; set; }
        [DataMember]
        public DateTime Created { get; set; }

        #endregion

        #region Methods

        public void RemovePrivatData()
        {
            Name = string.Empty;
            Notice = string.Empty;
            Phone = string.Empty;
            Street = string.Empty;
        }

        #endregion
    }
}
