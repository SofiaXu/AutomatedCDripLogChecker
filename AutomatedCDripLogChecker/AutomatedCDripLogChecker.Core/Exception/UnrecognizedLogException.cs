using System;

namespace AutomatedCDripLogChecker.Core
{
    [Serializable]
    public class UnrecognizedLogException : Exception
    {
        public UnrecognizedLogException(string fileName) : base($"Log is unrecognizable. Log Name : {fileName}")
        {
            Data.Add("FileName", fileName);
        }

        public UnrecognizedLogException(string fileName, string Message) : base($"Log is unrecognizable. Log Name : {fileName} More Info:\r\n{Message}")
        {
            Data.Add("MoreInfo", Message);
            Data.Add("FileName", fileName);
        }
    }
}