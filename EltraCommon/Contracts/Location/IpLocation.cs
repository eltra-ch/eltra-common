using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EltraCommon.Contracts.Location
{
    /// <summary>
    /// IpLocation
    /// </summary>
    [DataContract]
    public class IpLocation : GeoLocation
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
        /// Is IP address private
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public bool IsPrivateAddress { get; set; }

        private void OnIpChanged()
        {
            UpdatePrivateAddress();
        }

        private void UpdatePrivateAddress()
        {
            IsPrivateAddress = CountryCode == "-" || Ip == "127.0.0.1" || Ip.StartsWith("0.0.0");

            if (!IsPrivateAddress && _address != null && _address.Equals(IPAddress.IPv6Loopback))
            {
                IsPrivateAddress = true;
            }
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override bool Equals(GeoLocation location)
        {
            bool result = base.Equals(location);

            if (location is IpLocation ipLocation && ipLocation.Ip != Ip)
            {
                result = false;
            }

            return result;
        }
    }
}
