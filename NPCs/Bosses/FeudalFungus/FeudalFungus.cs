using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace AAMod.NPCs.Bosses.FeudalFungus
{
    [AutoloadBossHead]
    public class FeudalFungus : ModNPC
    {
        public int damage = 0;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feudal Fungus");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 1200;   //boss life
            npc.damage = 24;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0f;   //this boss will behavior like the DemonEye  //boss frame/animation 
            npc.value = Item.sellPrice(0, 0, 50, 0);
            npc.aiStyle = -1;
            npc.width = 74;
            npc.height = 108;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.buffImmune[46] = true;
            npc.buffImmune[47] = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            bossBag = mod.ItemType("FungusBag");
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Fungus");
            npc.alpha = 255;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public const int AISTATE_RAIN = 0, AISTATE_CHASE = 1, AISTATE_HORIZONTAL = 2, AISTATE_DROP = 3;
        private int State { get => (int)npc.ai[0]; set => npc.ai[0] = value; }
        private int AttackTime { get => (int)npc.ai[1]; set => npc.ai[1] = value; }
        private int AttackParameter { get => (int)npc.ai[2]; set => npc.ai[2] = value; }

        public override void AI()
        {
            // animation
            npc.frameCounter++;
            if (npc.frameCounter >= 10)
            {
                npc.frameCounter = 0;
                npc.frame.Y += 90;
                if (npc.frame.Y > (90 * 7))
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = 0;
                }
            }
            npc.rotation = npc.velocity.X * 0.15f;

            // expert damage scaling
            if (Main.expertMode) {
                damage = npc.damage / 4;
            } else {
                damage = npc.damage / 2;
            }

            // target (or despawn)
            npc.TargetClosest(false);
            if (!npc.HasValidTarget || !Main.player[npc.target].ZoneGlowshroom) {
                npc.velocity.Y -= 0.02f;
                if (npc.alpha <= 0) npc.velocity = Vector2.Zero;

                npc.alpha += 2;

                if (npc.alpha >= 255) {
                    npc.active = false;
                }

                npc.dontTakeDamage = true;
                return;
            } else if (npc.alpha > 0) {
                npc.alpha -= 2;
                if (npc.alpha < 0) {
                    npc.alpha = 0;
                }
                if (npc.alpha == 0) npc.dontTakeDamage = false;
            }
            Player player = Main.player[npc.target];

            // select new attack every 5 seconds
            if (Main.netMode != 1) {
                if (AttackTime++ > 300) {
                    int prevStage = State;
                    do {
                        // if we're in expert mode he may choose to end the rain attack with a dive
                        if (State == AISTATE_RAIN && Main.expertMode && Main.rand.NextBool()) {
                            State = AISTATE_DROP;
                            npc.velocity = new Vector2(0, -8);
                            npc.rotation = 0;
                        } else {
                            State = Main.rand.Next(3);
                        }
                    } while (prevStage == State); // loop if we're repeating the same attack because that's boring
                    
                    //
                    AttackTime = 0;
                    AttackParameter = 0;
                    npc.netUpdate = true;
                }
            }

            // attack AIs
            Vector2 moveVec;
            switch (State) {
                case AISTATE_RAIN:
                    // move above player
                    moveVec = player.Center + new Vector2(0, -300.0f);
                    moveVec = (moveVec - npc.Center);
                    moveVec.Normalize();
                    if (moveVec.HasNaNs()) moveVec = Vector2.Zero;
                    moveVec.Y *= 0.6f;
                    npc.velocity += moveVec * 0.14f;

                    if (npc.velocity.Length() > 6) {
                        npc.velocity.Normalize();
                        npc.velocity *= 6;
                    }
                    npc.rotation = 0;

                    // drop projectiles
                    if (Main.netMode != 1 && AttackTime > 0 && AttackTime % (Main.expertMode ? 25 : 40) == 0) {
                        Projectile.NewProjectile((npc.Bottom + npc.Center) / 2, new Vector2(0, 8), ModContent.ProjectileType<Mushshot>(), damage, 0, Main.myPlayer);
                    }
                    
                    break;

                case AISTATE_CHASE:
                    // move towards player
                    moveVec = player.Center - npc.Center;
                    moveVec.Normalize();
                    if (moveVec.HasNaNs()) moveVec = Vector2.Zero;
                    npc.velocity += moveVec * 0.05f;
                    if (npc.velocity.Length() > 3) {
                        npc.velocity.Normalize();
                        npc.velocity *= 3;
                    }

                    // summon clouds
                    if (Main.netMode != 1) {
                        if (Main.expertMode) {
                            if (AttackTime > 30 && Main.rand.Next(20) == 0) {
                                float angle = Main.rand.NextFloat() * (float)Math.PI * 2;
                                float magnitude = 0.6f + Main.rand.NextFloat() * (2.1f - 0.6f);
                                Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * magnitude;
                                Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<FungusCloud>(), damage, 0, Main.myPlayer);
                            }
                        } else {
                            if (AttackTime > 0 && AttackTime % 50 == 0) {
                                Vector2 vel = player.Center - npc.Center;
                                vel.Normalize();
                                vel *= 1.2f;
                                Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<FungusCloud>(), damage, 0, Main.myPlayer);
                            }
                        }
                    }

                    break;

                case AISTATE_HORIZONTAL:
                    // vertical movement
                    npc.velocity.Y *= 0.9f;
                    if (player.Center.Y > npc.Center.Y) {
                        npc.velocity.Y += 0.1f;
                    } else {
                        npc.velocity.Y -= 0.1f;
                    }

                    // if the attack just started we need to set up AttackParameter. We will accelerate in the direction we're currently going
                    if (Main.netMode != 1 && AttackParameter == 0) {
                        if (npc.velocity.X >= 0) {
                            AttackParameter = 1;
                        } else {
                            AttackParameter = -1;
                        }
                        npc.netUpdate = true;
                    }

                    // loop back if we've passed the player
                    float offset = player.Center.X - npc.Center.X;
                    if (Math.Abs(offset) > 350) {
                        int newDir = 0;
                        if (offset > 0) newDir = (int)Math.Ceiling(offset);
                        if (offset < 0) newDir = (int)Math.Floor(offset);
                        newDir = newDir / Math.Abs(newDir); // normalize it
                        if (newDir != AttackParameter) {
                            AttackParameter = newDir;
                            npc.netUpdate = true;
                        }
                    }

                    // accelerate in desired direction
                    npc.velocity.X += AttackParameter * 0.23f;
                    npc.velocity.X = Math.Min(npc.velocity.X, 6);
                    npc.velocity.X = Math.Max(npc.velocity.X, -6);

                    break;

                case AISTATE_DROP:
                    // fall
                    npc.velocity.Y += 0.12f;

                    // summon clouds if we've gained enough speed
                    if (Main.netMode != 1 && npc.velocity.Y > 3) {
                        if (AttackTime % 4 == 0) {
                            float x = (Main.rand.NextFloat() - 0.5f) * 3;
                            float y = (Main.rand.NextFloat() - 0.5f) * 0.5f;
                            int proj = Projectile.NewProjectile(npc.Center, new Vector2(x, y), ModContent.ProjectileType<FungusCloud>(), damage, 0, Main.myPlayer);
                            Main.projectile[proj].timeLeft = 1200;
                        }
                    }

                    // force the attack to end if we've passed the player
                    if (npc.position.Y > player.position.Y + 150) AttackTime = 1000;
                    break;

            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {   //boss drops
            potionType = ItemID.ManaPotion;
            AAWorld.downedFungus = true;
            Projectile.NewProjectile(npc.Center, npc.velocity, mod.ProjectileType("FungusIGoNow"), 0, 0, 255, npc.scale);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GlowingSporeSac"), Main.rand.Next(30, 35));
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FungusTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FungusMask"));
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GlowingMushium"), Main.rand.Next(25, 35));
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = mod.GetTexture("Glowmasks/FeudalFungus_Glow");
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 8, npc.frame, npc.GetAlpha(dColor), true);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 8, npc.frame, npc.GetAlpha(AAColor.Glow), true);
            return false;
        }
    }

    
}


