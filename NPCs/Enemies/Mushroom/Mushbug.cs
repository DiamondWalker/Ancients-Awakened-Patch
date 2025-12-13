using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using AAMod.Items.Usable;

namespace AAMod.NPCs.Enemies.Mushroom
{
    public class Mushbug : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushbug");
            Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.MushiLadybug);
            npc.width = 30;
            npc.height = 24;
            npc.aiStyle = 3;
            npc.damage = 10;
            npc.defense = 9;
            npc.lifeMax = 100;
            npc.HitSound = SoundID.NPCHit45;
            npc.DeathSound = SoundID.NPCDeath47;
            npc.knockBackResist = 0.3f;
            animationType = NPCID.MushiLadybug;
            npc.value = 1000f;
            npc.buffImmune[31] = false;
            npc.npcSlots = 0.3f;
            banner = npc.type;
			bannerItem = mod.ItemType("MushbugBanner");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            return spawnInfo.player.GetModPlayer<AAPlayer>().ZoneMush ? 1f : 0f;
        }

        public override void HitEffect(int hitDirection, double damage) {
            bool isDead = npc.life <= 0;
            int dustType = ModContent.DustType<Dusts.MushDust>();
            for (int m = 0; m < (isDead ? 35 : 6); m++) {
                Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, isDead ? 2f : 1.5f);
            }
        }

        public override void NPCLoot()
		{
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Mushroom);

            if (Main.rand.Next(50) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MegaMush>());
        }
	}
}