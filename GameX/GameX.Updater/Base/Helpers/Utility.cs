using System;

namespace GameX.Updater.Base.Helpers
{
    public class Utility
    {
        public static string StringBetween(string strSource, string strStart, string strEnd)
        {
            if (!strSource.Contains(strStart) || !strSource.Contains(strEnd)) 
                return "";

            int Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            int End = strSource.IndexOf(strEnd, Start, StringComparison.Ordinal);
            return strSource.Substring(Start, End - Start);
        }
    }
}
