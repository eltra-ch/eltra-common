using System.Runtime.Serialization;

namespace EltraCommon.Contracts.Results
{
    /// <summary>
    /// RequestResult
    /// </summary>
    [DataContract]
    public class RequestResult
    {
        /// <summary>
        /// RequestResult
        /// </summary>
        public RequestResult()
        {
            Header = DefaultHeader;
            Result = true;
            Message = "Success";
            ErrorCode = ErrorCodes.Success;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        public static string DefaultHeader = "AZF7";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        [DataMember]
        public bool Result { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// ErrorCode
        /// </summary>
        [DataMember]
        public ErrorCodes ErrorCode { get; set; }
    }
}
