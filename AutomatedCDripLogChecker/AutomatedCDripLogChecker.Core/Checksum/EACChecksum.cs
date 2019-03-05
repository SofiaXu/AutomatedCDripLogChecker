using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AutomatedCDripLogChecker.Core.Checksum
{
    /// <summary>
    /// Computes and checks EAC log checksum.
    /// </summary>
    public class EACChecksum : IDisposable, IChecksum
    {
        /// <summary>
        /// Start IV
        /// </summary>
        private readonly byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// EAC Rijndael Key
        /// </summary>
        private readonly string key = "9378716cf13e4265ae55338e940b376184da389e50647726b35f6f341ee3efd9";
        private bool disposedValue = false;
        private Rijndael rijndael = Rijndael.Create();

        public EACChecksum()
        {
            rijndael.Key = StrToToHexByte(key);
            rijndael.Mode = CipherMode.CBC;
            rijndael.BlockSize = 256;
            rijndael.Padding = PaddingMode.None;
            rijndael.IV = iv;
        }
        /// <summary>
        /// Min version of EAC logs which have checksum.
        /// </summary>
        public string MinVersion => "V1.0 beta 1";

        /// <summary>
        /// Check an EAC log Checksum.
        /// </summary>
        /// <param name="data">EAC log</param>
        /// <returns>Old checksum is correct or not.</returns>
        /// <exception cref="ArgumentNullException">Throw when length of log less than 1 or a null reference.</exception>
        /// <exception cref="UnrecognizedLogException">Throw when log is not an EAC log.</exception>
        /// <exception cref="NoChecksumException">Thrown when there is not a checksum in log.</exception>
        public bool CheckChecksum(string data)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException();
            if (!data.StartsWith("Exact Audio Copy"))
                throw new UnrecognizedLogException();
            var oldChecksum = Regex.Match(data, "==== Log checksum (?<sign>[0-9A-F]+) ====");
            if (!oldChecksum.Success)
                throw new NoChecksumException();
            else
                return oldChecksum.Groups[1].Value == GetChecksum(data);
        }

        /// <summary>
        /// Release all resource used by the current instance of the EACChecksum
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Get an EAC log Checksum.
        /// </summary>
        /// <param name="data">EAC log</param>
        /// <returns>log checksum</returns>
        /// <exception cref="ArgumentNullException">Throw when length of log less than 1 or a null reference.</exception>
        /// <exception cref="UnrecognizedLogException">Throw when log is not an EAC log.</exception>
        public string GetChecksum(string data)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException();
            if (!data.StartsWith("Exact Audio Copy"))
                throw new UnrecognizedLogException();
            data = data.Split(new string[] { "====" }, StringSplitOptions.None)[0].Replace("\r", "").Replace("\n", "").Replace("\ufeff", "").Replace("\ufffe", "");
            for (int i = 0; i < data.Length; i += 16)
            {
                string subList;
                try
                {
                    subList = data.Substring(i, 16);
                }
                catch (ArgumentOutOfRangeException)
                {
                    subList = data.Substring(i, data.Length - i).PadRight(16, '\x00');
                }
                rijndael.IV = rijndael.CreateEncryptor().TransformFinalBlock(Encoding.Unicode.GetBytes(subList), 0, 32);
            }
            return ByteToHexStr(rijndael.IV).ToUpper();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    rijndael.Dispose();
                }
                disposedValue = true;
            }
        }

        private string ByteToHexStr(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
                throw new ArgumentNullException();
            string returnStr = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                returnStr += bytes[i].ToString("X2");
            }
            return returnStr;
        }

        private void ResetIV()
        {
            rijndael.IV = iv;
        }

        private byte[] StrToToHexByte(string hexString)
        {
            if (hexString == null || hexString.Length <= 0)
                throw new ArgumentNullException();
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0) hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}