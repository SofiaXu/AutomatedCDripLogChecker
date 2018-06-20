using AutomatedCDripLogChecker.GUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCDripLogChecker.GUI.ViewModels
{
    public class SettingAndAboutWindowViewModel : ObservableObjectBase
    {
        public string SettingAndAbout { get; set; }
        public string SubOK { get; set; }
        public string Language { get; set; }
        public List<string> LanguageSelecting { get; set; } = new List<string>();
        public CommandBase OK { get; set; }
        public int SelectedID { get; set; }

        private string LanguageSetting;

        public SettingAndAboutWindowViewModel()
        {
            LanguageSetting = Properties.Settings.Default.UserCultureInfo;
            List<string> Lang;
            if (File.Exists(@"Resources\Lang." + LanguageSetting + ".csv"))
            {
                Lang = File.ReadAllLines(@"Resources\Lang." + LanguageSetting + ".csv").ToList();
            }
            else
            {
                Lang = File.ReadAllLines(@"Resources\Lang.en-us.csv").ToList();
            }
            SettingAndAbout = SettingAndAbout = Lang.Where(a => a.StartsWith("SettingAndAbout,")).ToList()[0].Split(',')[1];
            SubOK = SettingAndAbout = Lang.Where(a => a.StartsWith("SubOK,")).ToList()[0].Split(',')[1];
            Language = SettingAndAbout = Lang.Where(a => a.StartsWith("Language,")).ToList()[0].Split(',')[1];
            LanguageSelecting = File.ReadAllLines(@"Resources\LanguageList.csv").ToList();
            SelectedID = LanguageSelecting.IndexOf(LanguageSetting);
            OK = new CommandBase();
            OK.ExecuteCommand += new Action<object>(OKCommand);
        }

        private void OKCommand(object o)
        {
            Properties.Settings.Default.UserCultureInfo = LanguageSelecting[SelectedID];
            LanguageSetting = Properties.Settings.Default.UserCultureInfo;
            List<string> Lang;
            if (File.Exists(@"Resources\Lang." + LanguageSetting + ".csv"))
            {
                Lang = File.ReadAllLines(@"Resources\Lang." + LanguageSetting + ".csv").ToList();
            }
            else
            {
                Lang = File.ReadAllLines(@"Resources\Lang.en-us.csv").ToList();
            }
            SettingAndAbout = SettingAndAbout = Lang.Where(a => a.StartsWith("SettingAndAbout,")).ToList()[0].Split(',')[1];
            SubOK = SettingAndAbout = Lang.Where(a => a.StartsWith("SubOK,")).ToList()[0].Split(',')[1];
            Language = SettingAndAbout = Lang.Where(a => a.StartsWith("Language,")).ToList()[0].Split(',')[1];
        }
    }
}
