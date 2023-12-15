using System;
using System.IO;
using System.Runtime.InteropServices;
using EltraCommon.Os.Linux;

namespace EltraCommon.Dll
{
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
#pragma warning disable IDE1006 // Naming Styles

    /// <summary>
    /// EltraDllWrapper
    /// </summary>
    public static class EltraDllWrapper
    {
        #region Constants

        /// <summary>
        /// RtldNow
        /// </summary>
        public const int RtldNow = 2;
        /// <summary>
        /// RtldGlobal
        /// </summary>
        public const int RtldGlobal = 8;

        #endregion

        #region System

        /// <summary>
        /// dlopen
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("libdl.so")]

        public static extern IntPtr dlopen(string fileName, int flags);

        /// <summary>
        /// dlsym
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [DllImport("libdl.so")]
        public static extern IntPtr dlsym(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string symbol);

        /// <summary>
        /// dlclose
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("libdl.so")]
        public static extern int dlclose(IntPtr handle);

        /// <summary>
        /// dlerror
        /// </summary>
        /// <returns></returns>
        [DllImport("libdl.so")]
        public static extern IntPtr dlerror();

        /// <summary>
        /// GetDllInstance
        /// </summary>
        /// <param name="libName"></param>
        /// <returns></returns>
        public static IntPtr GetDllInstance(string libName)
        {
            IntPtr dll;
            string fileName;

            if (SystemHelper.IsLinux)
            {
                fileName = $"lib{libName}.so";

                dll = dlopen(fileName, RtldNow | RtldGlobal);

                if (dll == IntPtr.Zero)
                {
                    var errPtr = dlerror();

                    throw new FileNotFoundException($"{fileName} not found: " + Marshal.PtrToStringAnsi(errPtr));
                }
            }
            else
            {
                fileName = $"{libName}.dll";

                dll = Os.Windows.KernelDll.LoadLibrary(fileName);

                if (dll == IntPtr.Zero)
                {
                    throw new ArgumentNullException($"{fileName} not found!");
                }
            }

            return dll;
        }

        /// <summary>
        /// GetProcAddress
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <param name="funcName"></param>
        /// <returns></returns>
        public static IntPtr GetProcAddress(IntPtr dllHandle, string funcName)
        {
            IntPtr result;

            if (SystemHelper.IsLinux)
            {
                dlerror();

                result = dlsym(dllHandle, funcName);
            }
            else
            {
                result = Os.Windows.KernelDll.GetProcAddress(dllHandle, funcName);
            }

            return result;
        }

        /// <summary>
        /// FreeLibrary
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <returns></returns>
        public static bool FreeLibrary(IntPtr dllHandle)
        {
            bool result;

            if (SystemHelper.IsLinux)
            {
                result = dlclose(dllHandle) == 0;
            }
            else
            {
                result = Os.Windows.KernelDll.FreeLibrary(dllHandle);
            }

            return result;
        }

        #endregion
    }

#pragma warning restore CA1401 // P/Invokes should not be visible
#pragma warning restore CA2101 // Specify marshaling for P/Invoke string arguments
#pragma warning restore IDE1006 // Naming Styles
}
