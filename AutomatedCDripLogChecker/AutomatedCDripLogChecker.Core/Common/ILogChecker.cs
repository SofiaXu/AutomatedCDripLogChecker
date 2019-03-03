using System.Collections.Generic;

namespace AutomatedCDripLogChecker.Core
{
    internal interface ILogChecker
    {
        int Score { get; }
        RipperLog Log { get; }
        IList<CheckResult> CheckResults { get; set; }

        void ParseAndCheck(string log);
    }
}