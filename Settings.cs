using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenXStreamLoader
{
    internal class Settings
    {
        public string _streamlinkExePath;
        public string _defaultRecordsPath;
        public string _streamlinkOptions;
        public string _browserPath;
        public int _httpRequestDelay;
        public int _favoritesUpdateInterval;
        public int _waitingTaskInterval;
        public bool _minimizeToTray;
        public bool _showOnlineNotification;
        public bool _recordOnStart;
    }
}
