using KCore.Extensions.InsteadSLThree;
using SLThree.sys;
using System.Linq;

namespace Biker_0
{
    public class Player
    {
        public string Nickname { get; set; } = "default";
        public int TownID { get; set; } = 0;
        public long Money { get; set; } = 1000;
        public Town Town => Data.Towns[TownID];
        public Bike Bike { get; set; }

        public static Player CreateNew()
        {
            var ret = new Player();
            ret.Bike = slt.eval("global.get_start_bike()").Cast<Bike>();
            return ret;
        }
    }
}
