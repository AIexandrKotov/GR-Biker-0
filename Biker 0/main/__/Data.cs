using KCore.Extensions;
using SLThree;
using SLThree.Embedding;
using SLThree.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Biker_0
{
    public static class Data
    {
        public static readonly Town[] Towns;
        public static readonly Road[] Roads;
        public static readonly BikeBreak[] Breaks;
        public static readonly ChanceChooser<BikeBreak> BreakChooser;

        private static string ReadResource(string resname)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resname))
            {
                using (var tr = new StreamReader(stream))
                    return tr.ReadToEnd();
            }
        }

        public static void Init()
        {

        }

        static Data()
        {
            ScriptLayout.Execute(ReadResource("Biker_0.data.init.slt"));
            Towns = ReadResource("Biker_0.data.towns.slt")
                .RunScript().LocalVariables.GetAsDictionary().Values.OfType<Town>().ToArray();
            Roads = ReadResource("Biker_0.data.roads.slt")
                .RunScript().ReturnedValue.Cast<List<object>>().Select(x => x as Road).ToArray();
            Breaks = ReadResource("Biker_0.data.breaks.slt")
                .RunScript().LocalVariables.GetAsDictionary().Values.OfType<BikeBreak>().ToArray();
            BreakChooser = new ChanceChooser<BikeBreak>(Breaks.ConvertAll(x => (x, x.Frequency)));
            ReadResource("Biker_0.data.get_start_bike.slt").RunScript();
        }
    }
}
