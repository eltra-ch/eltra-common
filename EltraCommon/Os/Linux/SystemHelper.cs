using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EltraCommon.Os.Interface;

namespace EltraCommon.Os.Linux
{
    /// <summary>
    /// SystemHelper
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
    public class SystemHelper : ISystemHelper
    {
        #region Constants

        private const int RtldNow = 2;
        private const int RtldGlobal = 8;

        #endregion

        #region Properties

        /// <summary>
        /// IsLinux
        /// </summary>
        public static bool IsLinux
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// IsWindows
        /// </summary>
        /// <returns></returns>
        public bool IsWindows()
        {
            return false;
        }

        [DllImport("libdl.so", CharSet = CharSet.Unicode)]

        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl.so")]
        private static extern IntPtr dlsym(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string symbol);

        [DllImport("libdl.so")]
        private static extern int dlclose(IntPtr handle);

        [DllImport("libdl.so")]
        private static extern IntPtr dlerror();

        /// <summary>
        /// GetDllInstance
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public IntPtr GetDllInstance(string dllName)
        {
            IntPtr dll = dlopen(dllName, RtldNow | RtldGlobal);
            
            if (dll == IntPtr.Zero)
            {
                var errPtr = dlerror();
                throw new NotSupportedException($"{dllName} not found: " + Marshal.PtrToStringAnsi(errPtr));
            }

            return dll;
        }

        private static string GetLinuxFuncName(string funcName)
        {
            string result = string.Empty;
            const int PrefixOffset = 1;

            int postfix = funcName.IndexOf("@", StringComparison.Ordinal);

            if (postfix > PrefixOffset)
            {
                result = funcName.Substring(0, postfix);
                result = result.Substring(PrefixOffset);
            }
            
            return result;
        }

        /// <summary>
        /// GetProcAddress
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <param name="funcName"></param>
        /// <returns></returns>
        public IntPtr GetProcAddress(IntPtr dllHandle, string funcName)
        {
            dlerror();
            
            string linuxFuncName = GetLinuxFuncName(funcName);

            var res = dlsym(dllHandle, linuxFuncName);           

            return res;
        }

        /// <summary>
        /// FreeLibrary
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <returns></returns>
        public bool FreeLibrary(IntPtr dllHandle)
        {
            var result = dlclose(dllHandle) == 0;

            return result;
        }

        /// <summary>
        /// Is64BitProcess
        /// </summary>
        /// <returns></returns>
        public bool Is64BitProcess()
        {
            return false;
        }

        #endregion
    }
#pragma warning restore IDE1006 // Naming Styles
}
