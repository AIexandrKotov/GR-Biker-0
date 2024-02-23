using KCore;
using KCore.Forms;
using KCore.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-us");
            Console.Title = "Biker 0";
            Data.Init();
            new Game(Player.CreateNew()).Start();
            Terminal.Abort();
        }
    }
}
