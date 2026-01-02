using AAMod.Util;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Dimension.Void {
    public class CanyonsGenPass : GenPass {
        public CanyonsGenPass() : base("Canyons", 1f){ }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildCanyons");

            foreach (Rectangle island in IslandsGenPass.islands) {
                for (int x = island.Left; x < island.Right; x++) {
                    if (Main.rand.Next(800) == 0) {
                        StartCanyon(x, island.Top, island);
                    }
                }
            }
        }

        private void StartCanyon(int startX, int startY, Rectangle bounds) {
            double angle = 1.5 * Math.PI + Main.rand.NextDouble() * 0.5 - 0.25;
            float radius = Main.rand.Next(5, 20);
            int x = startX;
            int y = startY;
            Vector2 pos = new Vector2(x, y);

            while (bounds.Contains(x, y)) {
                angle += Main.rand.NextDouble() * 0.04 - 0.02;
                radius += Main.rand.NextFloat(-0.2f, 0.2f);

                if (radius <= 0) break;

                pos += new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle));
                x = (int)Math.Round(pos.X);
                y = (int)Math.Round(pos.Y);

                //WorldGenUtil.ReplaceCircle(x, y, (int)Math.Round(radius) + 1 + Main.rand.Next(3), ModContent.TileType<Doomstone>(), ModContent.TileType<Apocalyptite>(), bounds);
                WorldGenUtil.ClearCircle(x, y, (int)Math.Round(radius), bounds);
            }
        }
    }
}
