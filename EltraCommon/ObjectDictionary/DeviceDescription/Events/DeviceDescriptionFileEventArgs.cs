using System;

namespace EltraCommon.ObjectDictionary.DeviceDescription.Events
{
    public class DeviceDescriptionFileEventArgs : EventArgs
    {
        public DeviceDescriptionFile DeviceDescriptionFile { get; set; }

        public DeviceDescriptionState State { get; set; }

        public Exception Exception { get; set; }
    }
}
