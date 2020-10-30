using EltraCommon.Contracts.Devices;
using EltraCommon.Contracts.ToolSet;
using EltraCommon.Helpers;
using EltraCommon.Logger;
using EltraCommon.Transport;
using EltraXamCommon.Controls;
using Newtonsoft.Json;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace EltraXamCommon.Plugins
{
    public class EltraNavigoPluginManager
    {
        #region Private fields

        private List<EltraPluginCacheItem> _pluginCache;
        private IDialogService _dialogService;

        #endregion

        #region Constructors

        public EltraNavigoPluginManager(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #endregion

        #region Events

        public event EventHandler<List<ToolViewModel>> PluginAdded;

        private void OnPluginAdded(DeviceToolPayload payload, List<ToolViewModel> viewModels)
        {
            PluginAdded?.Invoke(payload, viewModels);
        }

        #endregion

        #region Properties

        public string Url { get; set; }

        public List<EltraPluginCacheItem> PluginCache => _pluginCache ?? (_pluginCache = new List<EltraPluginCacheItem>());

        public string LocalPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        #endregion

        #region Methods

        public async Task<bool> DownloadTool(EltraDevice device)
        {
            bool result = false;

            var deviceToolSet = device?.ToolSet;

            if (deviceToolSet != null)
            {
                foreach (var tool in deviceToolSet.Tools)
                {
                    if (tool.Status == DeviceToolStatus.Enabled)
                    {
                        result = await DownloadTool(tool);

                        if (!result)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private string GetPluginFilePath(string fileName)
        {
            return Path.Combine(LocalPath, fileName);
        }

        private bool GetMd5FileName(string fileFullPath, out string md5FullPath)
        {
            bool result = false;
            
            md5FullPath = string.Empty;

            try
            {
                var fi = new FileInfo(fileFullPath);

                string fileNameWithoutExtension = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                md5FullPath = Path.Combine(fi.DirectoryName, $"{fileNameWithoutExtension}.md5");

                result = true;
            }
            catch(Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - GetMd5FileName", e);
            }
            
            return result;
        }

        private async Task<bool> DownloadTool(string fileName, string hashCode)
        {
            bool result = false;

            try
            {
                var fileFullPath = GetPluginFilePath(fileName);

                var transport = new CloudTransporter();

                var query = HttpUtility.ParseQueryString(string.Empty);

                query["fileName"] = fileName;
                query["hashCode"] = hashCode;

                var url = UrlHelper.BuildUrl(Url, "api/description/payload-download", query);

                var json = await transport.Get(url);

                var payload = JsonConvert.DeserializeObject<DeviceToolPayload>(json);

                var base64EncodedBytes = Convert.FromBase64String(payload.Content);

                File.WriteAllBytes(fileFullPath, base64EncodedBytes);

                if(GetMd5FileName(fileFullPath, out string md5FullPath))
                {
                    File.WriteAllText(md5FullPath, hashCode);
                    
                    result = true;
                }
            }
            catch (Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - DownloadTool", e);
            }

            return result;
        }

        private EltraPluginCacheItem FindPluginInCache(string payloadHashCode)
        {
            EltraPluginCacheItem result = null;

            foreach(var pluginCacheItem in PluginCache)
            {
                if(pluginCacheItem.HashCode == payloadHashCode)
                {
                    result = pluginCacheItem;
                    break;
                }
            }

            return result;
        }

        private bool UpdatePluginCache(DeviceToolPayload payload)
        {
            bool result = false;
            var pluginFilePath = GetPluginFilePath(payload.FileName);

            if (File.Exists(pluginFilePath))
            {
                if(UpdatePluginCache(pluginFilePath, payload.HashCode))
                {
                    var cacheItem = FindPluginInCache(payload.HashCode);

                    if (cacheItem != null && cacheItem.Plugin != null)
                    {
                        var viewModels = cacheItem.Plugin.GetViewModels();

                        OnPluginAdded(payload, viewModels);

                        result = true;
                    }
                }
            }

            return result;
        }

        private EltraPluginCacheItem FindPluginInFileSystem(DeviceToolPayload payload)
        {
            EltraPluginCacheItem result = null;
            string fullPath = Path.Combine(LocalPath, payload.FileName);

            try
            {
                if (File.Exists(fullPath) && GetMd5FileName(fullPath, out string md5FullPath) && File.Exists(md5FullPath))
                {
                    var hashCode = File.ReadAllText(md5FullPath);

                    if(hashCode == payload.HashCode)
                    {
                        if (UpdatePluginCache(fullPath, hashCode))
                        {
                            var cacheItem = FindPluginInCache(payload.HashCode);

                            if (cacheItem != null && cacheItem.Plugin != null)
                            {
                                var viewModels = cacheItem.Plugin.GetViewModels();

                                OnPluginAdded(payload, viewModels);

                                result = cacheItem;
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MsgLogger.Exception($"{GetType().Name} - FindPluginInFileSystem", e);
            }

            return result;
        }

        private async Task<bool> DownloadTool(DeviceTool deviceTool)
        {
            bool result = false;

            foreach(var payload in deviceTool.PayloadSet)
            {
                var pluginCacheItem = FindPluginInCache(payload.HashCode);

                if (pluginCacheItem != null)
                {
                    MsgLogger.WriteFlow($"{GetType().Name} - DownloadTool", $"payload file name {payload.FileName} found in cache");
                    result = true;
                }
                else
                {
                    pluginCacheItem = FindPluginInFileSystem(payload);

                    if (pluginCacheItem != null)
                    {
                        MsgLogger.WriteFlow($"{GetType().Name} - DownloadTool", $"payload file name {payload.FileName} found in file system");
                        result = true;
                    }
                    else
                    {
                        if (await DownloadTool(payload.FileName, payload.HashCode))
                        {
                            result = UpdatePluginCache(payload);
                        }
                        else
                        {
                            MsgLogger.WriteError($"{GetType().Name} - DownloadTool", $"payload file name {payload.FileName} download failed!");
                        }
                    }
                }
            }

            return result;
        }

        private List<string> GetlayoutPlugins()
        {
            const string eltraNavigo = "EltraNavigo";

            List<string> result = new List<string>();

            string path = LocalPath;
            var fileList = new DirectoryInfo(path).GetFiles("*.dll", SearchOption.AllDirectories);
            string exceptFile = $"{eltraNavigo}.dll";

            foreach (var file in fileList)
            {
                if (file.Name.StartsWith(eltraNavigo) && file.Name != exceptFile)
                {
                    result.Add(file.FullName);
                }
            }

            result = result.Distinct().ToList();

            return result;
        }

        public List<ToolViewModel> GetToolViewModels()
        {
            var result = new List<ToolViewModel>();

            var pluginFiles = GetlayoutPlugins();
            
            PluginCache.Clear();

            foreach (var pluginFile in pluginFiles)
            {
                if (GetMd5FileName(pluginFile, out string pluginMd5File))
                {
                    if (File.Exists(pluginMd5File))
                    {
                        try
                        {
                            var pluginHashCode = File.ReadAllText(pluginMd5File);

                            if(UpdatePluginCache(pluginFile, pluginHashCode))
                            {
                                var cacheItem = FindPluginInCache(pluginHashCode);

                                if(cacheItem != null && cacheItem.Plugin != null)
                                {
                                    var viewModels = cacheItem.Plugin.GetViewModels();

                                    result.AddRange(viewModels);
                                }
                            }
                        }
                        catch(Exception e)
                        {
                            MsgLogger.Exception($"{GetType().Name} - GetToolViewModels", e);
                        }
                    }
                }
            }

            return result;
        }

        private bool UpdatePluginCache(string assemblyPath, string assemblyHashCode)
        {
            bool result = false;

            try
            {
                var theAssembly = Assembly.LoadFrom(assemblyPath);
                
                Type[] types = theAssembly.GetTypes();

                foreach (Type t in types)
                {
                    var type = t.GetInterface("EltraXamCommon.Plugins.IEltraNavigoPlugin");

                    if (type != null && !string.IsNullOrEmpty(t.FullName))
                    {
                        var assemblyInstace = theAssembly.CreateInstance(t.FullName, false);

                        if (assemblyInstace is IEltraNavigoPlugin pluginInterface)
                        {
                            pluginInterface.DialogService = _dialogService;

                            if(FindPluginInCache(assemblyHashCode) == null)
                            {
                                var pluginCacheItem = new EltraPluginCacheItem() 
                                { FullPath = assemblyPath, HashCode = assemblyHashCode, Plugin = pluginInterface };

                                PluginCache.Add(pluginCacheItem);
                            }

                            result = true;

                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }

            return result;
        }

        #endregion
    }
}
