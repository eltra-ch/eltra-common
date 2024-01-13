using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EltraCommon.Transport
{
    /// <summary>
    /// EltraHttpClient
    /// </summary>
    public class EltraHttpClient : IHttpClient
    {
        private HttpClient _client;

        /// <summary>
        /// Connection timeout in seconds
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Client
        /// </summary>
        protected HttpClient Client => _client ?? (_client = new HttpClient() { Timeout = TimeSpan.FromSeconds(Timeout) });

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken)
        {
            return Client.DeleteAsync(url, cancellationToken);
        }

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return Client.DeleteAsync(url);
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return Client.GetAsync(url);
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancelationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancelationToken)
        {
            return Client.GetAsync(url, cancelationToken);
        }

        /// <summary>
        /// PostAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PostAsync(string url, StringContent message)
        {
            return Client.PostAsync(url, message);
        }

        /// <summary>
        /// PutAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> PutAsync(string url, StringContent message)
        {
            return Client.PutAsync(url, message);
        }
    }
}
