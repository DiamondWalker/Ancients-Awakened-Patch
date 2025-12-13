using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using AAMod.Items.Usable;

namespace AAMod.NPCs.Enemies.Mushroom
{
    public class ShroomJelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mushroom Jelly");
            Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.FungoFish);
            animationType = NPCID.FungoFish;
            npc.noGravity = true;
            npc.width = 26;
            npc.height = 26;
            npc.aiStyle = 18;
            npc.damage = 20;
            npc.defense = 20;
            npc.lifeMax = 70;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 1000f;
            npc.alpha = 20;
            npc.npcSlots = 0.3f;
            banner = npc.type;
			bannerItem = mod.ItemType("ShroomJellyBanner");
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
            return spawnInfo.player.GetModPlayer<AAPlayer>().ZoneMush && spawnInfo.water ? .7f : 0f;
        }

        public override void NPCLoot()
		{
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Mushroom);

            if (Main.rand.Next(50) == 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MegaMush>());
        }
	}
}