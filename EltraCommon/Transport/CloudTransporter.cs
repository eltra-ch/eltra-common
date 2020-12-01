using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using EltraCommon.Contracts.Users;
using EltraCommon.Logger;
using EltraCommon.Transport.Events;

namespace EltraCommon.Transport
{
    /// <summary>
    /// CloudTransporter
    /// </summary>
    public class CloudTransporter
    {
        #region Private fields

        private const int DefaultMaxRedirectCount = 3;
        private const int DefaultMaxRetryCount = 1;
        private const int DefaultMaxWaitTimeInSec = 15;
        private const int DefaultRetryTimeout = 100;

        private SocketError _socketError;

        private Dictionary<string, HttpClient> _clients;

        #endregion

        #region Constructors

        /// <summary>
        /// CloudTransporter
        /// </summary>
        public CloudTransporter()
        {
            _clients = new Dictionary<string, HttpClient>();

            MaxRetryTimeout = DefaultRetryTimeout;
            MaxRetryCount = DefaultMaxRetryCount;
            MaxWaitTimeInSec = DefaultMaxWaitTimeInSec;
            MaxRedirectCount = DefaultMaxRedirectCount;

            SocketError = SocketError.Success;
        }

        #endregion

        #region Properties

        /// <summary>
        /// MaxRetryCount
        /// </summary>
        public int MaxRetryCount { get; set; }
        /// <summary>
        /// Max redirection count
        /// </summary>
        public int MaxRedirectCount { get; set; }
        /// <summary>
        /// Max retry timeout in ms.
        /// </summary>
        public int MaxRetryTimeout { get; set; }
        /// <summary>
        /// Max wait time in sec.
        /// </summary>
        public int MaxWaitTimeInSec { get; set; }

        /// <summary>
        /// SocketError
        /// </summary>
        public SocketError SocketError 
        { 
            get => _socketError;
            set
            {
                _socketError = value;
                OnSocketErrorChanged();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// SocketErrorChanged
        /// </summary>
        public event EventHandler<SocketErrorChangedEventAgs> SocketErrorChanged;

        #endregion

        #region Events handling

        private void OnSocketErrorChanged()
        {
            SocketErrorChanged?.Invoke(this, new SocketErrorChangedEventAgs() { SocketError = SocketError });
        }

        #endregion

        #region Methods

        private void ResetSocketError()
        {
            SocketError = SocketError.Success;
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(MaxWaitTimeInSec) };

            return client;
        }

        private void ExceptionHandling(Exception e)
        {
            if(e != null)
            {
                MsgLogger.Exception($"{GetType().Name} - ExceptionHandling", e);
            }
        }

        private void HttRequestExceptionHandling(HttpRequestException e)
        {
            ExceptionHandling(e.InnerException);

            if (e.InnerException is SocketException socketException)
            {
                SocketError = socketException.SocketErrorCode;
            }
            else
            {
                SocketError = SocketError.SocketError;
            }
        }

        private HttpClient GetHttpClient(UserIdentity identity)
        {
            HttpClient result = null;

            if (identity != null)
            {
                lock (this)
                {
                    if (_clients.ContainsKey(identity.Login))
                    {
                        result = _clients[identity.Login];
                    }
                    else
                    {
                        result = CreateHttpClient();

                        _clients.Add(identity.Login, result);
                    }
                }
            }
            else
            {
                throw new Exception("Identity not specified!");
            }

            return result;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <param name="json"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public async Task<TransporterResponse> Post(UserIdentity identity, string url, string path, string json, string apiVersion = "1")
        {
            TransporterResponse result = new TransporterResponse();
            
            int tryCount = 0;
            int redirectCount = 0;

            ResetSocketError();
            
            var client = GetHttpClient(identity);

            do
            {
                try
                {
                    var message = new StringContent(json, Encoding.UTF8, "application/json");
                    var builder = new UriBuilder(url) { Path = path };

                    tryCount++;

                    MsgLogger.WriteDebug($"{GetType().Name} - Post", $"post - url ='{url}' try count = {tryCount}/{MaxRetryCount}");
                    
                    if (apiVersion != "1")
                    {
                        var query = HttpUtility.ParseQueryString(string.Empty);

                        query["api-version"] = apiVersion;
                        
                        builder.Query = query.ToString();
                    }

                    var postResult = await client.PostAsync(builder.ToString(), message);

                    result.StatusCode = postResult.StatusCode;

                    if (postResult.IsSuccessStatusCode)
                    {
                        result.Content = await postResult.Content.ReadAsStringAsync();
                        tryCount = MaxRetryCount;
                    }
                    else if(postResult.StatusCode == HttpStatusCode.Redirect)
                    {
                        tryCount = 0;
                        redirectCount++;

                        MsgLogger.WriteDebug($"{GetType().Name} - Get", $"post - url ='{url}' redirection, count = {redirectCount}!");
                    }
                    else if(postResult.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MsgLogger.WriteDebug($"{GetType().Name} - Post", $"post - url ='{url}' 401 - unautorized!");
                        tryCount = MaxRetryCount;
                    }
                    else
                    {
                        MsgLogger.WriteError($"{GetType().Name} - Post", $"post - url ='{url}' failed! response = {postResult.IsSuccessStatusCode}");
						tryCount = MaxRetryCount;
                    }
                }
                catch (HttpRequestException e)
                {
                    HttRequestExceptionHandling(e);
                    result.Exception = e.InnerException;
                    tryCount = MaxRetryCount;
                }
                catch (Exception e)
                {
                    ExceptionHandling(e);
                    result.Exception = e;
                    tryCount = MaxRetryCount;
                }
            } while (tryCount < MaxRetryCount && redirectCount < MaxRedirectCount);
            
            return result;
        }

        /// <summary>
        /// GetStream
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<byte[]> GetStream(UserIdentity identity, string url)
        {
            byte[] result = null;
            int tryCount = 0;
            int redirectCount = 0;

            ResetSocketError();

            var client = GetHttpClient(identity);

            do
            {
                try
                {
                    tryCount++;

                    MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' try count = {tryCount}/{MaxRetryCount}");

                    using (var response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var stream = await response.Content.ReadAsStreamAsync();

                            if (stream != null)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    stream.CopyTo(ms);
                                    result = ms.ToArray();
                                }
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.Redirect)
                        {
                            tryCount = 0;
                            redirectCount++;

                            MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' redirection, count = {redirectCount}!");
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' not found!");
                            tryCount = MaxRetryCount;
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' 401 - unautorized!");
                            tryCount = MaxRetryCount;
                        }
                        else
                        {
                            MsgLogger.WriteError($"{GetType().Name} - Get", $"get - url ='{url}' failed! response = {response.IsSuccessStatusCode}");
                            tryCount = MaxRetryCount;
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    HttRequestExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
                catch (Exception e)
                {
                    ExceptionHandling(e); 
                    tryCount = MaxRetryCount;
                }
            } while (tryCount < MaxRetryCount && redirectCount < MaxRedirectCount);

            return result;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <param name="cancelationToken"></param>
        /// <returns></returns>
        public async Task<bool> Get(UserIdentity identity, string url, CancellationToken cancelationToken)
        {
            bool result = false;
            int redirectCount = 0;

            ResetSocketError();

            var client = GetHttpClient(identity);

            try
            {
                MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}'");
                int tryCount = 0;

                do
                {
                    using (var response = await client.GetAsync(url, cancelationToken))
                    {
                        if (response.StatusCode == HttpStatusCode.Redirect)
                        {
                            tryCount = 0;
                            redirectCount++;
                            MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' redirection!");
                        }
                        else if(response.StatusCode == HttpStatusCode.OK)
                        {
                            tryCount = MaxRetryCount;
                            result = true;
                        }
                    }

                    tryCount++;

                } while (tryCount < MaxRetryCount && redirectCount < MaxRedirectCount);
            }
            catch (HttpRequestException e)
            {
                MsgLogger.Exception($"{GetType().Name} - ExceptionHandling", e);
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - ExceptionHandling", e);
            }

            return result;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> Get(UserIdentity identity, string url)
        {
            string result = string.Empty;
            int tryCount = 0;
            int redirectCount = 0;

            ResetSocketError();
            
            var client = GetHttpClient(identity);

            do
            {
                try
                {
                    tryCount++;

                    MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' try count = {tryCount}/{MaxRetryCount}");
                    
                    using (var response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                using (var streamReader = new StreamReader(stream))
                                {
                                    result = await streamReader.ReadToEndAsync();
                                    tryCount = MaxRetryCount;
                                }
                            }
                        }
                        else if(response.StatusCode == HttpStatusCode.Redirect)
                        {
                            tryCount = 0;
                            redirectCount++;
                            
                            MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' redirection, count = {redirectCount}!");
                        }
                        else if(response.StatusCode == HttpStatusCode.NotFound)
                        {
                            MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' not found!");
                            tryCount = MaxRetryCount;
                        }
                        else if(response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' 401 - unautorized!");
                            tryCount = MaxRetryCount;
                        }
                        else
                        {
                            MsgLogger.WriteError($"{GetType().Name} - Get", $"get - url ='{url}' failed! response = {response.IsSuccessStatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    HttRequestExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
                catch (Exception e)
                {
                    ExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
            } while (tryCount < MaxRetryCount && redirectCount < MaxRedirectCount);

            return result;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Delete(UserIdentity identity, string url, CancellationToken cancellationToken)
        {
            bool result = false;

            int tryCount = 0;
            int redirectCount = 0;

            ResetSocketError();

            var client = GetHttpClient(identity);

            do
            {
                try
                {
                    tryCount++;

                    MsgLogger.WriteDebug($"{GetType().Name} - Delete", $"delete - url ='{url}' try count = {tryCount}/{MaxRetryCount}");

                    var deleteResult = await client.DeleteAsync(url, cancellationToken);

                    if (deleteResult.IsSuccessStatusCode)
                    {
                        result = true;
                        tryCount = MaxRetryCount;
                    }
                    else if (deleteResult.StatusCode == HttpStatusCode.Redirect)
                    {
                        MsgLogger.WriteDebug($"{GetType().Name} - Get", $"get - url ='{url}' redirection!");
                        tryCount = 0;
                        redirectCount++;
                    }
                    else
                    {
                        MsgLogger.WriteError($"{GetType().Name} - Delete", $"delete - url ='{url}' failed! response = {deleteResult.IsSuccessStatusCode}");
                        tryCount = MaxRetryCount;
                    }
                }
                catch (HttpRequestException e)
                {
                    HttRequestExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
                catch (Exception e)
                {
                    ExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
            } while (tryCount < MaxRetryCount && redirectCount < MaxRedirectCount);

            return result;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> Delete(UserIdentity identity, string url)
        {
            string result = string.Empty;

            int tryCount = 0;
            int redirectCount = 0;

            ResetSocketError();

            var client = GetHttpClient(identity);

            do
            {
                try
                {
                    tryCount++;

                    MsgLogger.WriteDebug($"{GetType().Name} - Delete", $"delete - url ='{url}' try count = {tryCount}/{MaxRetryCount}");
                    
                    var deleteResult = await client.DeleteAsync(url);

                    if (deleteResult.IsSuccessStatusCode)
                    {
                        result = await deleteResult.Content.ReadAsStringAsync();
                        tryCount = MaxRetryCount;
                    }
                    else if(deleteResult.StatusCode == HttpStatusCode.Redirect)
                    {
                        tryCount = 0;
                        redirectCount++;

                        MsgLogger.WriteDebug($"{GetType().Name} - Delete", $"get - url ='{url}' redirection, count = {redirectCount}!");
                    }
                    else
                    {
                        MsgLogger.WriteError($"{GetType().Name} - Delete", $"delete - url ='{url}' failed! response = {deleteResult.IsSuccessStatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    HttRequestExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
                catch (Exception e)
                {
                    ExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
            } while (tryCount < MaxRetryCount && redirectCount < MaxRedirectCount);

            return result;
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <param name="json"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public async Task<TransporterResponse> Put(UserIdentity identity, string url, string path, string json, string apiVersion = "1")
        {
            var result = new TransporterResponse();
            int tryCount = 0;
            int redirectCount = 0;

            ResetSocketError();

            var client = GetHttpClient(identity);

            do
            {
                try
                {
                    var message = new StringContent(json, Encoding.UTF8, "application/json");
                    var builder = new UriBuilder(url) { Path = path };
					
                    if (apiVersion != "1")
                    {
                        var query = HttpUtility.ParseQueryString(string.Empty);

                        query["api-version"] = apiVersion;

                        builder.Query = query.ToString();
                    }

                    tryCount++;
                    
                    var postResult = await client.PutAsync(builder.ToString(), message);

                    result.StatusCode = postResult.StatusCode;

                    if (postResult.IsSuccessStatusCode)
                    {
                        result.Content = await postResult.Content.ReadAsStringAsync();
                        tryCount = MaxRetryCount;
                    }
                    else if (postResult.StatusCode == HttpStatusCode.Redirect)
                    {
                        tryCount = 0;
                        redirectCount++;

                        MsgLogger.WriteDebug($"{GetType().Name} - Get", $"put - url ='{url}' redirection, count = {redirectCount}!");
                    }
                    else if(postResult.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MsgLogger.WriteDebug($"{GetType().Name} - Put", $"put - url ='{url}' 401 - unauthorized!");
                        tryCount = MaxRetryCount;
                    }
                    else
                    {
                        MsgLogger.WriteError($"{GetType().Name} - Put", $"put - url ='{url}' failed! response = {postResult.IsSuccessStatusCode}");
						tryCount = MaxRetryCount;
                    }
                }
                catch (HttpRequestException e)
                {
                    HttRequestExceptionHandling(e);
                    tryCount = MaxRetryCount;
                }
                catch (Exception e)
                {
                    ExceptionHandling(e);
                    tryCount = MaxRetryCount;
                    result.Exception = e;
                }
            } while (tryCount < MaxRetryCount && redirectCount < MaxRedirectCount);

            return result;
        }

        #endregion
    }
}
