namespace AutomatedCDripLogChecker.Core
{
    public class RipperLog
    {
        public string RipperName { get;}
        public bool IsChecksumLegal { get; set; }
        public bool HasChecksum { get; set; }

        public RipperLog(string ripperName)
        {
            RipperName = ripperName;
        }
    }
}