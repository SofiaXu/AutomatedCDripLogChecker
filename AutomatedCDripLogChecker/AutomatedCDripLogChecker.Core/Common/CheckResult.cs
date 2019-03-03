namespace AutomatedCDripLogChecker.Core
{
    public class CheckResult
    {
        public string FailReason { get; set; }
        public int FailRowNumber { get; set; }
        public string FailData { get; set; }
    }
}