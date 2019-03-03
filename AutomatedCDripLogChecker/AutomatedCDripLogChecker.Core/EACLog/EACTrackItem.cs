using System.Collections.Generic;

namespace AutomatedCDripLogChecker.Core
{
    public class EACTrackItem
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
        public List<string> SuspiciousPosition { get; }
        public int SuspiciousPositionCount => SuspiciousPosition.Count;
        public int TimingProblem { get; set; } = 0;
        public int MissingSample { get; set; } = 0;
        public int TrackNumber { get; set; } = 1;

        public EACTrackItem()
        {
            SuspiciousPosition = new List<string>();
        }
    }
}