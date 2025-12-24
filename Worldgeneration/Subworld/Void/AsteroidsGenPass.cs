using AAMod.Items.Blocks;
using AAMod.Tiles;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Subworld.Void {
    public class AsteroidsGenPass : GenPass {
        public AsteroidsGenPass() : base("Void Asteroids", 1f) {
        }

        public override void Apply(GenerationProgress progress) {
            for (int i = 0; i < 60; i++) {
                int x = Main.rand.Next(Main.maxTilesX);
                int y = Main.rand.Next(Main.maxTilesY);
                WorldGen.TileRunner(x, y, Main.rand.Next(90) + 10, 10, ModContent.TileType<Tiles.Doomstone>(), true, 0, 0);
                SLWorld.drawUnderworldBackground = false;
            }
        }
    }
}
