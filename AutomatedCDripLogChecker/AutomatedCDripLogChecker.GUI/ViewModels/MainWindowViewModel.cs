using AutomatedCDripLogChecker.Core;
using AutomatedCDripLogChecker.GUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatedCDripLogChecker.GUI.SubWindows;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AutomatedCDripLogChecker.GUI.ViewModels
{
    public class MainWindowViewModel : ObservableObjectBase
    {
        public CommandBase OpenLog { get; set; } = new CommandBase();
        public CommandBase MoreInfo { get; set; } = new CommandBase();
        public CommandBase OpenFolder { get; set; } = new CommandBase();
        public CommandBase Export { get; set; } = new CommandBase();
        public CommandBase ClearCollection { get; set; } = new CommandBase();
        public int SelectedId { get; set; } = -1;
        public ObservableCollection<LogModel> LogModels { get; set; } = new ObservableCollection<LogModel>();
        public string State { get; private set; }
        public event EventHandler LogConvertComplished;

        private EACLogChecker eACLogChecker;
        private DirectoryInfo SelectedDir;

        public MainWindowViewModel()
        {
            OpenLog.ExecuteCommand += new Action<object>(OpenLogCommandAsync);
            MoreInfo.ExecuteCommand += new Action<object>(MoreInfoCommand);
            OpenFolder.ExecuteCommand += new Action<object>(OpenFolderCommand);
            Export.ExecuteCommand += new Action<object>(ExportCommand);
            ClearCollection.ExecuteCommand += new Action<object>(ClearCollectionCommand);
            LogConvertComplished += MainWindowViewModel_LogConvertComplished;
            List<string> driveDB = Properties.Resources.DriveDB.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> rule = Properties.Resources.Rule_en_us.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
            Parallel.ForEach(driveDB, item =>
            {
                if (item.Length > 2)
                {
                    var items = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length > 1)
                    {
                        tuples.Add(new Tuple<string, string>(items[0], items[1]));
                    }
                }
            });
            Dictionary<string, RuleItem> valuePairs = new Dictionary<string, RuleItem>();
            foreach (var item in rule)
            {
                if (item.Length > 2)
                {
                    var items = item.Split(',');
                    valuePairs.Add(items[0], new RuleItem() { Name = items[0], Score = int.Parse(items[1]), Comment = items[2] });
                }
            }
            eACLogChecker = new EACLogChecker(valuePairs, tuples);
        }

        private void ClearCollectionCommand(object obj)
        {
            LogModels.Clear();
        }

        private void ExportCommand(object obj)
        {
            if (LogModels.Count > 0)
            {
                var log = "Automated CDrip Log Checker " + Application.ProductVersion.ToString() + " Checked Log\r\n\r\n";
                log += "Check Date (UTC): " + DateTime.UtcNow.ToString() + "\r\n\r\n";
                foreach (var item in LogModels)
                {
                    log += item.LogName + " Score: " + item.LogScore + "\r\nComment(s):\r\n";
                    foreach (var comment in item.CommentList)
                    {
                        log += comment + "\r\n";
                    }
                    log += "Checked Hash: "+ item.GetHashCode() + "\r\n\r\n";
                }
                log += "Log Hash: " + log.GetHashCode();
                File.WriteAllText("Log_" + DateTime.UtcNow.ToString("yyyyMMddhhmmss") + ".txt", log, Encoding.Unicode);
                MessageBox.Show("Log Exported");
            }
        }

        private void MainWindowViewModel_LogConvertComplished(object sender, EventArgs e) => LogModels.Add((LogModel)sender);

        private async void OpenLogCommandAsync(object o)
        {
            string log = "";
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Log Files",
                Filter = "Log File|*.log",
                FileName = string.Empty,
                FilterIndex = 1,
                Multiselect = true,
                RestoreDirectory = true,
                DefaultExt = "log",
                AddExtension = true
            };
            openFileDialog.ShowDialog();
            for (int i = 0; i < openFileDialog.FileNames.Length; i++)
            {
                if (openFileDialog.FileNames[i] != "")
                {
                    using (FileStream filestream = new FileStream(openFileDialog.FileNames[i], FileMode.Open))
                    {
                        StreamReader streamReader = new StreamReader(filestream, Encoding.ASCII);
                        log = await streamReader.ReadToEndAsync();
                    }
                    LogConverter(openFileDialog.SafeFileNames[i], log, "single");
                }
            }
        }

        private void MoreInfoCommand(object o)
        {
            if (SelectedId >= 0)
            {
                MoreInfoWindow moreInfo = new MoreInfoWindow(LogModels[SelectedId]);
                moreInfo.ShowDialog();
            }
        }

        private void OpenFolderCommand(object o)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                Description = "Open Logs Folder",
                ShowNewFolderButton = false,
                RootFolder = Environment.SpecialFolder.MyComputer
            };
            folderBrowserDialog.ShowDialog();
            if (folderBrowserDialog.SelectedPath != string.Empty && folderBrowserDialog.SelectedPath != "")
            {
                State = "Searching...";
                SelectedDir = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                Search(SelectedDir);
                State = "Completed";
            }
        }

        private void LogConverter(string logname, string log, string mode)
        {
            try
            {
                EACLog eACLog = eACLogChecker.ConvertfromString(log);
                Tuple<List<string>, int> tuple = eACLogChecker.GetScore(eACLog);
                //LogModels.Add(new LogModel() { LogName = logname, Log = eACLog, LogScore = tuple.Item2, CommentList = tuple.Item1, LogFile = log });
                LogConvertComplished(new LogModel() { LogName = logname, Log = eACLog, LogScore = tuple.Item2, CommentList = tuple.Item1, LogFile = log }, null);
            }
            catch (Exception e)
            {
                if (mode == "single")
                {
                    System.Windows.MessageBox.Show(e.Message + "\r\n" + "Log Name:" + logname);
                }
            }
        }

        private void Search() => Search(SelectedDir);

        private void Search(DirectoryInfo dir)
        {
            try
            {
                DirectoryInfo[] direct = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();
                Parallel.ForEach(files, async item =>
                 {
                     try
                     {
                         if (item.Name.EndsWith(".log"))
                         {
                             using (FileStream filestream = new FileStream(item.DirectoryName + "\\" + item.Name, FileMode.Open))
                             {
                                 StreamReader streamReader = new StreamReader(filestream, Encoding.ASCII);
                                 string log = await streamReader.ReadToEndAsync();
                                 LogConverter(item.Name, log, "m");
                             }
                         }
                     }
                     catch
                     {

                     }
                 });
                foreach (DirectoryInfo item in direct)
                {
                    try
                    {
                        Search(item);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
    }
}
