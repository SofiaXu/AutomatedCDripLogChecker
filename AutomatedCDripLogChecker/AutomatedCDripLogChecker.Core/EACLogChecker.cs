using System;
using System.Collections.Generic;
using System.IO;

namespace AutomatedCDripLogChecker.Core
{
    public class EACLogChecker : ILogChecker
    {
        private int score = 0;
        private int MaxScore = 100;
        private string comment = "";

        public Dictionary<string, int> MasterRule { get; set; }
        public Dictionary<string, int> TrackRule { get; set; }

        public string GetCommment(string log)
        {
            return GetResult(log).Item1;
        }

        public string GetCommment(ILog log)
        {
            return GetResult(log).Item1;
        }

        public Tuple<string, int> GetResult(string log)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, int> GetResult(ILog log)
        {
            throw new NotImplementedException();
        }

        public int GetScore(string log)
        {
            return GetResult(log).Item2;
        }

        public int GetScore(ILog log)
        {
            return GetResult(log).Item2;
        }

        public ILog LogCovertFromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public ILog LogCovertFromString(string log)
        {
            throw new NotImplementedException();
        }
    }
}
