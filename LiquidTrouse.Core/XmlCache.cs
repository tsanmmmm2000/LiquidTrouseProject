using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Xml;

namespace LiquidTrouse.Core
{
    public class XmlCache
    {
        private static readonly ObjectCache objectCache = MemoryCache.Default;

        private readonly string _filePath;
        public string FilePath
        {
            get { return _filePath; }
        }

        private readonly int _monitorIntervalInSeconds = 120;
        public int MonitorIntervalInSeconds
        {
            get { return _monitorIntervalInSeconds; }
        }

        public XmlCache(string filePath, int monitorIntervalInSeconds = 120)
        {
            _filePath = filePath;
            _monitorIntervalInSeconds = monitorIntervalInSeconds;
        }

        public XmlDocument Read()
        {
            var ds = new DataSet();
            if (String.IsNullOrEmpty(_filePath))
            {
                throw new ArgumentException("Invalid configFilePath.");
            }
            
            var key = _filePath;
            lock (this)
            {
                var data = objectCache[key] as XmlDocument;
                if (data != null)
                {
                    Utility.DebugLog(String.Format("[From Cache] XmlCacheConfig ({0})", _filePath));
                    return data;
                }

                if (!File.Exists(_filePath))
                {
                    throw new FileNotFoundException(String.Format("ConfigFile {0} not found.", _filePath));
                }

                var policy = new CacheItemPolicy();
                var monitor = new HostFileChangeMonitor(new List<string> { key });

                policy.ChangeMonitors.Add(monitor);
                policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(720);

                var wrapperData = new XmlDocument();

                var text = File.ReadAllText(_filePath);
                if (!String.IsNullOrEmpty(text))
                {
                    var bts = Encoding.UTF8.GetBytes(text);
                    wrapperData.Load(new MemoryStream(bts));
                }

                Utility.DebugLog(String.Format("[From Resource] XmlCacheConfig ({0})", _filePath));

                objectCache.Set(key, wrapperData, policy);

                return wrapperData;
            }
        }
    }
}
