using AAMod.Globals.Players;
using AAMod.Items.Usable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Enemies.Mushroom {
    public class MushroomCrab : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushroom Crab");
            Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 34;
            npc.aiStyle = 3;
            npc.damage = 16;
            npc.defense = 20;
            npc.lifeMax = 140;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            animationType = NPCID.AnomuraFungus;
            npc.knockBackResist = 0.3f;
            npc.value = 1300f;
            npc.buffImmune[31] = false;
            npc.npcSlots = 0.3f;
            banner = npc.type;
			bannerItem = mod.ItemType("MushroomCrabBanner");
        }

        public override void AI()
        {
            AAAI.InfernoFighterAI(npc, ref npc.ai, true, false, -1, 0.13f, 3f, 3, 4, 60, true, 10, 60, true, null, false);
        }

        public override void HitEffect(int hitDirection, double damage) {
            bool isDead = npc.life <= 0;
            int dustType = ModContent.DustType<Dusts.MushDust>();
            for (int m = 0; m < (isDead ? 35 : 6); m++) {
                Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, isDead ? 2f : 1.5f);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.player.GetModPlayer<AAPlayer>().ZoneMush ? .4f : 0f;
        }

        public override void NPCLoot()
		{
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Mushroom);

            if (Main.rand.Next(50) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MegaMush>());
        }
	}
}