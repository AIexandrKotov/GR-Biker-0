using KCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    public static class BikeSpeed
    {
        public interface IBoost
        {
            int BoostBikeGroundSpeed { get; set; }
            int BoostBikeAshpaltSpeed { get; set; }
        }
        public interface IMultiplier
        {
            double MultiplierBikeGroundSpeed { get; set; }
            double MultiplierBikeAsphaltSpeed { get; set; }
        }
    }

    public static class BikeAcceleration
    {
        public interface IBoost
        {
            int BoostBikeGroundAcceleration { get; set; }
            int BoostBikeAsphaltAcceleration { get; set; }
            int BoostBikeTrickAcceleration { get; set; }
        }
        public interface IMultiplier
        {
            double MultiplierBikeGroundAcceleration { get; set; }
            double MultiplierBikeAsphaltAcceleration { get; set; }
            double MultiplierBikeTrickAcceleration { get; set; }
        }
    }

    public static class BikeTrick
    {
        public interface IBoost
        {
            int BoostBikeTrickComfort { get; set; }
            int BoostBikeTrickBrake { get; set; }
        }
        public interface IMultiplier
        {
            double MultiplierBikeTrickComfort { get; set; }
            double MultiplierBikeTrickBrake { get; set; }
        }
    }

    public class AllBikeEffect
        : BikeSpeed.IBoost, BikeSpeed.IMultiplier,
          BikeAcceleration.IBoost, BikeAcceleration.IMultiplier,
          BikeTrick.IBoost, BikeTrick.IMultiplier
    {
        public int BoostBikeGroundSpeed { get; set; } = 0;
        public int BoostBikeAshpaltSpeed { get; set; } = 0;
        public double MultiplierBikeGroundSpeed { get; set; } = 1.0;
        public double MultiplierBikeAsphaltSpeed { get; set; } = 1.0;
        public int BoostBikeGroundAcceleration { get; set; } = 0;
        public int BoostBikeAsphaltAcceleration { get; set; } = 0;
        public int BoostBikeTrickAcceleration { get; set; } = 0;
        public double MultiplierBikeGroundAcceleration { get; set; } = 1.0;
        public double MultiplierBikeAsphaltAcceleration { get; set; } = 1.0;
        public double MultiplierBikeTrickAcceleration { get; set; } = 1.0;
        public int BoostBikeTrickComfort { get; set; } = 0;
        public int BoostBikeTrickBrake { get; set; } = 0;
        public double MultiplierBikeTrickComfort { get; set; } = 1.0;
        public double MultiplierBikeTrickBrake { get; set; } = 1.0;

        public static T Collapse<T>(IList<T> breakEffects) where T: 
            BikeSpeed.IBoost, BikeSpeed.IMultiplier,
            BikeAcceleration.IBoost, BikeAcceleration.IMultiplier,
            BikeTrick.IBoost, BikeTrick.IMultiplier,
            new()
        {
            var ret = new T()
            {
                BoostBikeAshpaltSpeed = breakEffects.Sum(x => x.BoostBikeAshpaltSpeed),
                BoostBikeAsphaltAcceleration = breakEffects.Sum(x => x.BoostBikeAsphaltAcceleration),
                BoostBikeGroundAcceleration = breakEffects.Sum(x => x.BoostBikeGroundAcceleration),
                BoostBikeGroundSpeed = breakEffects.Sum(x => x.BoostBikeGroundSpeed),
                BoostBikeTrickAcceleration = breakEffects.Sum(x => x.BoostBikeTrickAcceleration),
                BoostBikeTrickBrake = breakEffects.Sum(x => x.BoostBikeTrickBrake),
                BoostBikeTrickComfort = breakEffects.Sum(x => x.BoostBikeTrickComfort),

                MultiplierBikeAsphaltSpeed = breakEffects.Product(x => x.MultiplierBikeAsphaltSpeed),
                MultiplierBikeAsphaltAcceleration = breakEffects.Product(x => x.MultiplierBikeAsphaltAcceleration),
                MultiplierBikeGroundAcceleration = breakEffects.Product(x => x.MultiplierBikeGroundAcceleration),
                MultiplierBikeGroundSpeed = breakEffects.Product(x => x.BoostBikeGroundSpeed),
                MultiplierBikeTrickAcceleration = breakEffects.Product(x => x.BoostBikeTrickAcceleration),
                MultiplierBikeTrickBrake = breakEffects.Product(x => x.BoostBikeTrickBrake),
                MultiplierBikeTrickComfort = breakEffects.Product(x => x.BoostBikeTrickComfort),
            };
            if (ret.MultiplierBikeAsphaltSpeed < 0.1) ret.MultiplierBikeAsphaltSpeed = 0.1;
            if (ret.MultiplierBikeAsphaltAcceleration < 0.1) ret.MultiplierBikeAsphaltAcceleration = 0.1;
            if (ret.MultiplierBikeGroundAcceleration < 0.1) ret.MultiplierBikeGroundAcceleration = 0.1;
            if (ret.MultiplierBikeGroundSpeed < 0.1) ret.MultiplierBikeGroundAcceleration = 0.1;
            if (ret.MultiplierBikeTrickAcceleration < 0.1) ret.MultiplierBikeTrickAcceleration = 0.1;
            if (ret.MultiplierBikeTrickBrake < 0.1) ret.MultiplierBikeTrickBrake = 0.1;
            if (ret.MultiplierBikeTrickComfort < 0.1) ret.MultiplierBikeTrickComfort = 0.1;
            return ret;
        }
    }

    public class BreakEffect : AllBikeEffect
    {

    }
}
