using System;
using System.Drawing;
using System.IO;

namespace GameX.Launcher.Base.Helpers
{
    public static class Utility
    {
        public static Image GetImageFromStream(string File)
        {
            try
            {
                Image img = Image.FromStream(new MemoryStream(Encoder.GetDecodedStream(File)));
                return img;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}