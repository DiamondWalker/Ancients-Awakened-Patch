using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod.Dusts;
using AAMod.NPCs.Bosses.Zero;
using System;
using Terraria.Graphics.Shaders;
using AAMod.Util;

namespace AAMod.NPCs.Enemies.Void
{
    public class Scout : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Scout");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
            npc.width = 38;
            npc.height = 38;
            npc.value = 0;
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 4000;
            npc.defense = 40;
            npc.damage = 120;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
            npc.knockBackResist = 0.3f;
			npc.noGravity = true;
			//npc.noTileCollide = true;
			banner = npc.type;
			bannerItem = mod.ItemType("VoidScoutBanner");
		}

		public override void HitEffect(int hitDirection, double damage)
		{		
			bool isDead = npc.life <= 0;
			for (int m = 0; m < (isDead ? 25 : 5); m++)
			{
				int dustType = ModContent.DustType<VoidDust>();
				Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, isDead ? 2f : 1.1f);
			}
		}

        Vector2 playerPos = Vector2.Zero;
        Projectile beam = null;
		private bool BeamFiring { get => npc.ai[0] != 0; set => npc.ai[0] = value ? 1 : 0; }
        private int LaserTime { get => (int)npc.ai[1]; set => npc.ai[1] = value; }
        private float RotationSpeed { get => npc.ai[2]; set => npc.ai[2] = value; }
        public override void AI()
		{
            if (BeamFiring != (beam != null)) {
                BeamFiring = beam != null;
                npc.netUpdate = true;
            }

            bool slowRot = false;
            if (BeamFiring) {
                LaserTime++;
                if (Main.netMode != 1) {
                    if (LaserTime > 150) {
                        beam.Kill();
                        beam = null;
                        LaserTime = 0;
                        return;
                    }
                }

                slowRot = true;
            }
            // standard targetting code
            npc.TargetClosest(true);
            
            if (npc.HasValidTarget) {
                Player player = Main.player[npc.target];

                if (!BeamFiring) {
                    if (LaserTime < 150) {
                        if (Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height)) LaserTime++;
                        Vector2 moveVec = player.Center - npc.Center;
                        moveVec.Normalize();
                        moveVec *= 0.1f;
                        npc.velocity += moveVec;
                    } else {
                        LaserTime++;
                        npc.velocity *= 0.95f;

                        Vector2 pos = npc.Center + new Vector2((float)Math.Cos(npc.rotation), (float)Math.Sin(npc.rotation)) * 20;
                        int size = 40;
                        for (int i = 0; i < 3; i++) {
                            int num86 = Dust.NewDust(pos, size, size, 226, 0f, 0f, 100, default, 1.0f);
                            Main.dust[num86].shader = GameShaders.Armor.GetSecondaryShader(59, Main.LocalPlayer);
                            Main.dust[num86].position = pos;
                            Main.dust[num86].noGravity = true;
                        }

                        if (LaserTime >= 250) {
                            if (Main.netMode != 1) {
                                beam = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<NovaRay>(), (int)(npc.damage * 0.25f), 3f, Main.myPlayer, npc.whoAmI, 420)];
                                if (beam.modProjectile is NovaRay ray) {
                                    ray.MoveDistance = 35.0f;
                                    LaserTime = 0;
                                    npc.netUpdate = true;
                                } else {
                                    beam = null;
                                }
                            }
                        }

                        slowRot = true;
                    }
                }

                if (npc.collideX) npc.velocity.X = -0.8f * npc.oldVelocity.X;
                if (npc.collideY) npc.velocity.Y = -0.8f * npc.oldVelocity.Y;

                playerPos = player.Center;
            }

            float angleToPlayer = (float)Math.Atan2(playerPos.Y - npc.Center.Y, playerPos.X - npc.Center.X);
            float relativeAngle = MathUtil.GetRelativeAngle(angleToPlayer, npc.rotation);

            if (slowRot) {
                float rotationAcc = MathUtil.Signum(relativeAngle) * 0.001f;
                RotationSpeed += rotationAcc;
                RotationSpeed = MathHelper.Clamp(RotationSpeed, -0.02f, 0.02f);
            } else {
                RotationSpeed = MathUtil.Signum(relativeAngle) * 0.1f;
                if (Math.Abs(RotationSpeed) > Math.Abs(relativeAngle)) {
                    RotationSpeed = (RotationSpeed / Math.Abs(RotationSpeed)) * Math.Abs(relativeAngle);
                }
            }

            npc.rotation += RotationSpeed;
        }

		public override void FindFrame(int frameHeight)
		{
			if (npc.frameCounter++ > 5)
			{
				npc.frameCounter = 0;
				npc.frame.Y += frameHeight;
				if (npc.frame.Y > frameHeight * 3)
				{
					npc.frame.Y = 0;
				}
			}
		}

        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Apocalyptite"));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D GlowTex = mod.GetTexture("Glowmasks/Scout_Glow");

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 4, npc.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, GlowTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 4, npc.frame, AAColor.ZeroShield, true);
            return false;
        }
    }
}