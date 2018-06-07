using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedCDripLogChecker.Core
{
    public class EACLog
    {
        public string EACVision { get; set; }
        public string CopyDate { get; set; }
        public string TrackName { get; set; }
        public string UsedDrive { get; set; }
        public string Adapter { get; set; }
        public string ID { get; set; }
        public string ReadMode { get; set; }
        public string NormalizeTo { get; set; }
        public string UtilizeAccurateStream { get; set; }
        public string DefeatAudioCache { get; set; }
        public string MakeUseofC2Pointers { get; set; }
        public string ReadOffsetCorrection { get; set; }
        public string OverreadIntoLeadInandLeadOut { get; set; }
        public string FillUpMissingOffsetSamplesWithSilence { get; set; }
        public string DeleteLeadingTrailingSilentBlocks { get; set; }
        public string NullSamplesUsedinCRCCalculations { get; set; }
        public string UsedInterface { get; set; }
        public string GapHandling { get; set; }
        public string OutputFormat { get; set; }
        public string SelectedBitrate { get; set; }
        public string Quality { get; set; }
        public string AddID3Tag { get; set; }
        public string CommandLineCompressor { get; set; }
        public string AdditionalCommandLineOptions { get; set; }
        public string SimpleFormat { get; set; }
        public List<TOCItem> TOC { get; set; }
        public bool IsRange { get; set; } = false;
        public List<TrackItem> TrackList { get; set; }
        public bool IsAllAccuratelyRipped { get; set; } = false;
        public bool UseCompressionOffset { get; set; } = false;
        public string LogCheckSum { get; set; }

        public EACLog()
        {
            TOC = new List<TOCItem>();
            TrackList = new List<TrackItem>();
        }
    }
}
