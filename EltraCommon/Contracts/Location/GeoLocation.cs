using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Location
{
    /// <summary>
    /// GeoLocation
    /// </summary>
    [DataContract]
    public class GeoLocation
    {
        /// <summary>
        /// Country code
        /// </summary>
        [DataMember]
        public string CountryCode { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        [DataMember]
        public string Country { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        [DataMember]
        public string Region { get; set; }
        /// <summary>
        /// City
        /// </summary>
        [DataMember]
        public string City { get; set; }
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
        /// Equals
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public virtual bool Equals(GeoLocation location)
        {
            bool result = true;

            if (location.CountryCode != CountryCode)
            {
                result = false;
            }

            if (result && location.Country != Country)
            {
                result = false;
            }

            if (result && location.Region != Region)
            {
                result = false;
            }

            if (result && location.City != City)
            {
                result = false;
            }

            if (result && location.Latitude != Latitude)
            {
                result = false;
            }

            if (result && location.Longitude != Longitude)
            {
                result = false;
            }

            return result;
        }
    }
}
