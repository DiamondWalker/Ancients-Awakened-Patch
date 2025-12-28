using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Dimension.Void {
    public class CratersGenPass : GenPass {
        public CratersGenPass() : base("Craters", 1f) {

        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildCraters");

            Rectangle[] islands = IslandsGenPass.islands.ToArray();
            for (int i = 0; i < 16000; i++) {
                int x = Main.rand.Next(Main.maxTilesX);
                int y = Main.rand.Next(Main.maxTilesY);

                foreach (Rectangle island in islands) {
                    if (island.Contains(x, y)) {
                        if (Main.tile[x, y] == null || !Main.tile[x, y].active()) {
                            int radius = Main.rand.Next(25);
                            for (int x2 = x - radius; x2 <= x + radius; x2++) {
                                for (int y2 = y - radius; y2 <= y + radius; y2++) {
                                    int distX = x2 - x;
                                    int distY = y2 - y;
                                    if (island.Contains(x2, y2) && Math.Sqrt(distX * distX + distY * distY) <= radius && WorldGen.InWorld(x2, y2) && Main.tile[x2, y2] != null) {
                                        Main.tile[x2, y2].type = 0;
                                        Main.tile[x2, y2].active(false);
                                    }
                                }
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}
