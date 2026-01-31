using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.World.Generation;
using AAMod.Util;
using Microsoft.Xna.Framework;
using AAMod.Items.Materials;
using AAMod.Tiles;
using AAMod.Tiles.Crafters;
using AAMod.Worldgeneration.Dimension.Void.Passes.Islands;

namespace AAMod.Worldgeneration.Dimension.Void.Passes.Structures
{
    public class PodiumsGenPass : GenPass
    {

        public PodiumsGenPass() : base("Podiums", 1f)
        {

        }

        public override void Apply(GenerationProgress progress)
        {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildPodiums");

            int placed = 0;
            int count = RandUtil.InclusiveRand(1, IslandsGenPass.islands.Count);
            int fails = 0;
            while (placed < count)
            {
                if (fails++ > 50_000)
                {
                    AAMod.instance.Logger.Warn("Could only place " + placed + " out of " + count + " void podiums!");
                    break;
                }

                // pick random location to try and find an island
                int x = Main.rand.Next(Main.maxTilesX);
                int y = Main.rand.Next(Main.maxTilesY);
                if (WorldGenUtil.TileAt(x, y))
                {
                    while (WorldGenUtil.TileAt(x, y)) y--; // we're inside an island so now move up until we're at the surface

                    y -= 3; // shift it up a bit more so it's above the ground

                    // scan horizontally to make sure we aren't in a hole or something idk
                    bool validLocation = true;
                    for (int xOff = -50; xOff <= 50; xOff++)
                    {
                        if (WorldGenUtil.TileAt(x + xOff, y))
                        {
                            validLocation = false;
                            break;
                        }

                        // while we're at it also scan vertically to make sure we aren't near another podium
                        for (int yOff = -50; yOff <= 50; yOff++)
                        {
                            if (WorldGenUtil.IsTileOfType<BinaryReassembler>(x + xOff, y + yOff))
                            {
                                validLocation = false;
                            }
                        }
                    }

                    if (validLocation)
                    {
                        for (int yOff = 0; yOff < 5; yOff++)
                        {
                            bool atBottom = true;
                            for (int xOff = -yOff - 2; xOff <= yOff + 2; xOff++)
                            {
                                if (!WorldGenUtil.TileAt(x + xOff, y + yOff))
                                {
                                    atBottom = false;
                                }
                            }

                            if (atBottom)
                            {
                                // we've determined the depth. Start placing doomite scrap
                                for (int yOff2 = 0; yOff2 <= yOff + 2; yOff2++)
                                {
                                    for (int xOff = -yOff2 - 3; xOff <= yOff2 + 3; xOff++)
                                    {
                                        WorldGenUtil.PlaceTile<DoomitePlate>(x + xOff, y + yOff2);
                                    }

                                    if (!WorldGenUtil.TileAt(x - yOff2 - 4, y + yOff2) || !WorldGenUtil.TileAt(x - yOff2 - 5, y + yOff2))
                                    {
                                        WorldGenUtil.PlaceTile<DoomitePlate>(x - yOff2 - 4, y + yOff2);

                                        WorldGenUtil.SetSlope(TileSlope.DownRight, x - yOff2 - 4, y + yOff2);
                                    }

                                    if (!WorldGenUtil.TileAt(x + yOff2 + 4, y + yOff2) || !WorldGenUtil.TileAt(x + yOff2 + 5, y + yOff2))
                                    {
                                        WorldGenUtil.PlaceTile<DoomitePlate>(x + yOff2 + 4, y + yOff2);

                                        WorldGenUtil.SetSlope(TileSlope.DownLeft, x + yOff2 + 4, y + yOff2);
                                    }
                                }

                                SetTopDesignThingTile(x - 3, y - 1);
                                SetTopDesignThingTile(x + 3, y - 1);
                                SetTopDesignThingTile(x - 3, y - 2);
                                SetTopDesignThingTile(x + 3, y - 2);
                                SetTopDesignThingTile(x - 3, y - 3);
                                SetTopDesignThingTile(x + 3, y - 3);

                                SetTopDesignThingTile(x - 3, y - 4, TileSlope.DownRight);
                                SetTopDesignThingTile(x + 3, y - 4, TileSlope.DownLeft);

                                SetTopDesignThingTile(x - 2, y - 4, TileSlope.UpLeft);
                                SetTopDesignThingTile(x + 2, y - 4, TileSlope.UpRight);

                                SetTopDesignThingTile(x - 2, y - 5, TileSlope.DownRight);
                                SetTopDesignThingTile(x + 2, y - 5, TileSlope.DownLeft);

                                //SetTopDesignThingTile(x - 1, y - 4, TileSlope.UpRight);
                                //SetTopDesignThingTile(x + 1, y - 4, TileSlope.UpLeft);
                                WorldGenUtil.PlaceFurniture<BinaryReassembler>(x, y - 1);
                                placed++;
                                fails = 0;

                                break;
                            }
                        }
                    }
                }
            }
        }

        private void SetTopDesignThingTile(int x, int y, TileSlope slope = TileSlope.Full)
        {
            WorldGenUtil.PlaceTile<DoomitePlate>(x, y);
            WorldGenUtil.SetSlope(slope, x, y);
            WorldGenUtil.Actuate(x, y);
        }
    }
}
