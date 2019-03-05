using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedCDripLogChecker.Core
{
    /// <summary>
    /// The exception that is thrown when there is not a checksum in log.
    /// </summary>
    [Serializable]
    public class NoChecksumException : Exception
    {
        public NoChecksumException() : base("There isn't a Checksum in log.")
        {

        }
        public NoChecksumException(string fileName) : base($"There isn't a Checksum in log. Log file:{fileName}")
        {
            Data.Add("FileName", fileName);
        }
        public NoChecksumException(string fileName, string Message) : base($"There isn't a Checksum in log. Log file:{fileName} More Info:\r\n{Message}")
        {
            Data.Add("MoreInfo", Message);
            Data.Add("FileName", fileName);
        }
    }
}
