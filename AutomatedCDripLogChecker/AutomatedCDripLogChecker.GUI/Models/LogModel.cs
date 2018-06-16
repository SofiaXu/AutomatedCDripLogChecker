using AutomatedCDripLogChecker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCDripLogChecker.GUI.Models
{
    public class LogModel : ObservableObjectBase
    {
        public string LogName { get; set; }
        public int LogScore { get; set; }
        public string LogFile { get; set; }
        public EACLog Log { get; set; }
        public List<string> CommentList { get; set; }

    }
}
