using System.Collections.Generic;

namespace AutomatedCDripLogChecker.Core
{
    /// <summary>
    /// Provides methods to check CD ripper log
    /// </summary>
    public interface ILogChecker
    {
        /// <summary>
        /// Log check result
        /// </summary>
        IList<CheckResult> CheckResults { get; }
        /// <summary>
        /// Psrsed CD ripper log
        /// </summary>
        RipperLog Log { get; }
        /// <summary>
        /// Score of log
        /// </summary>
        int Score { get; }

        /// <summary>
        /// Check a CD ripper log
        /// </summary>
        /// <param name="log">CD ripper log</param>
        void Check(string log);

        /// <summary>
        /// Check a Parsed CD ripper log
        /// </summary>
        /// <param name="log">Parsed CD ripper log</param>
        void Check(RipperLog log);

        /// <summary>
        /// Check just Parsed CD ripper log
        /// </summary>
        void Check();

        /// <summary>
        /// Parse log to RipperLog
        /// </summary>
        /// <param name="log">CD ripper log</param>
        void Parse(string log);

        /// <summary>
        /// Parse and Check log
        /// </summary>
        /// <param name="log">CD ripper log</param>
        void ParseAndCheck(string log);
    }
}