namespace EltraXamCommon.Plugins
{
    public class EltraPluginCacheItem
    {
        public IEltraNavigoPluginService PluginService { get; set; }

        public string PayloadId { get; set; }

        public string FullPath { get; set; }

        public string HashCode { get; set; }
    }
}