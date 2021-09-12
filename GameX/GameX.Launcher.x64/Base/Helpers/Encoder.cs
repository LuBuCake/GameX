using System;
using System.IO;
using System.Text;

namespace GameX.Launcher.Base.Helpers
{
    public static class Encoder
    {
        public static byte[] GetDecodedStream(string path)
        {
            try
            {
                byte[] FileData = File.ReadAllBytes(path);
                char[] CharArray = Encoding.UTF8.GetChars(FileData);
                byte[] Decoded = Convert.FromBase64CharArray(CharArray, 0, CharArray.Length);

                return Decoded;
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }
    }
}