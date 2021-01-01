using EltraCommon.Threads;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EltraCommon.Extensions
{
    /// <summary>
    /// AsyncExtensions
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// WithCancellation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }

            return task.Result;
        }

        /// <summary>
        /// UseWaitAsync
        /// </summary>
        /// <param name="semaphore"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<IDisposable> UseWaitAsync(this SemaphoreSlim semaphore, CancellationToken token = default(CancellationToken))
        {
            await semaphore.WaitAsync(token).ConfigureAwait(false);

            return new SemaphoreSlimReleaseWrapper(semaphore);
        }
    }
}
