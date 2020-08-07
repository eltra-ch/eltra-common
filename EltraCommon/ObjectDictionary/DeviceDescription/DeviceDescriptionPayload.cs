using EltraCommon.Contracts.Devices;
using EltraCommon.Contracts.Node;
using EltraCommon.Helpers;
using System;
using System.Runtime.Serialization;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.DeviceDescription
{
    [DataContract]
    public class DeviceDescriptionPayload
    {
        #region Private fields

        private string _plainContent;

        #endregion

        #region Constructors

        public DeviceDescriptionPayload()
        {
        }

        public DeviceDescriptionPayload(EltraDeviceNode device)
        {
            Init(device);
        }

        #endregion

        #region Properties

        [DataMember]
        public string ChannelId { get; set; }

        [DataMember]
        public int NodeId { get; set; }

        [DataMember]
        public DeviceVersion Version { get; set; }

        [DataMember]
        public string Content { get; set; }

        public string PlainContent
        {
            get
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    var data = Convert.FromBase64String(Content);

                    _plainContent = ZipHelper.Deflate(data);
                }
                
                return _plainContent;
            }
            private set
            {
                _plainContent = value;
            }
        }

        [DataMember]
        public string Encoding { get; set; }

        public string HashCode 
        { 
            get
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(PlainContent))
                {
                    result = CryptHelpers.ToMD5(PlainContent);
                }

                return result;
            }
        }

        [DataMember]
        public DateTime Modified { get; set; }

        #endregion

        #region Methods

        private void Init(EltraDeviceNode device)
        {
            if (device != null)
            {                
                NodeId = device.NodeId;
                ChannelId = device.ChannelId;
                PlainContent = device.DeviceDescription?.DataSource;

                if (!string.IsNullOrEmpty(PlainContent))
                {
                    Content = Convert.ToBase64String(ZipHelper.Compress(PlainContent));
                    Encoding = "base64/zip";
                }

                Version = device.Version;
            }

            Modified = DateTime.Now;
        }

        #endregion
    }
}
