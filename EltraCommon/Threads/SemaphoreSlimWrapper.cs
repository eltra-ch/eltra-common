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
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _semaphore.Release();

            _isDisposed = true;
        }

        #endregion
    }
}
