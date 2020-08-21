namespace EltraCommon.Logger
{
#pragma warning disable 1591

    public interface ILogFormatter
    {
        string Format(string source, LogMsgType type, string msg);         
    }
}
