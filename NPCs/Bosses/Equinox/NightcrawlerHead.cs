using AAMod.Dusts;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Equinox {
    [AutoloadBossHead]		
	public class NightcrawlerHead : EquinoxHead
	{
        public bool preDeathRay = false;
        public bool isDeathRay = false;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Nightcrawler");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SendExtraAI(BinaryWriter writer) {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ) {
                if (IsHead) {
                    writer.Write(preDeathRay);
                    writer.Write(isDeathRay);
                }
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                if (IsHead) {
                    preDeathRay = reader.ReadBoolean();
                    isDeathRay = reader.ReadBoolean();
                }
            }
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

        public /*override*/ void ExtraAI(Player target) {
            if (npc.type == mod.NPCType("NightcrawlerHead")) {
                npc.defense = 9999;
                npc.TargetClosest(false);
                npc.velocity = new Vector2(internalAI[5], internalAI[6]);

                if (internalAI[2] < 120) {
                    Vector2 newvelocity = npc.velocity + Vector2.Normalize(npc.velocity.RotatedBy((float)Math.PI/2)) * 0.58f;
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                    npc.velocity = Vector2.Normalize(newvelocity) * 16f;
                } else {
                    Vector2 newvelocity = npc.velocity + Vector2.Normalize(npc.velocity.RotatedBy((float)Math.PI/2)) * 0.03625f;
                    npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                    npc.velocity = Vector2.Normalize(newvelocity) * 4f;
                }

                if (internalAI[2]++ == 90) {
                    for (int i = 0; i < Main.maxNPCs; i+=2) {
                        if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("NightcrawlerBody") && Main.npc[i].realLife == npc.whoAmI) {
                            if (Main.netMode != 1) {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("NightclawerDeathraySmall"), npc.damage / 2, 0, Main.myPlayer, 0, i);
                            }
                        }
                    }
                }
                if (internalAI[2] >= 90) {
                    for (int deathRay = 0; deathRay < Main.maxProjectiles; deathRay++) {
                        if (Main.projectile[deathRay].active && Main.projectile[deathRay].type == mod.ProjectileType("NightclawerDeathraySmall") || Main.projectile[deathRay].type == mod.ProjectileType("NightclawerDeathray") && Main.projectile[deathRay].ai[1] == npc.whoAmI) {
                            return;
                        }
                    }
                }

                internalAI[5] = npc.velocity.X;
                internalAI[6] = npc.velocity.Y;

                if (internalAI[2] > 400) {
                    internalAI[2] = 0;
                    isDeathRay = false;
                    preDeathRay = false;
                    npc.netUpdate = true;
                }
            }
        }

        public override bool DoWormAI(Player target) {
            if (isDeathRay) {
                ExtraAI(target);
                return false;
            }
            if (preDeathRay) {
                npc.defense = 9999;
                if ((npc.Center - target.Center).Length() < 300f) {
                    isDeathRay = true;
                    npc.netUpdate = true;
                }

                if (npc.Center.X < target.Center.X) {
                    npc.velocity.X += 0.5f;
                    if (npc.velocity.X < 0)
                        npc.velocity.X += 0.5f * 2;
                } else {
                    npc.velocity.X -= 0.5f;
                    if (npc.velocity.X > 0)
                        npc.velocity.X -= 0.5f * 2;
                }
                if (npc.Center.Y < target.Center.Y) {
                    npc.velocity.Y += 0.5f;
                    if (npc.velocity.Y < 0)
                        npc.velocity.Y += 0.5f * 2;
                } else {
                    npc.velocity.Y -= 0.5f;
                    if (npc.velocity.Y > 0)
                        npc.velocity.Y -= 0.5f * 2;
                }

                if (npc.velocity.X > 30f) npc.velocity.X = 30f;
                if (npc.velocity.Y > 30f) npc.velocity.Y = 30f;

                internalAI[5] = npc.velocity.X;
                internalAI[6] = npc.velocity.Y;
            }

            return true;
        }

        public override void NormalAI(bool wormStronger, bool isDay) {
            npc.spriteDirection = 1;
            prevWormStronger = wormStronger;

            if (IsHead && NPC.CountNPCS(ModContent.NPCType<NCCloud>()) < CloudCount && CloudCooldown > 0 && Main.netMode != 1) {
                CloudCooldown--;

                if (CloudCooldown <= 0) {
                    CloudCooldown = 0;
                }
            }

            if (!isDay && !preDeathRay) {
                if (IsHead) {
                    internalAI[1] += 1f;
                    if (Main.netMode != 1 && CloudCooldown <= 0) {
                        for (int i = 0; i < 200; i++) {
                            if (Main.npc[i].type == mod.NPCType("NCCloud")) {
                                Main.npc[i].life = 0;
                                Main.npc[i].NPCLoot();
                                Main.npc[i].active = false;
                            }
                        }
                        CloudCooldown = 400;
                        float rotation = 2f * (float)Math.PI / CloudCount;
                        for (int m = 0; m < CloudCount; m++) {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("NCCloud"), 0, 0, 0, 0, rotation * m);
                            if (Main.netMode == 2 && n < 200)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }

                    if (internalAI[1] % 380 == 90 && Main.netMode != 1) {
                        for (int i = 0; i < Main.maxNPCs; i+= 4) {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("NightcrawlerBody") && Main.npc[i].realLife == npc.whoAmI) {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * .5f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("NightclawerScythe"), npc.damage / 3, 0, Main.myPlayer, npc.rotation, npc.spriteDirection);
                                speed = -speed;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("NightclawerScythe"), npc.damage / 3, 0, Main.myPlayer, npc.rotation, npc.spriteDirection);
                            }
                        }
                    }


                    if (internalAI[1] % 120 == 90 && Main.netMode != 1) {
                        for (int i = 0; i < Main.maxNPCs; i++) {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("NightcrawlerBody") && Main.npc[i].realLife == npc.whoAmI && Main.rand.Next(10) == 0) {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f));
                                speed = (Main.rand.Next(2) == 0 ? 1 : -1) * speed;
                                float ai = Main.rand.Next(120);
                                Vector2 speedR = Vector2.Normalize(speed.RotatedByRandom(0.6)) * 20f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speedR.X, speedR.Y, mod.ProjectileType("NightclawerLaser"), npc.damage / 3, 0, Main.myPlayer, speed.ToRotation() + 1000f, ai);
                            }
                        }
                    }

                    if (internalAI[1] > 1200) {
                        internalAI[1] = 0f;
                        if (Main.expertMode) preDeathRay = true;
                        npc.netUpdate = true;
                    }
                }
            }
        }

        public override void HandleDayNightCycle() {
            bool bothExist = NPC.AnyNPCs(ModContent.NPCType<DaybringerHead>());
            if (bothExist) {
                if (!Main.dayTime && !preDeathRay) {
                    if (Main.expertMode) {
                        Main.fastForwardTime = true;
                        Main.dayRate = 20;
                    } else {
                        Main.fastForwardTime = true;
                        Main.dayRate = 15;
                    }
                } else if (preDeathRay || isDeathRay) {
                    Main.dayRate = 0;
                    Main.fastForwardTime = false;
                    Main.time--;
                }
            } else {
                Main.fastForwardTime = true;
                Main.dayTime = false;
                Main.dayRate = 0;
                if (preDeathRay || isDeathRay) {
                    Main.fastForwardTime = false;
                    Main.time--;
                }
            }
        }
    }
}