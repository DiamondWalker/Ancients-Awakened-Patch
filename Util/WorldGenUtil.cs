using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Util {
    public class WorldGenUtil {
        public static bool TileAt(int x, int y) {
            return WorldGen.InWorld(x, y) && Main.tile[x, y] != null && Main.tile[x, y].active();
        }

        public static bool WallAt(int x, int y) {
            return WorldGen.InWorld(x, y) && Main.tile[x, y] != null && Main.tile[x, y].wall != WallID.None;
        }

        public static void PlaceTile(int type, int x, int y) {
            if (!WorldGen.InWorld(x, y)) return;
            if (Main.tile[x, y] == null) Main.tile[x, y] = new Tile();
            Main.tile[x, y].type = (ushort)type;
            Main.tile[x, y].active(true);
        }

        public static void PlaceTile<T>(int x, int y) where T : ModTile {
            PlaceTile(ModContent.TileType<T>(), x, y);
        }

        public static void PlaceWall(int type, int x, int y) {
            if (!WorldGen.InWorld(x, y)) return;
            if (Main.tile[x, y] == null) Main.tile[x, y] = new Tile();
            Main.tile[x, y].wall = (ushort)type;
        }

        public static void PlaceWall<T>(int x, int y) where T : ModWall {
            PlaceWall(ModContent.WallType<T>(), x, y);
        }

        public static void PlaceChest(int type, int x, int y, ChestLootTable lootTable) {
            int chestID = WorldGen.PlaceChest(x, y, (ushort)type, false, 0);
            if (chestID >= 0) {
                Chest chest = Main.chest[chestID];

                int index = 0;
                foreach (ChestLootEntry[] entries in lootTable.LootEntries) {
                    ChestLootEntry entry = entries[Main.rand.Next(entries.Length)];
                    chest.item[index].SetDefaults(entry.type);
                    if (entry.minStack == entry.maxStack) {
                        chest.item[index].stack = entry.minStack;
                    } else {
                        chest.item[index].stack = Main.rand.Next(entry.minStack, entry.maxStack + 1);
                    }
                    index++;
                }
            }
        }

        public static void PlaceChest<T>(int x, int y, ChestLootTable lootTable) where T : ModTile {
            PlaceChest(ModContent.TileType<T>(), x, y, lootTable);
        }

        public static void DeleteTile(int x, int y) {
            if (!WorldGen.InWorld(x, y) || Main.tile[x, y] == null) return;
            Main.tile[x, y].type = 0;
            Main.tile[x, y].active(false);
        }

        public static void PlaceCircle(int type, int x, int y, int radius) {
            for (int x2 = x - radius; x2 <= x + radius; x2++) {
                for (int y2 = y - radius; y2 <= y + radius; y2++) {
                    int distX = x2 - x;
                    int distY = y2 - y;
                    if (Math.Sqrt(distX * distX + distY * distY) <= radius) {
                        PlaceTile(type, x2, y2);
                    }
                }
            }
        }

        public static void PlaceCircle<T>(int x, int y, int radius) where T : ModTile {
            PlaceCircle(ModContent.TileType<T>(), x, y, radius);
        }

        public static bool IsTileOfType(int type, int x, int y) {
            return TileAt(x, y) && Main.tile[x, y].type == type;
        }

        public static bool IsTileOfType<T>(int x, int y) where T : ModTile {
            return IsTileOfType(ModContent.TileType<T>(), x, y);
        }

        public static bool IsWallOfType(int type, int x, int y) {
            return TileAt(x, y) && Main.tile[x, y].wall == type;
        }

        public static bool IsWallOfType<T>(int x, int y) where T : ModWall {
            return IsWallOfType(ModContent.WallType<T>(), x, y);
        }

        public static void ClearCircle(int x, int y, int radius, Rectangle bounds = default) {
            for (int x2 = x - radius; x2 <= x + radius; x2++) {
                for (int y2 = y - radius; y2 <= y + radius; y2++) {
                    int distX = x2 - x;
                    int distY = y2 - y;
                    if ((bounds == default || bounds.Contains(x2, y2)) && Math.Sqrt(distX * distX + distY * distY) <= radius && TileAt(x2, y2)) {
                        DeleteTile(x2, y2);
                    }
                }
            }
        }
    }

    public class ChestLootTable {
        private List<ChestLootEntry[]> _lootEntries = new List<ChestLootEntry[]>();

        public IEnumerable<ChestLootEntry[]> LootEntries {
            get {
                return _lootEntries.AsReadOnly();
            }
        }



        public ChestLootTable AddEntry(ChestLootEntry entry) {
            _lootEntries.Add(new ChestLootEntry[] { entry });
            return this;
        }

        public ChestLootTable AddRandomEntry(ChestLootEntry[] entries) {
            _lootEntries.Add(entries);
            return this;
        }
    }

    public class ChestLootEntry {
        public readonly int type;
        public readonly int minStack;
        public readonly int maxStack;

        private ChestLootEntry(int type, int min, int max) {
            this.type = type;
            this.minStack = min;
            this.maxStack = max;
        }
        public static ChestLootEntry Create(int type, int minStack, int maxStack) {
            return new ChestLootEntry(type, minStack, maxStack);
        }

        public static ChestLootEntry Create(int type, int stack) {
            return new ChestLootEntry(type, stack, stack);
        }

        public static ChestLootEntry Create(int type) {
            return Create(type, 1);
        }

        public static ChestLootEntry Create<T>(int minStack, int maxStack) where T : ModItem {
            return Create(ModContent.ItemType<T>(), minStack, maxStack);
        }

        public static ChestLootEntry Create<T>(int stack) where T : ModItem {
            return Create(ModContent.ItemType<T>(), stack);
        }

        public static ChestLootEntry Create<T>() where T : ModItem {
            return Create(ModContent.ItemType<T>());
        }

    }
}
