using AAMod.Util;
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
        private const float MAX_SURFACE_JAGGEDNESS = 2.5f;
        private const float MAX_BOTTOM_JAGGEDNESS = 4.4f;

        public static HashSet<Rectangle> islands = null;
        public IslandsGenPass() : base("Islands", 1f) {

        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildIslands");

            islands = new HashSet<Rectangle>();

            int minX = (int)Math.Ceiling(0.2f * Main.maxTilesX);
            int maxX = (int)Math.Floor(0.8f * Main.maxTilesX);
            int minY = (int)Math.Ceiling(0.2f * Main.maxTilesY);
            int maxY = (int)Math.Floor(0.8f * Main.maxTilesY);

            int count = Main.rand.Next(5, 9);
            for (int i = 0; i < count; i++) {
                for (int tries = 0; tries < 20; tries++) {
                    int x = Main.rand.Next(minX, maxX + 1);
                    int y = Main.rand.Next(minY, maxY + 1);
                    int width = Main.rand.Next(500, 1400);
                    int height = (int)(Main.rand.NextFloat(0.2f, 0.5f) * width);
                    if (AttemptToGenerateIsland(x, y, width, height)) break;
                }
            }
        }

        private bool AttemptToGenerateIsland(int x, int y, int width, int height) {
            Rectangle newIsland = new Rectangle(x - width, y - height, width * 2, height * 2);

            foreach (Rectangle island in islands) {
                if (island.Intersects(newIsland)) return false;
            }

            float slant = Main.rand.NextFloat(-0.15f, 0.15f);

            x -= (width / 2);
            y -= (height / 2);
            float ySurfaceOffset = 0.0f;
            float yBottomOffset = 0.0f;

            float surfaceJaggedness = Main.rand.NextFloat(-MAX_SURFACE_JAGGEDNESS, MAX_SURFACE_JAGGEDNESS);
            float bottomJaggedness = Main.rand.NextFloat(-MAX_BOTTOM_JAGGEDNESS, MAX_BOTTOM_JAGGEDNESS);

            int i = 0;
            while (true) {
                float prog = (float)i / width;
                int currX = x + i;

                surfaceJaggedness += Main.rand.NextFloat(-0.15f, 0.15f);
                bottomJaggedness += Main.rand.NextFloat(-0.2f, 0.2f);

                if (surfaceJaggedness > MAX_SURFACE_JAGGEDNESS) {
                    surfaceJaggedness -= MAX_SURFACE_JAGGEDNESS * 2;
                } else if (surfaceJaggedness < -MAX_SURFACE_JAGGEDNESS) {
                    surfaceJaggedness += MAX_SURFACE_JAGGEDNESS * 2;
                }

                if (bottomJaggedness > MAX_BOTTOM_JAGGEDNESS) {
                    bottomJaggedness -= MAX_BOTTOM_JAGGEDNESS * 2;
                } else if (bottomJaggedness < -MAX_BOTTOM_JAGGEDNESS) {
                    bottomJaggedness += MAX_BOTTOM_JAGGEDNESS * 2;
                }

                ySurfaceOffset += Main.rand.NextFloat(-surfaceJaggedness, surfaceJaggedness);
                yBottomOffset += Main.rand.NextFloat(-bottomJaggedness, bottomJaggedness);

                int surfaceY = y - (int)Math.Round(ySurfaceOffset);
                int bottomY = y + (int)Math.Round(yBottomOffset) + (int)(ShapingFunction(prog) * height);

                int verticalSlantOffset = (int)Math.Round(slant * (prog - 0.5f) * width);

                surfaceY += verticalSlantOffset;
                bottomY += verticalSlantOffset;

                if (i >= width && surfaceY >= bottomY) break;

                for (int j = surfaceY; j < bottomY; j++) {
                    float progY = (float)(j - y) / (height * 2);
                    int horizontalSlantOffset = (int)Math.Round(slant * (progY - 0.5f) * width);
                    WorldGenUtil.PlaceTile(ModContent.TileType<Tiles.DoomstoneB>(), currX - horizontalSlantOffset, j);
                }

                i++;
            }

            islands.Add(newIsland);

            return true;
        }

        private float ShapingFunction(float f) {
            f = (f - 0.5f) * 2; // from -1 to 1
            f = Math.Abs(f); // from 0 (center) to 1 (edges)
            f -= 0.5f;
            f = -8 * f * f * f + 1;
            f /= 2;
            f = (float)Math.Pow(f, 1.0);
            return f;
            //return (ShapingFunctionInner(f) - ShapingFunctionInner(1.0f)) / ShapingFunctionInner(0.0f);
        }
    }
}
