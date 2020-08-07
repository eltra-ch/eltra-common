using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using EltraCommon.Logger;
using EltraCommon.Os.Interface;

namespace EltraCommon.Os.Windows
{
    /// <summary>
    /// SystemHelper
    /// </summary>
    public class SystemHelper : ISystemHelper
    {
        #region Methods

        /// <summary>
        /// IsWindows
        /// </summary>
        /// <returns></returns>
        public bool IsWindows()
        {
            return true;
        }

        /// <summary>
        /// GetDllInstance
        /// </summary>
        /// <param name="dllFileName"></param>
        /// <returns></returns>
        public IntPtr GetDllInstance(string dllFileName)
        {
            Assembly asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            string directory = Path.GetDirectoryName(asm.Location);

            // Working Directory
            FileInfo workingDirectory = new FileInfo(dllFileName);
            if (!File.Exists(workingDirectory.FullName))
            {
                // Application Directory
                FileInfo applicationDirectory = new FileInfo(directory + "\\" + dllFileName);
                if (!File.Exists(applicationDirectory.FullName))
                {
                    throw new Exception(string.Format("File {0} not found!\n\nWorking Directory: {1}\nApplication Directory: {2}", dllFileName, workingDirectory.FullName, applicationDirectory.FullName));
                }

                dllFileName = directory + "\\" + dllFileName;
            }

            var _eposCmdDll = KernelDll.LoadLibrary(dllFileName);

            if (_eposCmdDll == IntPtr.Zero)
            {
                throw new Exception(string.Format("{0} could not be loaded!", dllFileName));
            }

            return _eposCmdDll;
        }

        /// <summary>
        /// GetProcAddress
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <param name="funcName"></param>
        /// <returns></returns>
        public IntPtr GetProcAddress(IntPtr dllHandle, string funcName)
        {
            // Name "_VCM_xxxx@12"
            var result = KernelDll.GetProcAddress(dllHandle, funcName);

            if (result == IntPtr.Zero && funcName.Contains("@"))
            {
                funcName = funcName.Substring(1, funcName.IndexOf('@') - 1);

                // Name "VCM_xxxx"
                result = KernelDll.GetProcAddress(dllHandle, funcName);
            }

            return result;
        }

        /// <summary>
        /// Is64BitProcess
        /// </summary>
        /// <returns></returns>
        public bool Is64BitProcess()
        {
            if (IntPtr.Size == 8)
            {
                // 64-bit (IntPtr.Size = 8)
                return true;
            }

            return false;
        }

        /// <summary>
        /// FreeLibrary
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <returns></returns>
        public bool FreeLibrary(IntPtr dllHandle)
        {
            bool result;
            
            do
            {
                try
                {
                    result = KernelDll.FreeLibrary(dllHandle);
                }
                catch(SEHException se)
                {
                    MsgLogger.Exception($"{GetType().Name} - FreeLibrary", se.InnerException != null ? se.InnerException : se);
                    result = false;
                }
                catch(Exception e)
                {
                    MsgLogger.Exception($"{GetType().Name} - FreeLibrary", e);
                    result = false;
                }                
            }
            while(result);

            return result;
        }

        #endregion
    }
}