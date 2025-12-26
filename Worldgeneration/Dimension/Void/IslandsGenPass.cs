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
    public class IslandsGenPass : GenPass {
        public IslandsGenPass() : base("Islands", 1f) {

        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildIslands");

            int minX = (int)Math.Ceiling(0.2f * Main.maxTilesX);
            int maxX = (int)Math.Floor(0.8f * Main.maxTilesX);
            int minY = (int)Math.Ceiling(0.1f * Main.maxTilesY);
            int maxY = (int)Math.Floor(0.9f * Main.maxTilesY);

            int count = Main.rand.Next(3, 7);
            for (int i = 0; i < count; i++) {
                GenerateIsland(Main.rand.Next(minX, maxX + 1), Main.rand.Next(minY, maxY + 1));
            }
        }

        private void GenerateIsland(int x, int y) {
            int width = Main.rand.Next(500, 1400);
            int height = (int)(Main.rand.NextFloat(0.2f, 0.5f) * width);
            x -= (width / 2);

            float slant = Main.rand.NextFloat(-0.5f, 0.5f);
            float yOffset = 0.0f;
            for (int i = 0; i < width; i++) {
                yOffset += Main.rand.NextFloat(-1.0f, 1.0f);
                float prog = (float)i / width;
                int currX = x + i;
                float f = 1.0f - (prog - 0.5f) * (prog - 0.5f) * 4;
                int startY = y + (int)Math.Round(yOffset);
                int endY = y + (int)(f * height / 2);
                for (int j = startY; j < endY; j++) {
                    PlaceTile(ModContent.TileType<Tiles.DoomstoneB>(), currX, y + j + (int)Math.Round(slant * (prog - 0.5f) * width));
                }
            }
        }

        private void PlaceTile(int type, int x, int y) {
            if (!WorldGen.InWorld(x, y)) return;
            if (Main.tile[x, y] == null) Main.tile[x, y] = new Tile();
            Main.tile[x, y].type = (ushort)type;
            Main.tile[x, y].active(true);
        }
    }
}
