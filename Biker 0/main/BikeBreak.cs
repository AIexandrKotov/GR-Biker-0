using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    public class BikeBreak
    {
        // ORIGINAL BIKER 0
        public string Name { get; set; }
        public string Description { get; set; }
        public long RepairPrice { get; set; }

        // GODNOTA RESSURRECTION CONTENT
        public double Frequency { get; set; } = 1.0;
        public BreakEffect Effect { get; set; }
    }
}
