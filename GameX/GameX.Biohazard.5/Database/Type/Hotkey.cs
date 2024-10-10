using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Database.Type
{
    public class Hotkey
    {
        public string Name { get; set; }
        public int Key { get; set; }
        public string Image { get; set; }

        public Hotkey(string name, int key, string image)
        {
            Name = name;
            Key = key;
            Image = image;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
