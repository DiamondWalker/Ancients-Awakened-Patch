using AAMod.Util;
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
            int cratersCount = (int)((1.0 + 0.5 * AAWorld.GetWorldSize()) * 8000);
            for (int i = 0; i < cratersCount; i++) {
                int x = Main.rand.Next(Main.maxTilesX);
                int y = Main.rand.Next(Main.maxTilesY);

                foreach (Rectangle island in islands) {
                    if (island.Contains(x, y)) {
                        if (Main.tile[x, y] == null || !Main.tile[x, y].active()) {
                            WorldGenUtil.ClearCircle(x, y, Main.rand.Next(25), island);
                        }

                        break;
                    }
                }
            }
        }
    }
}
