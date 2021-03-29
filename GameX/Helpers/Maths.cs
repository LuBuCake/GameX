using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Helpers
{
    public static class Maths
    {
        public static bool CompareByteArray(byte[] Array1, byte[] Array2, int Length)
        {
            if (Array1.Length != Array2.Length)
                return false;

            for (int i = 0; i < Length; i++)
                if (Array1[i] != Array1[i])
                    return false;

            return true;
        }
    }
}
