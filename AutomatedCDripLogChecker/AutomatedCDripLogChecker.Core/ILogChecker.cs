using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutomatedCDripLogChecker.Core
{
    public interface ILogChecker
    {
        Dictionary<string, int> MasterRule { get; set; }
        Dictionary<string, int> TrackRule { get; set; }

        int GetScore(string log);
        int GetScore(ILog log);
        string GetCommment(string log);
        string GetCommment(ILog log);
        Tuple<string, int> GetResult(string log);
        Tuple<string, int> GetResult(ILog log);
        ILog LogCovertFromStream(Stream stream);
        ILog LogCovertFromString(string log);
    }
}
