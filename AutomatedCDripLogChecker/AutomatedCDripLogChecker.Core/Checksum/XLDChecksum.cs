using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AutomatedCDripLogChecker.Core.Checksum
{
    public class XLDChecksum : IDisposable, IChecksum
    {
        readonly string alphabat = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz._";
        readonly string key = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        private readonly XLDSHA256 sha256 = new XLDSHA256();
        private bool disposedValue = false;

        /// <summary>
        /// Min version of XLD logs which have checksum.
        /// </summary>
        public string MinVersion => "20121027";

        /// <summary>
        /// Check an XLD log Checksum.
        /// </summary>
        /// <param name="data">XLD log</param>
        /// <returns>Old checksum is correct or not.</returns>
        /// <exception cref="ArgumentNullException">Throw when length of log less than 1 or a null reference.</exception>
        /// <exception cref="UnrecognizedLogException">Throw when log is not an XLD log.</exception>
        /// <exception cref="NoChecksumException">Thrown when there is not a checksum in log.</exception
        public bool CheckChecksum(string data)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException();
            if (!data.StartsWith("X Lossless Decoder"))
                throw new UnrecognizedLogException();
            var oldChecksum = Regex.Match(data, @"\n-----BEGIN XLD SIGNATURE-----\n(?<sign>[0-9a-zA-Z\._]+)\n-----END XLD SIGNATURE-----\n");
            if (!oldChecksum.Success)
                throw new NoChecksumException();
            else
                return oldChecksum.Groups[1].Value == GetChecksum(data);
        }

        /// <summary>
        /// Release all resource used by the current instance of the XLDChecksum
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Get an XLD log Checksum.
        /// </summary>
        /// <param name="data">XLD log</param>
        /// <returns>log checksum</returns>
        /// <exception cref="ArgumentNullException">Throw when length of log less than 1 or a null reference.</exception>
        /// <exception cref="UnrecognizedLogException">Throw when log is not an XLD log.</exception>
        public string GetChecksum(string data)
        {
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException();
            if (!data.StartsWith("X Lossless Decoder"))
                throw new UnrecognizedLogException();
            data = data.Split(new string[] { "\n-----BEGIN XLD SIGNATURE-----\n" }, StringSplitOptions.None)[0];
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            var scrambled = Scramble(Encoding.ASCII.GetBytes(ByteToHexStr(hash).ToLower() + "\nVersion=0001").ToList());
            string checksum = "";
            var base64 = Convert.ToBase64String(scrambled).TrimEnd('=');
            foreach (var item in base64)
            {
                checksum += alphabat[key.IndexOf(item)];
            }
            return checksum;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sha256.Dispose();
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

        private uint RotateLeft(uint x, int n)
        {
            return ((x << n) | (x >> (0x20 - n)));
        }

        private byte[] Scramble(List<byte> data)
        {
            var magicConstants = new uint[] { 0x99036946, 0xE99DB8E7, 0xE3AE2FA7, 0xA339740, 0xF06EB6A9, 0x92FF9B65, 0x28F7873, 0x9070E316 };
            List<byte> unalignedChunk = new List<byte>();

            if (data.Count % 8 != 0)
            {
                var stop = 8 * (data.Count / 8);
                unalignedChunk = data.GetRange(stop, data.Count - stop);
                data = data.GetRange(0, stop);
                data.AddRange(new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 });
            }

            var output = new List<byte>();

            var X = (uint)0x6479B873;
            var Y = (uint)0x48853AFC;

            for (int i = 0; i < data.Count; i += 8)
            {
                X ^= BitConverter.ToUInt32(data.GetRange(i, 4).ToArray().Reverse().ToArray(), 0);
                Y ^= BitConverter.ToUInt32(data.GetRange(i + 4, 4).ToArray().Reverse().ToArray(), 0);

                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Y ^= X;

                        var a = (magicConstants[4 * k + 0] + Y) & 0xFFFFFFFF;
                        var b = (a - 1 + RotateLeft(a, 1)) & 0xFFFFFFFF;

                        X ^= b ^ RotateLeft(b, 4);

                        var c = (magicConstants[4 * k + 1] + X) & 0xFFFFFFFF;
                        var d = (c + 1 + RotateLeft(c, 2)) & 0xFFFFFFFF;

                        var e = (magicConstants[4 * k + 2] + (d ^ RotateLeft(d, 8))) & 0xFFFFFFFF;
                        var f = (RotateLeft(e, 1) - e) & 0xFFFFFFFF;


                        Y ^= (X | f) ^ RotateLeft(f, 16);


                        var g = (magicConstants[4 * k + 3] + Y) & 0xFFFFFFFF;
                        X ^= (g + 1 + RotateLeft(g, 2)) & 0xFFFFFFFF;
                    }
                }
                output.AddRange(BitConverter.GetBytes(X).Reverse());
                output.AddRange(BitConverter.GetBytes(Y).Reverse());
            }
            if (unalignedChunk.Count > 0)
            {
                var lastScramble = output.GetRange(output.Count - 8, 8);
                output.RemoveRange(output.Count - 8, 8);
                for (int i = 0; i < unalignedChunk.Count; i++)
                {
                    unalignedChunk[i] = (byte)(lastScramble[i] ^ unalignedChunk[i]);
                }
                output.AddRange(unalignedChunk);
            }
            return output.ToArray();
        }
    }
}