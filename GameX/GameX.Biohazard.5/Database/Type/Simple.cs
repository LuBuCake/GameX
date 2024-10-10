using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Database.Type
{
    public class Simple
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public Simple()
        {

        }

        public Simple(string Text)
        {
            this.Text = Text;
        }

        public Simple(string Text, int Value)
        {
            this.Text = Text;
            this.Value = Value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
