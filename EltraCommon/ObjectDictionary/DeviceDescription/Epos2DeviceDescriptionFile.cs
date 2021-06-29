using System;
using EltraCommon.Logger;
using EltraCommon.Contracts.Devices;

namespace EltraCommon.ObjectDictionary.DeviceDescription
{
    class Epos2DeviceDescriptionFile : DeviceDescriptionFile
    {
        public Epos2DeviceDescriptionFile(EltraDevice device) : base(device)
        {   
        }

        protected override void ReadProductName()
        {
            try
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    foreach (var line in Content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        const string productNameTag = "ProductName=";
                        if (line.Contains(productNameTag))
                        {
                            ProductName = line.Substring(productNameTag.Length).TrimEnd();

                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadProductName", e);
            }
        }
    }
}
