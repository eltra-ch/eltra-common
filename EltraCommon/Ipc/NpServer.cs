using EltraCommon.Ipc.Events;
using EltraCommon.Logger;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace EltraCommon.Ipc
{
    public class NpServer
    {
        #region Private fields

        Task _task;

        #endregion

        public NpServer()
        {
            Name = "NpServer";
            ExclusiveMode = true;
        }

        #region Properties

        public string Name { get; set; }

        public bool ExclusiveMode { get; set; }

        #endregion

        #region Events

        public event EventHandler StepRequested;

        public event EventHandler EchoReceived;

        public event EventHandler<NpMessageEventArgs> MessageReceived;

        public event EventHandler<NpMessageEventArgs> ErrorOccured;

        #endregion

        #region Events handling

        private void OnStopRequested()
        {
            StepRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnEchoReceived()
        {
            EchoReceived?.Invoke(this, EventArgs.Empty);
        }

        private void OnMessageReceived(string message, bool error = false, string reason = "")
        {
            MessageReceived?.Invoke(this, new NpMessageEventArgs() { Message = message, Error = error, Reason = reason });
        }

        private void OnErrorOccured(string message, string reason)
        {
            ErrorOccured?.Invoke(this, new NpMessageEventArgs() { Message = message, Error = true, Reason = reason });
        }

        #endregion

        #region Methods

        private bool IsServerRunning
        {
            get
            {
                bool result = false;
                
                var npc = new NpClient() { Name = Name, Timeout = 1000 };

                if (npc.Echo() && ExclusiveMode)
                {
                    result = true;
                }

                return result;
            }            
        }

        public bool Start()
        {
            bool result = false;

            Stop();

            if(!IsServerRunning)
            {
                _task = Task.Run(() => { Execute(); });

                result = true;
            }
            else
            {
                MsgLogger.WriteError($"{GetType().Name} - Start", "another server with same name is already running!");
            }

            return result;
        }

        public bool Stop()
        {
            bool result = false;

            if(_task != null && !_task.IsCompleted)
            {
                var npc = new NpClient() { Name = Name };

                result = npc.Stop();
                
                _task.Wait();
            }

            return result;
        }

        private void Execute()
        {
            using (var namedPipeServerStream = new NamedPipeServerStream(Name, PipeDirection.In))
            {
                namedPipeServerStream.WaitForConnection();

                try
                {
                    using (var reader = new StreamReader(namedPipeServerStream))
                    {
                        string message;

                        while ((message = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(message))
                            {
                                message = message.ToLower();

                                switch (message.ToLower())
                                {
                                    case "stop":
                                        OnStopRequested();
                                        break;
                                    case "echo":
                                        OnEchoReceived();
                                        break;
                                    default:
                                        OnMessageReceived(message);
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (IOException e)
                {
                    MsgLogger.Exception($"{GetType().Name} - Execute", e);

                    OnErrorOccured("I/O Exception", e.Message);
                }
                catch(Exception e)
                {
                    MsgLogger.Exception($"{GetType().Name} - Execute", e);

                    OnErrorOccured("Exception", e.Message);
                }
            }            
        }

        #endregion
    }
}
