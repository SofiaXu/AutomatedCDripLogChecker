namespace AutomatedCDripLogChecker.Core.Checksum
{
    internal interface IChecksum
    {
        string GetChecksum(string data);

        bool CheckChecksum(string data);
    }
}