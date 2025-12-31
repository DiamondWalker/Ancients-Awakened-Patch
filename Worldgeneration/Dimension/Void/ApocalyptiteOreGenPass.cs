using AAMod.Sounds.Sounds;
using AAMod.Tiles.Ore;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Dimension.Void {
    public class ApocalyptiteOreGenPass : GenPass {
        public ApocalyptiteOreGenPass() : base("Apocalyptite", 1f) {
        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildApocalyptiteVeins");

            foreach (Rectangle island in IslandsGenPass.islands) {
                int islandArea = island.Width * island.Height;
                for (int i = 0; i < islandArea / 3200; i++) {
                    int x = island.X + Main.rand.Next(island.Width);
                    int y = island.Y + Main.rand.Next(island.Height);
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 13), ModContent.TileType<Apocalyptite>(), false, 0f, 0f, false, true);
                }
            }
            /*for (int k = 0; k < posIslands.Count; ++k) {
                for (int FuckWorldGen = 0; FuckWorldGen < 6; ++FuckWorldGen) {
                    Point randompoint = new Point(
                        posIslands[k].X + WorldGen.genRand.Next(-30, 31),
                        posIslands[k].Y + WorldGen.genRand.Next(7, 42));
                    WorldGen.TileRunner(randompoint.X, randompoint.Y, WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 13), mod.TileType("Apocalyptite"), false, 0f, 0f, false, true);
                }
            }*/
        }
    }
}
