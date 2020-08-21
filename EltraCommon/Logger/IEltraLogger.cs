namespace EltraCommon.Logger
{
#pragma warning disable 1591

    public interface IEltraLogger
    {
        bool NewLine { get; set;}

        void Info(string source, string msg);
        void Error(string source, string msg);
        void Exception(string source, string msg);
        void Debug(string source, string msg);
        void Warning(string source, string msg);
    }
}
