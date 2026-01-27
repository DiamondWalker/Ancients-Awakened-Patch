using AAMod.Globals.Worlds;
using AAMod.NPCs.Bosses.Akuma.Awakened;
using AAMod.NPCs.Bosses.Akuma;
using AAMod.NPCs.Bosses.Anubis.Forsaken;
using AAMod.NPCs.Bosses.Athena.Olympian;
using AAMod.NPCs.Bosses.Shen;
using AAMod.NPCs.Bosses.Yamata.Awakened;
using AAMod.NPCs.Bosses.Yamata;
using AAMod.Worldgeneration.Dimension.Void;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using AAMod.Buffs;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;
using Terraria.ID;

namespace AAMod.Globals.Players {
    public class AABiomesPlayer : ModPlayer {
        public bool ZoneMire = false;
        public bool ZoneInferno = false;
        public bool ZoneVoid = false;
        public bool ZoneMush = false;
        public bool ZoneStorm = false;
        public bool ZoneRisingSunPagoda = false;
        public bool ZoneRisingMoonLake = false;
        public bool ZoneShip = false;
        public bool VoidUnit = false;
        public bool SunAltar = false;
        public bool MoonAltar = false;
        public bool AkumaAltar = false;
        public bool YamataAltar = false;
        public bool Terrarium = false;
        public bool ZoneStars = false;
        public bool ZoneHoard = false;
        public bool ZoneAcropolis = false;
        public bool AshCurse;
        public int VoidGrav = 0;
        public static int Ashes = 0;
        public int CthulhuCountdown = 10800;
        public bool Leave = false;

        public bool ZoneTower;

        public override void UpdateBiomes() {
            ZoneTower = player.ZoneTowerSolar || player.ZoneTowerNebula || player.ZoneTowerStardust || player.ZoneTowerVortex;
            ZoneMire = AAWorld.mireTiles > 100 || BaseAI.GetNPC(player.Center, ModContent.NPCType<Yamata>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<YamataA>(), 5000) != -1;
            ZoneInferno = AAWorld.infernoTiles > 100 || BaseAI.GetNPC(player.Center, ModContent.NPCType<Akuma>(), 5000) != -1 || BaseAI.GetNPC(player.Center, ModContent.NPCType<AkumaA>(), 5000) != -1;
            ZoneMush = AAWorld.mushTiles > 100;
            Terrarium = AAWorld.terraTiles >= 1;
            ZoneVoid = VoidSubworld.IsInside() || AAWorld.voidTiles > 20 && player.ZoneSkyHeight || AAWorld.voidTiles > 100 && !player.ZoneSkyHeight;
            ZoneRisingMoonLake = AAWorld.lakeTiles >= 1;
            ZoneRisingSunPagoda = AAWorld.pagodaTiles >= 1;
            ZoneStars = AAWorld.Radium >= 20;
            ZoneHoard = AAWorld.HoardTiles > 1 && !ZoneStars;
            ZoneAcropolis = AAWorld.CloudTiles > 1;
        }

        public override void UpdateBiomeVisuals() {
            bool Underground = player.Center.Y > Main.worldSurface * 16;
            bool useAthena = NPC.AnyNPCs(ModContent.NPCType<AthenaA>());
            bool useShenA = NPC.AnyNPCs(ModContent.NPCType<ShenA>());
            bool useShen = NPC.AnyNPCs(ModContent.NPCType<Shen>()) && !useShenA;
            bool useAkuma = NPC.AnyNPCs(ModContent.NPCType<AkumaA>()) || AkumaAltar;
            bool useYamata = NPC.AnyNPCs(ModContent.NPCType<YamataA>()) || YamataAltar;
            bool useAnu = NPC.AnyNPCs(ModContent.NPCType<ForsakenAnubis>());
            bool useMire = (ZoneMire || MoonAltar) && !useYamata && !useShen && !useShenA && !useAnu;
            bool useInferno = (ZoneInferno || SunAltar) && !useAkuma && !useShen && !useShenA && !useAnu;
            bool useVoid = (ZoneVoid || VoidUnit) && !useShen && !useShenA && !useAnu;

            player.ManageSpecialBiomeVisuals("AAMod:AnubisSky", useAnu);
            player.ManageSpecialBiomeVisuals("AAMod:AthenaSky", useAthena);
            player.ManageSpecialBiomeVisuals("AAMod:ShenSky", useShen);
            player.ManageSpecialBiomeVisuals("AAMod:ShenASky", useShenA);
            player.ManageSpecialBiomeVisuals("AAMod:AkumaSky", useAkuma);
            player.ManageSpecialBiomeVisuals("AAMod:YamataSky", useYamata);

            if (!Underground) {
                player.ManageSpecialBiomeVisuals("AAMod:InfernoSky", useInferno);
                player.ManageSpecialBiomeVisuals("AAMod:MireSky", useMire);
            }

            if (Main.UseHeatDistortion) {
                player.ManageSpecialBiomeVisuals("HeatDistortion", useAkuma || useInferno);
            }

            player.ManageSpecialBiomeVisuals("AAMod:VoidSky", useVoid);
        }

        public override void Initialize() {
            ZoneInferno = false;
            ZoneMire = false;
            ZoneMush = false;
            ZoneStorm = false;
            ZoneVoid = false;
            ZoneRisingMoonLake = false;
            ZoneRisingSunPagoda = false;
            ZoneShip = false;
            ZoneTower = false;
            ZoneStars = false;
            ZoneHoard = false;
            ZoneAcropolis = false;
        }

        public override bool CustomBiomesMatch(Player other) {
            AABiomesPlayer modOther = other.GetModPlayer<AABiomesPlayer>();
            return ZoneMire == modOther.ZoneMire &&
                ZoneInferno == modOther.ZoneInferno &&
                ZoneVoid == modOther.ZoneVoid &&
                ZoneMush == modOther.ZoneMush &&
                Terrarium == modOther.Terrarium &&
                ZoneStorm == modOther.ZoneStorm &&
                ZoneShip == modOther.ZoneShip &&
                ZoneStars == modOther.ZoneStars &&
                ZoneHoard == modOther.ZoneHoard &&
                ZoneAcropolis == modOther.ZoneAcropolis;
        }

        public override void CopyCustomBiomesTo(Player other) {
            AABiomesPlayer modOther = other.GetModPlayer<AABiomesPlayer>();
            modOther.ZoneInferno = ZoneInferno;
            modOther.ZoneMire = ZoneMire;
            modOther.ZoneVoid = ZoneVoid;
            modOther.ZoneMush = ZoneMush;
            modOther.Terrarium = Terrarium;
            modOther.ZoneStorm = ZoneStorm;
            modOther.ZoneRisingMoonLake = ZoneRisingMoonLake;
            modOther.ZoneRisingSunPagoda = ZoneRisingSunPagoda;
            modOther.ZoneShip = ZoneShip;
            modOther.ZoneStars = ZoneStars;
            modOther.ZoneHoard = ZoneHoard;
            modOther.ZoneAcropolis = ZoneAcropolis;
        }

        public override void SendCustomBiomes(BinaryWriter bb) {
            BitsByte zoneByte = 0;
            zoneByte[0] = ZoneInferno;
            zoneByte[1] = ZoneMire;
            zoneByte[2] = ZoneVoid;
            zoneByte[3] = ZoneMush;
            zoneByte[4] = Terrarium;
            zoneByte[5] = ZoneStorm;
            zoneByte[6] = ZoneRisingSunPagoda;
            zoneByte[7] = ZoneRisingMoonLake;
            bb.Write(zoneByte);

            BitsByte zoneByte2 = 0;
            zoneByte2[0] = ZoneShip;
            zoneByte2[1] = ZoneStars;
            zoneByte2[2] = ZoneHoard;
            zoneByte2[3] = ZoneAcropolis;
            bb.Write(zoneByte2);
        }

        public override void ReceiveCustomBiomes(BinaryReader bb) {
            BitsByte zoneByte = bb.ReadByte();
            ZoneInferno = zoneByte[0];
            ZoneMire = zoneByte[1];
            ZoneVoid = zoneByte[2];
            ZoneMush = zoneByte[3];
            Terrarium = zoneByte[4];
            ZoneStorm = zoneByte[5];
            ZoneRisingSunPagoda = zoneByte[6];
            ZoneRisingMoonLake = zoneByte[7];

            BitsByte zoneByte2 = bb.ReadByte();
            ZoneShip = zoneByte2[0];
            ZoneStars = zoneByte2[1];
            ZoneHoard = zoneByte2[2];
            ZoneAcropolis = zoneByte2[3];
        }

        public override void PreUpdate() {
            if (player.GetModPlayer<AABiomesPlayer>().ZoneVoid || player.GetModPlayer<AABiomesPlayer>().ZoneInferno || player.GetModPlayer<AABiomesPlayer>().ZoneRisingSunPagoda) {
                if (Main.raining) {
                    Main.rainTime = 0;
                    Main.raining = false;
                    Main.maxRaining = 0f;
                }
            }

            if (player.GetModPlayer<AABiomesPlayer>().ZoneMire || player.GetModPlayer<AABiomesPlayer>().ZoneRisingMoonLake) {
                if (Main.raining) {
                    if (Main.rand.Next(5) == 0) {
                        Main.rainTime++;
                    }
                }
            }
        }

        public override void PostUpdate() {
            if (player.ZoneSandstorm && (ZoneInferno || ZoneMire)) {
                EmitDust();
            }

            if (player.GetModPlayer<AABiomesPlayer>().ZoneMire || player.GetModPlayer<AABiomesPlayer>().ZoneRisingMoonLake) {
                if (Main.dayTime && !AAWorld.downedYamata) {
                    if (!player.GetModPlayer<AAPlayer>().FogRemover) {
                        player.AddBuff(ModContent.BuffType<Clueless>(), 5);
                    }
                }
            }

            if (player.GetModPlayer<AABiomesPlayer>().Terrarium) {
                player.AddBuff(ModContent.BuffType<Terrarium>(), 2);
                player.AddBuff(BuffID.DryadsWard, 2);
            }

            if (player.GetModPlayer<AABiomesPlayer>().ZoneInferno || player.GetModPlayer<AABiomesPlayer>().ZoneRisingSunPagoda) {
                if (AshCurse) {
                    AshRain(player);
                }
            }

            if (player.GetModPlayer<AABiomesPlayer>().ZoneRisingMoonLake || player.GetModPlayer<AABiomesPlayer>().ZoneRisingSunPagoda) {
                if (AAWorld.shenUnlocked && !AAWorld.downedShen) {
                    EmberRain(player);
                }
            }

            if (ZoneVoid) {
                player.gravity = Player.defaultGravity + .1f;
            }
        }

        #region Dust Effects

        public static void EmitDust() {
            if (Main.gamePaused) {
                return;
            }

            int sandTiles = Main.sandTiles;
            Player player = Main.LocalPlayer;
            bool flag = Sandstorm.Happening && player.ZoneSandstorm && (Main.bgStyle == 2 || Main.bgStyle == 5) && Main.bgDelay < 50;
            Sandstorm.HandleEffectAndSky(flag && Main.UseStormEffects);

            if (sandTiles < 100 || player.position.Y > Main.worldSurface * 16.0 || player.ZoneBeach) {
                return;
            }

            if (!flag) {
                return;
            }

            int maxValue = 1;
            if (Main.rand.Next(maxValue) != 0) {
                return;
            }

            int num = Math.Sign(Main.windSpeed);
            float num2 = Math.Abs(Main.windSpeed);
            if (num2 < 0.01f) {
                return;
            }

            float num3 = num * MathHelper.Lerp(0.9f, 1f, num2);
            float num4 = 2000f / sandTiles;
            float num5 = 3f / num4;
            num5 = MathHelper.Clamp(num5, 0.77f, 1f);
            int num6 = (int)num4;
            float num7 = Main.screenWidth / (float)Main.maxScreenW;
            int num8 = (int)(1000f * num7);
            float num9 = 20f * Sandstorm.Severity;
            float num10 = num8 * (Main.gfxQuality * 0.5f + 0.5f) + num8 * 0.1f - Dust.SandStormCount;
            if (num10 <= 0f) {
                return;
            }

            float num11 = Main.screenWidth + 1000f;
            float num12 = Main.screenHeight;
            Vector2 value = Main.screenPosition + player.velocity;

            WeightedRandom<Color> weightedRandom = new WeightedRandom<Color>();
            weightedRandom.Add(new Color(200, 160, 20, 180), Main.screenTileCounts[53] + Main.screenTileCounts[396] + Main.screenTileCounts[397]);
            weightedRandom.Add(new Color(103, 98, 122, 180), Main.screenTileCounts[112] + Main.screenTileCounts[400] + Main.screenTileCounts[398]);
            weightedRandom.Add(new Color(135, 43, 34, 180), Main.screenTileCounts[234] + Main.screenTileCounts[401] + Main.screenTileCounts[399]);
            weightedRandom.Add(new Color(213, 196, 197, 180), Main.screenTileCounts[116] + Main.screenTileCounts[403] + Main.screenTileCounts[402]);

            float num13 = MathHelper.Lerp(0.2f, 0.35f, Sandstorm.Severity);
            float num14 = MathHelper.Lerp(0.5f, 0.7f, Sandstorm.Severity);
            int num15 = 0;

            while (num15 < num9) {
                if (Main.rand.Next(num6 / 4) == 0) {
                    Vector2 vector = new Vector2(Main.rand.NextFloat() * num11 - 500f, Main.rand.NextFloat() * -50f);

                    if (Main.rand.Next(3) == 0 && num == 1) {
                        vector.X = Main.rand.Next(500) - 500;
                    } else if (Main.rand.Next(3) == 0 && num == -1) {
                        vector.X = Main.rand.Next(500) + Main.screenWidth;
                    }

                    if (vector.X < 0f || vector.X > Main.screenWidth) {
                        vector.Y += Main.rand.NextFloat() * num12 * 0.9f;
                    }

                    vector += value;

                    int num16 = (int)vector.X / 16;
                    int num17 = (int)vector.Y / 16;

                    if (Main.tile[num16, num17] != null && Main.tile[num16, num17].wall == 0) {
                        for (int i = 0; i < 1; i++) {
                            Dust dust = Main.dust[Dust.NewDust(vector, 10, 10, 268, 0f, 0f, 0)];
                            dust.velocity.Y = 2f + Main.rand.NextFloat() * 0.2f;

                            Dust expr_460_cp_0 = dust;
                            expr_460_cp_0.velocity.Y *= dust.scale;

                            Dust expr_47A_cp_0 = dust;
                            expr_47A_cp_0.velocity.Y *= 0.35f;

                            dust.velocity.X = num3 * 5f + Main.rand.NextFloat() * 1f;

                            Dust expr_4B7_cp_0 = dust;
                            expr_4B7_cp_0.velocity.X += num3 * num14 * 20f;

                            dust.fadeIn += num14 * 0.2f;
                            dust.velocity *= 1f + num13 * 0.5f;
                            dust.color = weightedRandom;
                            dust.velocity *= 1f + num13;
                            dust.velocity *= num5;
                            dust.scale = 0.9f;

                            num10 -= 1f;
                            if (num10 <= 0f) {
                                break;
                            }
                        }

                        if (num10 <= 0f) {
                            return;
                        }
                    }
                }

                num15++;
            }
        }

        public static void AshRain(Player player) {
            if (Main.gamePaused) {
                return;
            }

            if ((player.GetModPlayer<AABiomesPlayer>().ZoneInferno || player.GetModPlayer<AABiomesPlayer>().ZoneRisingSunPagoda) && player.GetModPlayer<AABiomesPlayer>().AshCurse) {
                if (!player.GetModPlayer<AAPlayer>().AshRemover || !(player.ZoneSkyHeight || player.ZoneOverworldHeight)) {
                    player.AddBuff(ModContent.BuffType<BurningAsh>(), 5);
                }

                if (AAWorld.infernoTiles > 0 && Main.LocalPlayer.position.Y < Main.worldSurface * 16.0) {
                    int maxValue = 800 / AAWorld.infernoTiles;
                    float num = Main.screenWidth / (float)Main.maxScreenW;
                    int num2 = (int)(500f * num);
                    num2 = (int)(num2 * (1f + 2f * Main.cloudAlpha));
                    float num3 = 1f + 50f * Main.cloudAlpha;
                    int num4 = 0;

                    while (num4 < num3) {
                        try {
                            if (Ashes >= num2 * (Main.gfxQuality / 2f + 0.5f) + num2 * 0.1f) {
                                break;
                            }

                            if (Main.rand.Next(maxValue) == 0) {
                                int num5 = Main.rand.Next(Main.screenWidth + 1000) - 500;
                                int num6 = (int)Main.screenPosition.Y - Main.rand.Next(50);

                                if (Main.LocalPlayer.velocity.Y > 0f) {
                                    num6 -= (int)Main.LocalPlayer.velocity.Y;
                                }

                                if (Main.rand.Next(5) == 0) {
                                    num5 = Main.rand.Next(500) - 500;
                                } else if (Main.rand.Next(5) == 0) {
                                    num5 = Main.rand.Next(500) + Main.screenWidth;
                                }

                                if (num5 < 0 || num5 > Main.screenWidth) {
                                    num6 += Main.rand.Next((int)(Main.screenHeight * 0.8)) + (int)(Main.screenHeight * 0.1);
                                }

                                num5 += (int)Main.screenPosition.X;

                                int num7 = num5 / 16;
                                int num8 = num6 / 16;

                                if (Main.tile[num7, num8] != null && Main.tile[num7, num8].wall == 0) {
                                    int dust = Dust.NewDust(new Vector2(num5, num6), 10, 10, ModContent.DustType<Dusts.AshRain>(), 0f, 0f, 0);
                                    Main.dust[dust].velocity.Y = 3f + Main.rand.Next(30) * 0.1f;

                                    Dust expr_292_cp_0 = Main.dust[dust];
                                    expr_292_cp_0.velocity.Y *= Main.dust[dust].scale;

                                    if (!player.GetModPlayer<AABiomesPlayer>().AshCurse) {
                                        Main.dust[dust].velocity.X = Main.rand.Next(-10, 10) * 0.1f;

                                        Dust expr_2EC_cp_0 = Main.dust[dust];
                                        expr_2EC_cp_0.velocity.X += Main.windSpeed * Main.cloudAlpha * 10f;
                                    } else {
                                        Main.dust[dust].velocity.X = (Main.cloudAlpha + 0.5f) * 25f + Main.rand.NextFloat() * 0.2f - 0.1f;

                                        Dust expr_370_cp_0 = Main.dust[dust];
                                        expr_370_cp_0.velocity.Y *= 0.5f;
                                    }

                                    Dust expr_38E_cp_0 = Main.dust[dust];
                                    expr_38E_cp_0.velocity.Y *= 1f + 0.3f * Main.cloudAlpha;

                                    Main.dust[dust].scale += Main.cloudAlpha * 0.2f;
                                    Main.dust[dust].velocity *= 1f + Main.cloudAlpha * 0.5f;
                                }
                            }
                        } catch {
                        }

                        num4++;
                    }
                }
            }
        }

        public static void EmberRain(Player player) {
            if (Main.gamePaused) {
                return;
            }

            if ((player.GetModPlayer<AABiomesPlayer>().ZoneRisingSunPagoda || player.GetModPlayer<AABiomesPlayer>().ZoneRisingMoonLake) && AAWorld.shenUnlocked && !AAWorld.downedShen) {
                if (Main.LocalPlayer.position.Y < Main.worldSurface * 16.0) {
                    int maxValue = 8;
                    float num = Main.screenWidth / (float)Main.maxScreenW;
                    int num2 = (int)(500f * num);
                    num2 = (int)(num2 * (1f + 2f * Main.cloudAlpha));
                    float num3 = 1f + 25f/*Main.cloudAlpha*/;
                    int num4 = 0;

                    while (num4 < num3) {
                        try {
                            if (Ashes >= num2 * (Main.gfxQuality / 2f + 0.5f) + num2 * 0.1f) {
                                break;
                            }

                            if (Main.rand.Next(maxValue) == 0) {
                                int num5 = Main.rand.Next(Main.screenWidth + 1000) - 500;
                                int num6 = (int)Main.screenPosition.Y - Main.rand.Next(50);

                                if (Main.LocalPlayer.velocity.Y > 0f) {
                                    num6 -= (int)Main.LocalPlayer.velocity.Y;
                                }

                                if (Main.rand.Next(5) == 0) {
                                    num5 = Main.rand.Next(500) - 500;
                                } else if (Main.rand.Next(5) == 0) {
                                    num5 = Main.rand.Next(500) + Main.screenWidth;
                                }

                                if (num5 < 0 || num5 > Main.screenWidth) {
                                    num6 += Main.rand.Next((int)(Main.screenHeight * 0.8)) + (int)(Main.screenHeight * 0.1);
                                }

                                num5 += (int)Main.screenPosition.X;

                                int num7 = num5 / 16;
                                int num8 = num6 / 16;

                                if (Main.tile[num7, num8] != null && Main.tile[num7, num8].wall == 0) {
                                    int dust = Dust.NewDust(new Vector2(num5, num6), 10, 10, ModContent.DustType<Dusts.Discord>(), 0f, 0f, 0, Color.White);
                                    Main.dust[dust].velocity.Y = 3f + Main.rand.Next(30) * 0.1f;

                                    Dust expr_292_cp_0 = Main.dust[dust];
                                    expr_292_cp_0.velocity.Y *= Main.dust[dust].scale;

                                    Main.dust[dust].velocity.X = (Main.cloudAlpha + 0.5f) * 25f + Main.rand.NextFloat() * 0.2f - 0.1f;
                                    Dust expr_370_cp_0 = Main.dust[dust];
                                    expr_370_cp_0.velocity.Y *= 0.5f;

                                    Dust expr_38E_cp_0 = Main.dust[dust];
                                    expr_38E_cp_0.velocity.Y *= 1f + 0.3f * Main.cloudAlpha;

                                    Main.dust[dust].scale += /*Main.cloudAlpha **/ 0.1f;
                                    Main.dust[dust].velocity *= 1f + Main.cloudAlpha * 0.5f;
                                }
                            }
                        } catch {
                        }

                        num4++;
                    }
                }
            }
        }

        #endregion

        public override Texture2D GetMapBackgroundImage() {
            if (ZoneMire || ZoneRisingMoonLake) {
                return mod.GetTexture("Map/MireMap");
            } else if (ZoneInferno || ZoneRisingSunPagoda) {
                return mod.GetTexture("Map/InfernoMap");
            } else if (ZoneVoid) {
                return mod.GetTexture("Map/VoidMap");
            }

            return null;
        }

        public override void ResetEffects() {
            AshCurse = !Main.dayTime && !AAWorld.downedAkuma;
        }
    }
}
