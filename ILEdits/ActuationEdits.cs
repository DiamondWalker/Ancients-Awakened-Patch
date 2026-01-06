using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.ILEdits {
    public class ActuationEdits {
        public static void ApplyEdits() {
            On.Terraria.Wiring.ActuateForced += Wiring_ActuateForced;
            On.Terraria.Wiring.Actuate += Actuate;
        }

        private static void Wiring_ActuateForced(On.Terraria.Wiring.orig_ActuateForced orig, int i, int j) {
            Tile tile = Main.tile[i, j];
            if (tile.type == ModContent.TileType<Tiles.AcropolisBlock2>() || tile.type == ModContent.TileType<Tiles.AcropolisBlock>() ||
                tile.type == ModContent.TileType<Tiles.GreedStone>() || tile.type == ModContent.TileType<Tiles.GreedBrick>()) {
                return;
            }
            orig(i, j);
        }

        private static bool Actuate(On.Terraria.Wiring.orig_Actuate orig, int i, int j) {
            Tile tile = Main.tile[i, j];
            if (tile.type == ModContent.TileType<Tiles.AcropolisBlock2>() || tile.type == ModContent.TileType<Tiles.AcropolisBlock>() ||
                tile.type == ModContent.TileType<Tiles.GreedStone>() || tile.type == ModContent.TileType<Tiles.GreedBrick>()) {
                return false;
            }
            return orig(i, j);
        }
    }
}
