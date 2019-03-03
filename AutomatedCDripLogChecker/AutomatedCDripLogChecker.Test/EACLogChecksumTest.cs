using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomatedCDripLogChecker.Core.Checksum;
using System.Text;
using System.Text.RegularExpressions;

namespace AutomatedCDripLogChecker.Test
{
    [TestClass]
    public class EACLogChecksumTest
    {
        [TestMethod]
        public void EnglishLogTest()
        {
            using (EACChecksum checksum = new EACChecksum())
            {
                var log = File.ReadAllText("TestFile\\EAC\\EnglishLogTest1.log", Encoding.Unicode);
                var result = checksum.GetChecksum(log);
                Assert.IsTrue(result == "333FF1B0BAE41283E2DED42CDB3E3528303A89601F58B79554FB0C2D24AC56A0");
            }
        }
    }
}
