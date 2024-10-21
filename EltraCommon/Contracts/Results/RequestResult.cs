using System.ComponentModel;
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
            Result = true;
            Message = "Success";
            ErrorCode = ErrorCodes.Success;
        }

        /// <summary>
        /// DefaultHeader
        /// </summary>
        private const string DefaultDiscriminator = "RequestResult";

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        [DefaultValue(DefaultDiscriminator)]
        public string Discriminator { get; set; } = DefaultDiscriminator;

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
