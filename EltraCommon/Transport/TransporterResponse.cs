using System;
using System.Net;

namespace EltraCommon.Transport
{
    /// <summary>
    /// TransporterResponse
    /// </summary>
    public class TransporterResponse
    {
        /// <summary>
        /// TransporterResponse
        /// </summary>
        public TransporterResponse()
        {
            StatusCode = HttpStatusCode.NotImplemented;
        }

        /// <summary>
        /// StatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; set; }
    }
}
