using KCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    public class Bike : BikeSpeed.IBoost, BikeAcceleration.IBoost, BikeTrick.IBoost
    {
        public string Name { get; set; }

        public List<int> BreaksIds { get; set; } = new List<int>();
        public IEnumerable<BikeBreak> Breaks => BreaksIds.Select(x => Data.Breaks[x]);

        public int BoostBikeGroundSpeed { get; set; }
        public int BoostBikeAshpaltSpeed { get; set; }
        public int BoostBikeGroundAcceleration { get; set; }
        public int BoostBikeAsphaltAcceleration { get; set; }
        public int BoostBikeTrickAcceleration { get; set; }
        public int BoostBikeTrickComfort { get; set; }
        public int BoostBikeTrickBrake { get; set; }
        private IEnumerable<object> GetBonuses()
        {
            yield return this;
            yield return AllBikeEffect.Collapse(Breaks.Select(x => x.Effect).ToArray());
        }

        public double AsphaltSpeed => Cache.BikeSpeedCache.BoostBikeAshpaltSpeed * Cache.BikeSpeedCache.MultiplierBikeAsphaltSpeed;
        public double GroundSpeed => Cache.BikeSpeedCache.BoostBikeGroundSpeed * Cache.BikeSpeedCache.MultiplierBikeGroundSpeed;
        public double AsphaltAcceleration => Cache.BikeAccelerationCache.BoostBikeAsphaltAcceleration * Cache.BikeAccelerationCache.MultiplierBikeAsphaltAcceleration;
        public double GroundAcceleration => Cache.BikeAccelerationCache.BoostBikeGroundAcceleration * Cache.BikeAccelerationCache.MultiplierBikeGroundAcceleration;
        public double TrickAcceleration => Cache.BikeTrickCache.BoostBikeTrickAcceleration * Cache.BikeTrickCache.MultiplierBikeTrickAcceleration;
        public double TrickComfort => Cache.BikeTrickCache.BoostBikeTrickComfort * Cache.BikeTrickCache.MultiplierBikeTrickComfort;
        public double TrickBrake => Cache.BikeTrickCache.BoostBikeTrickBrake * Cache.BikeTrickCache.MultiplierBikeTrickBrake;

        #region CACHE
        public class BikeCache
        {
            public readonly BikeSpeedCacheClass BikeSpeedCache = new BikeSpeedCacheClass();
            public readonly BikeAccelerationCacheClass BikeAccelerationCache = new BikeAccelerationCacheClass();
            public readonly BikeTrickCacheClass BikeTrickCache = new BikeTrickCacheClass();

            public class BikeSpeedCacheClass : BikeSpeed.IBoost, BikeSpeed.IMultiplier
            {
                public int BoostBikeGroundSpeed { get; set; } = 0;
                public int BoostBikeAshpaltSpeed { get; set; } = 0;
                public double MultiplierBikeGroundSpeed { get; set; } = 1.0;
                public double MultiplierBikeAsphaltSpeed { get; set; } = 1.0;
            }
            public class BikeAccelerationCacheClass : BikeAcceleration.IBoost, BikeAcceleration.IMultiplier
            {
                public int BoostBikeGroundAcceleration { get; set; } = 0;
                public int BoostBikeAsphaltAcceleration { get; set; } = 0;
                public double MultiplierBikeGroundAcceleration { get; set; } = 1.0;
                public double MultiplierBikeAsphaltAcceleration { get; set; } = 1.0;
            }
            public class BikeTrickCacheClass : BikeTrick.IBoost, BikeTrick.IMultiplier
            {
                public int BoostBikeTrickComfort { get; set; } = 0;
                public int BoostBikeTrickBrake { get; set; } = 0;
                public int BoostBikeTrickAcceleration { get; set; } = 0;
                public double MultiplierBikeTrickComfort { get; set; } = 1.0;
                public double MultiplierBikeTrickBrake { get; set; } = 1.0;
                public double MultiplierBikeTrickAcceleration { get; set; } = 1.0;
            }
        }
        private BikeCache cache;
        public BikeCache Cache
        {
            get
            {
                if (cache == null) throw new Exception("Call the CalculateCache!");
                return cache;
            }
        }
        #endregion

        public void UpdateCache(Player player)
        {
            cache = new BikeCache();
            foreach (var bonus in GetBonuses())
            {
                if (bonus is BikeSpeed.IBoost bs_boost)
                {
                    cache.BikeSpeedCache.BoostBikeGroundSpeed += bs_boost.BoostBikeGroundSpeed;
                    cache.BikeSpeedCache.BoostBikeAshpaltSpeed += bs_boost.BoostBikeAshpaltSpeed;
                }
                if (bonus is BikeSpeed.IMultiplier bs_mult)
                {
                    cache.BikeSpeedCache.MultiplierBikeGroundSpeed *= bs_mult.MultiplierBikeGroundSpeed;
                    cache.BikeSpeedCache.MultiplierBikeAsphaltSpeed *= bs_mult.MultiplierBikeAsphaltSpeed;
                }
                if (bonus is BikeAcceleration.IBoost ba_boost)
                {
                    cache.BikeAccelerationCache.BoostBikeGroundAcceleration += ba_boost.BoostBikeGroundAcceleration;
                    cache.BikeAccelerationCache.BoostBikeAsphaltAcceleration += ba_boost.BoostBikeAsphaltAcceleration;
                }
                if (bonus is BikeAcceleration.IMultiplier ba_mult)
                {
                    cache.BikeAccelerationCache.MultiplierBikeGroundAcceleration *= ba_mult.MultiplierBikeGroundAcceleration;
                    cache.BikeAccelerationCache.MultiplierBikeAsphaltAcceleration *= ba_mult.MultiplierBikeAsphaltAcceleration;
                }
                if (bonus is BikeTrick.IBoost bt_boost)
                {
                    cache.BikeTrickCache.BoostBikeTrickBrake += bt_boost.BoostBikeTrickBrake;
                    cache.BikeTrickCache.BoostBikeTrickComfort += bt_boost.BoostBikeTrickComfort;
                    cache.BikeTrickCache.BoostBikeTrickAcceleration += bt_boost.BoostBikeTrickAcceleration;
                }
                if (bonus is BikeTrick.IMultiplier bt_mult)
                {
                    cache.BikeTrickCache.MultiplierBikeTrickBrake *= bt_mult.MultiplierBikeTrickBrake;
                    cache.BikeTrickCache.MultiplierBikeTrickComfort *= bt_mult.MultiplierBikeTrickComfort;
                    cache.BikeTrickCache.MultiplierBikeTrickAcceleration *= bt_mult.MultiplierBikeTrickAcceleration;
                }
            }
        }

    }
}
