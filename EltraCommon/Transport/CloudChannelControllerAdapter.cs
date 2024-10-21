using EltraCommon.Contracts.Channels;

namespace EltraCommon.Transport
{
    /// <summary>
    /// CloudChannelControllerAdapter
    /// </summary>
    public class CloudChannelControllerAdapter : CloudControllerAdapter
    {
        #region Constructors

        /// <summary>
        /// CloudChannelControllerAdapter
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="channel"></param>
        public CloudChannelControllerAdapter(IHttpClient httpClient, string url, Channel channel)
            : base(httpClient, url)
        {
            Channel = channel;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Channel
        /// </summary>
        protected Channel Channel { get; }

        #endregion
    }
}
