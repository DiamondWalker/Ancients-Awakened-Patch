using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubworldLibrary;
using Terraria;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Dimension.Void
{
    public class VoidSubworld : Subworld
    {
        public override int width => Main.maxTilesX;

        public override int height => Main.maxTilesY;

        public override bool saveSubworld => true;

        public override bool saveModData => true;

        public override List<GenPass> tasks => new List<GenPass>()
        {
            new AsteroidsGenPass()
        };

        public static void Enter() {
            AAWorld.CacheDataForSubworlds();
            Subworld.Enter<VoidSubworld>();
        }

        public static bool IsInside() {
            return Subworld.IsActive<VoidSubworld>();
        }

        public static void Exit() {
            Subworld.Exit();
        }

        public override void Load() {
            base.Load();
            SLWorld.drawUnderworldBackground = false;
            Main.worldSurface = Main.rockLayer = Main.maxTilesY - 1;
            //SLWorld.noReturn = true;
        }

        public override void Unload() {
            base.Unload();
            AAWorld.CacheDataForSubworlds();
        }
    }
}
