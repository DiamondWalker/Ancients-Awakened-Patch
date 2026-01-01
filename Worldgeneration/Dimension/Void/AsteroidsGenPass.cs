using AAMod.Items.Blocks;
using AAMod.Items.Usable;
using AAMod.Tiles;
using AAMod.Tiles.Chests;
using AAMod.Tiles.Furniture.Doom;
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
using Terraria.ID;
using AAMod.Items.Throwing;
using AAMod.Items.Ranged.Ammo;
using AAMod.Items.Materials;
using AAMod.Items.Accessories;

namespace AAMod.Worldgeneration.Dimension.Void {
    public class AsteroidsGenPass : GenPass {
        public AsteroidsGenPass() : base("Void Asteroids", 1f) {
        }

        public override void Apply(GenerationProgress progress) {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildAsteroids");

            int chestsGenerated = 0;

            IChestLootComponent chestLootTable = new CombinationLootComponent(
                new SelectRandomLootComponent(
                    new SingleItemLootComponent(ModContent.ItemType<CodeMagnetOff>()),
                    new SingleItemLootComponent(ModContent.ItemType<RiftMirror>()),
                    new SingleItemLootComponent(ModContent.ItemType<Replicator>())
                ),
                new CombinationLootComponent(
                    new ChanceLootComponent(
                        0.5f,
                        new ItemStackLootComponent(ItemID.SuperHealingPotion, 3, 5)
                    ),
                    new ChanceLootComponent(
                        0.5f,
                        new SelectRandomLootComponent(
                            new ItemStackLootComponent(ItemID.LunarBar, 3, 5),
                            new ItemStackLootComponent(ModContent.ItemType<RadiumBar>(), 3, 5),
                            new ItemStackLootComponent(ModContent.ItemType<DarkMatter>(), 3, 5),
                            new ItemStackLootComponent(ModContent.ItemType<ApocalyptitePlate>(), 3, 5)
                        )
                    ),
                    new ChanceLootComponent(
                        0.5f,
                        new SelectRandomLootComponent(
                            new ItemStackLootComponent(ItemID.FragmentSolar, 5, 10),
                            new ItemStackLootComponent(ItemID.FragmentVortex, 5, 10),
                            new ItemStackLootComponent(ItemID.FragmentNebula, 5, 10),
                            new ItemStackLootComponent(ItemID.FragmentStardust, 5, 10)
                        )
                    ),
                    new ChanceLootComponent(
                        0.5f,
                        new SelectRandomLootComponent(
                            new ItemStackLootComponent(ModContent.ItemType<DarkmatterKunai>(), 50, 99),
                            new ItemStackLootComponent(ItemID.MoonlordArrow, 50, 99),
                            new ItemStackLootComponent(ModContent.ItemType<RadiumArrow>(), 50, 99),
                            new ItemStackLootComponent(ModContent.ItemType<DarkmatterArrow>(), 50, 99)
                        )
                    ),
                    new ChanceLootComponent(
                        0.8f,
                        new SelectRandomLootComponent(
                            // buff potions
                        )
                    ),
                    new ChanceLootComponent(
                        0.5f,
                        new ItemStackLootComponent(ItemID.PlatinumCoin, 1, 2)
                    )
                )
            );

            int asteroids = (int)((1.0 + 0.5 * AAWorld.GetWorldSize()) * 60);
            for (int i = 0; i < asteroids; i++) {
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

                    // treasure cache
                    if (Main.rand.Next(5) == 0) {
                        if (Main.rand.NextBool()) startX++;
                        if (Main.rand.NextBool()) startY++;
                        int minX = startX - 3;
                        int maxX = startX + 2;
                        int minY = startY - 3;
                        int maxY = startY + 3;
                        bool generate = true;
                        for (int x = minX - 2; x <= maxX + 2; x++) {
                            for (int y = minY - 2; y <= maxY + 2; y++) {
                                if (!WorldGenUtil.IsTileOfType<Tiles.DoomstoneB>(x, y)) {
                                    generate = false;
                                }
                            }
                        }

                        if (generate) {
                            for (int x = minX + 1; x <= maxX - 1 ; x++) {
                                for (int y = minY + 1; y <= maxY - 1; y++) {
                                    WorldGenUtil.DeleteTile(x, y);
                                    WorldGenUtil.PlaceWall<Walls.Bricks.DoomiteWall>(x, y);
                                }
                            }

                            for (int x = minX; x <= maxX; x++) {
                                WorldGenUtil.PlaceTile<Tiles.DoomitePlate>(x, maxY);
                                WorldGenUtil.PlaceTile<Tiles.DoomitePlate>(x, minY);
                            }
                            for (int y = minY + 1; y <= maxY - 1 ; y++) {
                                WorldGenUtil.PlaceTile<Tiles.DoomitePlate>(maxX, y);
                                WorldGenUtil.PlaceTile<Tiles.DoomitePlate>(minX, y);
                            }

                            WorldGenUtil.PlaceChest<DoomChest>(startX - 1, maxY - 1, chestLootTable);
                            chestsGenerated++;
                        }
                    }
                }
            }

            Main.NewText(chestsGenerated + " chests generated");
        }

        private bool OverlapsIslands(int x, int y) {
            foreach (Rectangle island in IslandsGenPass.islands) {
                if (island.Contains(x, y)) return true;
            }
            return false;
        }
    }
}
