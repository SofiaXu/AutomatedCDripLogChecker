using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AutomatedCDripLogChecker.Core
{
    [XmlRoot]
    public class RuleItem
    {
        internal readonly object StartScore;

        public string Name { get; set; }
        public string NickName { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
    }
}
