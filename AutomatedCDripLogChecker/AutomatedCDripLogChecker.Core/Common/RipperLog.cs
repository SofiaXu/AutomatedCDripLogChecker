namespace AutomatedCDripLogChecker.Core
{
    public class RipperLog
    {
        public string RipperName { get; set; }

        public RipperLog(string ripperName)
        {
            RipperName = ripperName;
        }
    }
}