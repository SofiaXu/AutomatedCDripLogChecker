using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutomatedCDripLogChecker.Core
{
    public class EACTranslater
    {
        private EACLogLanguage _sourceLanguage;
        private EACLogLanguage _targetLanguage;

        public enum EACLogLanguage
        {
            /// <summary>
            /// Bulgarian
            /// </summary>
            BG,
            /// <summary>
            /// Czech
            /// </summary>
            CS,
            /// <summary>
            /// German
            /// </summary>
            DE,
            /// <summary>
            /// English
            /// </summary>
            EN,
            /// <summary>
            /// Spanish
            /// </summary>
            ES,
            /// <summary>
            /// French
            /// </summary>
            FR,
            /// <summary>
            /// Italian
            /// </summary>
            IT,
            /// <summary>
            /// Japanese
            /// </summary>
            JP,
            /// <summary>
            /// Japanese (EAC V0.99 and before)
            /// </summary>
            JP99,
            /// <summary>
            /// Dutch
            /// </summary>
            NL,
            /// <summary>
            /// Polish
            /// </summary>
            PL,
            /// <summary>
            /// Russian
            /// </summary>
            RU,
            /// <summary>
            /// Sami, Northern (Sweden)
            /// </summary>
            SE,
            /// <summary>
            /// Slovak
            /// </summary>
            SK,
            /// <summary>
            /// Serbian
            /// </summary>
            SR,
            /// <summary>
            /// Swedish
            /// </summary>
            SV,
            /// <summary>
            /// Chinese
            /// </summary>
            ZH

        }
        public IReadOnlyDictionary<EACLogLanguage, string> LanguageKeyWord { get; private set; }
        public EACLogLanguage SourceLanguage { get => _sourceLanguage; private set { _sourceLanguage = value; SourceLanguageWord = InitializationLanguageWord(value); } }
        public IReadOnlyDictionary<string, string> SourceLanguageWord { get; private set; }
        public EACLogLanguage TargetLanguage { get => _targetLanguage; private set { _targetLanguage = value; TargetLanguageWord = InitializationLanguageWord(value); } }
        public IReadOnlyDictionary<string, string> TargetLanguageWord { get; private set; }

        public EACTranslater()
        {
            InitializationLanguageKeyWord();
            SourceLanguage = EACLogLanguage.EN;
            TargetLanguage = EACLogLanguage.EN;
        }

        public string Translate(string log) => Translate(log, EACLogLanguage.EN);

        public string Translate(string log, EACLogLanguage target)
        {
            if (log == null || log.Length <= 0)
                throw new ArgumentNullException();
            if (!log.StartsWith("Exact Audio Copy"))
                throw new UnrecognizedLogException();
            TargetLanguage = target;
            SourceLanguage = DetectLanguage(log);
            if (SourceLanguage == TargetLanguage)
                return log;
            foreach (var item in SourceLanguageWord)
            {
                if (TargetLanguageWord.TryGetValue(item.Key, out string value))
                {
                    log = log.Replace(item.Value, value);
                }
            }  
            return log;
        }

        public EACLogLanguage DetectLanguage(string log)
        {
            if (log == null || log.Length <= 0)
                throw new ArgumentNullException();
            if (!log.StartsWith("Exact Audio Copy"))
                throw new UnrecognizedLogException();
            EACLogLanguage result = EACLogLanguage.EN;
            foreach (var item in LanguageKeyWord)
            {
                if (Regex.IsMatch(log, item.Value))
                {
                    result = item.Key;
                }
            }
            return result;
        }

        private void InitializationLanguageKeyWord()
        {
            LanguageKeyWord = new Dictionary<EACLogLanguage, string>()
            {
                { EACLogLanguage.EN, "EAC extraction logfile from " },
                { EACLogLanguage.RU, "Отчёт EAC об извлечении, выполненном " },
                { EACLogLanguage.BG,"Отчет на EAC за извличане, извършено на " },
                { EACLogLanguage.CS, "Protokol extrakce EAC z " },
                { EACLogLanguage.NL, "EAC uitlezen log bestand van " },
                { EACLogLanguage.DE, "EAC Auslese-Logdatei vom " },
                { EACLogLanguage.IT, "File di log EAC per l'estrazione del " },
                { EACLogLanguage.PL, "Sprawozdanie ze zgrywania programem EAC z " },
                { EACLogLanguage.ZH, "EAC 抓取日志文件从" },
                { EACLogLanguage.SR, "EAC-ov fajl dnevnika ekstrakcije iz " },
                { EACLogLanguage.SE, "EAC extraheringsloggfil frĺn " },
                { EACLogLanguage.SK, "EAC log súbor extrakcie z " },
                { EACLogLanguage.ES, "Archivo Log de extracciones desde " },
                { EACLogLanguage.SV, "EAC extraheringsloggfil från " },
                { EACLogLanguage.FR, "Journal d'extraction EAC depuis " },
                { EACLogLanguage.JP, "EAC 展開 ログファイル 日付： " },
                { EACLogLanguage.JP99, "EAC 取り込みログファイル 日付" }
            };
        }

        private IReadOnlyDictionary<string, string> InitializationLanguageWord(EACLogLanguage language)
        {
            Dictionary<string, string> languageWord = new Dictionary<string, string>();
            string languageFile = GetLanguageFileByName(language);
            if (languageFile == null)
                throw new NullReferenceException($"Not found such language file. Language Name: {language.ToString()}");
            foreach (var item in languageFile.Split(new string[] { "\r\n" }, StringSplitOptions.None))
            {
                var items = item.Split(new string[] { " => " }, StringSplitOptions.None);
                if (items != null && items.Length >= 2)
                {
                    languageWord.Add(items[0], items[1]);
                }
            }
            return languageWord;
        }

        private string GetLanguageFileByName(EACLogLanguage language)
        {
            string result = null;
            switch (language)
            {
                case EACLogLanguage.BG:
                    result = Lists.BG;
                    break;
                case EACLogLanguage.CS:
                    result = Lists.CS;
                    break;
                case EACLogLanguage.DE:
                    result = Lists.DE;
                    break;
                case EACLogLanguage.EN:
                    result = Lists.EN;
                    break;
                case EACLogLanguage.ES:
                    result = Lists.ES;
                    break;
                case EACLogLanguage.FR:
                    result = Lists.FR;
                    break;
                case EACLogLanguage.IT:
                    result = Lists.IT;
                    break;
                case EACLogLanguage.JP:
                    result = Lists.JP;
                    break;
                case EACLogLanguage.JP99:
                    result = Lists.JP99;
                    break;
                case EACLogLanguage.NL:
                    break;
                case EACLogLanguage.PL:
                    result = Lists.PL;
                    break;
                case EACLogLanguage.RU:
                    result = Lists.RU;
                    break;
                case EACLogLanguage.SE:
                    result = Lists.SE;
                    break;
                case EACLogLanguage.SK:
                    result = Lists.SK;
                    break;
                case EACLogLanguage.SR:
                    result = Lists.SR;
                    break;
                case EACLogLanguage.SV:
                    result = Lists.SV;
                    break;
                case EACLogLanguage.ZH:
                    result = Lists.ZH;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
