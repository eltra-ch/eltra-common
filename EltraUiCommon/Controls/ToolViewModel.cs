using System;
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
                
        private bool _isSetUp;
        private IDeviceVcsFactory _deviceFactory;
        private Task _updateViewModelsTask;
        private CancellationTokenSource _updateViewModelsCts;
        
        private Task _updateTask = Task.CompletedTask;
        private Task _registerUpdateTask = Task.CompletedTask;
        private Task _unregisterUpdateTask = Task.CompletedTask;

        #endregion

        #region Constructors

        public ToolViewModel()
        {
            const int defaultUpdateInterval = 1000;

            _updateViewModelsTask = Task.CompletedTask;
            _updateViewModelsCts = new CancellationTokenSource();

            UpdateInterval = defaultUpdateInterval;
            UpdateViewModels = false;

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

            if (parent is ToolViewModel toolViewModel)
            {
                Agent = toolViewModel.Agent;
            }

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

		public bool Persistenced { get; set; }

        public bool CanSetUp { get => !_isSetUp; }

        #endregion

        #region Events

        public event EventHandler DeviceInitialized;
        public event EventHandler RefreshRequested;

        #endregion

        #region Events handling

        private void OnDeviceInitialized()
        {
            DeviceInitialized?.Invoke(this, EventArgs.Empty);
        }

        public void OnRefresh()
        {
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

        protected override void Init(VirtualCommandSet vcs)
        {
            _device = vcs.Device;
            _agent = vcs.Connector;

            base.Init(vcs);
        }

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
                foreach(var child in SafeChildrenArray)
                {
                    if (child is ToolViewModel toolViewModel)
                    {
                        toolViewModel.Agent = Agent;
                    }
                }

                if (Agent.Status == AgentStatus.Bound)
                {
                    Init();
                }
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

        protected virtual Task RegisterAutoUpdate()
        {
            return Task.CompletedTask;
        }

        private Task RegisterAutoUpdateAsync()
        {
            if (_registerUpdateTask.IsCompleted)
            {
                _registerUpdateTask = Task.Run(async () => { await RegisterAutoUpdate(); });
            }

            return _registerUpdateTask;
        }

        protected virtual Task UnregisterAutoUpdate()
        {
            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            RegisterAutoUpdateAsync();

            UpdateAllControlsAsync();
        }

        private Task UnregisterAutoUpdateAsync()
        {
            if (_unregisterUpdateTask.IsCompleted)
            {
                _unregisterUpdateTask = Task.Run(async () => { await UnregisterAutoUpdate(); });
            }


            return _unregisterUpdateTask;
        }

        protected virtual Task UpdateAllControls()
        {
            return Task.CompletedTask;
        }

        private Task UpdateAllControlsAsync()
        {
            if (_updateTask.IsCompleted)
            {
                _updateTask = Task.Run(async () =>
                {
                    await UpdateAllControls();
                });

            }

            return _updateTask;
        }

        public override bool StartCommunication()
        {
            RegisterAutoUpdateAsync();

            UpdateAllControlsAsync();

            return base.StartCommunication();
        }

        public override bool StopCommunication()
        {
            UnregisterAutoUpdateAsync();

            return base.StopCommunication();
        }

        public override Task Show()
        {
            RegisterAutoUpdateAsync();

            UpdateAllControlsAsync();

            return base.Show();
        }

        public override Task Hide()
        {
            UnregisterAutoUpdateAsync();

            return base.Hide();
        }

        #endregion
    }
}
