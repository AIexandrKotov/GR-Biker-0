using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    public class Town
    {
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool HasRepair { get; set; }
        public bool HasBikeShop { get; set; }
        public bool HasTrickSchool { get; set; }
        public bool HasTrickArena { get; set; }
        public bool HasRaceArena { get; set; }

        public string ToSuperText()
        {
            return $"{Name} {(HasRepair?"%=>Green%M ":"")}{(HasBikeShop ? "%=>Blue%B " : "")}{(HasTrickSchool ? "%=>Cyan%T " : "")}{(HasTrickArena ? "%=>Magenta%A " : "")}{(HasRaceArena ? "%=>Red%R" : "")}%=>reset%";
        }
    }
}
