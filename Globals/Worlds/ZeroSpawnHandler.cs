using AAMod.Globals.Worlds;
using AAMod.NPCs.Bosses.Zero;
using AAMod.Worldgeneration.Dimension.Void;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AAMod.Globals.World {
    public class ZeroSpawnHandler : ModWorld {
        public static int ZX = -1;
        public static int ZY = -1;
        public static int Shield;

        public override void Initialize() {
            ZX = -1;
            ZY = -1;
        }

        public override TagCompound Save() {
            var tag = new TagCompound();
            if (ZX != -1) {
                tag.Add("ZX", ZX);
                tag.Add("ZY", ZY);
            }
            return tag;
        }

        public override void Load(TagCompound tag) {
            Reset(); //reset it so it doesn't fuck up between world loads	
            if (tag.ContainsKey("ZX")) {
                ZX = tag.GetInt("ZX");
                ZY = tag.GetInt("ZY");
                if (!AAWorld.downedZero && VoidSubworld.IsInside())
                    NPC.NewNPC(ZX, ZY, mod.NPCType("ZeroDeactivated"));
            }
        }

        public override void PostUpdate() {
            if (Main.netMode != 1 && !AAWorld.downedZero && VoidSubworld.IsInside()) {
                SpawnDeactivatedZero();
            }
        }

        public void Reset() {
            ZX = -1;
            ZY = -1;
        }

        public void SpawnDeactivatedZero() {
            int VoidHeight = 140;

            Point spawnTilePos = new Point((Main.maxTilesX / 15 * 14) + (Main.maxTilesX / 15 / 2) - 100, VoidHeight);
            Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);
            bool anyZerosExist = NPC.AnyNPCs(mod.NPCType("ZeroDeactivated")) || NPC.AnyNPCs(mod.NPCType("Zero")) || NPC.AnyNPCs(mod.NPCType("ZeroProtocol"));
            if (!anyZerosExist) {
                int whoAmI = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<ZeroDeactivated>());
                ZX = (int)spawnPos.X;
                ZY = (int)spawnPos.Y;
                if (Main.netMode == 2 && whoAmI != -1 && whoAmI < 200) {
                    NetMessage.SendData(MessageID.SyncNPC, number: whoAmI);
                }
            }
        }
    }
}
