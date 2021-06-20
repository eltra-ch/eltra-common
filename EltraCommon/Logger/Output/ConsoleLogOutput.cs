using EltraCommon.Logger.Formatter;
using System;

namespace EltraCommon.Logger.Output
{
    class ConsoleLogOutput : ILogOutput
    {
        #region Private fields

        private readonly ILogFormatter _formatter;
        private bool _newLineActive;

        #endregion

        #region Constructors

        public ConsoleLogOutput()
        {
            _formatter = new DefaultLogFormatter();
        }

        #endregion

        #region Properties

        public string Name => "Console";
        public ILogFormatter Formatter { get => _formatter; }

        #endregion

        #region Methods

        public void Write(string source, LogMsgType type, string msg, bool newLine)
        {
            try
            {
                string formattedMsg = Formatter.Format(source, type, msg);
                var previuosFgColor = Console.ForegroundColor;

                if (!string.IsNullOrEmpty(formattedMsg))
                {
                    bool fgColorChanged = ChangeForegroundColor(type);

                    if (newLine)
                    {
                        if (!_newLineActive)
                        {
                            Console.Write("\r\n");
                        }

                        Console.WriteLine(formattedMsg);

                        _newLineActive = true;
                    }
                    else
                    {
                        formattedMsg += "\r";

                        Console.Write(formattedMsg);

                        _newLineActive = false;
                    }

                    if (fgColorChanged)
                    {
                        Console.ForegroundColor = previuosFgColor;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{GetType().Name} - Write", e.Message);
            }
        }

        private static bool ChangeForegroundColor(LogMsgType type)
        {
            bool result = false;

            switch (type)
            {
                case LogMsgType.Error:
                case LogMsgType.Exception:
                    Console.ForegroundColor = ConsoleColor.Red;
                    result = true;
                    break;
                case LogMsgType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    result = true;
                    break;
                case LogMsgType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    result = true;
                    break;
            }

            return result;
        }

        #endregion
    }
}
