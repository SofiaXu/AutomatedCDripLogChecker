using AutomatedCDripLogChecker.Core;
using AutomatedCDripLogChecker.GUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using AutomatedCDripLogChecker.GUI.SubWindows;

namespace AutomatedCDripLogChecker.GUI.ViewModels
{
    public class MainWindowsViewModel : ObservableObjectBase
    {
        public CommandBase OpenLog { get; set; } = new CommandBase();
        public CommandBase OpenLogFolder { get; set; } = new CommandBase();
        public CommandBase MoreInfo { get; set; } = new CommandBase();
        public CommandBase About { get; set; } = new CommandBase();
        public int SelectedId { get; set; } = -1;
        public ObservableCollection<LogModel> LogModels { get; set; } = new ObservableCollection<LogModel>();


        private EACLogChecker eACLogChecker;

        public MainWindowsViewModel()
        {
            OpenLog.ExecuteCommand += new Action<object>(OpenLogCommandAsync);
            MoreInfo.ExecuteCommand += new Action<object>(MoreInfoCommand);
            List<string> driveDB = File.ReadAllLines(@"Resources\DriveDB.csv").ToList();
            List<string> rule = File.ReadAllLines(@"Resources\Rule." + CultureInfo.CurrentUICulture.Name + ".csv").ToList();
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
            Parallel.ForEach(driveDB, item => 
            {
            if (item.Length > 2)
            {
                    var items = item.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    tuples.Add(new Tuple<string, string>(items[0], items[1]));
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

        private async void OpenLogCommandAsync(object o)
        {
            string log = "";
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "选择记录文件",
                Filter = "记录文件|*.log",
                FileName = string.Empty,
                FilterIndex = 1,
                Multiselect = false,
                RestoreDirectory = true,
                DefaultExt = "log",
                AddExtension = true
            };
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                using (FileStream filestream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    StreamReader streamReader = new StreamReader(filestream, Encoding.ASCII);
                    log = await streamReader.ReadToEndAsync();
                }
                try
                {
                    EACLog eACLog = eACLogChecker.ConvertfromString(log);
                    Tuple<List<string>, int> tuple = eACLogChecker.GetScore(eACLog);
                    LogModels.Add(new LogModel() { LogName = openFileDialog.SafeFileName, Log = eACLog, LogScore = tuple.Item2, CommentList = tuple.Item1, LogFile = log });
                }
                catch (Exception e)
                {
                    log += e.Message + "/r/n";
                }
            }
        }

        private void OpenLogFolderCommand(object o)
        {

        }

        private void MoreInfoCommand(object o)
        {
            if (SelectedId >= 0)
            {
                MoreInfoWindow moreInfo = new MoreInfoWindow(LogModels[SelectedId]);
                moreInfo.ShowDialog();
            }
        }
    }
}
