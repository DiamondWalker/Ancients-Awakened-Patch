using AAMod.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Equinox {
    [AutoloadBossHead]		
	public class NightcrawlerHead : DaybringerHead
	{
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Nightcrawler");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override bool Empowered => !Main.dayTime;

        public override int[] SegmentTypes => new int[] { mod.NPCType("NightcrawlerHead"), mod.NPCType("NightcrawlerBody"), mod.NPCType("NightcrawlerTail") };

        public override int AIUpdates { get => 4; }

        public override int WormDustType => ModContent.DustType<NightcrawlerDust>();

        public override int WormLength { get => 24; }

        public override float EmpoweredMoveSpeed { get => 12f; }

        public override int EmpoweredDefense { get => 150; }

        public override void SpawnGore() {
            bool isHead = npc.type == mod.NPCType("NightcrawlerHead");
            bool isBody = npc.type == mod.NPCType("NightcrawlerBody");
            if (isHead) {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore2"), 1f);
            } else if (isBody) {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore3"), 1f);
            } else {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/NCGore4"), 1f);
            }
        }
    }
}