using AAMod.Dusts;
using AAMod.Globals.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Equinox {
    [AutoloadBossHead]
    public class DaybringerHead : EquinoxHead {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Daybringer");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override bool Empowered { get => Main.dayTime; }
        public override int[] SegmentTypes { get => new int[] { mod.NPCType("DaybringerHead"), mod.NPCType("DaybringerBody"), mod.NPCType("DaybringerTail") }; }
        public override int AIUpdates { get => 6; }
        public override int WormDustType { get => ModContent.DustType<DaybringerDust>(); }
        public override int WormLength { get => 30; }
        public override float EmpoweredMoveSpeed { get => 15f; }
        public override int EmpoweredDefense { get => 120; }
        public override void SpawnGore() {
            bool isHead = npc.type == mod.NPCType("DaybringerHead");
            bool isBody = npc.type == mod.NPCType("DaybringerBody");
            if (isHead) {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore2"), 1f);
            } else
            if (isBody) {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore3"), 1f);
            } else {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore4"), 1f);
            }
        }
        // everything above this point is daybring exclusive
    }   
}