using AAMod.Util;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Dimension.Void {
    public class RemoveSpareRocksGenPass : GenPass {
        public RemoveSpareRocksGenPass() : base("Remove Spare Rocks", 1f){ }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildSpareRocks");

            HashSet<long> visitedTiles = new HashSet<long>();
            for (int x = 0; x < Main.maxTilesX; x++) {
                for (int y = 0; y < Main.maxTilesY; y++) {
                    if (WorldGenUtil.TileAt(x, y) && !visitedTiles.Contains(BitUtil.CombineInts(x, y))) {
                        HashSet<long> chunk = new HashSet<long>();
                        GatherTilesInChunk(x, y, chunk);

                        if (chunk.Count >= 20) {
                            visitedTiles.Concat(chunk);
                        } else {
                            foreach (long tile in chunk) {
                                WorldGenUtil.DeleteTile(BitUtil.GetLeftInt(tile), BitUtil.GetRightInt(tile));
                            }
                        }
                    }
                }
            }
        }

        private void GatherTilesInChunk(int x, int y, HashSet<long> chunk) {
            if (chunk.Count >= 20) return;
            if (!WorldGenUtil.TileAt(x, y)) return;
            if (!chunk.Add(BitUtil.CombineInts(x, y))) return;
            GatherTilesInChunk(x + 1, y, chunk);
            GatherTilesInChunk(x - 1, y, chunk);
            GatherTilesInChunk(x, y + 1, chunk);
            GatherTilesInChunk(x, y - 1, chunk);
        }
    }
}
