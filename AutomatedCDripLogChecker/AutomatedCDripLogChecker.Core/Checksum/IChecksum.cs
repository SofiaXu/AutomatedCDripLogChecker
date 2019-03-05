namespace AutomatedCDripLogChecker.Core.Checksum
{
    /// <summary>
    /// Provides methods to check CD ripper log checksum.
    /// </summary>
    public interface IChecksum
    {
        /// <summary>
        /// Min version of logs which have checksum.
        /// </summary>
        string MinVersion { get; }

        /// <summary>
        /// Check a CD ripper log Checksum.
        /// </summary>
        /// <param name="data">CD ripper log</param>
        /// <returns>Old checksum is correct or not.</returns>
        bool CheckChecksum(string data);

        /// <summary>
        /// Get a CD ripper log Checksum.
        /// </summary>
        /// <param name="data">CD ripper log</param>
        /// <returns>log checksum</returns>
        string GetChecksum(string data);
    }
}