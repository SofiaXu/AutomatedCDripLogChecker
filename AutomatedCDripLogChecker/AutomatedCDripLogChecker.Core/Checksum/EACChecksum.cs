using System;
using System.Security.Cryptography;
using System.Text;

namespace AutomatedCDripLogChecker.Core.Checksum
{
    public class EACChecksum : IDisposable, IChecksum
    {
        private Rijndael rijndael = Rijndael.Create();
        private readonly string key = "9378716cf13e4265ae55338e940b376184da389e50647726b35f6f341ee3efd9";
        private readonly byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public EACChecksum()
        {
            rijndael.Key = StrToToHexByte(key);
            rijndael.Mode = CipherMode.CBC;
            rijndael.BlockSize = 256;
            rijndael.Padding = PaddingMode.None;
            rijndael.IV = iv;
        }

        public string GetChecksum(string logData)
        {
            if (logData == null || logData.Length <= 0)
                throw new ArgumentNullException();
            logData = logData.Split(new string[] { "====" }, StringSplitOptions.None)[0].Replace("\r", "").Replace("\n", "").Replace("\ufeff", "").Replace("\ufffe", "");
            for (int i = 0; i < logData.Length; i += 16)
            {
                string subList;
                try
                {
                    subList = logData.Substring(i, 16);
                }
                catch (ArgumentOutOfRangeException)
                {
                    subList = logData.Substring(i, logData.Length - i).PadRight(16, '\x00');
                }
                rijndael.IV = rijndael.CreateEncryptor().TransformFinalBlock(Encoding.Unicode.GetBytes(subList), 0, 32);
            }
            return ByteToHexStr(rijndael.IV).ToUpper();
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

        private bool disposedValue = false;

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

        public void Dispose()
        {
            Dispose(true);
        }

        public bool CheckChecksum(string data)
        {
            throw new NotImplementedException();
        }
    }
}