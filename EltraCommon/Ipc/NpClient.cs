using EltraCommon.Ipc.Events;
using EltraCommon.Logger;
using System;
using System.IO;
using System.IO.Pipes;

#pragma warning disable 1591

namespace EltraCommon.Ipc
{
    public class NpClient
    {
        #region Constructors

        public NpClient()
        {
            Name = "NpServer";
            Timeout = 30000;
        }

        #endregion

        #region Events

        public event EventHandler<NpMessageEventArgs> MessageSent;
        
        public event EventHandler<NpMessageEventArgs> ErrorOccured;

        #endregion

        #region Properties

        public string Name { get; set; }

        public int Timeout { get; set; }

        #endregion

        #region Events handling

        private void OnMessageSent(string message, bool error = false, string reason = "")
        {
            MessageSent?.Invoke(this, new NpMessageEventArgs() { Message = message, Error = error, Reason = reason });
        }

        private void OnErrorOccured(string message, string reason)
        {
            ErrorOccured?.Invoke(this, new NpMessageEventArgs() { Message = message, Error = true, Reason = reason });
        }

        #endregion

        #region Methods

        public bool Echo()
        {
            return SendMessage("echo");
        }

        public bool Stop()
        {
            return SendMessage("Stop");
        }

        public bool SendMessage(string msg)
        {
           bool result = false;

            try
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    using (var namedPipeClientStream = new NamedPipeClientStream(".", Name, PipeDirection.Out))
                    {
                        namedPipeClientStream.Connect(Timeout);

                        using (var writer = new StreamWriter(namedPipeClientStream))
                        {
                            writer.AutoFlush = true;

                            writer.WriteLine(msg);

                            OnMessageSent(msg);

                            result = true;
                        }
                    }
                }
                else
                {
                    OnErrorOccured(msg, "empty message");
                }
            }
            catch (TimeoutException e)
            {
                MsgLogger.Exception($"{GetType().Name} - SendMessage", e);

                OnErrorOccured("Timeout exception", e.Message);

                result = false;
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - SendMessage", e);

                OnErrorOccured("Exception", e.Message);

                result = false;
            }
           
            return result;
        }

        #endregion
    }
}
