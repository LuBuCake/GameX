using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GameX.Base.Modules;

namespace GameX.Base.Helpers
{
    public static class Encoder
    {
        /*
         * File extensions:
         * Image = EIF (Encoded Image Format)
         * Audio = EAF (Encoded Audio Format)
         * General = EFF (Encoded File Format)
         */

        public static string GetFileExtension(string RefereceExtension)
        {
            Dictionary<string, string> Extensions = new Dictionary<string, string>();
            Extensions.Add(".png", ".eia");
            Extensions.Add(".jpg", ".eib");
            Extensions.Add(".jpeg", ".eic");
            Extensions.Add(".bmp", ".eid");
            Extensions.Add(".tif", ".eif");
            Extensions.Add(".tiff", ".eig");
            Extensions.Add(".gif", ".eih");

            if (Extensions.ContainsKey(RefereceExtension))
            {
                return Extensions[RefereceExtension];
            }

            if (Extensions.Any(x => x.Value == RefereceExtension))
            {
                return Extensions.First(x => x.Value == RefereceExtension).Key;
            }

            return ".eff";
        }

        public static byte[] GetDecodedStream(string path)
        {
            try
            {
                byte[] FileData = File.ReadAllBytes(path);
                char[] CharArray = Encoding.UTF8.GetChars(FileData);
                byte[] Decoded = Convert.FromBase64CharArray(CharArray, 0, CharArray.Length);

                return Decoded;
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                return new byte[0];
            }
        }

        public static bool EncodeFile(string path, string extension)
        {
            try
            {
                extension = extension.ToLower();

                byte[] FileData = File.ReadAllBytes(path);
                string Base64Data = Convert.ToBase64String(FileData);

                if (path.Contains(extension))
                    path = path.Replace(extension, "");

                path += GetFileExtension(extension);

                FileStream FS = new FileStream(path, FileMode.Create);
                BinaryWriter BW = new BinaryWriter(FS);

                BW.Write(Base64Data.ToCharArray());

                BW.Dispose();
                FS.Dispose();

                return true;
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                return false;
            }
        }

        public static bool DecodeFile(string path, string extension)
        {
            try
            {
                extension = extension.ToLower();

                byte[] Decoded = GetDecodedStream(path);

                if (path.Contains(extension))
                    path = path.Replace(extension, "");

                path += GetFileExtension(extension);

                FileStream FS = new FileStream(path, FileMode.Create);
                BinaryWriter BW = new BinaryWriter(FS);

                BW.Write(Decoded);

                BW.Dispose();
                FS.Dispose();

                return true;
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                return false;
            }
        }
    }
}