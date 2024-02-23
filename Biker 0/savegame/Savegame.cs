using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    public class Savegame
    {
        public static string GetSavegamePath(string nickname) => Path.Combine(KCore.Storage.KCoreStorage.GetApplicationPath(), $"{nickname}.biker0save");
    }
}
