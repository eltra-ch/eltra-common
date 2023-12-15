using System;
using System.Threading;

namespace EltraCommon.Threads
{
    /// <summary>
    /// SemaphoreSlimReleaseWrapper
    /// </summary>
    internal class SemaphoreSlimReleaseWrapper : IDisposable
    {
        #region Private fields

        private readonly SemaphoreSlim _semaphore;

        private bool _isDisposed;

        #endregion

        #region Constructors
        /// <summary>
        /// SemaphoreSlimReleaseWrapper
        /// </summary>
        /// <param name="semaphore"></param>
        public SemaphoreSlimReleaseWrapper(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (_isDisposed)
                {
                    return;
                }

                _semaphore.Release();

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
