﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EltraCommon.Logger;
using EltraCommon.Contracts.Devices;
using EltraConnector.Agent;
using EltraCommon.Contracts.Channels;
using EltraConnector.UserAgent.Definitions;
using EltraUiCommon.Device.Factory;

namespace EltraUiCommon.Controls
{
    public class ToolViewModel : ToolViewBaseModel
    {
        #region Private fields

        private AgentConnector _agent;

        private EltraDevice _device;
        private bool _isOnline;
        private bool _isSetUp;
        private IDeviceVcsFactory _deviceFactory;
        private Task _updateViewModelsTask;
        private CancellationTokenSource _updateViewModelsCts;

        #endregion

        #region Constructors

        public ToolViewModel()
        {
            const int defaultUpdateInterval = 1000;

            _updateViewModelsTask = Task.CompletedTask;
            _updateViewModelsCts = new CancellationTokenSource();

            UpdateInterval = defaultUpdateInterval;
            UpdateViewModels = true;

            IsSupported = true;
            Persistenced = true;
        }

        public ToolViewModel(ToolViewBaseModel parent)
            : base(parent)
        {
            const int defaultUpdateInterval = 1000;

            _updateViewModelsTask = Task.CompletedTask;
            _updateViewModelsCts = new CancellationTokenSource();

            UpdateInterval = defaultUpdateInterval;

            IsSupported = true;
            Persistenced = true;
        }

        #endregion

        #region Properties

        public AgentConnector Agent
        {
            get => _agent;
            set
            {
                if(_agent != value)
                {
                    var newAgent = value;
                    bool equal = false;
                    if(_agent!=null && newAgent != null)
                    {
                        if(_agent.Channel != null && newAgent.Channel != null && _agent.Channel.Id == newAgent.Channel.Id)
                        {
                            equal = true;
                        }
                    }

                    if (!equal)
                    {
                        _agent = value;
                        OnAgentChanged();
                    }
                }
            }
        }

        public string Uuid { get; set; }

        public bool UpdateViewModels { get; set; }

        public int UpdateInterval { get; set; }

        public EltraDevice Device 
        { 
            get => _device; 
            set
            {
                if(_device != value)
                { 
                    _device = value;
                    OnDeviceChanged();
                }
            }
        }

        public bool IsOnline
        {
            get => _isOnline;
            set => SetProperty(ref _isOnline, value);
        }
		
		public bool Persistenced { get; set; }

        public bool CanSetUp { get => !_isSetUp; }

        #endregion

        #region Events

        public event EventHandler DeviceInitialized;

        #endregion

        #region Events handling

        private void OnDeviceInitialized()
        {
            DeviceInitialized?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

        public virtual void SetUp()
        {
            _isSetUp = true;
        }

    	public virtual void ButtonPressed(string classId)
        {   
        }

        public virtual void ButtonReleased(string classId)
        {
        }

        protected virtual void OnAgentChanged()
        {
            if (Agent != null)
            {
                Agent.StatusChanged += OnAgentStatusChanged;

                if (Agent.Status == AgentStatus.Bound)
                {
                    Init();
                }
            }
        }

        private void OnAgentStatusChanged(object sender, AgentStatusEventArgs e)
        {
            if (e.Status == AgentStatus.Bound)
            {
                //Init();
            }
        }

        protected virtual void OnDeviceChanged()
        {
            if (Agent != null && Agent.Status == AgentStatus.Bound)
            {
                Init();
            }
        }
		
		public virtual void Reset()
        {
        }

        protected virtual IDeviceVcsFactory GetDeviceFactory()
        {
            IDeviceVcsFactory result = null;

            if(_deviceFactory!=null)
            {
                result = _deviceFactory;
            }
            
            return result;
        }

        protected void SetDeviceFactory(IDeviceVcsFactory deviceVcsFactory)
        {
            _deviceFactory = deviceVcsFactory;
        }

        public void Init()
        {
            if(Agent != null && Device != null && IsSupported)
            { 
                if(UpdateViewModels)
                { 
                    if (IsRunning())
                    {
                        Stop();
                    }
                }

                VirtualCommandSet agentVcs;

                var deviceFactory = GetDeviceFactory();
                
                if (deviceFactory != null)
                {
                    agentVcs = deviceFactory.CreateVcs(Agent, Device);
                }
                else
                {
                    agentVcs = new VirtualCommandSet(Agent, Device);
                }
                
                var channel = Agent.Channel;

                if (channel != null)
                {
                    IsOnline = channel.Status == ChannelStatus.Online;

                    channel.StatusChanged += (sender, args) =>
                    {
                        IsOnline = args.Status == ChannelStatus.Online;
                    };
                }

                if (Device.Status == DeviceStatus.Ready)
                {
                    Init(agentVcs);

                    OnDeviceInitialized();
                }
                else
                {
                    Device.StatusChanged += (sender, args) =>
                    {
                        if (Device.Status == DeviceStatus.Ready)
                        {
                            Init(agentVcs);

                            OnDeviceInitialized();
                        }
                    };
                }

                if (UpdateViewModels)
                { 
                    Task.Run(() => { Run(); });
                }
            }
        }

        private async void Run()
        {
            const int updateSessionInterval = 200;

            while (ShouldRun())
            {
                try
                {
                    await UpdateViewModelsTree();
                }
                catch (Exception e)
                {
                    MsgLogger.Exception($"{GetType().Name} - Run", e);
                }
                
                var watch = new Stopwatch();

                watch.Start();

                while (watch.ElapsedMilliseconds < UpdateInterval && ShouldRun())
                {
                    await Task.Delay(updateSessionInterval);
                }
            }
        }
        
        private bool ShouldRun()
        {
            return !_updateViewModelsCts.IsCancellationRequested;
        }

        private bool IsRunning()
        {
            return !_updateViewModelsTask.IsCompleted;
        }

        public void Stop()
        {
            if (IsRunning())
            {
                _updateViewModelsCts.Cancel();

                _updateViewModelsTask.Wait();
            }
        }

        public virtual void ResetAgent()
        {
            IsOnline = false;

            _agent = null;

            foreach (var child in SafeChildrenArray)
            {
                if(child is ToolViewModel toolViewModel)
                {
                    toolViewModel.ResetAgent();
                }
            }
        }

        public override Task<bool> StartUpdate()
        {
            if (Agent != null && Agent.Channel != null)
            {
                IsOnline = Agent.Channel.Status == ChannelStatus.Online;
            }

            return base.StartUpdate();
        }

        public override Task<bool> StopUpdate()
        {
            IsOnline = false;

            return base.StopUpdate();
        }

        #endregion
    }
}
