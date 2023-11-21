using System;
using System.Diagnostics;
using System.Threading;

namespace EltraCommon.Logger
{
    class LogOutput
    {
        #region Private fields
                
        private readonly Mutex _logMutex = new Mutex();

        #endregion

        #region Methods

        public void Lock()
        {
            try
            {
                _logMutex.WaitOne();
            }
            catch(Exception e)
            {
                Debug.Print(e.Message);
            }
        }

        public void Unlock()
        {
            try
            {
                _logMutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
        }

        #endregion
    }
}