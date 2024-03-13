using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EltraCommon.Transport
{
    /// <summary>
    /// IHttpClient
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Connection timeout in seconds
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        IHttpClient Clone();

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancellationToken);
        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteAsync(string url);
        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string url);
        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cancelationToken"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancelationToken);
        /// <summary>
        /// PostAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(string url, StringContent message);
        /// <summary>
        /// PutAsync
        /// </summary>
        /// <param name="url"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> PutAsync(string url, StringContent message);
    }
}
