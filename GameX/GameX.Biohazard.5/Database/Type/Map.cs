using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Database.Type
{
    public class Map
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public int Chapter { get; set; }
        public short Stage { get; set; }

        public Map(string Name, string Alias, int Chapter, short Stage)
        {
            this.Name = Name;
            this.Alias = Alias;
            this.Chapter = Chapter;
            this.Stage = Stage;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
