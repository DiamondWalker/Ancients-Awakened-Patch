using AAMod.Globals.Worlds;
using AAMod.NPCs.Other.VoidRift;
using AAMod.Worldgeneration.Dimension.Void;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AAMod.Globals.World {
    public class RiftSpawnHandler : ModWorld {
        public static int RiftX = -1;
        public static int RiftY = -1;

        public override void Initialize() {
            RiftX = -1;
            RiftY = -1;
        }

        public override TagCompound Save() {
            var tag = new TagCompound();
            if (RiftX != -1) {
                tag.Add("RiftX", RiftX);
                tag.Add("RiftY", RiftY);
            }
            return tag;
        }

        public override void Load(TagCompound tag) {
            Reset(); //reset it so it doesn't fuck up between world loads	
            if (tag.ContainsKey("RiftX")) {
                RiftX = tag.GetInt("RiftX");
                RiftY = tag.GetInt("RiftY");
                if (AAWorld.downedEquinox && !VoidSubworld.IsInside())
                    NPC.NewNPC(RiftX, RiftY, mod.NPCType("VoidRift"));
            }
        }

        public override void PostUpdate() {
            if (Main.netMode != 1) {
                if (VoidSubworld.IsInside()) {
                    Point spawnTilePos = new Point(Main.spawnTileX, Main.spawnTileY - 5);
                    Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);
                    bool anyRiftsExist = NPC.AnyNPCs(mod.NPCType("VoidRift"));
                    if (!anyRiftsExist) {
                        int whoAmI = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<VoidRift>());
                        RiftX = (int)spawnPos.X;
                        RiftY = (int)spawnPos.Y;
                        if (Main.netMode == 2 && whoAmI != -1 && whoAmI < 200) {
                            NetMessage.SendData(MessageID.SyncNPC, number: whoAmI);
                        }
                    }
                } else {
                    int VoidHeight = 140;

                    Point spawnTilePos = new Point((Main.maxTilesX / 15 * 14) + (Main.maxTilesX / 15 / 2) - 100, VoidHeight);
                    Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);
                    bool anyRiftsExist = NPC.AnyNPCs(mod.NPCType("VoidRift"));
                    if (!anyRiftsExist) {
                        int whoAmI = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<VoidRift>());
                        RiftX = (int)spawnPos.X;
                        RiftY = (int)spawnPos.Y;
                        if (Main.netMode == 2 && whoAmI != -1 && whoAmI < 200) {
                            NetMessage.SendData(MessageID.SyncNPC, number: whoAmI);
                        }
                    }
                }
            }
        }

        public void Reset() {
            RiftX = -1;
            RiftY = -1;
        }
    }
}
