using AAMod.NPCs.TownNPCs;
using AAMod.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Enemies.Void {
    public class Null : ModNPC
	{
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Null");
            Main.npcFrameCount[npc.type] = 10;
        }
		
		public override void SetDefaults()
		{
            //npc.CloneDefaults(NPCID.Poltergeist);
            npc.noTileCollide = true;
			npc.aiStyle = -1;
            npc.width = 24;
            npc.height = 40;
            npc.damage = 85;
            npc.defense = 9999999;
            npc.lifeMax = 20;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.alpha = 255;
            npc.dontTakeDamage = true;
            npc.value = 7000f;
            npc.knockBackResist = 0.0f;
            npc.noGravity = true;
            banner = npc.type;
			bannerItem = mod.ItemType("NullBanner");
        }

        public int staticCount { get => (int)npc.ai[0]; set => npc.ai[0] = value; }
		public override void PostAI()
		{
            if (staticCount > 0) {
                int currFrame = AnimationHelper.GetCurrentFrame(npc);
                int newFrame;
                do {
                    newFrame = RandUtil.InclusiveRand(1, 10);
                } while (newFrame == currFrame);
                AnimationHelper.SetFrame(npc, newFrame);
            } else {
                AnimationHelper.SetFrame(npc, 0);
            }
			npc.spriteDirection = npc.velocity.X > 0 ? -1 : 1;
			npc.rotation = npc.velocity.X * 0.25f;
		}

        public override void AI()
        {
            if (IsFadingInOrTeleporting) { // either this is fading in or teleporting
                if (npc.alpha > 0 && npc.HasValidTarget) npc.alpha = Math.Max(0, npc.alpha - 3);
                if (staticCount > 0) staticCount--;

                if (IsFadingInOrTeleporting) {
                    npc.dontTakeDamage = true;
                    npc.damage = 0;
                } else {
                    npc.dontTakeDamage = false;
                    npc.damage = npc.defDamage;
                }
            }

            npc.TargetClosest(false);
            if (npc.HasValidTarget) {
                Player target = Main.player[npc.target];
                if (target.Center.X > npc.Center.X) {
                    npc.velocity.X += 0.13f;
                } else {
                    npc.velocity.X -= 0.13f;
                }
                if (target.Center.Y > npc.Center.Y) {
                    npc.velocity.Y += 0.08f;
                } else {
                    npc.velocity.Y -= 0.08f;
                }

                npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -2, 2);
                npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -1.1f, 1.1f);

                if (Main.netMode != 1) {
                    float dist = npc.Distance(target.Center);

                    if (staticCount == 15) {
                        int prevDir = MathUtil.Signum(npc.Center.X - target.Center.X);

                        npc.Center = target.Center + RandUtil.RandomPolar(Math.Min(dist, 300));

                        if (MathUtil.Signum(npc.Center.X - target.Center.X) != prevDir) {
                            npc.velocity.X = -npc.velocity.X;
                        }

                        npc.netUpdate = true;
                    } else if (RandUtil.Chance(300)) {
                        Teleport();
                    }
                }
            } else {
                npc.alpha += 1;
                if (npc.alpha > 255) npc.active = false;
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            if (Main.netMode != 1 && RandUtil.Chance(5)) {
                Teleport();
            }
            base.HitEffect(hitDirection, damage);
        }

        private void Teleport() {
            if (IsFadingInOrTeleporting) return;

            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Glitch"), npc.Center);
            staticCount = 30;
            npc.netUpdate = true;
        }

        private bool IsFadingInOrTeleporting { get => staticCount > 0 || npc.alpha > 0; }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) {
            if (staticCount > 0) {
                BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc, new Color(1.0f, 1.0f, 1.0f, MathHelper.Clamp(255 - npc.alpha, 0.0f, 255.0f) / 255), false);
                return false;
            }
            return base.PreDraw(spriteBatch, drawColor);
        }

        public override void NPCLoot()
        {
            int amount = RandUtil.InclusiveRand(0, 3);
            if (amount > 0) Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("VoidEnergy"), amount);

            if (Main.rand.Next(1000) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ono"), 1);
            }
        }
    }
}