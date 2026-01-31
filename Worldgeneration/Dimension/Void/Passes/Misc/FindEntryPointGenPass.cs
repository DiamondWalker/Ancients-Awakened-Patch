using AAMod.Worldgeneration.Dimension.Void.Passes.Islands;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.World.Generation;
using AAMod.Util;
using Microsoft.Xna.Framework;
using AAMod.Projectiles.Akuma.Dawnstrike;

namespace AAMod.Worldgeneration.Dimension.Void.Passes.Misc {
    public class FindEntryPointGenPass : GenPass {
        public FindEntryPointGenPass() : base("Entry Point", 1f) {

        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildCleanup");

            while (true) {
                int x = Main.rand.Next(Main.maxTilesX);
                int y = Main.rand.Next(Main.maxTilesY);

                while (WorldGen.InWorld(x, y) && !WorldGenUtil.TileAt(x, y)) y++; // move down until we find an island

                while (!PortalFits(x, y)) y--;

                if (x > 100 && x < Main.maxTilesX - 100 && y > 100 && y < Main.maxTilesY - 100) {
                    if (!IsInIsland(x, y)) {
                        Main.spawnTileX = x;
                        Main.spawnTileY = y;

                        return;
                    }
                }
            }
        }

        private bool PortalFits(int x, int y) {
            for (int xOff = - 3; xOff <= 3; xOff++) {
                for (int yOff = - 3; yOff <= 3; yOff++) {
                    if (WorldGenUtil.TileAt(x + xOff, y + yOff)) return false;
                }
            }

            return true;
        }

        private bool IsInIsland(int x, int y) {
            foreach (Rectangle island in IslandsGenPass.islands) {
                if (island.Contains(x, y)) return true;
            }

            return false;
        }
    }
}
