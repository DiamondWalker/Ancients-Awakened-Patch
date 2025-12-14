using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using AAMod.Dusts;
using AAMod.NPCs.Bosses.Zero;
using System;

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
            npc.lifeMax = 1200;
            npc.defense = 40;
            npc.damage = 80;
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

        Projectile beam = null;
		private bool IsMoving { get => npc.ai[2] != 0; set => npc.ai[2] = value ? 1 : 0; }
        private int LaserTime { get => (int)npc.ai[3]; set => npc.ai[3] = value; }
        public override void AI()
		{
            // standard targetting code
            npc.TargetClosest(true);
            if (!npc.HasValidTarget) {
                return;
            }
            Player player = Main.player[npc.target];

			if (beam != null) {
                if (Main.netMode != 1) {
                    LaserTime++;
                    if (LaserTime > 150) {
                        beam.Kill();
						beam = null;
						return;
                    }
                }

                npc.velocity = Vector2.Zero;
				float angleToPlayer = (float)(Math.Atan2(npc.Center.Y - player.Center.Y, npc.Center.X - player.Center.X) + Math.PI);
				angleToPlayer -= npc.rotation;
                if (Math.Abs(angleToPlayer) > Math.PI) {
					if (angleToPlayer > 0) {
                        angleToPlayer -= (float)Math.PI * 2;
                    } else {
                        angleToPlayer += (float)Math.PI * 2;
                    }
				}

				if (angleToPlayer > 0) {
					npc.rotation += Math.Min(angleToPlayer, 0.025f);
				} else if (angleToPlayer < 0) {
					npc.rotation += Math.Max(angleToPlayer, -0.025f);
				}
				npc.rotation = npc.rotation % ((float)Math.PI * 2);
            } else {
				LaserTime++;
				if (LaserTime < 400) {
                    Vector2 moveVec = player.Center - npc.Center;
                    moveVec.Normalize();
                    moveVec *= 0.1f;
                    npc.velocity += moveVec;
                    npc.rotation = (float)(Math.Atan2(player.Center.Y - npc.Center.Y, player.Center.X - npc.Center.X) + Math.PI * 2);
                } else {
					npc.velocity *= 0.95f;

                    float angleToPlayer = (float)(Math.Atan2(npc.Center.Y - player.Center.Y, npc.Center.X - player.Center.X) + Math.PI);
                    angleToPlayer -= npc.rotation;
                    if (Math.Abs(angleToPlayer) > Math.PI) {
                        if (angleToPlayer > 0) {
                            angleToPlayer -= (float)Math.PI * 2;
                        } else {
                            angleToPlayer += (float)Math.PI * 2;
                        }
                    }

                    if (angleToPlayer > 0) {
                        npc.rotation += Math.Min(angleToPlayer, 0.025f);
                    } else if (angleToPlayer < 0) {
                        npc.rotation += Math.Max(angleToPlayer, -0.025f);
                    }
                    npc.rotation = npc.rotation % ((float)Math.PI * 2);

                    if (Main.netMode != 1 && LaserTime > 600) {
                        beam = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<NovaRay>(), (int)(npc.damage * 0.75f), 3f, Main.myPlayer, npc.whoAmI, 420)];
                        if (beam.modProjectile is NovaRay ray) {
                            ray.MoveDistance = 35.0f;
                            LaserTime = 0;
                            npc.netUpdate = true;
                        } else {
                            beam = null;
                        }
                    }
                }
			}
			
			/*BaseAI.AISkull(npc, ref npc.ai, false, 6f, 350f, 0.6f, 0.15f);
			Player player = Main.player[npc.target];
			bool playerActive = player != null && player.active && !player.dead;
            if (shootAI < 60)
            {
                BaseAI.LookAt(player.Center, npc, 3, 0, .1f, false);
            }
            if (Main.netMode != 1 && playerActive)
			{
				shootAI++;
				if(shootAI >= 90)
				{
					shootAI = 0;
                    int projType = mod.ProjType("Neutralizer");

                    if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, projType, (int)(npc.damage * 0.25f), 3f, Main.myPlayer, npc.whoAmI);

                    }
                }
			}*/
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