﻿using System.Xml;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Xdd.DeviceDescription.Profiles.Application.Parameters;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles.Device.DataRecorder.Channels
{
    public class ChannelBuffer
    {
        #region Properties

        public ParamBuffer ParamBuffer { get; set; }
        public ChannelSubBuffers ChannelSubBuffers { get; set; }

        #endregion

        #region Methods

        public bool Parse(XmlNode node)
        {
            bool result = true;

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "paramBuffer")
                {
                    var paramBuffer = new ParamBuffer();

                    if (!paramBuffer.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    ParamBuffer = paramBuffer;
                }
                else if (childNode.Name == "channelSubBuffers")
                {
                    var channelSubBuffers = new ChannelSubBuffers();

                    if (!channelSubBuffers.Parse(childNode))
                    {
                        result = false;
                        break;
                    }

                    ChannelSubBuffers = channelSubBuffers;
                }
            }

            return result;
        }

        public void Resolve(XddParameterList parameterList)
        {
            ParamBuffer.Resolve(parameterList);
            ChannelSubBuffers.Resolve(parameterList);
        }

        public Parameter GetCompleteBufferParameter()
        {
            return ParamBuffer?.Parameter;
        }

        #endregion
    }
}
