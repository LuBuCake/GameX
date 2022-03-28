using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Database.Type
{
    public class Character
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public List<Costume> Costumes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
