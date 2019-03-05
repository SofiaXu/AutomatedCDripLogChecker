using System;

namespace AutomatedCDripLogChecker.Core
{
    /// <summary>
    /// The exception that is thrown when the log cannot be Unrecognized as a CD ripper log.
    /// </summary>
    [Serializable]
    public class UnrecognizedLogException : Exception
    {
        public UnrecognizedLogException() : base($"Log is unrecognizable.")
        {

        }

        public UnrecognizedLogException(string fileName) : base($"Log is unrecognizable. Log Name: {fileName}")
        {
            Data.Add("FileName", fileName);
        }

        public UnrecognizedLogException(string fileName, string Message) : base($"Log is unrecognizable. Log Name: {fileName} More Info:\r\n{Message}")
        {
            Data.Add("MoreInfo", Message);
            Data.Add("FileName", fileName);
        }
    }
}