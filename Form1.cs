using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Threading;
using System.Xml;

namespace OpenXStreamLoader
{
    public partial class Form1 : Form
    {
        internal class TaskData
        {
            public Task _task;
            public ListViewItem _listItem;
            public string _consoleOutput;
        };

        private enum OnlineCheckPriority
        {
            Low,
            High
        }

        public enum OnlineStatus
        {
            Unknown,
            Checking,
            Public,
            Private,
            Away,
            Offline,
            Error
        }

        internal class FavoriteData
        {
            public ListViewItem _item;
            public OnlineStatus _status;
            public Image _profileImage;
        }

        private readonly float _version = 0.2f;
        private readonly string _streamlinkDefaultOptions = "--hls-timeout 120 --hls-playlist-reload-attempts 20 --hls-segment-timeout 90 --hds-segment-threads 8 --hls-segment-threads 8 --hds-timeout 120 --hds-segment-timeout 90 --hds-segment-attempts 20";
        private readonly object _onlineCheckQueueLock = new object();

        private Settings _settings;
        private Dictionary<string, TaskData> _tasks;
        private LinkedList<string> _onlineCheckLowPriorityQueue;
        private LinkedList<string> _onlineCheckHighPriorityQueue;
        private bool _onlineCheckIsRunning;
        private Thread _onlineCheckThread;
        private Dictionary<string, FavoriteData> _favoritesMap;
        private Dispatcher _dispatcher;
        private CookieContainer _cookies;
        private long _sizeOffline = -1;
        private PreviewForm _previewForm;
        private bool _showingProfilePictures = false;

        public Form1()
        {
            InitializeComponent();
            lvTasks.enableDoubleBuffering(true);
            lvFavorites.enableDoubleBuffering(true);
            _previewForm = new PreviewForm();

            _settings = new Settings();
            _tasks = new Dictionary<string, TaskData>();
            _onlineCheckLowPriorityQueue = new LinkedList<string>();
            _onlineCheckHighPriorityQueue = new LinkedList<string>();
            _favoritesMap = new Dictionary<string, FavoriteData>();
            _dispatcher = Dispatcher.CurrentDispatcher;
            _cookies = new CookieContainer();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //initWebRequest();
            setVersion();
            loadSettings();
            tmrFavoritesStatusCheck.Interval = _settings._favoritesUpdateInterval * 1000;
            tmrFavoritesStatusCheck.Enabled = true;

            _onlineCheckIsRunning = true;
            _onlineCheckThread = new Thread(new ThreadStart(onlineCheckProc));
            _onlineCheckThread.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopOnlineCheckThread();
            saveSettings();
        }

        private void setVersion()
        {
            var versionString = _version.ToString();

            Text += versionString;
            lbVersion.Text += versionString;
        }

        private void loadSettings()
        {
            setDefaultSettings();
            loadSettingsFromXml();
            applySettings();
        }

        private void setDefaultSettings()
        {
            _settings._streamlinkExePath = "Streamlink_Portable\\Streamlink.exe";
            _settings._streamlinkOptions = _streamlinkDefaultOptions;
            _settings._defaultRecordsPath = "";
            _settings._browserPath = "";
            _settings._httpRequestDelay = 5000;
            _settings._favoritesUpdateInterval = 300;
            _settings._waitingTaskInterval = 60;
        }

        private void applySettings()
        {
            tbStreamlinkExePath.Text = _settings._streamlinkExePath;
            tbStreamlinkOptions.Text = _settings._streamlinkOptions;
            tbDefaultRecordsPath.Text = _settings._defaultRecordsPath;
            tbDefaultRecordsPath.Text = _settings._defaultRecordsPath;
            nuHttpRequestDelay.Value = _settings._httpRequestDelay;

            nuFavoritesUpdateInterval.Value = _settings._favoritesUpdateInterval;
            tmrFavoritesStatusCheck.Interval = _settings._favoritesUpdateInterval * 1000;

            nuWaitingTaskInterval.Value = _settings._waitingTaskInterval;
        }

        private void loadSettingsFromXml()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load("config.xml");

                if (doc.GetElementsByTagName("Configuration").Count == 0)
                {
                    return;
                }

                var configurationElement = doc.GetElementsByTagName("Configuration")[0];
                var settingsElement = configurationElement["Settings"];
                var lastViewedElement = configurationElement["LastViewed"];
                var favoritesElement = configurationElement["Favorites"];

                var streamlinkExeElement = settingsElement["StreamlinkExe"];
                var streamlinkOptionsElement = settingsElement["StreamlinkOptions"];
                var defaultRecordsPathElement = settingsElement["DefaultRecordsPath"];
                var browserPathElement = settingsElement["BrowserPath"];

                if (streamlinkExeElement != null)
                {
                    _settings._streamlinkExePath = streamlinkExeElement.InnerText;
                }

                if (streamlinkOptionsElement != null)
                {
                    _settings._streamlinkOptions = streamlinkOptionsElement.InnerText;
                }

                if (defaultRecordsPathElement != null)
                {
                    _settings._defaultRecordsPath = defaultRecordsPathElement.InnerText;
                }

                if (browserPathElement != null)
                {
                    _settings._browserPath = browserPathElement.InnerText;
                }

                if (settingsElement.Attributes["HttpRequestDelay"] != null)
                {
                    _settings._httpRequestDelay = settingsElement.Attributes["HttpRequestDelay"].InnerText.ToInt32Def(5000).Clamp(nuHttpRequestDelay.Minimum.ToInt32(), nuHttpRequestDelay.Maximum.ToInt32());
                }

                if (settingsElement.Attributes["FavoritesUpdateInterval"] != null)
                {
                    _settings._favoritesUpdateInterval = settingsElement.Attributes["FavoritesUpdateInterval"].InnerText.ToInt32Def(300).Clamp(nuFavoritesUpdateInterval.Minimum.ToInt32(), nuFavoritesUpdateInterval.Maximum.ToInt32());
                }

                if (settingsElement.Attributes["WaitingTaskInterval"] != null)
                {
                    _settings._waitingTaskInterval = settingsElement.Attributes["WaitingTaskInterval"].InnerText.ToInt32Def(60).Clamp(nuWaitingTaskInterval.Minimum.ToInt32(), nuWaitingTaskInterval.Maximum.ToInt32());
                }

                if (lastViewedElement != null)
                {
                    var lastViewedElements = lastViewedElement.GetElementsByTagName("*");

                    for (int i = 0; i < lastViewedElements.Count; i++)
                    {
                        if (lastViewedElements[i].Attributes["Url"] != null)
                        {
                            cbId.Items.Add(lastViewedElements[i].Attributes["Url"].InnerText);
                        }
                    }
                }

                if (favoritesElement != null)
                {
                    var favoriteElements = favoritesElement.GetElementsByTagName("*");

                    for (int i = 0; i < favoriteElements.Count; i++)
                    {
                        if (favoriteElements[i].Attributes["Url"] != null)
                        {
                            string url = favoriteElements[i].Attributes["Url"].InnerText;
                            Image profileImage = null;

                            if (favoriteElements[i].Attributes["ProfileImage"] != null)
                            {
                                try
                                {
                                    using (var stream = new MemoryStream(System.Convert.FromBase64String(favoriteElements[i].Attributes["ProfileImage"].InnerText)))
                                    {
                                        profileImage = Image.FromStream(stream);
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }

                            addToFavorites(url, profileImage);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to load settings from \"config.xml\": " + exception.Message);
            }
        }

        private void saveSettings()
        {
            XmlDocument doc = new XmlDocument();
            XmlWriterSettings settings = new XmlWriterSettings();

            var configurationElement = doc.CreateElement("Configuration");
            var settingsElement = doc.CreateElement("Settings");
            var lastViewedElement = doc.CreateElement("LastViewed");
            var favoritesElement = doc.CreateElement("Favorites");

            doc.AppendChild(configurationElement);
            configurationElement.AppendChild(settingsElement);
            configurationElement.AppendChild(lastViewedElement);
            configurationElement.AppendChild(favoritesElement);

            var streamlinkExeElement = doc.CreateElement("StreamlinkExe");
            var streamlinkOptionsElement = doc.CreateElement("StreamlinkOptions");
            var defaultRecordsPathElement = doc.CreateElement("DefaultRecordsPath");
            var browserPathElement = doc.CreateElement("BrowserPath");

            streamlinkExeElement.InnerText = _settings._streamlinkExePath;
            streamlinkOptionsElement.InnerText = _settings._streamlinkOptions;
            defaultRecordsPathElement.InnerText = _settings._defaultRecordsPath;
            browserPathElement.InnerText = _settings._browserPath;

            settingsElement.AppendChild(streamlinkExeElement);
            settingsElement.AppendChild(streamlinkOptionsElement);
            settingsElement.AppendChild(defaultRecordsPathElement);
            settingsElement.AppendChild(browserPathElement);

            settingsElement.SetAttribute("HttpRequestDelay", _settings._httpRequestDelay.ToString());
            settingsElement.SetAttribute("FavoritesUpdateInterval", _settings._favoritesUpdateInterval.ToString());
            settingsElement.SetAttribute("WaitingTaskInterval", _settings._waitingTaskInterval.ToString());

            for (int i = 0; i < cbId.Items.Count; i++)
            {
                var url = cbId.Items[i].ToString();
                var element = doc.CreateElement(getIdFromUrl(url));

                element.SetAttribute("Url", url);
                lastViewedElement.AppendChild(element);
            }

            for (int i = 0; i < lvFavorites.Items.Count; i++)
            {
                var url = lvFavorites.Items[i].Text;
                var data = _favoritesMap[url];
                var element = doc.CreateElement(getIdFromUrl(url));

                element.SetAttribute("Url", url);

                if (data._profileImage != null)
                {
                    using (var imageStream = new MemoryStream())
                    using (var bmp = new Bitmap(data._profileImage))
                    {
                        bmp.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        element.SetAttribute("ProfileImage", System.Convert.ToBase64String(imageStream.ToArray()));
                    }
                }

                favoritesElement.AppendChild(element);
            }

            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.Encoding = Encoding.UTF8;
            settings.NewLineChars = "\r\n";

            doc.Save(XmlWriter.Create("config.xml", settings));
        }

        private HttpWebRequest creatWebRequest(string url, int timeout = 5000 /*ms*/)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;

            request.CookieContainer = _cookies;
            request.Method = "GET";
            request.KeepAlive = false;
            request.Timeout = timeout;

            return request;
        }

        private bool checkForNewVersion()
        {
            try
            {
                using (var response = creatWebRequest("http://github.com/voidtemp/OpenXStreamLoader/releases", 8000).GetResponse())
                {
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    string pageText = streamReader.ReadToEnd();
                    Regex regex = new Regex("<a href=\"\\/voidtemp\\/OpenXStreamLoader\\/tree\\/.*\\\" class=\\\"muted-link css-truncate\\\" title=\\\"(?<string>.*)\\\">");
                    string versionString = regex.Match(pageText).Groups["string"].ToString().ToLower();
                    float version = versionString.ToFloat32Def(0.0f);

                    if (version > _version)
                    {
                        lbVersionInfo.Text = "New version is available: v" + versionString;

                        if (MessageBox.Show("New version available: v" + versionString + "\nOpen github releases page?", "OpenXStreamLoader", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            openUrlInBrowser("https://github.com/voidtemp/OpenXStreamLoader/releases");
                        }

                        return true;
                    }
                    else
                    {
                        lbVersionInfo.Text = "Current version is the latest.";
                    }
                }
            }
            catch (Exception)
            {
                lbVersionInfo.Text = "Failed to retrieve latest version info.";
            }

            return false;
        }

        private void btChooseStreamlinkExe_Click(object sender, EventArgs e)
        {
            if (openStreamlinkExeDialog.ShowDialog() == DialogResult.OK)
            {
                _settings._streamlinkExePath = openStreamlinkExeDialog.FileName;
                tbStreamlinkExePath.Text = openStreamlinkExeDialog.FileName;
            }
        }

        private void tbStreamlinkExePath_TextChanged(object sender, EventArgs e)
        {
            _settings._streamlinkExePath = tbStreamlinkExePath.Text.Trim();
        }

        private void btChooseDefaultRecordsPath_Click(object sender, EventArgs e)
        {
            using (var openFolderDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true
            })
            {
                if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _settings._defaultRecordsPath = openFolderDialog.FileName;
                    tbDefaultRecordsPath.Text = _settings._defaultRecordsPath;
                }
            }
        }

        private void btStartRecord_Click(object sender, EventArgs e)
        {
            string url = cbId.Text.Trim();
            string quality = cbQuality.Text.Trim();

            if (String.IsNullOrEmpty(url))
            {
                return;
            }

            if (!File.Exists(_settings._streamlinkExePath))
            {
                MessageBox.Show("Please provide path to Streamlink.exe");
                tabsControl.SelectTab(2);

                return;
            }

            if (_tasks.ContainsKey(url))
            {
                MessageBox.Show("Task \"" + url + "\" already exists.");

                return;
            }

            cbId.Text = url; //trimmed
            addLastViewed(url);

            string fileNameTemplate = getFinalFileNameTemplate();
            string fullPath = Utils.getFullPathWithEndingSlash(fileNameTemplate);

            if (!String.IsNullOrEmpty(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            ListViewItem listItem = new ListViewItem(url);

            listItem.SubItems.Add(cbOnlineCheck.Checked ? "✓" : "");
            listItem.SubItems.Add("<unknown>");
            listItem.SubItems.Add(quality);
            listItem.SubItems.Add("");
            listItem.SubItems.Add("");
            listItem.SubItems.Add("");
            lvTasks.Items.Add(listItem);

            Task task = new Task(url, quality, cbOnlineCheck.Checked, _settings._streamlinkExePath, _settings._streamlinkOptions, fileNameTemplate, onTaskStatusChangedEvent, checkTastUrlOnline, getFinalFileNameFromTemplate, _settings._waitingTaskInterval);

            _tasks.Add(url, new TaskData()
            {
                _task = task,
                _listItem = listItem
            });

            task.Start();
        }

        private void cbId_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkIdName();
            printFinalFileName();
        }

        private void cbId_TextChanged(object sender, EventArgs e)
        {
            checkIdName();
            printFinalFileName();
        }

        private string getIdFromUrl(string url)
        {
            Regex regex = new Regex("\\.com\\/(?<string>(.*))\\/");

            return regex.Match(url).Groups["string"].ToString();
        }

        private void checkIdName()
        {
            if (cbSameNameAsId.Checked)
            {
                string id = getIdFromUrl(cbId.Text);

                if (!String.IsNullOrEmpty(id))
                {
                    tbFileName.Text = id;
                }
            }
        }

        private string getFinalFileNameTemplate()
        {
            string fileName = tbFileName.Text.Trim();
            string quality = cbQuality.Text.Trim();

            if (!Path.IsPathRooted(fileName))
            {
                if (!String.IsNullOrEmpty(_settings._defaultRecordsPath))
                {
                    fileName = Utils.pathAddBackSlash(_settings._defaultRecordsPath) + fileName;
                }
            }

            string fullPath = Utils.getFullPathWithEndingSlash(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            if (String.IsNullOrEmpty(extension))
            {
                extension = ".ts";
            }

            fileName = fullPath + fileNameWithoutExtension + " [%DATE%][" + quality + "].ts";
            //todo C# Sanitize File Name

            return fileName;
        }

        private string getFinalFileNameFromTemplate(string fileNameTemplate)
        {
            return Utils.getNonExistingFileName(fileNameTemplate.Replace("%DATE%", DateTime.Now.ToString("dd-MM-yyyy HH꞉mm꞉ss")));
        }

        private string getFinalFileName()
        {
            return getFinalFileNameFromTemplate(getFinalFileNameTemplate());
        }

        private void printFinalFileName()
        {
            tbFinalFileName.Text = getFinalFileName();
        }

        private void tbDefaultRecordsPath_TextChanged(object sender, EventArgs e)
        {
            _settings._defaultRecordsPath = tbDefaultRecordsPath.Text.Trim();
        }

        private void onTaskStatusChangedEvent(string url, Task.IStatusView status)
        {
            _dispatcher.Invoke(() =>
            {
                onTaskStatusChanged(url, status);
            });
        }

        private void onTaskStatusChanged(string url, Task.IStatusView status)
        {
            if (!_tasks.ContainsKey(url))
            {
                return;
            }

            var task = _tasks[url];
            var item = task._listItem;

            task._consoleOutput = status.ConsoleOutput;
            lvTasks.BeginUpdate();
            item.SubItems[4].Text = "";
            item.SubItems[5].Text = "";
            item.SubItems[6].Text = "";

            switch (status.State)
            {
                case Task.TaskState.InProgress:
                {
                    item.SubItems[2].Text = "Recording...";
                    item.SubItems[4].Text = getDurationString(status.Created);
                    item.SubItems[5].Text = Utils.formatBytes(status.FileSize);
                    item.SubItems[6].Text = status.FileName;
                    item.BackColor = Color.Lime;

                    break;
                }

                case Task.TaskState.Waiting:
                {
                    item.SubItems[2].Text = "Waiting...";
                    item.BackColor = Color.Gold;

                    if (status.FileSize > 0)
                    {
                        item.SubItems[4].Text = getDurationString(status.Created);
                        item.SubItems[5].Text = Utils.formatBytes(status.FileSize);
                        item.SubItems[6].Text = status.FileName;
                    }

                    break;
                }

                case Task.TaskState.Finished:
                {
                    item.SubItems[2].Text = "Finished";
                    item.SubItems[4].Text = getDurationString(status.Created);
                    item.SubItems[5].Text = Utils.formatBytes(status.FileSize);
                    item.SubItems[6].Text = status.FileName;
                    item.BackColor = SystemColors.Window;

                    break;
                }

                case Task.TaskState.StartProcessError:
                {
                    item.SubItems[2].Text = "Error";
                    item.BackColor = Color.Red;

                    break;
                }

                default:
                {
                    item.BackColor = SystemColors.Window;

                    break;
                }
            }

            lvTasks.EndUpdate();
        }

        private void addLastViewed(string url)
        {
            if (!cbId.Items.Contains(url))
            {
                cbId.Items.Insert(0, url);
            }
        }

        private void tbFileName_TextChanged(object sender, EventArgs e)
        {
            printFinalFileName();
        }

        private void cbSameNameAsId_CheckedChanged(object sender, EventArgs e)
        {
            checkIdName();
        }

        private void btAddToFavorites_Click(object sender, EventArgs e)
        {
            addToFavorites(cbId.Text.Trim(), null, OnlineCheckPriority.High);
        }

        private void addToFavorites(string url, Image profileImage = null, OnlineCheckPriority priority = OnlineCheckPriority.Low)
        {
            if (String.IsNullOrEmpty(url) || hasFavorite(url))
            {
                return;
            }

            ListViewItem item = new ListViewItem(url);

            _favoritesMap.Add(url, new FavoriteData { _item = item, _status = OnlineStatus.Checking, _profileImage = profileImage });
            item.ImageKey = "Checking";
            item.SubItems.Add("checking...");
            lvFavorites.Items.Add(item);

            if (profileImage != null)
            {
                updateImageList(ilProfilePictures, url, new Bitmap(profileImage, ilFavImages.ImageSize));
            }

            queueOnlineStatusCheck(url, priority);
        }

        private bool hasFavorite(string url)
        {
            return _favoritesMap.ContainsKey(url);
        }

        private void setFavoriteStatus(string url, OnlineStatus onlineStatus)
        {
            if (!_favoritesMap.ContainsKey(url))
            {
                return;
            }

            var data = _favoritesMap[url];
            var item = data._item;

            data._status = onlineStatus;
            lvFavorites.BeginUpdate();
            item.BackColor = SystemColors.Window;
            item.ImageKey = "offline";

            switch (onlineStatus)
            {
                case OnlineStatus.Public:
                {
                    item.SubItems[1].Text = "Public";
                    item.BackColor = Color.Lime;
                    updateFavoriteImage(url);

                    break;
                }

                case OnlineStatus.Private:
                {
                    item.SubItems[1].Text = "Private";
                    item.BackColor = Color.Gold;

                    break;
                }

                case OnlineStatus.Away:
                {
                    item.SubItems[1].Text = "Away";

                    break;
                }

                case OnlineStatus.Offline:
                {
                    item.SubItems[1].Text = "Offline";

                    break;
                }

                case OnlineStatus.Error:
                {
                    item.SubItems[1].Text = "Http error";
                    item.BackColor = Color.Red;
                    item.ImageKey = "HttpError";

                    break;
                }

                case OnlineStatus.Unknown:
                default:
                {
                    item.SubItems[1].Text = "<unknown>";
                    item.BackColor = Color.LightSteelBlue;

                    break;
                }
            }

            lvFavorites.EndUpdate();
        }

        private void updateFavoriteImage(string url)
        {
            if (!_favoritesMap.ContainsKey(url))
            {
                return;
            }

            var data = _favoritesMap[url];
            Regex regex = new Regex("\\.com\\/(?<string>(.*))\\/");
            string id = regex.Match(url).Groups["string"].ToString();

            try
            {
                using (var response = (HttpWebResponse)creatWebRequest("https://roomimg.stream.highwebmedia.com/ri/" + id + ".jpg").GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK &&
                        response.ContentType == "image/jpeg")
                    {
                        var bitmap = new Bitmap(response.GetResponseStream());
                        var bitmapResized = new Bitmap(bitmap, ilFavImages.ImageSize);

                        updateImageList(ilFavImages, url, bitmapResized);
                        updateImageList(ilProfilePictures, url, bitmapResized);
                        data._profileImage = bitmap;
                        data._item.ImageKey = url;
                    }
                    else
                    {
                        data._item.ImageKey = "unknown";
                    }
                }
            }
            catch (Exception)
            {
                data._item.ImageKey = "HttpError";
            }
        }

        private void updateFavoritesStatus()
        {
            for (int i = 0; i < lvFavorites.Items.Count; i++)
            {
                queueOnlineStatusCheck(lvFavorites.Items[i].Text, OnlineCheckPriority.Low);
            }
        }

        private void clearFavoritesStatus()
        {
            lvFavorites.BeginUpdate();

            for (int i = 0; i < lvFavorites.Items.Count; i++)
            {
                var item = lvFavorites.Items[i];
                var url = item.Text;

                item.ImageKey = "Checking";
                item.SubItems[1].Text = "checking...";
                item.BackColor = SystemColors.Window;

                if (_favoritesMap.ContainsKey(url))
                {
                    _favoritesMap[url]._status = OnlineStatus.Checking;
                }
            }

            lvFavorites.EndUpdate();
        }

        private void tmrFavoritesStatusCheck_Tick(object sender, EventArgs e)
        {
            updateFavoritesStatus();
        }

        private void checkTastUrlOnline(string url)
        {
            queueOnlineStatusCheck(url, OnlineCheckPriority.High);
        }

        private void queueOnlineStatusCheck(string url, OnlineCheckPriority priority)
        {
            lock (_onlineCheckQueueLock)
            {
                if (_onlineCheckHighPriorityQueue.Contains(url))
                {
                    return;
                }

                if (priority == OnlineCheckPriority.High)
                {
                    if (_onlineCheckLowPriorityQueue.Contains(url))
                    {
                        _onlineCheckLowPriorityQueue.Remove(url);
                    }

                    _onlineCheckHighPriorityQueue.AddLast(url);
                }
                else if (priority == OnlineCheckPriority.Low)
                {
                    if (_onlineCheckLowPriorityQueue.Contains(url))
                    {
                        return;
                    }

                    _onlineCheckLowPriorityQueue.AddLast(url);
                }

                Monitor.Pulse(_onlineCheckQueueLock);
            }
        }

        private void stopOnlineCheckThread()
        {
            _onlineCheckIsRunning = false;

            lock (_onlineCheckQueueLock)
            {
                Monitor.Pulse(_onlineCheckQueueLock);
            }

            _onlineCheckThread.Join();
        }

        private void onlineCheckProc()
        {
            int _httpRequestDelay = _settings._httpRequestDelay;
            string url;

            while (_onlineCheckIsRunning)
            {
                for (int high = 0; (getQueueCount(_onlineCheckHighPriorityQueue) > 0 || getQueueCount(_onlineCheckLowPriorityQueue) > 0) && _onlineCheckIsRunning; high++)
                {
                    _httpRequestDelay = _settings._httpRequestDelay;

                    if (high < 2)
                    {
                        if (getUrlFromQueue(_onlineCheckHighPriorityQueue, out url))
                        {
                            dispatchOnlineCheckResult(url, requestUrlOnlineStatus(url));
                            System.Threading.Thread.Sleep(_httpRequestDelay);

                            continue;
                        }
                    }

                    high = 0;

                    if (getUrlFromQueue(_onlineCheckLowPriorityQueue, out url))
                    {
                        dispatchOnlineCheckResult(url, requestUrlOnlineStatus(url));
                        System.Threading.Thread.Sleep(_httpRequestDelay);
                    }
                }

                lock (_onlineCheckQueueLock)
                {
                    while (_onlineCheckHighPriorityQueue.Count == 0 && _onlineCheckLowPriorityQueue.Count == 0 && _onlineCheckIsRunning)
                    {
                        Monitor.Wait(_onlineCheckQueueLock);
                    }
                }
            }
        }

        private bool getUrlFromQueue(LinkedList<string> queue, out string url)
        {
            lock (_onlineCheckQueueLock)
            {
                if (queue.Count > 0)
                {
                    url = queue.First.Value;
                    queue.RemoveFirst();

                    return true;
                }
            }

            url = "";

            return false;
        }

        private int getQueueCount(LinkedList<string> queue)
        {
            lock (_onlineCheckQueueLock)
            {
                return queue.Count;
            }
        }

        private void dispatchOnlineCheckResult(string url, OnlineStatus status)
        {
            _dispatcher.InvokeAsync(() =>
            {
                onOnlineCheckResult(url, status);
            });
        }

        private void onOnlineCheckResult(string url, OnlineStatus status)
        {
            if (_favoritesMap.ContainsKey(url))
            {
                setFavoriteStatus(url, status);
            }

            if (_tasks.ContainsKey(url))
            {
                updateTask(url, status);
            }
        }

        private void updateTask(string url, OnlineStatus status)
        {
            if (!_tasks.ContainsKey(url))
            {
                return;
            }

            if (status == OnlineStatus.Public)
            {
                _tasks[url]._task.Start();
            }
        }

        private OnlineStatus requestUrlOnlineStatus(string url)
        {
            OnlineStatus result = OnlineStatus.Unknown;

            try
            {
                using (var response = (HttpWebResponse)creatWebRequest(url).GetResponse())
                {
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    string pageText = streamReader.ReadToEnd();
                    Regex regex = new Regex("room_status\\\\u0022: \\\\u0022(?<string>.*)\\\\u0022, \\\\u0022edge_auth");
                    string statusString = regex.Match(pageText).Groups["string"].ToString().ToLower();

                    if (statusString == "public")
                    {
                        result = OnlineStatus.Public;
                    }
                    else if (statusString == "private")
                    {
                        result = OnlineStatus.Private;
                    }
                    else if (statusString == "away")
                    {
                        result = OnlineStatus.Away;
                    }
                    else if (statusString == "offline")
                    {
                        result = OnlineStatus.Offline;
                    }
                }
            }
            catch (Exception)
            {
                result = OnlineStatus.Error;
            }

            return result;
        }

        private OnlineStatus requestUrlOnlineStatusMethod2(string url)
        {
            string id = getIdFromUrl(url);

            try
            {
                using (var response = (HttpWebResponse)creatWebRequest("https://roomimg.stream.highwebmedia.com/ri/" + id + ".jpg").GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK &&
                        response.ContentType == "image/jpeg")
                    {
                        return response.ContentLength == _sizeOffline ? OnlineStatus.Offline : OnlineStatus.Public;
                    }
                    else
                    {
                        return OnlineStatus.Error;
                    }
                }
            }
            catch (Exception)
            {
                return OnlineStatus.Error;
            }
        }

        private void updateNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearFavoritesStatus();
            updateFavoritesStatus();
        }

        private void btChooseBrowserPath_Click(object sender, EventArgs e)
        {
            using (var openFolderDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true
            })
            {
                if (openFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _settings._browserPath = openFolderDialog.FileName;
                    tbBrowserPath.Text = _settings._browserPath;
                }
            }
        }

        private void tbBrowserPath_TextChanged(object sender, EventArgs e)
        {
            _settings._browserPath = tbBrowserPath.Text.Trim();
        }

        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFavoriteInBrowser();
        }

        private void lvFavorites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            openFavoriteInBrowser();
        }

        private void openFavoriteInBrowser()
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            openUrlInBrowser(lvFavorites.SelectedItems[0].Text);
        }

        private void openUrlInBrowser(string url)
        {
            try
            {
                if (String.IsNullOrEmpty(_settings._browserPath))
                {
                    System.Diagnostics.Process.Start(url);
                }
                else
                {
                    if (!Utils.runCmd(_settings._browserPath + " " + url))
                    {
                        MessageBox.Show("Failed to open browser");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to open browser: " + exception.Message);
            }
        }

        private void cmFavorites_Opening(object sender, CancelEventArgs e)
        {
            bool isItemClicked = lvFavorites.SelectedItems.Count > 0;

            startRecordToolStripMenuItem.Enabled = isItemClicked;
            openInBrowserToolStripMenuItem.Enabled = isItemClicked;
            copyURLToClipboardToolStripMenuItem.Enabled = isItemClicked;
            showImageorHoverWithCtrlPressedToolStripMenuItem.Enabled = isItemClicked;
            deleteFavToolStripMenuItem.Enabled = isItemClicked;
            updateThisToolStripMenuItem.Enabled = isItemClicked;
        }

        private void startRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFavoriteForRecord();
        }

        private void openFavoriteForRecord()
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            cbId.Text = lvFavorites.SelectedItems[0].Text;
            tabsControl.SelectTab(0);
            btStartRecord_Click(null, null);
        }

        private void showImageorHoverWithCtrlPressedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            var screen = Screen.FromControl(this).Bounds;

            _previewForm.setImage(_favoritesMap[lvFavorites.SelectedItems[0].Text]._profileImage);
            _previewForm.Location = new Point((screen.Width - MousePosition.X > _previewForm.Width) ? MousePosition.X : screen.Width - _previewForm.Width,
                (screen.Height - MousePosition.Y > _previewForm.Height) ? MousePosition.Y : screen.Height - _previewForm.Height);
            _previewForm.Activate();
            _previewForm.Show();
        }

        private void copyURLToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            Clipboard.SetText(lvFavorites.SelectedItems[0].Text);
        }

        private void deleteFavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            string url = lvFavorites.SelectedItems[0].Text;

            if (MessageBox.Show("Delete \"" + url + "\"?", "OpenXStreamLoader", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (_favoritesMap.ContainsKey(url))
            {
                lvFavorites.Items.Remove(_favoritesMap[url]._item);
                _favoritesMap.Remove(url);
            }
        }

        private void updateThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            var item = lvFavorites.SelectedItems[0];
            string url = item.Text;

            if (_favoritesMap.ContainsKey(url))
            {
                item.ImageKey = "Checking";
                item.SubItems[1].Text = "checking...";
                item.BackColor = SystemColors.Window;

                if (_favoritesMap.ContainsKey(url))
                {
                    _favoritesMap[url]._status = OnlineStatus.Checking;
                }

                queueOnlineStatusCheck(url, OnlineCheckPriority.High);
            }
        }

        private string getDurationString(DateTime start)
        {
            return DateTime.Now.Subtract(start).ToString(@"hh\:mm\:ss");
        }

        private void viewStreamLinkOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            var url = lvTasks.SelectedItems[0].Text;

            if (_tasks.ContainsKey(url))
            {
                MessageBox.Show(_tasks[url]._consoleOutput);
            }
        }

        private void cmTasks_Opening(object sender, CancelEventArgs e)
        {
            bool isItemClicked = lvTasks.SelectedItems.Count > 0;

            openFileToolStripMenuItem.Enabled = isItemClicked;
            openTaskUrlInBrowserToolStripMenuItem.Enabled = isItemClicked;
            showInFileExplorerToolStripMenuItem.Enabled = isItemClicked;
            addTaskToFavoritesToolStripMenuItem.Enabled = isItemClicked;
            copyURLToInputFieldToolStripMenuItem.Enabled = isItemClicked;
            deleteToolStripMenuItem.Enabled = isItemClicked;
            viewStreamLinkOutputToolStripMenuItem.Enabled = isItemClicked;
        }

        private void openTaskUrlInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            openUrlInBrowser(lvTasks.SelectedItems[0].Text);
        }

        private void showInFileExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            var filename = lvTasks.SelectedItems[0].SubItems[6].Text;

            if (String.IsNullOrEmpty(filename))
            {
                return;
            }

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + filename + "\"");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to open file \"" + filename + "\":\n" + exception.Message);
            }
        }

        private void addTaskToFavoritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            addToFavorites(lvTasks.SelectedItems[0].Text);
        }

        private void copyURLToInputFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            Clipboard.SetText(lvTasks.SelectedItems[0].Text);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            string url = lvTasks.SelectedItems[0].Text;

            if (MessageBox.Show("Delete \"" + url + "\"?", "OpenXStreamLoader", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (_tasks.ContainsKey(url))
            {
                _tasks[url]._task.Dispose();
                _tasks.Remove(url);
                lvTasks.Items.Remove(lvTasks.SelectedItems[0]);
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openTaskFile();
        }

        private void lvTasks_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            openTaskFile();
        }

        private void openTaskFile()
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            var filename = lvTasks.SelectedItems[0].SubItems[6].Text;

            if (String.IsNullOrEmpty(filename))
            {
                return;
            }

            try
            {
                System.Diagnostics.Process.Start(filename);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to open file \"" + filename + "\":\n" + exception.Message);
            }
        }

        private void nuFavoritesUpdateInterval_ValueChanged(object sender, EventArgs e)
        {
            _settings._favoritesUpdateInterval = nuFavoritesUpdateInterval.Value.ToInt32();
            tmrFavoritesStatusCheck.Interval = _settings._favoritesUpdateInterval * 1000;
        }

        private void nuWaitingTaskInterval_ValueChanged(object sender, EventArgs e)
        {
            _settings._waitingTaskInterval = nuWaitingTaskInterval.Value.ToInt32();
        }

        private void nuHttpRequestDelay_ValueChanged(object sender, EventArgs e)
        {
            _settings._httpRequestDelay = nuHttpRequestDelay.Value.ToInt32();
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            ActiveControl = lvFavorites;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("Close application?", "OpenXStreamLoader", MessageBoxButtons.YesNo) == DialogResult.No;
        }

        private void btStreamlinkDefaultOptions_Click(object sender, EventArgs e)
        {
            _settings._streamlinkOptions = _streamlinkDefaultOptions;
            tbStreamlinkOptions.Text = _settings._streamlinkOptions;
        }

        private void tbStreamlinkOptions_TextChanged(object sender, EventArgs e)
        {
            _settings._streamlinkOptions = tbStreamlinkOptions.Text;
        }

        private void tmrCheckForNewVersion_Tick(object sender, EventArgs e)
        {
            tmrCheckForNewVersion.Enabled = false;
            checkForNewVersion();
        }

        private void lbProductPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrlInBrowser(lbProductPage.Text);
        }

        private void lbReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrlInBrowser(lbReleases.Text);
        }

        private void lbStreamlinkOnlineHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openUrlInBrowser("https://streamlink.github.io/cli.html");
        }

        private void lvFavorites_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                _showingProfilePictures = true;
                showFavoriteProfilePictures();
            }
        }

        private void lvFavorites_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control && _showingProfilePictures)
            {
                _showingProfilePictures = false;
                showFavoriteStatusPictures();
            }
        }

        private void showFavoriteProfilePictures()
        {
            lvFavorites.BeginUpdate();
            lvFavorites.LargeImageList = ilProfilePictures;

            foreach (ListViewItem item in lvFavorites.Items)
            {
                if (ilProfilePictures.Images.ContainsKey(item.Text))
                {
                    item.ImageKey = item.Text;
                }
                else
                {
                    item.ImageKey = "noimage";
                }
            }

            lvFavorites.EndUpdate();
        }

        private void showFavoriteStatusPictures()
        {
            lvFavorites.BeginUpdate();
            lvFavorites.LargeImageList = ilFavImages;

            foreach (ListViewItem item in lvFavorites.Items)
            {
                if (_favoritesMap.ContainsKey(item.Text))
                {
                    var data = _favoritesMap[item.Text];

                    switch (data._status)
                    {
                        case OnlineStatus.Public:
                        {
                            item.ImageKey = item.Text;

                            break;
                        }

                        case OnlineStatus.Checking:
                        {
                            item.ImageKey = "Checking";

                            break;
                        }

                        case OnlineStatus.Error:
                        {
                            item.ImageKey = "HttpError";

                            break;
                        }

                        case OnlineStatus.Unknown:
                        {
                            item.ImageKey = "unknown";

                            break;
                        }

                        default:
                        {
                            item.ImageKey = "offline";

                            break;
                        }
                    }
                }
            }

            lvFavorites.EndUpdate();
        }

        private void updateImageList(ImageList imageList, string key, Image image)
        {
            if (imageList.Images.ContainsKey(key))
            {
                imageList.Images[imageList.Images.IndexOfKey(key)] = image;
            }
            else
            {
                imageList.Images.Add(key, image);
            }
        }
    }
}
