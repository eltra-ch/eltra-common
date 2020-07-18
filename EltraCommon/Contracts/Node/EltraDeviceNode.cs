using System.Runtime.Serialization;

using EltraCommon.Contracts.Devices;
using EltraCommon.Logger;
using EltraCommon.ObjectDictionary.DeviceDescription;
using EltraCommon.ObjectDictionary.DeviceDescription.Factory;

namespace EltraCommon.Contracts.Node
{
    [DataContract]
    public class EltraDeviceNode : EltraDevice
    {
        #region Properties

        [DataMember]
        public string ChannelId { get; set; }

        [DataMember]
        public int NodeId { get; set; }

        #endregion

        #region Methods
        
        public override bool CreateDeviceDescription(DeviceDescriptionFile deviceDescriptionFile)
        {
            bool result = false;
            var content = deviceDescriptionFile?.Content;

            if (content != null)
            {
                var deviceDescription = DeviceDescriptionFactory.CreateDeviceDescription(this, deviceDescriptionFile);

                if (deviceDescription.Parse())
                {
                    DeviceDescription = deviceDescription;

                    result = true;
                }
                else
                {
                    MsgLogger.WriteError($"{GetType().Name} - CreateDeviceDescription", "Parsing device description failed!");
                }
            }
            else
            {
                MsgLogger.WriteError($"{GetType().Name} - CreateDeviceDescription", "Content is empty!");
            }

            return result;
        }

        #endregion
    }
}
