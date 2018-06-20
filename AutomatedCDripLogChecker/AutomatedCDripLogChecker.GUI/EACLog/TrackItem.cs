using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedCDripLogChecker.Core
{
    public class TrackItem
    {
        public string Filename { get; set; }
        public string PeakLevel { get; set; }
        public string ExtractionSpeed { get; set; }
        public string TrackQuality { get; set; }
        public string TestCRC { get; set; }
        public string CopyCRC { get; set; }
        public string AccuratelyRipped { get; set; }
        public string CopyStatus { get; set; }
        public string PreGapLength { get; set; }
        public List<string> SuspiciousPosition { get; set; }
        public int SuspiciousPositionCount => SuspiciousPosition.Count;
        public int TimingProblem { get; set; } = 0;
        public int MissingSample { get; set; } = 0;
        public int TrackNumber { get; set; } = 1;

        public TrackItem()
        {
            SuspiciousPosition = new List<string>();
        }
    }
}
