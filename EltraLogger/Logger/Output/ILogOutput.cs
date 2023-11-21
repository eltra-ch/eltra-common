namespace EltraCommon.Logger
{
#pragma warning disable 1591

    public interface ILogOutput
    {
        ILogFormatter Formatter { get; }

        string Name { get; }

        void Write(string source, LogMsgType type, string msg, bool newLine = true);
    }
}
