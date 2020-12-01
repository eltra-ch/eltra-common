using System;
using System.Net.Sockets;
using System.Collections.Generic;
using EltraCommon.Transport.Events;
using System.Threading.Tasks;
using EltraCommon.Contracts.Users;

namespace EltraCommon.Transport
{
    /// <summary>
    /// CloudControllerAdapter
    /// </summary>
    public class CloudControllerAdapter
    {
        #region Private fields

        private bool _good;
        private List<CloudControllerAdapter> _children;
        private static CloudTransporter _cloudTransporter;

        #endregion

        #region Constructors

        /// <summary>
        /// CloudControllerAdapter
        /// </summary>
        /// <param name="url"></param>
        public CloudControllerAdapter(string url)
        {
            Url = url;

            _good = true;
            _children = new List<CloudControllerAdapter>();

            RegisterEvents();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Transporter
        /// </summary>
        public CloudTransporter Transporter => _cloudTransporter ?? (_cloudTransporter = new CloudTransporter());

        /// <summary>
        /// State
        /// </summary>
        public bool Good 
        {
            get => _good;
            set
            {
                if(_good != value)
                {
                    _good = value;

                    OnGoodChanged();
                }                
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// State changed
        /// </summary>
        public event EventHandler<GoodChangedEventArgs> GoodChanged;

        #endregion

        #region Events handling

        private void OnSocketErrorChanged(object sender, SocketErrorChangedEventAgs e)
        {
            Good = e.SocketError == SocketError.Success;
        }

        private void OnGoodChanged()
        {
            GoodChanged?.Invoke(this, new GoodChangedEventArgs() { Good = Good });
        }

        private void OnChildGoodChanged(object sender, GoodChangedEventArgs e)
        {
            if (Good)
            {
                foreach (var child in _children)
                {
                    if(!child.Good)
                    {
                        Good = false;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Methods

        private void RegisterEvents()
        {
            Transporter.SocketErrorChanged += OnSocketErrorChanged;
        }

        /// <summary>
        /// Add child controller
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(CloudControllerAdapter child)
        {
            if (child != null)
            {
                child.GoodChanged += OnChildGoodChanged;

                _children.Add(child);
            }
        }

        /// <summary>
        /// Start controller
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            return true;
        }

        /// <summary>
        /// Stop controller
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            return true;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<string> Get(UserIdentity identity, string url)
        {
            return Transporter.Get(identity, url);
        }

        #endregion
    }
}
