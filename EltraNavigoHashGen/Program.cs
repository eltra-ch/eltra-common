using EltraCommon.Logger;

namespace EltraNavigoHashGen
{
    class Program
    {
        static int Main(string[] args)
        {
            int result = 1;
            MsgLogger.LogOutputs = "Console";

            MsgLogger.WriteLine("Eltra navigo hash generator, Copyright (c) 2020 Dawid Sienkiewicz!");

            if(args.Length > 0)
            {
                string fileName = args[0];

                var hashGenerator = new HashGenerator() { InputFileName = fileName };

                if (hashGenerator.Run())
                {
                    MsgLogger.WriteLine($"File '{fileName}' hash generation success!");
                    MsgLogger.WriteLine($"File '{hashGenerator.OutputFileName}' hash generation success!");

                    result = 0;
                }
                else
                {
                    MsgLogger.WriteError("Program - Main", $"File '{fileName}' hash generation failed!");
                }
            }
            else
            {
                MsgLogger.WriteError("Program - Main", "No file specified!");
            }

            return result;
        }
    }
}
