using AAMod.Dusts;
using AAMod.Globals.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.NPCs.Bosses.Equinox {
    [AutoloadBossHead]
    public class DaybringerHead : EquinoxHead {
        public bool preShootingSun = false;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Daybringer");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SendExtraAI(BinaryWriter writer) {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ) {
                if (IsHead) {
                    writer.Write(preShootingSun);
                }
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                if (IsHead) {
                    preShootingSun = reader.ReadBoolean();
                }
            }
        }

        public override bool Empowered { get => Main.dayTime; }
        public override int[] SegmentTypes { get => new int[] { mod.NPCType("DaybringerHead"), mod.NPCType("DaybringerBody"), mod.NPCType("DaybringerTail") }; }
        public override int AIUpdates { get => 6; }
        public override int WormDustType { get => ModContent.DustType<DaybringerDust>(); }
        public override int WormLength { get => 30; }
        public override float EmpoweredMoveSpeed { get => 15f; }
        public override int EmpoweredDefense { get => 120; }
        public override void SpawnGore() {
            bool isHead = npc.type == mod.NPCType("DaybringerHead");
            bool isBody = npc.type == mod.NPCType("DaybringerBody");
            if (isHead) {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore2"), 1f);
            } else
            if (isBody) {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore3"), 1f);
            } else {
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/DBGore4"), 1f);
            }
        }

        public override bool DoWormAI(Player target) {
            if (preShootingSun) {
                npc.defense = 9999;
                npc.TargetClosest(false);
                ExtraAI(target);
                return false;
            }

            return true;
        }

        public /*override*/ void ExtraAI(Player target) {
            if (npc.type == mod.NPCType("DaybringerHead")) {
                npc.defense = 9999;
                npc.TargetClosest(false);
                npc.velocity = new Vector2(internalAI[5], internalAI[6]);
                Vector2 targetpos = target.Center - new Vector2(0, 2000f);
                Vector2 targetpos2 = target.Center - new Vector2(1000f, 1000f);
                Vector2 targetpos3 = target.Center - new Vector2(-1000f, 1000f);

                if (internalAI[4] == 0) {
                    if (Math.Abs(npc.Center.X - targetpos.X) + Math.Abs(npc.Center.Y - targetpos.Y) < 100f) {
                        internalAI[4] = 1f;
                    }
                } else if (internalAI[4] == 1) {
                    targetpos = targetpos2;
                    if (Math.Abs(npc.Center.X - targetpos.X) + Math.Abs(npc.Center.Y - targetpos.Y) < 100f) {
                        internalAI[4] = 2f;
                        if (Main.netMode != 1) {
                            for (int i = 0; i < Main.maxNPCs; i+= 3) {
                                if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI && AAGlobalProjectile.CountProjectiles(mod.ProjectileType("DaybringerSun")) < 3) {
                                    Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                    Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, -speed.X, -speed.Y, mod.ProjectileType("DaybringerSun"), npc.damage / 3, 1, 255);
                                }
                            }
                        }
                    }
                } else if (internalAI[4] == 2) {
                    targetpos = targetpos3;
                    if (Math.Abs(npc.Center.X - targetpos.X) + Math.Abs(npc.Center.Y - targetpos.Y) < 100f) {
                        internalAI[4] = 1f;
                        if (Main.netMode != 1) {
                            for (int i = 0; i < Main.maxNPCs; i+= 3) {
                                if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI && AAGlobalProjectile.CountProjectiles(mod.ProjectileType("DaybringerSun")) < 3) {
                                    Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                    Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, -speed.X, -speed.Y, mod.ProjectileType("DaybringerSun"), npc.damage / 3, 1, 255);
                                }
                            }
                        }
                    }
                }
                if (internalAI[3] % 200 == 60 && Main.netMode != 1) {
                    Vector2 speed = Vector2.Normalize(npc.velocity) * 8f;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, mod.ProjectileType("DaybringerSun"), npc.damage / 2, 1, 255);
                }
                if (npc.Center.X < targetpos.X) {
                    npc.velocity.X += 0.5f;
                    if (npc.velocity.X < 0)
                        npc.velocity.X += 0.5f * 2;
                } else {
                    npc.velocity.X -= 0.5f;
                    if (npc.velocity.X > 0)
                        npc.velocity.X -= 0.5f * 2;
                }
                if (npc.Center.Y < targetpos.Y) {
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

                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

                internalAI[5] = npc.velocity.X;
                internalAI[6] = npc.velocity.Y;

                if (internalAI[3]++ > 700) {
                    internalAI[3] = 0;
                    internalAI[5] = 0;
                    internalAI[6] = 0;
                    preShootingSun = false;
                    npc.netUpdate = true;
                }
            }
        }

        public override void NormalAI(bool wormStronger, bool isDay) {
            npc.spriteDirection = 1;
            prevWormStronger = wormStronger;

            if (isDay && !preShootingSun) {
                if (IsHead) {
                    internalAI[0] += 1f;
                    if (internalAI[0] % 360 == 0) {
                        for (int playerid = 0; playerid < 255; playerid++) {
                            if (Main.player[playerid].active && !Main.player[playerid].dead && Main.player[playerid] != null && Main.player[playerid].ownedProjectileCounts[mod.ProjectileType("DaybringerStars")] <= 0) {
                                if (npc.life > npc.lifeMax / 2) {
                                    if (Main.rand.Next(2) == 0) {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y - 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                    } else {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y + 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                    }
                                } else {
                                    if (Main.rand.Next(2) == 0) {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y + 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, -200f, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 200f, Main.player[playerid].Center.Y - 200f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 200f, playerid);
                                    } else {
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y + 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X, Main.player[playerid].Center.Y - 300f, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X + 300f, Main.player[playerid].Center.Y, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                        Projectile.NewProjectile(Main.player[playerid].Center.X - 300f, Main.player[playerid].Center.Y, 0, 0, mod.ProjectileType("DaybringerStars"), npc.damage / 3, 5, playerid, 0, playerid);
                                    }
                                }
                            }
                        }
                    }
                    if (internalAI[0] % 120 == 30 && Main.netMode != 1) {
                        for (int i = 0; i < Main.maxNPCs; i += 2) {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI) {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 12f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DayBringerDarts"), npc.damage / 3, 0, Main.myPlayer);
                                speed = -speed;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DayBringerDarts"), npc.damage / 3, 0, Main.myPlayer);
                            }
                        }
                    }
                    if (internalAI[0] % 120 == 60 && Main.netMode != 1) {
                        for (int i = 0; i < Main.maxNPCs; i+=4) {
                            if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("DaybringerBody") && Main.npc[i].realLife == npc.whoAmI && Main.rand.Next(15) == 0) {
                                Vector2 speed = Vector2.Normalize(new Vector2(1f, 0f).RotatedBy(Main.npc[i].rotation + 3.1415f)) * 8f;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DaybringerOrb"), npc.damage / 3, 0, Main.myPlayer, 0, npc.whoAmI);
                                speed = -speed;
                                Projectile.NewProjectile(Main.npc[i].Center.X, Main.npc[i].Center.Y, speed.X, speed.Y, mod.ProjectileType("DaybringerOrb"), npc.damage / 3, 0, Main.myPlayer, 0, npc.whoAmI);
                            }
                        }
                    }


                    if (internalAI[0] > 1200) {
                        if (Main.expertMode) preShootingSun = true;
                        internalAI[0] = 0f;
                        npc.netUpdate = true;
                    }
                }
            }
        }

        public override void HandleDayNightCycle() {
            bool bothExist = NPC.AnyNPCs(ModContent.NPCType<NightcrawlerHead>());
            if (bothExist) {
                if (Main.dayTime && !preShootingSun) {
                    if (Main.expertMode) {
                        Main.fastForwardTime = true;
                        Main.dayRate = 20;
                    } else {
                        Main.fastForwardTime = true;
                        Main.dayRate = 15;
                    }
                } else if (preShootingSun) {
                    Main.dayRate = 0;
                    Main.fastForwardTime = false;
                    Main.time--;
                }
            } else {
                Main.fastForwardTime = true;
                Main.dayTime = true;
                Main.dayRate = 0;
                if (preShootingSun) {
                    Main.fastForwardTime = false;
                    Main.time--;
                }
            }
        }
    }   
}