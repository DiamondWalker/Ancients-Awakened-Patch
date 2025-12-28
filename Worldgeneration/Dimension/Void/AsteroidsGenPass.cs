using AAMod.Items.Blocks;
using AAMod.Tiles;
using AAMod.Util;
using Microsoft.Xna.Framework;
using SubworldLibrary;
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
    public class AsteroidsGenPass : GenPass {
        public AsteroidsGenPass() : base("Void Asteroids", 1f) {
        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildAsteroids");

            for (int i = 0; i < 45; i++) {
                int startX = Main.rand.Next(Main.maxTilesX);
                int startY = Main.rand.Next(Main.maxTilesY);
                if (!OverlapsIslands(startX, startY)) {
                    int tiles = Main.rand.Next(200, 3600);
                    HashSet<long> positions = new HashSet<long>();
                    HashSet<long> visited = new HashSet<long>();
                    positions.Add(BitUtil.CombineInts(startX, startY));

                    while (tiles > 0) {
                        long selection = positions.ToArray()[Main.rand.Next(positions.Count)];
                        positions.Remove(selection);
                        visited.Add(selection);

                        int x = BitUtil.GetLeftInt(selection);
                        int y = BitUtil.GetRightInt(selection);

                        WorldGenUtil.PlaceCircle(ModContent.TileType<Tiles.DoomstoneB>(), x, y, 5);//WorldGenUtil.PlaceTile(ModContent.TileType<Tiles.DoomstoneB>(), x, y);
                        tiles--;

                        long left = BitUtil.CombineInts(x - 1, y);
                        long right = BitUtil.CombineInts(x + 1, y);
                        long up = BitUtil.CombineInts(x, y - 1);
                        long down = BitUtil.CombineInts(x, y + 1);

                        if (!visited.Contains(left)) positions.Add(left);
                        if (!visited.Contains(right)) positions.Add(right);
                        if (!visited.Contains(up)) positions.Add(up);
                        if (!visited.Contains(down)) positions.Add(down);
                    }
                }
            }
        }

        private bool OverlapsIslands(int x, int y) {
            foreach (Rectangle island in IslandsGenPass.islands) {
                if (island.Contains(x, y)) return true;
            }
            return false;
        }
    }
}
