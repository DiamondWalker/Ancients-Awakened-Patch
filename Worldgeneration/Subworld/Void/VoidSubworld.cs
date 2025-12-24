using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Subworld.Void
{
    public class VoidSubworld : SubworldLibrary.Subworld
    {
        public override int width => 8000;

        public override int height => 1000;

        public override bool saveSubworld => true;

        public override List<GenPass> tasks => new List<GenPass>()
        {
            new AsteroidsGenPass()
        };
    }
}
