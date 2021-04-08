using System;

namespace EltraCommon.Ipc.Events
{
    /// <summary>
    /// NpMessageEventArgs
    /// </summary>
    public class NpMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error occured
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Error reason
        /// </summary>
        public string Reason { get; set; }
    }
}
