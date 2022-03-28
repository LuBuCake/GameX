using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Database.ViewBag
{
    public class TemporaryItemViewBag
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public short MaxQuantity { get; set; }
        public byte Firepower { get; set; }
        public byte ReloadSpeed { get; set; }
        public byte Capacity { get; set; }
        public byte Critical { get; set; }
        public byte Piercing { get; set; }
        public byte Range { get; set; }
        public byte Scope { get; set; }
    }
}
