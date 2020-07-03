using System;
using System.Runtime.Serialization;

namespace EltraCommon.Enka.Contacts
{
    [DataContract]
    public class Vote
    {
        private Rating _rating;

        [DataMember]
        public string ContactUuid { get; set; }
        [DataMember]
        public string OrderUuid { get; set; }
        [DataMember]
        public Rating Rating 
        { 
            get => _rating ?? (_rating = new Rating()); 
            set { _rating = value; } 
        }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
    }
}
