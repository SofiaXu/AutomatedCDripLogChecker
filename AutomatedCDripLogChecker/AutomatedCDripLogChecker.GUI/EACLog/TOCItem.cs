using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedCDripLogChecker.Core
{
    public class TOCItem
    {
        public string TrackNumber { get; set; }
        public string StartTime { get; set; }
        public string Length { get; set; }
        public string StartSector { get; set; }
        public string EndSector { get; set; }
    }
}
