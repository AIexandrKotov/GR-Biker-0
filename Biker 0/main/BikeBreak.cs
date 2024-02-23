using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    public class TrickTrack
    {
        public class State
        {
            public double CurrentDistance { get; set; }
            public double CurrentLoop { get; set; }
            public double Speed { get; set; }
        }

        public string Name { get; set; }
        public double Distance { get; set; }
        public Dictionary<double, double> JumpRamps { get; set; }
        public Dictionary<double, double> SpeedUpRamps { get; set; }
    }

    public class RacingTrack
    {
        public string Name { get; set; }
        public (double, Road.RoadQuality)[] Distances { get; set; }
    }

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
