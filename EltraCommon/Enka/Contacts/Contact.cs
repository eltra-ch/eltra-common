using System;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Contacts
{
    /// <summary>
    /// Contact
    /// </summary>
    [DataContract]
    public class Contact
    {
        #region Properties

        /// <summary>
        /// Uuid
        /// </summary>
        [DataMember]
        public string Uuid { get; set; }
        /// <summary>
        /// FirstName
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        [DataMember]
        public string Phone { get; set; }
        /// <summary>
        /// Street
        /// </summary>
        [DataMember]
        public string Street { get; set; }
        /// <summary>
        /// City
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        [DataMember]
        public string Region { get; set; }
        /// <summary>
        /// PostalCode
        /// </summary>
        [DataMember]
        public string PostalCode { get; set; }
        /// <summary>
        /// Notice
        /// </summary>
        [DataMember]
        public string Notice { get; set; }
        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember]
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember]
        public double Longitude { get; set; }
        /// <summary>
        /// Modified
        /// </summary>
        [DataMember]
        public DateTime Modified { get; set; }
        /// <summary>
        /// Created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// RemovePrivatData
        /// </summary>
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
