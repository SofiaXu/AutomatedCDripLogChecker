using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomatedCDripLogChecker.Core
{
    public class EACLogChecker
    {
        public Dictionary<string, RuleItem> MasterRule { get;private set; }
        public List<Tuple<string,string>> DriveDB { get; private set; }

        /// <summary>
        /// 新建一个带规则文件和驱动器列表的检查器
        /// </summary>
        /// <param name="masterRule">主要规则文件</param>
        /// <param name="driveDB">驱动器列表</param>
        public EACLogChecker(Dictionary<string, RuleItem> masterRule, List<Tuple<string, string>> driveDB)
        {
            MasterRule = masterRule ?? throw new ArgumentNullException(nameof(masterRule));
            DriveDB = driveDB ?? throw new ArgumentNullException(nameof(driveDB));
        }

        /// <summary>
        /// 将由UTF-16编码的EAC抓轨记录转换为EACLog类
        /// </summary>
        /// <param name="log">表示一个完整的由UTF-16编码的EAC抓轨记录</param>
        /// <returns></returns>
        public EACLog ConvertfromString(string log)
        {
            #region Pre.
            string[] splitPoint = new string[] { ": " };
            EACLog eACLog = new EACLog();
            int head = 0;
            int range = 0;
            int trackcount = 0;
            List<string> sublist;
            List<string> loglines = log.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            if (!loglines[0].StartsWith("Exact Audio Copy"))
            {
                throw new Exception("Error:000001");//Error:000001 表示该log不是EAC log
            }
            if (loglines.Exists((s) => s.Trim() == "Range status and errors" ? true : false))
            {
                eACLog.IsRange = true;
            }
            #endregion
            #region Handle header
            eACLog.EACVision = (loglines[0].Substring(17).Split(new string[] { "from " }, StringSplitOptions.None))[0];

            eACLog.CopyDate = (loglines[2].Split(new string[] { "from " }, StringSplitOptions.None))[1];

            eACLog.LogCheckSum = loglines.Where(a => a.StartsWith("==== Log checksum")).ToList()[0].Split(' ')[3];

            eACLog.TrackName = loglines[4];

            eACLog.UsedDrive = SearchStringStartsWithinList("Used drive", loglines)?.Split(splitPoint, StringSplitOptions.None)[1].Replace("Adapter", "");
            eACLog.Adapter = SearchStringStartsWithinList("Used drive", loglines)?.Split(splitPoint, StringSplitOptions.None)[2].Replace("ID", "");
            eACLog.ID = SearchStringStartsWithinList("Used drive", loglines)?.Split(splitPoint, StringSplitOptions.None)[3];

            eACLog.ReadMode = SearchStringStartsWithinList("Read mode", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.UtilizeAccurateStream = SearchStringStartsWithinList("Utilize accurate stream", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.DefeatAudioCache = SearchStringStartsWithinList("Defeat audio cache", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.MakeUseofC2Pointers = SearchStringStartsWithinList("Make use of C2 pointers", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];

            eACLog.ReadOffsetCorrection = SearchStringStartsWithinList("Read offset correction", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.OverreadIntoLeadInandLeadOut = SearchStringStartsWithinList("Overread into Lead-In and Lead-Out", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.FillUpMissingOffsetSamplesWithSilence = SearchStringStartsWithinList("Fill up missing offset samples with silence", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.DeleteLeadingTrailingSilentBlocks = SearchStringStartsWithinList("Delete leading and trailing silent blocks", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.NullSamplesUsedinCRCCalculations = SearchStringStartsWithinList("Null samples used in CRC calculations", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.UsedInterface = SearchStringStartsWithinList("Used interface", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.GapHandling = SearchStringStartsWithinList("Gap handling", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.NormalizeTo = SearchStringStartsWithinList("Normalize to", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            eACLog.UseCompressionOffset = loglines.Exists(a => a.Trim() == "Use compression offset");

            eACLog.OutputFormat = SearchStringStartsWithinList("Used output format", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            if (eACLog.OutputFormat == "User Defined Encoder")
            {
                eACLog.SelectedBitrate = SearchStringStartsWithinList("Selected bitrate", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
                eACLog.Quality = SearchStringStartsWithinList("Quality", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
                eACLog.AddID3Tag = SearchStringStartsWithinList("Add ID3 tag", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
                eACLog.CommandLineCompressor = SearchStringStartsWithinList("Command line compressor", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
                eACLog.AdditionalCommandLineOptions = SearchStringStartsWithinList("Additional command line options", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            }
            else if (eACLog.OutputFormat == "Internal WAV Routines")
            {
                eACLog.SimpleFormat = SearchStringStartsWithinList("Simple format", loglines)?.Split(splitPoint, StringSplitOptions.None)[1];
            }
            eACLog.IsAllAccuratelyRipped = loglines.Exists(a => a.Trim() == "All tracks accurately ripped");
            #endregion
            #region Handle TOC
            head = loglines.IndexOf("TOC of the extracted CD") + 4;
            range = 0;
            if (eACLog.IsRange)
            {
                range = loglines.IndexOf("Range status and errors") - 1 - head;
            }
            else
            {
                range = loglines.IndexOf("Track  1") - 1 - head;
            }
            sublist = loglines.GetRange(head, range);
            foreach (var item in sublist)
            {
                string[] subitem = item.Split('|');
                if (int.TryParse(subitem[0].Trim(), out int a))
                {
                    TOCItem tempTOC = new TOCItem()
                    {
                        TrackNumber = subitem[0].Trim(),
                        StartTime = subitem[1].Trim(),
                        Length = subitem[2].Trim(),
                        StartSector = subitem[3].Trim(),
                        EndSector = subitem[4].Trim()
                    };
                    eACLog.TOC.Add(tempTOC);
                }

            }
            #endregion
            #region Handle Track
            head = 0;
            range = 0;
            if (eACLog.IsRange)
            {
                head = loglines.IndexOf("Range status and errors");
                range = loglines.IndexOf("End of status report") - 1 - head;
            }
            else
            {
                head = loglines.IndexOf("Track  1");
                range = loglines.IndexOf("End of status report") - 1 - head;
            }
            sublist = loglines.GetRange(head, range);
            foreach (var item in sublist)
            {
                var itemt = item.Trim();
                if (item.StartsWith("Track ") || item.StartsWith("Selected range"))
                {
                    trackcount++;
                    eACLog.TrackList.Add(new TrackItem());
                    eACLog.TrackList[trackcount - 1].TrackNumber = trackcount;
                }
                else if (itemt.StartsWith("Pre-gap length"))
                {
                    eACLog.TrackList[trackcount - 1].PeakLevel = itemt.Substring(15);
                }
                else if (itemt.StartsWith("Filename"))
                {
                    eACLog.TrackList[trackcount - 1].Filename = itemt.Substring(9);
                }
                else if (itemt.StartsWith("Peak level"))
                {
                    eACLog.TrackList[trackcount - 1].PeakLevel = itemt.Substring(11);
                }
                else if (itemt.StartsWith("Extraction speed"))
                {
                    eACLog.TrackList[trackcount - 1].ExtractionSpeed = itemt.Substring(17);
                }
                else if (itemt.StartsWith("Track quality"))
                {
                    eACLog.TrackList[trackcount - 1].TrackQuality = itemt.Substring(14);
                }
                else if (itemt.StartsWith("Test CRC"))
                {
                    eACLog.TrackList[trackcount - 1].TestCRC = itemt.Substring(9);
                }
                else if (itemt.StartsWith("Copy CRC"))
                {
                    eACLog.TrackList[trackcount - 1].CopyCRC = itemt.Substring(9);
                }
                else if (itemt.StartsWith("Accurately ripped"))
                {
                    eACLog.TrackList[trackcount - 1].AccuratelyRipped = itemt;
                }
                else if (itemt.StartsWith("Copy "))
                {
                    eACLog.TrackList[trackcount - 1].CopyStatus = itemt.Substring(5);
                }
                else if (itemt.StartsWith("Suspicious position"))
                {
                    eACLog.TrackList[trackcount - 1].SuspiciousPosition.Add(itemt.Substring(20));
                }
                else if (itemt.StartsWith("Timing problem"))
                {
                    eACLog.TrackList[trackcount - 1].TimingProblem++;
                }
                else if (itemt.StartsWith("Missing sample"))
                {
                    eACLog.TrackList[trackcount - 1].MissingSample++;
                }
            }
            #endregion
            return eACLog;
        }
        /// <summary>
        /// 对一个EAC抓轨记录类进行评分
        /// </summary>
        /// <param name="log">表示一个EAC抓轨记录类</param>
        /// <returns></returns>
        public Tuple<List<string>, int> GetScore(EACLog log)
        {
            #region Pre.
            int dealscore = 0;
            List<string> commentlist = new List<string>();
            bool is095 = (new Regex("V0.95")).IsMatch(log.EACVision);
            #endregion
            #region Check Get
            if (!log.IsRange)
            {
                #region Check Tracks
                TrackItem trackItem;
                for (int i = 0; i < log.TrackList.Count; i++)
                {
                    trackItem = log.TrackList[i];
                    if (trackItem.PreGapLength == null && i == 0 && is095)
                    {
                        dealscore += MasterRule["PreGapLength"].Score;
                        commentlist.Add(TrackCommentBuild("PreGapLength", i+1));
                    }
                    if (trackItem.SuspiciousPositionCount > 0)
                    {
                        dealscore += MasterRule["SuspiciousPosition"].Score;
                        commentlist.Add(TrackCommentBuild("SuspiciousPosition", i+1));
                    }
                    if (trackItem.MissingSample > 0)
                    {
                        dealscore += MasterRule["MissingSample"].Score;
                        commentlist.Add(TrackCommentBuild("MissingSample", i+1));
                    }
                    if (trackItem.TimingProblem > 0)
                    {
                        dealscore += MasterRule["TimingProblem"].Score;
                        commentlist.Add(TrackCommentBuild("TimingProblem", i+1));
                    }
                    if (trackItem.CopyStatus != "OK")
                    {
                        dealscore += MasterRule["CopyStatus"].Score;
                        commentlist.Add(TrackCommentBuild("CopyStatus", i+1));
                    }
                    if (trackItem.TestCRC != null && trackItem.TestCRC != trackItem.CopyCRC)
                    {
                        if (log.ReadMode != "Secure")
                        {
                            dealscore += MasterRule["CRCUncertainAndNotSecure"].Score;
                            commentlist.Add(TrackCommentBuild("CRCUncertainAndNotSecure", i+1));
                        }
                        else
                        {
                            dealscore += MasterRule["CRCUncertain"].Score;
                            commentlist.Add(TrackCommentBuild("CRCUncertain", i+1));
                        }
                    }
                    if (trackItem.AccuratelyRipped == null)
                    {
                        if (!log.IsAllAccuratelyRipped)
                        {
                            dealscore += MasterRule["NotAccuratelyRipped"].Score;
                            commentlist.Add(TrackCommentBuild("NotAccuratelyRipped", i+1));
                        }

                    }
                    else
                    {
                        if (is095)
                        {
                            dealscore += MasterRule["Fake"].Score;
                            commentlist.Add(TrackCommentBuild("Fake", i+1));
                        }
                    }
                    if (trackItem.TrackQuality == null && log.ReadMode == "Secure" && !is095)
                    {
                        dealscore += MasterRule["Fake"].Score;
                        commentlist.Add(TrackCommentBuild("Fake", i+1));
                    }
                }
                #endregion
            }
            else
            {
                dealscore += MasterRule["IsRange"].Score;
                commentlist.Add(MasterRule["IsRange"].Comment);
            }
            #endregion
            #region Check Header
            if (!log.IsAllAccuratelyRipped)
            {
                dealscore += MasterRule["IsNotAllAccuratelyRipped"].Score;
                commentlist.Add(MasterRule["IsNotAllAccuratelyRipped"].Comment);
            }
            if (log.NormalizeTo != null)
            {
                dealscore += MasterRule["NormalizeToOn"].Score;
                commentlist.Add(MasterRule["NormalizeToOn"].Comment);
            }
            if (log.UseCompressionOffset)
            {
                dealscore += MasterRule["UseCompressionOffset"].Score;
                commentlist.Add(MasterRule["UseCompressionOffset"].Comment);
            }
            if (log.MakeUseofC2Pointers == "Yes")
            {
                dealscore += MasterRule["UsedC2"].Score;
                commentlist.Add(MasterRule["UsedC2"].Comment);
            }
            if (log.FillUpMissingOffsetSamplesWithSilence != "Yes")
            {
                dealscore += MasterRule["NotFillUpMissingOffsetSamplesWithSilence"].Score;
                commentlist.Add(MasterRule["NotFillUpMissingOffsetSamplesWithSilence"].Comment);
                if (log.NullSamplesUsedinCRCCalculations != "Yes")
                {
                    dealscore += MasterRule["NotNullSamplesUsedinCRCCalculations"].Score;
                    commentlist.Add(MasterRule["NotNullSamplesUsedinCRCCalculations"].Comment);
                }
            }
            if (log.DeleteLeadingTrailingSilentBlocks == "Yes")
            {
                dealscore += MasterRule["DeleteLeadingTrailingSilentBlocks"].Score;
                commentlist.Add(MasterRule["DeleteLeadingTrailingSilentBlocks"].Comment);
            }
            if (log.GapHandling != "Appended to previous track" && !is095)
            {
                dealscore += MasterRule["NotGapHandle"].Score;
                commentlist.Add(MasterRule["NotGapHandle"].Comment);
            }
            if (log.AddID3Tag == "Yes")
            {
                dealscore += MasterRule["AddID3Tag"].Score;
                commentlist.Add(MasterRule["AddID3Tag"].Comment);
            }
            if (log.ReadMode != "Secure")
            {
                dealscore += MasterRule["InSecure"].Score;
                commentlist.Add(MasterRule["InSecure"].Comment);
            }
            if (log.DefeatAudioCache != "Yes" && log.ReadMode != "Burst")
            {
                dealscore += MasterRule["NoDefeatAudioCache"].Score;
                commentlist.Add(MasterRule["NoDefeatAudioCache"].Comment);
            }
            if ((new Regex("Generic DVD-ROM SCSI CdRom Device")).IsMatch(log.UsedDrive))
            {
                dealscore += MasterRule["VirtualDriveUsed"].Score;
                commentlist.Add(MasterRule["VirtualDriveUsed"].Comment);
            }
            else
            {
                var drivename = ReplaceString(@"/[^0-9a-z]/i", ReplaceString(@"\s+", ReplaceString(@"\s+-\s", log.UsedDrive)).Trim()).Split(' ');
                var drive = drivename[drivename.Length - 1];
                var q = from item in DriveDB
                        where item?.Item1.IndexOf(drive) >= 0
                        select item.Item2;
                var list = q.ToList();
                if (list.Count > 0 && list.Count <= 30 && list != null)
                {
                    if (!list.Exists(a => a.Replace(" ", "") == log.ReadOffsetCorrection))
                    {
                        dealscore += MasterRule["OffsetNotRight"].Score;
                        commentlist.Add(MasterRule["OffsetNotRight"].Comment);
                    }
                }
                else if (log.ReadOffsetCorrection == "0")
                {
                    dealscore += MasterRule["OffsetNotFound"].Score;
                    commentlist.Add(MasterRule["OffsetNotFound"].Comment);
                }
                else if (log.ReadOffsetCorrection != "0")
                {
                    dealscore += MasterRule["OffsetNotInDB"].Score;
                    commentlist.Add(MasterRule["OffsetNotInDB"].Comment);
                }
            }
            #endregion
            return new Tuple<List<string>, int>(commentlist, 100 - dealscore);
        }

        private string SearchStringStartsWithinList(string name, List<string> list)
        {
            if (list.Exists(a => a.StartsWith(name)))
            {
                return list.Find(a => a.StartsWith(name));
            }
            else
            {
                return null;
            }
        }
        private string TrackCommentBuild(string name, int i)
        {
            return String.Format(MasterRule[name].Comment, i.ToString(), MasterRule[name].Score);
        }
        private string ReplaceString(string pattern, string s) => Regex.Replace(s, pattern, " ", RegexOptions.None);
    }
}
