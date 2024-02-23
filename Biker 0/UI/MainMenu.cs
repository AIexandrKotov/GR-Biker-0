using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCore;
using KCore.Extensions;
using KCore.Graphics;
using KCore.Graphics.Widgets;

namespace Biker_0
{

    public class MainMenu : Form
    {
        protected override void OnAllRedraw()
        {
            var i = 1;
            foreach (var town in Data.Towns)
            {
                Terminal.Set(1, i++);
                town.ToSuperText().PrintSuperText();
            }
            this.RealizeAnimation(new Game(new Player()));
        }
    }
}
