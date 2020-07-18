using System.Net;

namespace EltraCommon.Contracts.Channels
{
    public class IpLocation
    {
        private readonly IPAddress _address;
        private string _ip;

        public IpLocation()
        {
            Ip = "255.255.255.255";
        }
        public IpLocation(string ip)
        {
            Ip = ip;
            
            UpdatePrivateAddress();
        }

        public IpLocation(IPAddress address)
        {
            _address = address;

            Ip = address.ToString();

            UpdatePrivateAddress();
        }

        public string Ip 
        { 
            get => _ip; 
            set 
            {
                _ip = value;
                OnIpChanged();
            } 
        }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsPrivateAddress { get; set; }
        
        private void OnIpChanged()
        {
            UpdatePrivateAddress();
        }

        private void UpdatePrivateAddress()
        {
            IsPrivateAddress = CountryCode == "-" || Ip == "127.0.0.1" || Ip == "0.0.0.0";

            if(!IsPrivateAddress && _address!=null)
            {
                if (_address.Equals(IPAddress.IPv6Loopback))
                {
                    IsPrivateAddress = true;
                }
            }
        }

        public bool Equals(IpLocation location)
        {
            bool result = true;
            
            if(location.Ip != Ip)
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
