using System;

namespace EltraCommon.Os.Interface
{
    /// <summary>
    /// ISystemHelper
    /// </summary>
    public interface ISystemHelper
    {
        /// <summary>
        /// GetDllInstance
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        IntPtr GetDllInstance(string dllName);
        /// <summary>
        /// Is64BitProcess
        /// </summary>
        /// <returns></returns>
        bool Is64BitProcess();
        /// <summary>
        /// IsWindows
        /// </summary>
        /// <returns></returns>
        bool IsWindows();
        /// <summary>
        /// GetProcAddress
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <param name="funcName"></param>
        /// <returns></returns>
        IntPtr GetProcAddress(IntPtr dllHandle, string funcName);
        /// <summary>
        /// FreeLibrary
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <returns></returns>
        bool FreeLibrary(IntPtr dllHandle);
    }
}
