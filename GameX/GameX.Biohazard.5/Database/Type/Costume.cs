using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Database.Type
{
    public class Costume
    {
        public string Name { get; set; }
        public string Portrait { get; set; }
        public string File { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
