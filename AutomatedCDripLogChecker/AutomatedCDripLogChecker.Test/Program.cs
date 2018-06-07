using AutomatedCDripLogChecker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCDripLogChecker.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateTime = DateTime.Now;
            EACLog eACLog = new EACLog();
            EACLogChecker eACLogChecker = new EACLogChecker();
            eACLog = eACLogChecker.ConvertfromString(test);
            Console.WriteLine(DateTime.Now - dateTime);
            Console.WriteLine(eACLog.EACVision);
            Console.WriteLine(eACLog.UsedInterface);
            Console.WriteLine(eACLog.LogCheckSum);
            Console.WriteLine(string.Format("{0}","a","b"));
            Console.Read();
        }
        static string test = @"Exact Audio Copy V1.0 beta 3 from 29. August 2011

EAC extraction logfile from 15. January 2012, 16:23

Cristina Ortiz, Pascal Rogé, London Sinfonietta, Philharmonia Orchestra, Charles Dutoit / Carnaval Des Animaux, Danse Macabre, etc.

Used drive  : hp DVDRAM GT31L Adapter: 1  ID: 0

Read mode               : Secure
Utilize accurate stream : Yes
Defeat audio cache      : Yes
Make use of C2 pointers : No

Read offset correction                      : 103
Overread into Lead-In and Lead-Out          : No
Fill up missing offset samples with silence : Yes
Delete leading and trailing silent blocks   : No
Null samples used in CRC calculations       : Yes
Used interface                              : Native Win32 interface for Win NT & 2000
Gap handling                                : Appended to previous track

Used output format              : User Defined Encoder
Selected bitrate                : 768 kBit/s
Quality                         : High
Add ID3 tag                     : No
Command line compressor         : C:\Program Files(x86)\Exact Audio Copy\FLAC\FLAC.EXE
Additional command line options : -8 -V -T %source% -o %dest%


TOC of the extracted CD

     Track |   Start  |  Length  | Start sector | End sector
    ---------------------------------------------------------
        1  |  0:00.32 |  2:18.50 |        32    |    10431
        2  |  2:19.07 |  0:56.70 |     10432    |    14701
        3  |  3:16.02 |  0:30.33 |     14702    |    16984
        4  |  3:46.35 |  2:16.30 |     16985    |    27214
        5  |  6:02.65 |  1:30.52 |     27215    |    34016
        6  |  7:33.42 |  0:56.40 |     34017    |    38256
        7  |  8:30.07 |  2:31.13 |     38257    |    49594
        8  | 11:01.20 |  0:44.32 |     49595    |    52926
        9  | 11:45.52 |  2:33.05 |     52927    |    64406
        10 | 14:18.57 |  1:11.30 |     64407    |    69761
        11 | 15:30.12 |  1:20.43 |     69762    |    75804
        12 | 16:50.55 |  1:19.42 |     75805    |    81771
        13 | 18:10.22 |  2:45.23 |     81772    |    94169
        14 | 20:55.45 |  2:00.35 |     94170    |   103204
        15 | 22:56.05 |  9:05.17 |    103205    |   144096
        16 | 32:01.22 |  7:47.55 |    144097    |   179176
        17 | 39:49.02 |  7:02.28 |    179177    |   210854


Range status and errors

Selected range

     Filename C:\Users\sl201\Desktop\[180523] TVアニメ「宇宙戦艦ティラミス」主題歌 「Breakthrough／DURANDAL」／スバル·イチノセ(CV：石川界人)\COCC-17455.wav

     Peak level 98.8 %
     Extraction speed 1.4 X
     Range quality 100.0 %
     Copy CRC D85BFBB1
     Copy OK

No errors occurred

End of status report


==== Log checksum 763F2940331FFA47700C10A3703F9BB3EB07ED2BCE6F0C66C4A18BAA4D4886E8 ====";
    }
}
