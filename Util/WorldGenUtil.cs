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

        public static void PlaceChest(int type, int x, int y, IChestLootComponent lootTable) {
            int chestID = WorldGen.PlaceChest(x, y, (ushort)type, false, 0);
            if (chestID >= 0) {
                Chest chest = Main.chest[chestID];

                var list = lootTable.Provide();
                for (int i = 0; i < list.Count; i++) {
                    chest.item[i].SetDefaults(list[i].type);
                    chest.item[i].stack = list[i].stackSize;
                }
            }

            NetMessage.SendObjectPlacment(-1, x, y, (ushort)type, 0, 0, -1, -1);
        }

        public static void PlaceChest<T>(int x, int y, IChestLootComponent lootTable) where T : ModTile {
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

        public static void ReplaceTile(int x, int y, int typeToReplace, int replacementType) {
            if (IsTileOfType(typeToReplace, x, y)) {
                Main.tile[x, y].type = (ushort)replacementType;
                //NetMessage.SendObjectPlacment(-1, x, y, (ushort)replacementType, 0, 0, -1, -1);
            }
        }

        public static void ReplaceCircle(int x, int y, int radius, int typeToReplace, int replacementType, Rectangle bounds = default) {
            for (int x2 = x - radius; x2 <= x + radius; x2++) {
                for (int y2 = y - radius; y2 <= y + radius; y2++) {
                    int distX = x2 - x;
                    int distY = y2 - y;
                    if ((bounds == default || bounds.Contains(x2, y2)) && Math.Sqrt(distX * distX + distY * distY) <= radius) {
                        ReplaceTile(x2, y2, typeToReplace, replacementType);
                    }
                }
            }
        }
    }

    public interface IChestLootComponent {
        List<ChestLootEntry> Provide();
    }

    public class SingleItemLootComponent : IChestLootComponent {
        private readonly int type;

        public SingleItemLootComponent(int type) {
            this.type = type;
        }

        public List<ChestLootEntry> Provide() {
            return new List<ChestLootEntry> {
                new ChestLootEntry(type, 1)
            };
        }
    }

    public class ItemStackLootComponent : IChestLootComponent {
        private readonly int type;
        private readonly int minStack;
        private readonly int maxStack;

        public ItemStackLootComponent(int type, int minStack, int maxStack) {
            this.type=type;
            this.minStack=minStack;
            this.maxStack=maxStack;
        }

        public ItemStackLootComponent(int type, int stack) : this(type, stack, stack) {}

        public List<ChestLootEntry> Provide() {
            return new List<ChestLootEntry> {
                new ChestLootEntry(type, Main.rand.Next(minStack, maxStack + 1))
            };
        }
    }

    public class ShuffleLootComponent : IChestLootComponent {
        private readonly IChestLootComponent[] nested;

        public ShuffleLootComponent(params IChestLootComponent[] nested) {
            this.nested=nested;
        }

        public List<ChestLootEntry> Provide() {
            var pool = nested.ToList();
            var list = new List<ChestLootEntry>();
            while (pool.Any()) {
                int selected = Main.rand.Next(pool.Count);
                list.AddRange(pool[selected].Provide());
                pool.RemoveAt(selected);
            }
            return list;
        }
    }

    public class CombinationLootComponent : IChestLootComponent {
        private readonly IChestLootComponent[] nested;

        public CombinationLootComponent(params IChestLootComponent[] nested) {
            this.nested=nested;
        }

        public List<ChestLootEntry> Provide() {
            var list = nested[0].Provide();
            for (int i = 1; i < nested.Length; i++) {
                list.AddRange(nested[i].Provide());
            }
            return list;
        }
    }

    public class SelectRandomLootComponent : IChestLootComponent {
        private readonly IChestLootComponent[] nested;
        private readonly int draws;
        private readonly bool allowDuplicates;

        public SelectRandomLootComponent(int draws, bool allowDuplicates, params IChestLootComponent[] nested) {
            this.nested = nested;
            this.draws = draws;
            this.allowDuplicates = allowDuplicates;
        }

        public SelectRandomLootComponent(int draws, params IChestLootComponent[] nested) : this(draws, false, nested) {}

        public SelectRandomLootComponent(params IChestLootComponent[] nested) : this(1, nested) {}

        public List<ChestLootEntry> Provide() {
            return allowDuplicates ? ProvideWithDuplicates() : ProvideWithoutDuplicates();
        }

        private List<ChestLootEntry> ProvideWithDuplicates() {
            var list = new List<ChestLootEntry>();
            for (int i = 0; i < draws; i++) {
                list.AddRange(nested[Main.rand.Next(nested.Length)].Provide());
            }
            return list;
        }

        private List<ChestLootEntry> ProvideWithoutDuplicates() {
            var pool = nested.ToList();
            var list = new List<ChestLootEntry>();

            for (int i = 0; i < draws; i++) {
                if (pool.Count() <= 0) break;
                int selection = Main.rand.Next(pool.Count);
                list.AddRange(pool[selection].Provide());
                pool.RemoveAt(selection);
            }

            return list;
        }
    }

    public class ChanceLootComponent : IChestLootComponent {
        private readonly IChestLootComponent nested;
        private float chance;

        public ChanceLootComponent(float chance, IChestLootComponent nested) {
            if (chance > 1.0f || chance < 0.0f) throw new ArgumentOutOfRangeException("Change must be between 0 and 1, not " + chance + "!");
            this.chance = chance;
            this.nested = nested;
        }

        public List<ChestLootEntry> Provide() {
            return Main.rand.NextFloat() < chance ? nested.Provide() : new List<ChestLootEntry>();
        }
    }

    public class ChestLootEntry : IChestLootComponent {
        public readonly int type;
        public readonly int stackSize;

        public ChestLootEntry(int type, int stackSize) {
            this.type = type;
            this.stackSize = stackSize;
        }

        public List<ChestLootEntry> Provide() {
            return new List<ChestLootEntry> {
                this
            };
        }
    }
}
