using System;
using AutomatedCDripLogChecker.Core;

namespace AutomatedCDripLogChecker.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            EACLog eACLog = new EACLog();
            EACLogChecker eACLogChecker = new EACLogChecker();
            eACLog = eACLogChecker.ConvertfromString(args[0]);
            Console.WriteLine(eACLog.EACVision);
            Console.WriteLine(eACLog.UsedInterface);
            Console.Read();
        }
    }
}
