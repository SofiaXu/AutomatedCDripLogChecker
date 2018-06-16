using AutomatedCDripLogChecker.GUI.Models;
using AutomatedCDripLogChecker.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutomatedCDripLogChecker.GUI.SubWindows
{
    /// <summary>
    /// MoreInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MoreInfoWindow : Window
    {
        public MoreInfoWindow(LogModel logModel)
        {
            InitializeComponent();
            Title = logModel.LogName + " " + logModel.LogScore;
            TextRange textRange = new TextRange(LogFile.Document.ContentStart, LogFile.Document.ContentEnd)
            {
                Text = logModel.LogFile
            };
            var comment = "";
            foreach (var item in logModel.CommentList)
            {
                comment += item + "\r\n";
            }
            TextRange textRange2 = new TextRange(Comment.Document.ContentStart, Comment.Document.ContentEnd)
            {
                Text = comment
            };
        }
    }
}
