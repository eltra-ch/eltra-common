using System;

namespace EltraXamCommon.Controls.Helpers
{
    public static class ThreadHelper
    {
        public static int MainThreadId { get; private set; }

        public static void Initialize(int mainThreadId)
        {
            MainThreadId = mainThreadId;
        }

        public static bool IsOnMainThread => Environment.CurrentManagedThreadId == MainThreadId;

        public static void RunOnMainThread(Action function)
        {
            if (ThreadHelper.IsOnMainThread)
            {
                function.Invoke();
            }
            else
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(function);
            }
        }
    }
}
