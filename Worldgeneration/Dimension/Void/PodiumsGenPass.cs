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
using AAMod.Items.Materials;
using AAMod.Tiles;

namespace AAMod.Worldgeneration.Dimension.Void {
    public class PodiumsGenPass : GenPass {

        public PodiumsGenPass() : base("Podiums", 1f) {

        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildPodiums");

            bool placed = false;
            int attempts = 0;
            while (attempts < 100 || !placed) {
                int x = Main.rand.Next(Main.maxTilesX);
                int y = Main.rand.Next(Main.maxTilesY);
                
                if (WorldGenUtil.TileAt(x, y)) {
                    while (WorldGenUtil.TileAt(x, y)) y--;

                    y -= 3; // shift it up a bit so it's above the ground

                    // scan horizontally to make sure we aren't in a hole or something idk
                    bool validLocation = true;
                    for (int xOff = -20; xOff <= 20; xOff++) {
                        if (WorldGenUtil.TileAt(x + xOff, y)) {
                            validLocation = false;
                            break;
                        }
                    }

                    if (validLocation) {
                        for (int yOff = 0; yOff < 10; yOff++) {
                            bool atBottom = true;
                            for (int xOff = -yOff - 2; xOff <= yOff + 2; xOff++) {
                                if (!WorldGenUtil.TileAt(x + xOff, y + yOff)) {
                                    atBottom = false;
                                    WorldGenUtil.PlaceTile<DoomitePlate>(x + xOff, y + yOff);
                                }
                            }
                            if (atBottom) break;
                        }
                        placed = true;
                    }
                }

                attempts++;
            }
        }
    }
}
