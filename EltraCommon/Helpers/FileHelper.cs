using EltraCommon.Logger;
using System;
using System.IO;

namespace EltraCommon.Helpers
{
    /// <summary>
    /// FileHelper - file manipulation helper class
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Change file name extension and return new file name with modified extension
        /// </summary>
        /// <param name="fileFullPath">original file full path</param>
        /// <param name="toExtension">extension without dot</param>
        /// <param name="changedFileFullPath">full path to changed file name</param>
        /// <returns></returns>
        public static bool ChangeFileNameExtension(string fileFullPath, string toExtension, out string changedFileFullPath)
        {
            bool result = false;

            changedFileFullPath = string.Empty;

            try
            {
                var fi = new FileInfo(fileFullPath);

                string fileNameWithoutExtension = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                changedFileFullPath = Path.Combine(fi.DirectoryName, $"{fileNameWithoutExtension}.{toExtension}");

                result = true;
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"FileHelper - ChangeFileNameExtension", e);
            }

            return result;
        }
    }
}
