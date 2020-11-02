using System;

namespace EltraUiCommon.System
{
    public interface IInvokeOnMainThread
    {
        void BeginInvokeOnMainThread(Action action);
    }
}
