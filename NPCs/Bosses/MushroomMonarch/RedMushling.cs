using Terraria;
using Terraria.ModLoader;

using Terraria.ID;
using AAMod.Util;
using System;
using Microsoft.Xna.Framework;

namespace AAMod.NPCs.Bosses.MushroomMonarch
{
    public class RedMushling : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushling");
            Main.npcFrameCount[npc.type] = 7;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 10;
            npc.damage = 6;
            npc.defense = 5; 
            npc.knockBackResist = 1f;
            npc.value = Item.sellPrice(0, 0, 0, 0);
            npc.aiStyle = -1;
            npc.width = 30;
            npc.height = 44;
            npc.npcSlots = 0f;
            npc.lavaImmune = false;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.buffImmune[46] = true;
            npc.buffImmune[47] = true;
            npc.netAlways = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        private const int STANDING_FRAME = 0;
        private static readonly int[] ANIM_WALK = new int[] { 0, 1, 2, 3, 4, 5 };
        private const int JUMP_FRAME = 6;

        public override void AI()
        {
            Player player = Main.player[npc.target]; // makes it so you can reference the player the npc is targetting

            if (npc.Center.X < player.Center.X) {
                npc.spriteDirection = -1;
            } else {
                npc.spriteDirection = 1;
            }

            BaseAI.AIZombie(npc, ref npc.ai, false, false, -1, .09f, 2, 3, 5, 120, true, 10, 10, true);

            if (npc.velocity.Y == 0)
            {
                if (npc.velocity.X == 0) {
                    AnimationHelper.SetFrame(npc, STANDING_FRAME);
                } else {
                    AnimationHelper.UpdateAnimation(npc, ANIM_WALK, 15, (int)Math.Ceiling(Math.Abs(npc.velocity.X * 2)));
                }
            }
            else
            {
                AnimationHelper.SetFrame(npc, JUMP_FRAME);
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            bool isDead = npc.life <= 0;
            int dustType = ModContent.DustType<Dusts.MushDust>();
            for (int m = 0; m < (isDead ? 35 : 6); m++) {
                Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, default, isDead ? 2f : 1.5f);
            }
        }
    }
}


