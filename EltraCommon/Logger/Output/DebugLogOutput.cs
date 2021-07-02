using EltraCommon.Logger.Formatter;
using System.Diagnostics;

namespace EltraCommon.Logger.Output
{
    class DebugLogOutput : ILogOutput
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

        public void Write(string source, LogMsgType type, string msg, bool newLine)
        {
            lock (this)
            {
                string formattedMsg = Formatter.Format(source, type, msg);

                if (!string.IsNullOrEmpty(formattedMsg) && type == LogMsgType.Debug)
                {
                    Debug.WriteLine(formattedMsg);
                }
            }
        }

        #endregion
    }
}
