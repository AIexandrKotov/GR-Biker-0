using KCore;
using KCore.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Biker 0";
            new MainMenu().StartAnimation();
            Terminal.Abort();
        }
    }
}
