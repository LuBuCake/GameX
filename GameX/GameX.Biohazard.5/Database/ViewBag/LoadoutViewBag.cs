using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Database.ViewBag
{
    public class LoadoutViewBag
    {
        public string Name { get; set; }
        public TemporaryItemViewBag[] Slots { get; set; }
    }
}
