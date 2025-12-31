using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAMod.Sounds.Sounds;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Dimension.Void
{
    public class VoidSubworld : Subworld
    {
        public override int width => Main.maxTilesX;

        public override int height => Main.maxTilesY;

        public override bool saveSubworld => false;//true;

        public override bool saveModData => true;

        public override List<GenPass> tasks => new List<GenPass>()
        {
            new IslandsGenPass(),
            new ApocalyptiteOreGenPass(),
            new CratersGenPass(),
            new CanyonsGenPass(),
            new RemoveSpareRocksGenPass(),
            new AsteroidsGenPass(),
            //new ApocalyptiteGenPass()
        };

        public static void Enter() {
            if (IsInside()) return;
            AAWorld.CacheDataForSubworlds();
            Subworld.Enter<VoidSubworld>();
        }

        public static bool IsInside() {
            return Subworld.IsActive<VoidSubworld>();
        }

        public static void Exit() {
            if (!IsInside()) return;
            Subworld.Exit();
        }

        public override void Load() {
            base.Load();
            Main.dayTime = false;
            Main.time = 15600; // midnight
            Main.moonPhase = 0;
            SLWorld.drawUnderworldBackground = false;
            Main.worldSurface = Main.rockLayer = Main.maxTilesY - 1;
            for (int i = 125; i <= 145; i++) {
                Main.backgroundTexture[i] = ModContent.GetInstance<AAMod>().GetTexture("BlankTex");
            }
            for (int i = 185; i <= 187; i++) {
                Main.backgroundTexture[i] = ModContent.GetInstance<AAMod>().GetTexture("BlankTex");
            }
            //SLWorld.noReturn = true;
        }

        public override void Unload() {
            base.Unload();
            AAWorld.CacheDataForSubworlds();
            for (int i = 125; i <= 145; i++) {
                Main.backgroundTexture[i] = ModContent.GetTexture("Terraria/Background_" + i);
            }
            for (int i = 185; i <= 187; i++) {
                Main.backgroundTexture[i] = ModContent.GetTexture("Terraria/Background_" + i);
            }
        }
    }
}
