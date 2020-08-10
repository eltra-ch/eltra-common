using System.Net;
using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Location
{
    /// <summary>
    /// IpLocation
    /// </summary>
    [DataContract]
    public class IpLocation
    {
        private readonly IPAddress _address;
        private string _ip;

        /// <summary>
        /// IpLocation- default constructor
        /// </summary>
        public IpLocation()
        {
            Ip = "255.255.255.255";
        }

        /// <summary>
        /// IpLocation
        /// </summary>
        /// <param name="ip">IP address</param>
        public IpLocation(string ip)
        {
            Ip = ip;

            UpdatePrivateAddress();
        }

        /// <summary>
        /// IpLocation
        /// </summary>
        /// <param name="address">IP address</param>
        public IpLocation(IPAddress address)
        {
            _address = address;

            Ip = address.ToString();

            UpdatePrivateAddress();
        }

        /// <summary>
        /// IP address as string
        /// </summary>
        [DataMember]
        public string Ip
        {
            get => _ip;
            set
            {
                _ip = value;
                OnIpChanged();
            }
        }
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
        /// Is IP address private
        /// </summary>
        [IgnoreDataMember]
        public bool IsPrivateAddress { get; set; }

        private void OnIpChanged()
        {
            UpdatePrivateAddress();
        }

        private void UpdatePrivateAddress()
        {
            IsPrivateAddress = CountryCode == "-" || Ip == "127.0.0.1" || Ip == "0.0.0.0";

            if (!IsPrivateAddress && _address != null)
            {
                if (_address.Equals(IPAddress.IPv6Loopback))
                {
                    IsPrivateAddress = true;
                }
            }
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool Equals(IpLocation location)
        {
            bool result = true;

            if (location.Ip != Ip)
            {
                result = false;
            }

            if (result && location.CountryCode != CountryCode)
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
