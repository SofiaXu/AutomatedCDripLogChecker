using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomatedCDripLogChecker.Core.Checksum;
using System.Text;
using System.Text.RegularExpressions;
using AutomatedCDripLogChecker.Core;

namespace AutomatedCDripLogChecker.Test
{
    [TestClass]
    public class EACLogChecksumTest
    {
        [TestMethod]
        public void EnglishLogGetChecksumTest()
        {
            using (EACChecksum checksum = new EACChecksum())
            {
                var log = File.ReadAllText("TestFile\\EAC\\EnglishChecksum1.log", Encoding.Unicode);
                var result = checksum.GetChecksum(log);
                Assert.IsTrue(result == "333FF1B0BAE41283E2DED42CDB3E3528303A89601F58B79554FB0C2D24AC56A0", "GetChecksum fail!");
            }
        }
        [TestMethod]
        public void EnglishLogCheckChecksumTest()
        {
            using (EACChecksum checksum = new EACChecksum())
            {
                var log = File.ReadAllText("TestFile\\EAC\\EnglishChecksum1.log", Encoding.Unicode);
                Assert.IsTrue(checksum.CheckChecksum(log), "CheckChecksum fail!");
            }
        }
        [TestMethod]
        public void ChineseLogCheckChecksumTest()
        {
            using (EACChecksum checksum = new EACChecksum())
            {
                var log = File.ReadAllText("TestFile\\EAC\\ChineseNoChecksum1.log", Encoding.Unicode);
                Assert.ThrowsException<NoChecksumException>(() => checksum.CheckChecksum(log), "CheckChecksum fail!");
            }
        }
    }
}
