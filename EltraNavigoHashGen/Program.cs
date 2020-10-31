using EltraCommon.Logger;
using System.IO;

namespace EltraNavigoHashGen
{
    class Program
    {
        static int Main(string[] args)
        {
            int result = 1;
            MsgLogger.LogOutputs = "Console;Debug;";
            MsgLogger.LogLevels = MsgLogger.SupportedLogLevels;

            MsgLogger.WriteLine("Eltra navigo hash generator, Copyright (c) 2020 Dawid Sienkiewicz!");

            if(args.Length > 0)
            {
                string fileName = args[0];

                var hashGenerator = new HashGenerator() { InputFileName = fileName };

                if (hashGenerator.Run())
                {
                    FileInfo fi = new FileInfo(fileName);
                    FileInfo fo = new FileInfo(hashGenerator.OutputFileName);

                    MsgLogger.WriteLine($"File '{fi.Name}' hash '{hashGenerator.OutputHashCode}' generation success!");
                    
                    MsgLogger.WriteLine($"File '{fo.Name}' generation success!");
                    MsgLogger.WriteLine($"File output path '{fo.DirectoryName}'");

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
