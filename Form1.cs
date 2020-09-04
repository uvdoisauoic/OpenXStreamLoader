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
using System.Security.Policy;

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

        private enum OnlineStatus
        {
            Unknown,
            Public,
            Private,
            Away,
            Offline,
            Error
        }

        private readonly object _onlineCheckQueueLock = new object();

        private Settings _settings;
        private Dictionary<string, TaskData> _tasks;
        private LinkedList<string> _onlineCheckLowPriorityQueue;
        private LinkedList<string> _onlineCheckHighPriorityQueue;
        private bool _onlineCheckIsRunning;
        private Thread _onlineCheckThread;
        private Dictionary<string, ListViewItem> _favoritesMap;
        private Dispatcher _dispatcher;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            _settings = new Settings();
            _tasks = new Dictionary<string, TaskData>();
            _onlineCheckLowPriorityQueue = new LinkedList<string>();
            _onlineCheckHighPriorityQueue = new LinkedList<string>();
            _favoritesMap = new Dictionary<string, ListViewItem>();
            _dispatcher = Dispatcher.CurrentDispatcher;

            LoadSettings();
            tmrFavoritesStatusCheck.Interval = _settings._favoritesUpdateInterval * 1000;
            tmrFavoritesStatusCheck.Enabled = true;

            _onlineCheckIsRunning = true;
            _onlineCheckThread = new Thread(new ThreadStart(onlineCheckProc));
            _onlineCheckThread.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopOnlineCheckThread();
            SaveSettings();
        }

        private void LoadSettings()
        {
            _settings._streamlinkExePath = Utils.INI_ReadValueFromFile("Settings", "StreamlinkExe", "", ".\\config.ini");
            tbStreamlinkExePath.Text = _settings._streamlinkExePath;

            _settings._defaultRecordsPath = Utils.INI_ReadValueFromFile("Settings", "DefaultRecordsPath", "", ".\\config.ini");
            tbDefaultRecordsPath.Text = _settings._defaultRecordsPath;

            _settings._browserPath = Utils.INI_ReadValueFromFile("Settings", "BrowserPath", "", ".\\config.ini");
            tbBrowserPath.Text = _settings._browserPath;

            _settings._httpRequestDelay = Utils.INI_ReadValueFromFile("Settings", "HttpRequestDelay", "", ".\\config.ini").ToInt32Def(5000).Clamp(nuHttpRequestDelay.Minimum.ToInt32(), nuHttpRequestDelay.Maximum.ToInt32());
            nuHttpRequestDelay.Value = _settings._httpRequestDelay;

            _settings._favoritesUpdateInterval = Utils.INI_ReadValueFromFile("Settings", "FavoritesUpdateInterval", "", ".\\config.ini").ToInt32Def(300).Clamp(nuFavoritesUpdateInterval.Minimum.ToInt32(), nuFavoritesUpdateInterval.Maximum.ToInt32());
            nuFavoritesUpdateInterval.Value = _settings._favoritesUpdateInterval;

            _settings._waitingTaskInterval = Utils.INI_ReadValueFromFile("Settings", "WaitingTaskInterval", "", ".\\config.ini").ToInt32Def(60).Clamp(nuWaitingTaskInterval.Minimum.ToInt32(), nuWaitingTaskInterval.Maximum.ToInt32());
            nuWaitingTaskInterval.Value = _settings._waitingTaskInterval;

            for (int i = 0; ; i++)
            {
                string lastViewed = Utils.INI_ReadValueFromFile("LastViewed", "LastViewed" + i.ToString(), "", ".\\config.ini");

                if(lastViewed == "")
                {
                    break;
                }

                cbId.Items.Add(lastViewed);
            }

            for (int i = 0; ; i++)
            {
                string url = Utils.INI_ReadValueFromFile("Favorites", "Fav" + i.ToString(), "", ".\\config.ini");

                if (url == "")
                {
                    break;
                }

                addToFavorites(url);
            }
        }

        private void SaveSettings()
        {
            Utils.INI_WriteValueToFile("Settings", "StreamlinkExe", _settings._streamlinkExePath, ".\\config.ini");
            Utils.INI_WriteValueToFile("Settings", "DefaultRecordsPath", _settings._defaultRecordsPath, ".\\config.ini");
            Utils.INI_WriteValueToFile("Settings", "BrowserPath", _settings._browserPath, ".\\config.ini");
            Utils.INI_WriteValueToFile("Settings", "HttpRequestDelay", _settings._httpRequestDelay.ToString(), ".\\config.ini");
            Utils.INI_WriteValueToFile("Settings", "FavoritesUpdateInterval", _settings._favoritesUpdateInterval.ToString(), ".\\config.ini");
            Utils.INI_WriteValueToFile("Settings", "WaitingTaskInterval", _settings._waitingTaskInterval.ToString(), ".\\config.ini");

            Native.WritePrivateProfileString("LastViewed", null, null, ".\\config.ini");

            for (int i = 0; i < cbId.Items.Count; i ++)
            {
                Utils.INI_WriteValueToFile("LastViewed", "LastViewed" + i.ToString(), cbId.Items[i].ToString(), ".\\config.ini");
            }

            Native.WritePrivateProfileString("Favorites", null, null, ".\\config.ini");

            for (int i = 0; i < lvFavorites.Items.Count; i++)
            {
                Utils.INI_WriteValueToFile("Favorites", "Fav" + i.ToString(), lvFavorites.Items[i].Text, ".\\config.ini");
            }
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

            if(String.IsNullOrEmpty(url))
            {
                return;
            }

            if(!File.Exists(_settings._streamlinkExePath))
            {
                MessageBox.Show("Please provide path to Streamlink.exe");
                tabsControl.SelectTab(2);

                return;
            }

            if(_tasks.ContainsKey(url))
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

            listItem.SubItems.Add(cbOnlineCheck.Checked? "✓" : "");
            listItem.SubItems.Add("<unknown>");
            listItem.SubItems.Add(quality);
            listItem.SubItems.Add("");
            listItem.SubItems.Add("");
            listItem.SubItems.Add("");
            lvTasks.Items.Add(listItem);

            Task task = new Task(url, quality, cbOnlineCheck.Checked, _settings._streamlinkExePath, fileNameTemplate, onTaskStatusChangedEvent, checkTastUrlOnline, getFinalFileNameFromTemplate, _settings._waitingTaskInterval);

            _tasks.Add(url, new TaskData()
            {
                _task = task,
                _listItem = listItem
            });

            task.Start();
            queueOnlineStatusCheck(url, OnlineCheckPriority.High);
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

        private void checkIdName()
        {
            if(cbSameNameAsId.Checked)
            {
                Regex regex = new Regex("\\.com\\/(?<string>(.*))\\/");
                string id = regex.Match(cbId.Text).Groups["string"].ToString();

                if(!String.IsNullOrEmpty(id))
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

                    if(status.FileSize > 0)
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
            addToFavorites(cbId.Text.Trim(), OnlineCheckPriority.High);
        }

        private void addToFavorites(string url, OnlineCheckPriority priority = OnlineCheckPriority.Low)
        {
            if (String.IsNullOrEmpty(url) || hasFavorite(url))
            {
                return;
            }

            ListViewItem item = new ListViewItem(url);

            _favoritesMap.Add(url, item);
            item.SubItems.Add("checking...");
            lvFavorites.Items.Add(item);

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

            var item = _favoritesMap[url];

            lvFavorites.BeginUpdate();
            item.BackColor = SystemColors.Window;

            switch (onlineStatus)
            {
                case OnlineStatus.Public:
                {
                    item.SubItems[1].Text = "Public";
                    item.BackColor = Color.Lime;

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

        private void updateFavoritesStatus()
        {
            lock (_onlineCheckQueueLock)
            {
                for (int i = 0; i < lvFavorites.Items.Count; i++)
                {
                    queueOnlineStatusCheck(lvFavorites.Items[i].Text, OnlineCheckPriority.Low);
                }
            }
        }

        private void clearFavoritesStatus()
        {
            lvFavorites.BeginUpdate();

            for (int i = 0; i < lvFavorites.Items.Count; i++)
            {
                var item = lvFavorites.Items[i];

                item.SubItems[1].Text = "checking...";
                item.BackColor = SystemColors.Window;
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

            for (; _onlineCheckIsRunning; )
            {
                for(; _onlineCheckIsRunning; )
                {
                    if (getUrlFromQueue(_onlineCheckHighPriorityQueue, out url))
                    {
                        dispatchOnlineCheckResult(url, requestUrlOnlineStatus(url));
                        System.Threading.Thread.Sleep(_httpRequestDelay);

                        continue;
                    }

                    if (getUrlFromQueue(_onlineCheckLowPriorityQueue, out url))
                    {
                        dispatchOnlineCheckResult(url, requestUrlOnlineStatus(url));
                        System.Threading.Thread.Sleep(_httpRequestDelay);

                        continue;
                    }

                    break;
                }

                lock(_onlineCheckQueueLock)
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

        private void dispatchOnlineCheckResult(string url, OnlineStatus status)
        {
            _dispatcher.InvokeAsync(() =>
            {
                onOnlineCheckResult(url, status);
            });
        }

        private void onOnlineCheckResult(string url, OnlineStatus status)
        {
            if(_favoritesMap.ContainsKey(url))
            {
                setFavoriteStatus(url, status);
            }

            if(_tasks.ContainsKey(url))
            {
                updateTask(url, status);
            }
        }

        //todo Rename task ->task
        private void updateTask(string url, OnlineStatus status)
        {
            if(!_tasks.ContainsKey(url))
            {
                return;
            }

            if(status == OnlineStatus.Public)
            {
                _tasks[url]._task.Start();
            }
        }

        private static OnlineStatus requestUrlOnlineStatus(string url)
        {
            OnlineStatus result = OnlineStatus.Unknown;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
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
            catch(Exception)
            {
                result = OnlineStatus.Error;
            }

            return result;
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
            if(lvFavorites.SelectedItems.Count == 0)
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
                    if(!Utils.runCmd(_settings._browserPath + " " + url))
                    {
                        MessageBox.Show("Failed to open browser");
                    }
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show("Failed to open browser: " + exception.Message);
            }
        }

        private void cmFavorites_Opening(object sender, CancelEventArgs e)
        {
            bool isItemClicked = lvFavorites.SelectedItems.Count > 0;

            startRecordToolStripMenuItem.Enabled = isItemClicked;
            openInBrowserToolStripMenuItem.Enabled = isItemClicked;
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
        }

        private void deleteFavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            string url = lvFavorites.SelectedItems[0].Text;

            if (_favoritesMap.ContainsKey(url))
            {
                lvFavorites.Items.Remove(_favoritesMap[url]);
                _favoritesMap.Remove(url);
            }
        }

        private void updateThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFavorites.SelectedItems.Count == 0)
            {
                return;
            }

            string url = lvFavorites.SelectedItems[0].Text;

            if (_favoritesMap.ContainsKey(url))
            {
                lvFavorites.SelectedItems[0].SubItems[1].Text = "checking...";
                queueOnlineStatusCheck(url, OnlineCheckPriority.High);
            }
        }

        private string getDurationString(DateTime start)
        {
            return DateTime.Now.Subtract(start).ToString(@"hh\:mm\:ss");
        }

        private void viewStreamLinkOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            var url = lvTasks.SelectedItems[0].Text;

            if(_tasks.ContainsKey(url))
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
            catch(Exception exception)
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

            cbId.Text = lvTasks.SelectedItems[0].Text;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvTasks.SelectedItems.Count == 0)
            {
                return;
            }

            var url = lvTasks.SelectedItems[0].Text;

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

            if(String.IsNullOrEmpty(filename))
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

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            lvFavorites.Focus();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
    }
}
