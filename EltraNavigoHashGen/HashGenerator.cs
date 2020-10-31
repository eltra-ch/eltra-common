using EltraCommon.Helpers;
using EltraCommon.Logger;
using System;
using System.IO;

namespace EltraNavigoHashGen
{
    class HashGenerator
    {
        public string InputFileName { get; set; }

        public string OutputFileName { get; set; }

        public string OutputHashCode { get; set; }

        public bool Run()
        {
            bool result = false;

            try
            {
                if(File.Exists(InputFileName))
                {
                    MsgLogger.WriteLine($"Read input file '{InputFileName}'");
                    
                    if(ReadInput(out var base64EncodedInput))
                    {
                        result = GenerateOutput(base64EncodedInput);
                    }
                    else
                    {
                        MsgLogger.WriteError($"{GetType().Name} - Run", $"Reading file '{InputFileName}' failed!");
                    }
                }
                else
                {
                    MsgLogger.WriteError($"{GetType().Name} - Run", $"Specified file '{InputFileName}' doesn't exist!");
                }
            }
            catch(Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - Run", e);
            }

            return result;
        }

        private bool ReadInput(out string base64EncodedInput)
        {
            bool result = false;

            base64EncodedInput = string.Empty;

            try
            {
                var bytes = File.ReadAllBytes(InputFileName);

                base64EncodedInput = Convert.ToBase64String(bytes);

                result = base64EncodedInput.Length > 0;
            }
            catch(Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ReadInput", e);
            }

            return result;
        }

        private bool GenerateOutput(string base64EncodedInput)
        {
            bool result = false;

            if (FileHelper.ChangeFileNameExtension(InputFileName, "md5", out var md5Path))
            {
                try
                {
                    string hashCode = CryptHelpers.ToMD5(base64EncodedInput);

                    OutputHashCode = hashCode;

                    File.WriteAllText(md5Path, hashCode);

                    OutputFileName = md5Path;
                }
                catch(Exception e)
                {
                    MsgLogger.Exception($"{GetType().Name} - GenerateOutput", e);
                }

                result = true;
            }
            else
            {
                MsgLogger.WriteError($"{GetType().Name} - Run", $"Cannot change file '{InputFileName}' extension!");
            }

            return result;
        }

    }
}
