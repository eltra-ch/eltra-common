using EltraCommon.Contracts.Devices;
using EltraCommon.ObjectDictionary.Xdd;
using EltraCommon.ObjectDictionary.Common.DeviceDescription.Profiles.Application.Parameters;
using EltraCommon.ObjectDictionary.Epos4.DeviceDescription;
using EltraCommon.ObjectDictionary.Epos4.DeviceDescription.Profiles.Device.DataRecorder;
using System.Collections.Generic;

#pragma warning disable 1591

namespace EltraCommon.ObjectDictionary.Epos4
{
    public class Epos4ObjectDictionary : XddObjectDictionary
    {
        public Epos4ObjectDictionary(EltraDevice device)
            : base(device)
        {
        }

        public List<DataRecorder> DataRecorders { get; set; }

        public override bool Open()
        {
            bool result = base.Open();

            if (result && GetDeviceDescription() is Epos4DeviceDescription xdd)
            {
                DataRecorders = xdd.DataRecorders;
            }

            return result;
        }

        public Parameter GetRecorderChannelParameter(byte channelNumber)
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetChannelParameter(channelNumber);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public byte GetRecorderChannelCount()
        {
            byte result = 0;

            foreach (var dataRecorder in DataRecorders)
            {
                result += dataRecorder.GetChannelCount();
            }

            return result;
        }

        public Parameter GetRecorderSamplingPeriodParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetSamplingPeriodParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderMaxNumberOfSamplesParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetMaxNumberOfSamplesParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderPrecedingSamplesParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetPrecedingSamplesParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderControlWordParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetControlWordParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderStatusWordParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetStatusWordParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderTriggerVariableParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetTriggerVariableParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderTriggerModeParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetTriggerModeParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderTriggerHighValueParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetTriggerHighValueParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderTriggerLowValueParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetTriggerLowValueParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderTriggerMaskParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetTriggerMaskParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderChannelBufferParameter(byte channelNumber)
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetChannelBufferParameter(channelNumber);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderBufferTimestampParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetTimestampParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public Parameter GetRecorderSamplesCountParameter()
        {
            Parameter result = null;

            foreach (var dataRecorder in DataRecorders)
            {
                result = dataRecorder.GetSamplesCountParameter();

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }
    }
}
