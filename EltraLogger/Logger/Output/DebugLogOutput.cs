using EltraCommon.Logger.Formatter;
using System.Diagnostics;



namespace EltraCommon.Logger.Output
{
    class DebugLogOutput : LogOutput, ILogOutput
    {
        #region Constructors

        public DebugLogOutput()
        {
            Formatter = new DefaultLogFormatter();
        }

        #endregion

        #region Properties

        public string Name => "Debug";
        public ILogFormatter Formatter { get; }

        #endregion

        #region Methods

        public void Write(string source, LogMsgType type, string msg, bool newLine = true)
        {
            Lock();
            
            string formattedMsg = Formatter.Format(source, type, msg);

            if (!string.IsNullOrEmpty(formattedMsg))
            {
                Trace.WriteLine(formattedMsg);
            }
            
            Unlock();
        }

        #endregion
    }
}
