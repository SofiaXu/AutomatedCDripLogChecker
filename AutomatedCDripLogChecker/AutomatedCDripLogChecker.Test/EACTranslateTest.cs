using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomatedCDripLogChecker.Core;
using System.IO;
using System.Text.RegularExpressions;

namespace AutomatedCDripLogChecker.Test
{
    /// <summary>
    /// EACTranslateTest 的摘要说明
    /// </summary>
    [TestClass]
    public class EACTranslateTest
    {
        [TestMethod]
        public void EnglishDetectTest()
        {
            EACTranslater Translater = new EACTranslater();
            var result = Translater.DetectLanguage(File.ReadAllText("TestFile\\EAC\\EnglishChecksum1.log", Encoding.Unicode));
            Assert.IsTrue(result == EACTranslater.EACLogLanguage.EN, "English Detect Test fail!");
        }
        [TestMethod]
        public void ChineseDetectTest()
        {
            EACTranslater Translater = new EACTranslater();
            var result = Translater.DetectLanguage(File.ReadAllText("TestFile\\EAC\\ChineseNoChecksum1.log"));
            Assert.IsTrue(result == EACTranslater.EACLogLanguage.ZH, "Chinese Detect Test fail!");
        }
        [TestMethod]
        public void ChineseTranslateToEnglishTest()
        {
            EACTranslater Translater = new EACTranslater();
            var result = Translater.Translate(File.ReadAllText("TestFile\\EAC\\ChineseNoChecksum1.log"));
            Assert.IsTrue(Regex.IsMatch(result, "End of status report"), "Chinese Translate Test fail!");
        }
        [TestMethod]
        public void EnglishTranslateToChineseTest()
        {
            EACTranslater Translater = new EACTranslater();
            var result = Translater.Translate(File.ReadAllText("TestFile\\EAC\\EnglishChecksum1.log"), EACTranslater.EACLogLanguage.ZH);
            Assert.IsTrue(Regex.IsMatch(result, "可靠Secure"), "English Translate To Chinese Test fail!");
        }
    }
}
