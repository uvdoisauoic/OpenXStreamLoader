using System;
using System.Diagnostics;
using System.Timers;
using System.IO;

namespace OpenXStreamLoader
{
    class Task : IDisposable
    {
        public enum TaskState
        {
            Unknown,
            Waiting,
            InProgress,
            Finished,
            StartProcessError
        };

        public interface IStatusView
        {
            TaskState State { get; }
            string FileName { get; }
            long FileSize { get; }
            DateTime Created { get; }
            string ConsoleOutput { get; }
        };

        internal class Status : IStatusView
        {
            public TaskState State { get; set; } = TaskState.Unknown;
            public string FileName { get; set; }
            public long FileSize { get; set; } = 0;
            public DateTime Created { get; set; }
            public string ConsoleOutput { get; set; }
        }

        public delegate void StatusChangedCallback(string url, IStatusView status);
        public delegate void CheckOnlineCallback(string url);
        public delegate string GetFinalFileNameFromTemplate(string fileNameTemplate);

        private string _url;
        private string _quality;
        private bool _performOnlineCheck;
        private string _executablePath;
        private string _outputFileNameTemplate;
        private string _fileName;
        private Process _process;
        private System.Timers.Timer _onlineCheckTimer;
        private System.Timers.Timer _statusCheckTimer;
        private StatusChangedCallback _statusChanged;
        CheckOnlineCallback _checkOnline;
        private int _waitingTaskInterval;
        GetFinalFileNameFromTemplate _getFinalFileNameFromTemplate;
        private Status _status;
        private bool _processExiting = false;

        public Task(string url, string quality, bool performOnlineCheck, string executablePath, string outputFileNameTemplate, StatusChangedCallback statusChanged, CheckOnlineCallback checkOnline, GetFinalFileNameFromTemplate getFinalFileNameFromTemplate, int waitingTaskInterval)
        {
            _url = url;
            _quality = quality;
            _performOnlineCheck = performOnlineCheck;
            _executablePath = executablePath;
            _outputFileNameTemplate = outputFileNameTemplate;
            _statusChanged = statusChanged;
            _checkOnline = checkOnline;
            _getFinalFileNameFromTemplate = getFinalFileNameFromTemplate;
            _waitingTaskInterval = waitingTaskInterval;

            _status = new Status();

            _onlineCheckTimer = new System.Timers.Timer();
            _onlineCheckTimer.Enabled = false;
            _onlineCheckTimer.Interval = _waitingTaskInterval * 1000; // ms
            _onlineCheckTimer.Elapsed += new ElapsedEventHandler(_onOnlineCheckTimer);

            _statusCheckTimer = new System.Timers.Timer();
            _statusCheckTimer.Enabled = false;
            _statusCheckTimer.Interval = 3 * 1000; // ms
            _statusCheckTimer.Elapsed += new ElapsedEventHandler(_onStatusCheckTimer);
        }

        ~Task()
        {
            Dispose();
        }

        public void Dispose()
        {
            _onlineCheckTimer.Enabled = false;
            _statusCheckTimer.Enabled = false;
            stopProcess();
        }

        public void Start()
        {
            try
            {
                if (!_process.HasExited)
                {
                    return;
                }
            }
            catch (Exception)
            {

            }
            
            startProcess();
        }

        private void startProcess()
        {
            _process = new Process();
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.FileName = _executablePath;
            _process.StartInfo.CreateNoWindow = true;
            _process.EnableRaisingEvents = true;
            _process.StartInfo.WorkingDirectory = "";
            _process.OutputDataReceived += new DataReceivedEventHandler(onOutputDataReceived);
            _process.Exited += new EventHandler(onProcessExited);
            _fileName = _getFinalFileNameFromTemplate(_outputFileNameTemplate);
            _status.FileName = _fileName;
            _status.Created = DateTime.Now;
            _status.ConsoleOutput = "";
            _process.StartInfo.Arguments = " --hds-segment-threads 10 --hls-segment-threads 10 -o \"" + _fileName + "\" -f " + _url + " " + _quality;

            try
            {
                _status.State = TaskState.InProgress;
                _process.Start();
                _process.BeginOutputReadLine();
                _onlineCheckTimer.Enabled = false;
                _statusCheckTimer.Enabled = true;
            }
            catch (Exception exception)
            {
                _status.State = TaskState.StartProcessError;
                _status.ConsoleOutput += exception.Message + "\n";
            }

            reportStatus();
        }

        private void stopProcess()
        {
            _processExiting = true;

            try
            {
                if (_process.HasExited)
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                var id = _process.Id;

                if (id == 0)
                {
                    return;
                }

                //https://stackoverflow.com/questions/2055753/how-to-gracefully-terminate-a-process
                //https://github.com/gapotchenko/Gapotchenko.FX/blob/1accd5c03a310a925939ee55a9bd3055dadb4baa/Source/Gapotchenko.FX.Diagnostics.Process/ProcessExtensions.End.cs#L247-L328

                if (!Native.AttachConsole(id))
                {
                    return;
                }

                if (!Native.SetConsoleCtrlHandler(null, true))
                {
                    return;
                }

                Native.GenerateConsoleCtrlEvent(Native.CTRL_C_EVENT, 0);
            }
            finally
            {
                Native.FreeConsole();
                Native.SetConsoleCtrlHandler(null, false);
            }

            _process = null;
        }

        private void onProcessExited(object sender, System.EventArgs e)
        {
            if(_processExiting)
            {
                return;
            }

            _statusCheckTimer.Enabled = false;

            if(playableStreamFound() && !streamSupportsQuality())
            {
                _status.State = TaskState.StartProcessError;
            }
            else if (_performOnlineCheck)
            {
                _onlineCheckTimer.Enabled = true;
                _status.State = TaskState.Waiting;
            }
            else
            {
                _status.State = TaskState.Finished;
            }

            reportStatus();
        }

        private void _onOnlineCheckTimer(object source, ElapsedEventArgs e)
        {
            _checkOnline(_url);
        }

        private void _onStatusCheckTimer(object source, ElapsedEventArgs e)
        {
            _status.FileSize = getFileSize();
            reportStatus();
        }

        private void reportStatus()
        {
            _statusChanged(_url, _status);
        }

        private void onOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //todo lock
            _status.ConsoleOutput += e.Data + "\n";
        }

        private long getFileSize()
        {
            try
            {
                return new FileInfo(_fileName).Length;
            }
            catch(Exception)
            {
                return -1;
            }
        }

        private bool playableStreamFound()
        {
            return !_status.ConsoleOutput.Contains("No playable streams found");
        }

        private bool streamSupportsQuality()
        {
            return !_status.ConsoleOutput.Contains("The specified stream(s) '" + _quality + "' could not be found.");
        }
    }
}
