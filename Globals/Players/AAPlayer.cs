using AAMod.Buffs;
using AAMod.Globals.Worlds;
using AAMod.Items;
using AAMod.Items.Base;
using AAMod.Items.Boss.Shen;
using AAMod.Items.FishingItem;
using AAMod.NPCs.Bosses.Akuma;
using AAMod.NPCs.Bosses.Akuma.Awakened;
using AAMod.NPCs.Bosses.Anubis.Forsaken;
using AAMod.NPCs.Bosses.Athena;
using AAMod.NPCs.Bosses.Athena.Olympian;
using AAMod.NPCs.Bosses.Shen;
using AAMod.NPCs.Bosses.Yamata;
using AAMod.NPCs.Bosses.Yamata.Awakened;
using AAMod.NPCs.Bosses.Zero.Protocol;
using AAMod.Util;
using AAMod.Worldgeneration.Dimension.Void;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace AAMod.Globals.Players {
    public class AAPlayer : ModPlayer {
        #region Variables

        #region Minions
        public bool FireSpirit = false;
        public bool ImpServant = false;
        public bool ImpSlave = false;
        public bool MoonBee = false;
        public bool Searcher = false;
        public bool enderMinion = false;
        public bool enderMinionEX = false;
        public bool LungMinion = false;
        public bool DragonMinion = false;
        public bool BabyPhoenix = false;
        public bool GripMinion = false;
        public bool ProbeMinion = false;
        public bool SkullMinion = false;
        public bool EaterMinion = false;
        public bool CrimeraMinion = false;
        public bool CrowMinion = false;
        public bool DemonMinion = false;
        public bool DevilMinion = false;
        public bool DoomiteProbe = false;
        public bool DoomiteProbeC = false;
        public bool TerraMinion = false;
        public bool HallowedPrism = false;
        public bool TrueHallowedPrism = false;
        public bool SnakeMinion = false;
        public bool dustDevil = false;
        public bool KrakenMinion = false;
        public bool Fishnado = false;
        public bool MadnessElemental = false;
        public bool FlameSoul = false;
        public bool Orbiters = false;
        public bool Protocol = false;
        public bool ScoutMinion = false;
        public bool SagOrbiter = false;
        public bool Rabbitcopter = false;
        public bool RabbitcopterR = false;
        public bool Sock = false;
        public bool Socc = false;
        public bool Squirrel = false;
        public bool DapperSquirrel = false;
        public bool CyberClaw = false;
        public bool ChaosClaw = false;
        public bool MiniZero = false;
        public bool TerraSummon = false;
        public bool DragonSpirit = false;
        public bool Seraph = false;
        public bool Athena = false;
        public bool Baron = false;
        public bool Xiao = false;
        public bool ChaosConstruct = false;
        public bool CCBook = false;
        public bool CCBookEX = false;
        public bool WeakCCRune = false;
        public bool CCRune = false;
        #endregion

        #region Armor bools.
        public bool AncientGoldBody = false;
        public bool AncientGoldLeg = false;
        public bool AncientGoldSet = false;
        public bool StripeManFish = false;
        public bool StripeManOre = false;
        public bool StripeManSpawn = false;
        public bool StripeManSet = false;
        public bool MoonSet;
        public bool goblinSlayer;
        public bool IsGoblin;
        public bool leatherSet;
        public bool mushiumSet;
        public bool kindledSet;
        public bool depthSet;
        public bool impSet;
        public bool DynaskullSet;
        public bool fleshrendSet;
        public bool nightsSet;
        public bool deathlySet;
        public bool tribalSet;
        public bool demonSet;
        public bool demonBonus;
        public bool terraSet;
        public bool chaosSet;
        public bool darkmatterSetMe;
        public bool darkmatterSetRa;
        public bool darkmatterSetMa;
        public bool darkmatterSetSu;
        public bool darkmatterSetTh;
        public bool radiumMe;
        public bool radiumRa;
        public bool radiumMa;
        public bool radiumSu;
        public bool DarkmatterSet;
        public bool dracoSet;
        public bool dreadSet;
        public bool zeroSet1;
        public bool zeroSet;
        public bool entropySet;
        public bool valkyrieSet;
        public bool infinitySet;
        public bool Alpha;
        public bool Palladium;
        public bool fulgurite;
        public bool ringActive = false;
        public bool doomite;
        public bool Radium;
        public bool Darkmatter;
        public bool perfectChaos;
        public bool perfectChaosMe;
        public bool perfectChaosRa;
        public bool perfectChaosMa;
        public bool perfectChaosSu;
        public bool Assassin;
        public bool AbyssalStealth;
        public bool Witch;

        public bool ChaosMe = false;
        public bool ChaosRa = false;
        public bool ChaosMe1 = false;
        public bool ChaosRa2 = false;
        public bool ChaosMa = false;
        public bool ChaosSu = false;

        public bool Olympian = false;
        public bool StoneSoldier = false;

        public bool ChampionMe = false;
        public bool ChampionRa = false;
        public bool ChampionMa = false;
        public int CarrotBuff = 0;
        public bool ChampionSu = false;

        public bool TerraMe = false;
        public bool TerraRa = false;
        public bool TerraSu = false;
        public int CrystalMode = 0;
        public bool TerraMa = false;
        public int RoseCooldown = 0;

        public bool ono;

        public bool AsheFlame;
        public float AsheFlameScale = 0f;
        public int AsheCooldown = 0;

        public int EntropyCooldown = 0;
        #endregion

        #region Accessory bools
        public bool artifactJudgement;
        public int artifactJudgementCharge = 0;
        public bool artifactGuilt;
        public int artifactGuiltCharge = 0;
        public bool clawsOfChaos;
        public bool HydraPendant;
        public bool demonGauntlet;
        public bool BrokenCode = false;
        public bool CodeOn = false;
        public Vector2 headOffset = Vector2.Zero;
        public Vector2 bodyOffset = Vector2.Zero;
        public Vector2 legOffset = Vector2.Zero;
        public int CodeCD = 0;
        public int AbilityCD = 180;
        public bool AshRemover;
        public bool FogRemover;
        public bool Baolei;
        public bool Naitokurosu;
        public bool Duality;
        public bool DragonShell;
        public bool ammo20percentdown = false;
        public int AADash;
        public int AADashTime;
        public int dashDelayAA;
        public bool RStar;
        public bool DVoid;
        public int[] AADoubleTapKeyTimer = new int[4];
        public int[] AAHoldDownKeyTimer = new int[4];
        public bool DiscordShredder;
        public bool lantern = false;
        public bool HeartP = false;
        public bool HeartS = false;
        public bool HeartA = false;
        public bool DragonsGuard = false;
        public bool ShadowBand = false;
        public bool RajahCape = false;
        public bool olympianWings = false;
        public bool BlackLotusEmblem = false;

        public bool SagShield = false;
        public bool ShieldUp = false;
        public int SagCooldown = 0;

        public bool GreedCharm;
        public bool GreedTalisman;
        public bool OldOneCharm = false;
        public bool SpellBookofRagnarok;
        public bool CursedEyeofSoulBinder;

        public bool RealityStone = false;

        public bool Replicator = false;

        public bool MegaMush = false;
        #endregion

        #region debuffs
        public bool CursedHellfire = false;
        public bool infinityOverload = false;
        public bool discordInferno = false;
        public bool dragonFire = false;
        public bool hydraToxin = false;
        public int hydraToxinTime = 0;
        public bool terraBlaze = false;
        public bool Snagged = false;
        public bool Snagged1 = false;
        public bool YamataCount = false;
        public bool YamataACount = false;
        public bool Clueless = false;
        public bool Yanked = false;
        public bool InfinityScorch = false;
        public bool LockedOn = false;
        public bool shroomed = false;
        public bool riftbent = false;
        public bool DestinedToDie = false;
        public int TeleportTimer = 0;
        public bool YamataGravity = false;
        public bool YamataAGravity = false;
        public bool Hunted = false;
        public bool IB = false;
        public bool Spear = false;
        public bool AkumaPain = false;
        public bool FFlames = false;
        #endregion

        #region buffs

        public bool Ronin = false;
        //public bool Glitched = false;
        public bool Greed1 = false;
        public bool Greed2 = false;
        public float GreedyDamage = 0;

        public bool luckycalm = false;
        public bool luckythorns = false;
        public bool StripeCrasyLucky = false;
        public bool CrasyLucky = false;
        #endregion

        #region pets
        public bool Broodmini = false;
        public bool Raidmini = false;
        public bool MiniProbe = false;
        public bool Sharkron = false;
        public bool RoyalKitten = false;
        public bool Mudkip = false;
        public bool MudkipS = false;
        public bool BoomBoi = false;
        public bool DragonSoul = false;
        public bool Glowmoss = false;
        public bool Cerberus = false;
        public bool K9 = false;
        public bool Lunamini = false;
        public bool ZeroBab = false;
        #endregion

        //NPCcount
        public static int yamata = -1;

        public static int ZeroKills = 0;

        public int ManaLantern = 0;

        #region Misc
        public bool Compass = false;
        public Vector2 RiftMirrorReturnPos = new Vector2(0, 0);
        public int PrismCooldown = 0;
        public bool WorldgenReminder = false;
        public bool DemonSun = false;
        public bool AnubisBook = false;
        public bool GivenAnuSummon = false;
        public bool GivenWormIdol = false;

        public float spellbookDamage = 1f;
        public float MaxMovespeedboost = 0;
        public bool bossactive = false;
        public bool nohitplayer = true;
        #endregion

        #endregion

        #region Save/Load
        public override TagCompound Save() {
            var saved = new List<string>();
            if (AnubisBook) saved.Add("Book");
            if (GivenAnuSummon) saved.Add("Stick");
            if (GivenWormIdol) saved.Add("Idol");
            if (MegaMush) saved.Add("MegaMush");
            return new TagCompound
            {
                { "saved", saved }
            };
        }

        public override void Load(TagCompound tag) {
            var saved = tag.GetList<string>("saved");
            AnubisBook = saved.Contains("Book");
            GivenAnuSummon = saved.Contains("Stick");
            GivenWormIdol = saved.Contains("Idol");
            MegaMush = saved.Contains("MegaMush");
        }

        #endregion

        #region Reset Effects

        public override void ResetEffects() {
            ResetMinionEffect();
            ResetArmorEffect();
            ResetAccessoryEffect();
            ResetBuffEffect();
            ResetDebuffEffect();
            ResetPetsEffect();

            spellbookDamage = 1f;
            MaxMovespeedboost = 0;
            bossactive = false;

            //EnemyChecks
            IsGoblin = false;
            ResetMiscEffect();
        }

        private void ResetMiscEffect() {
            Compass = false;
            DemonSun = false;
        }

        private void ResetMinionEffect() {
            FireSpirit = false;
            ImpServant = false;
            ImpSlave = false;
            MoonBee = false;
            Searcher = false;
            enderMinion = false;
            enderMinionEX = false;
            BabyPhoenix = false;
            LungMinion = false;
            DragonMinion = false;
            GripMinion = false;
            ProbeMinion = false;
            SkullMinion = false;
            EaterMinion = false;
            CrimeraMinion = false;
            CrowMinion = false;
            DemonMinion = false;
            DevilMinion = false;
            DoomiteProbe = false;
            DoomiteProbeC = false;
            HallowedPrism = false;
            TrueHallowedPrism = false;
            TerraMinion = false;
            SnakeMinion = false;
            dustDevil = false;
            KrakenMinion = false;
            Fishnado = false;
            MadnessElemental = false;
            FlameSoul = false;
            Orbiters = false;
            Protocol = false;
            ScoutMinion = false;
            SagOrbiter = false;
            Rabbitcopter = false;
            RabbitcopterR = false;
            Sock = false;
            Socc = false;
            Squirrel = false;
            DapperSquirrel = false;
            CyberClaw = false;
            ChaosClaw = false;
            MiniZero = false;
            TerraSummon = false;
            DragonSpirit = false;
            Seraph = false;
            Athena = false;
            Baron = false;
            Xiao = false;
            ChaosConstruct = false;
            CCBook = false;
            CCBookEX = false;
            WeakCCRune = false;
            CCRune = false;
        }

        private void ResetArmorEffect() {
            artifactJudgement = false;
            artifactGuilt = false;
            MoonSet = false;
            valkyrieSet = false;
            kindledSet = false;
            depthSet = false;
            demonSet = false;
            demonBonus = false;
            fleshrendSet = false;
            goblinSlayer = false;
            tribalSet = false;
            impSet = false;
            terraSet = false;
            chaosSet = false;
            DynaskullSet = false;
            zeroSet = false;
            zeroSet1 = false;
            entropySet = false;
            dracoSet = false;
            dreadSet = false;
            darkmatterSetMe = false;
            darkmatterSetRa = false;
            darkmatterSetMa = false;
            darkmatterSetSu = false;
            darkmatterSetTh = false;
            infinitySet = false;
            Alpha = false;
            Palladium = false;
            fulgurite = false;
            doomite = false;
            DarkmatterSet = false;
            perfectChaos = false;
            Assassin = false;
            AbyssalStealth = false;
            AsheFlame = false;
            Witch = false;
            ChaosMe = false;
            ChaosMe1 = false;
            ChaosRa = false;
            ChaosRa2 = false;
            ChaosMa = false;
            ChaosSu = false;
            Olympian = false;
            AncientGoldBody = false;
            AncientGoldLeg = false;
            AncientGoldSet = false;
            StripeManFish = false;
            StripeManOre = false;
            StripeManSpawn = false;
            StripeManSet = false;
            ChampionMe = false;
            ChampionRa = false;
            ChampionMa = false;
            ChampionSu = false;
            StoneSoldier = false;
            TerraMe = false;
            TerraRa = false;
            TerraSu = false;
            TerraMa = false;
        }

        private void ResetAccessoryEffect() {
            AshRemover = false;
            FogRemover = false;
            clawsOfChaos = false;
            HydraPendant = false;
            demonGauntlet = false;
            BrokenCode = false;
            Baolei = false;
            Duality = false;
            Naitokurosu = false;
            ammo20percentdown = false;
            AADash = 0;
            DiscordShredder = false;
            RStar = false;
            DVoid = false;
            lantern = false;
            HeartP = false;
            HeartS = false;
            HeartA = false;
            BlackLotusEmblem = false;
            SagShield = false;
            ShieldUp = false;
            DragonsGuard = false;
            ShadowBand = false;
            RajahCape = false;
            GreedCharm = false;
            GreedTalisman = false;
            Greed1 = false;
            Greed2 = false;
            olympianWings = false;
            OldOneCharm = false;
            SpellBookofRagnarok = false;
            CursedEyeofSoulBinder = false;
            Replicator = false;
            RealityStone = false;
            ono = false;
        }

        private void ResetBuffEffect() {
            Ronin = false;
            luckycalm = false;
            luckythorns = false;
            CrasyLucky = false;
        }

        private void ResetDebuffEffect() {
            CursedHellfire = false;
            infinityOverload = false;
            discordInferno = false;
            dragonFire = false;
            hydraToxin = false;
            terraBlaze = false;
            Clueless = false;
            Yanked = false;
            InfinityScorch = false;
            LockedOn = false;
            shroomed = false;
            riftbent = false;
            DestinedToDie = false;
            YamataGravity = false;
            YamataAGravity = false;
            Hunted = false;
            IB = false;
            Spear = false;
            AkumaPain = false;
            Greed1 = false;
            Greed2 = false;
            FFlames = false;
        }

        private void ResetPetsEffect() {
            Broodmini = false;
            Raidmini = false;
            MiniProbe = false;
            Sharkron = false;
            RoyalKitten = false;
            Mudkip = false;
            MudkipS = false;
            BoomBoi = false;
            DragonSoul = false;
            Glowmoss = false;
            Cerberus = false;
            K9 = false;
            Lunamini = false;
            ZeroBab = false;
        }

        public override void Initialize() {
            AbilityCD = 0;
            ManaLantern = 0;
            WorldgenReminder = false;
            GivenAnuSummon = false;
            GivenWormIdol = false;
        }

        #endregion

        #region Hit Effects

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit) {
            if (npc.HasBuff(mod.BuffType("ForsakenWeak"))) {
                damage -= damage/5;
            }

            if (luckythorns) {
                if (player.whoAmI == Main.myPlayer && !player.immune && !npc.dontTakeDamage) {
                    int RDamage = (int)(npc.damage * player.allDamage * 0.433f);
                    int direc = -1;
                    if (npc.position.X + npc.width / 2 < player.position.X + player.width / 2) {
                        direc = 1;
                    }
                    player.ApplyDamageToNPC(npc, RDamage, 10f, -direc, false);
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) {
            if (Palladium) {
                player.AddBuff(BuffID.RapidHealing, 300);
            }

            if (StoneSoldier) {
                if (target.life <= 0 && Main.rand.Next(80) == 0) {
                    Projectile.NewProjectile(target.Center, Vector2.Zero, ProjectileID.CoinPortal, 0, 0, Main.myPlayer);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) {
            if (TerraRa && proj.ranged && Main.rand.Next(3) == 0) {
                float screenX;
                float screenY;
                if (Main.rand.Next(2) == 0) {
                    screenX = Main.screenPosition.X;
                    if (Main.rand.Next(2) == 0) {
                        screenX += Main.screenWidth;
                    }
                    screenY = Main.screenPosition.Y;
                    screenY += Main.rand.Next(Main.screenHeight);
                } else {
                    screenY = Main.screenPosition.Y;
                    if (Main.rand.Next(2) == 0) {
                        screenY += Main.screenHeight;
                    }
                    screenX = Main.screenPosition.X;
                    screenX += Main.rand.Next(Main.screenWidth);
                }
                Vector2 vector = new Vector2(screenX, screenY);
                float velocityX = target.Center.X - vector.X;
                float velocityY = target.Center.Y - vector.Y;
                velocityX += Main.rand.Next(-50, 51) * 0.1f;
                velocityY += Main.rand.Next(-50, 51) * 0.1f;
                float num6 = 24 / (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY);
                velocityX *= num6;
                velocityY *= num6;
                Projectile p = Projectile.NewProjectileDirect(new Vector2(screenX, screenY), new Vector2(velocityX, velocityY), ModContent.ProjectileType<Items.Armor.Terra.Projectiles.TerraBullet>(), damage / 3, 0f, player.whoAmI);
                p.tileCollide = false;
            }

            if (Palladium) {
                player.AddBuff(BuffID.RapidHealing, 300);
            }

            if (StoneSoldier) {
                target.AddBuff(BuffID.Midas, 600);
                if (target.life <= 0 && Main.rand.Next(80) == 0) {
                    Projectile.NewProjectile(target.Center, Vector2.Zero, ProjectileID.CoinPortal, 0, 0, Main.myPlayer);
                }
            }

            if (target.HasBuff(mod.BuffType("Forsaken")) && proj.type == mod.ProjectileType("EnchancedMummyArrow")) {
                float num1 = 9f;
                Vector2 vector2 = new Vector2(player.position.X + player.width * 0.5f, player.position.Y + player.height * 0.5f);
                float f1 = target.Center.X - vector2.X;
                float f2 = target.Center.Y - vector2.Y;
                float num4 = (float)Math.Sqrt(f1 * (double)f1 + f2 * (double)f2);
                float num5;
                if (float.IsNaN(f1) && float.IsNaN(f2) || f1 == 0.0 && f2 == 0.0) {
                    f1 = player.direction;
                    f2 = 0.0f;
                    num5 = num1;
                } else
                    num5 = num1 / num4;
                float SpeedX = f1 * num5;
                float SpeedY = f2 * num5;

                float numberProjectiles = 3;
                float rotation = MathHelper.ToRadians(3);
                vector2 += Vector2.Normalize(new Vector2(SpeedX, SpeedY)) * 45f;
                for (int i = 0; i < numberProjectiles; i++) {
                    Vector2 perturbedSpeed = new Vector2(SpeedX, SpeedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
                    Projectile.NewProjectile(vector2.X, vector2.Y, perturbedSpeed.X*2, perturbedSpeed.Y*2, mod.ProjectileType("ForsakenArrow"), damage/2, knockback, player.whoAmI);
                }
                target.buffImmune[mod.BuffType("Forsaken")] = true;
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit) {
            if (artifactJudgement) {
                artifactJudgementCharge += damage;
            }
            if (artifactGuilt) {
                artifactGuiltCharge += damage;
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit) {
            if (TerraMe) {
                Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("TerraSphere"), 30, 4, Main.myPlayer, 0, npc.whoAmI);
            }

            if (DragonsGuard || ChaosMe) {
                npc.AddBuff(BuffID.OnFire, 120);
            }

            if (artifactJudgement) {
                artifactJudgementCharge += damage;
            }
            if (artifactGuilt) {
                artifactGuiltCharge += damage;
            }

            if (fleshrendSet && Main.rand.Next(2) == 0) {
                if (player.whoAmI == Main.myPlayer) {
                    for (int i = 0; i < 40; i++) {
                        Vector2 position = new Vector2(player.Center.X - 40, player.Center.Y - 40);
                        Dust.NewDust(position, 80, 80, 108, 0f, 0f, 124, new Color(255, 50, 0), 1f);
                    }

                    for (int i = 0; i < Main.maxNPCs; i++) {
                        NPC target = Main.npc[i];
                        float dist = npc.Distance(player.Center);

                        if (target.active && !target.dontTakeDamage && !target.friendly && target.immune[player.whoAmI] == 0 && dist < 100f) {
                            player.ApplyDamageToNPC(target, 30, 0, 0, false); // target , damage, knockback, direction, crit
                        }
                    }
                }
            }

            if (ChaosMe) {
                npc.AddBuff(ModContent.BuffType<DragonFire>(), 180);
                npc.AddBuff(ModContent.BuffType<HydraToxin>(), 180);
            }

            if (npc.type == NPCID.GoblinArcher
                || npc.type == NPCID.GoblinPeon
                || npc.type == NPCID.GoblinScout
                || npc.type == NPCID.GoblinSorcerer
                || npc.type == NPCID.GoblinSummoner
                || npc.type == NPCID.GoblinThief
                || npc.type == NPCID.GoblinWarrior
                || npc.type == NPCID.DD2GoblinBomberT1
                || npc.type == NPCID.DD2GoblinBomberT2
                || npc.type == NPCID.DD2GoblinBomberT3
                || npc.type == NPCID.DD2GoblinT1
                || npc.type == NPCID.DD2GoblinT2
                || npc.type == NPCID.DD2GoblinBomberT3
                || npc.type == NPCID.BoundGoblin
                || npc.type == NPCID.GoblinTinkerer) {
                player.endurance += .8f;
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) {
            if (goblinSlayer) {
                if (target.type == NPCID.GoblinArcher
                    || target.type == NPCID.GoblinPeon
                    || target.type == NPCID.GoblinScout
                    || target.type == NPCID.GoblinSorcerer
                    || target.type == NPCID.GoblinSummoner
                    || target.type == NPCID.GoblinThief
                    || target.type == NPCID.GoblinWarrior
                    || target.type == NPCID.DD2GoblinBomberT1
                    || target.type == NPCID.DD2GoblinBomberT2
                    || target.type == NPCID.DD2GoblinBomberT3
                    || target.type == NPCID.DD2GoblinT1
                    || target.type == NPCID.DD2GoblinT2
                    || target.type == NPCID.DD2GoblinBomberT3
                    || target.type == NPCID.BoundGoblin
                    || target.type == NPCID.GoblinTinkerer) {
                    damage *= 5;
                    IsGoblin = true;
                }
            }

            if (perfectChaosMe) {
                target.AddBuff(ModContent.BuffType<DiscordInferno>(), 300);
            }

            if (valkyrieSet) {
                target.AddBuff(BuffID.Frostburn, 180);
                target.AddBuff(BuffID.Chilled, 180);
            }

            if (Baolei) {
                int buff = Main.dayTime ? BuffID.Daybreak : BuffID.OnFire;
                target.AddBuff(buff, 1000);
            }

            if (Naitokurosu) {
                int buff = Main.dayTime ? BuffID.Venom : ModContent.BuffType<Moonraze>();
                target.AddBuff(buff, 1000);
            }

            if (Duality) {
                int buff = Main.dayTime ? BuffID.Daybreak : ModContent.BuffType<Moonraze>();
                target.AddBuff(buff, 1000);
            }

            if (darkmatterSetMe) {
                target.AddBuff(mod.BuffType("Electrified"), 500);
            }

            if (kindledSet) {
                player.magmaStone = true;
            }

            if (clawsOfChaos) {
                player.ApplyDamageToNPC(target, 5, 0, 0, false);
            }

            if (DiscordShredder) {
                player.ApplyDamageToNPC(target, 30, 0, 0, false);
                target.AddBuff(ModContent.BuffType<DiscordInferno>(), 300);
            }

            if (demonGauntlet) {
                int buff = WorldGen.crimson ? BuffID.Ichor : BuffID.CursedInferno;
                target.AddBuff(buff, 180);
            }

            if (HeartP && player.statLife > player.statLifeMax / 3) {
                target.AddBuff(ModContent.BuffType<DragonFire>(), 600);
            } else if (HeartP && player.statLife < player.statLifeMax / 3) {
                target.AddBuff(BuffID.Daybreak, 600);
            }

            if (HeartS && player.statLife > player.statLifeMax / 3) {
                target.AddBuff(ModContent.BuffType<HydraToxin>(), 600);
            } else if (HeartS && player.statLife < player.statLifeMax / 3) {
                target.AddBuff(ModContent.BuffType<Moonraze>(), 600);
            }

            if (dracoSet) {
                target.AddBuff(BuffID.Daybreak, 600);
            }

            if (Alpha && !target.boss) {
                target.AddBuff(BuffID.Wet, 600);
            }

            if (player.HasBuff(mod.BuffType("DragonfireFlaskBuff"))) {
                target.AddBuff(mod.BuffType("DragonFire"), 900);
            }

            if (player.HasBuff(mod.BuffType("HydratoxinFlaskBuff"))) {
                target.AddBuff(mod.BuffType("Hydratoxin"), 900);
            }
            if (StoneSoldier) {
                target.AddBuff(BuffID.Midas, 600);
            }

            if (ChampionMa) {
                if (Main.rand.Next(30) == 0) {
                    int i = Item.NewItem(target.Hitbox, mod.ItemType("CarrotBooster"), 1, false, 0, true);
                    Main.item[i].velocity = new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
                }
            }
        }


        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (proj.melee) {
                if (perfectChaosMe) {
                    target.AddBuff(ModContent.BuffType<DiscordInferno>(), 300);
                }

                if (dracoSet) {
                    target.AddBuff(BuffID.Daybreak, 600);
                }

                if (valkyrieSet) {
                    target.AddBuff(BuffID.Frostburn, 180);
                    target.AddBuff(BuffID.Chilled, 180);
                }

                if (darkmatterSetMe) {
                    target.AddBuff(mod.BuffType("Electrified"), 500);
                }

                if (ChaosMe || ChaosMe1) {
                    string buffName = Main.rand.Next(2) == 0 ? "DragonFire" : "HydraToxin";
                    target.AddBuff(mod.BuffType(buffName), 180);
                }

                if (demonGauntlet) {
                    int buff = WorldGen.crimson ? BuffID.Ichor : BuffID.CursedInferno;
                    target.AddBuff(buff, 180);
                }

                if (player.HasBuff(mod.BuffType("DragonfireFlaskBuff"))) {
                    target.AddBuff(mod.BuffType("DragonFire"), 900);
                }

                if (player.HasBuff(mod.BuffType("HydratoxinFlaskBuff"))) {
                    target.AddBuff(mod.BuffType("Hydratoxin"), 900);
                }
            }

            if (proj.ranged) {
                if (perfectChaosRa) {
                    target.AddBuff(ModContent.BuffType<DiscordInferno>(), 300);
                }

                if (dreadSet) {
                    target.AddBuff(ModContent.BuffType<Moonraze>(), 600);
                }

                if (DynaskullSet && Main.rand.Next(4) == 0) {
                    target.AddBuff(BuffID.Confused, 180);
                }

                if (depthSet) {
                    target.AddBuff(BuffID.Poisoned, 180);
                }

                if (darkmatterSetRa) {
                    target.AddBuff(mod.BuffType("Electrified"), 500);
                }

                if (ChaosRa || ChaosRa2) {
                    string buffName = Main.rand.Next(2) == 0 ? "DragonFire" : "HydraToxin";
                    target.AddBuff(mod.BuffType(buffName), 180);
                }
            }

            if (proj.magic) {
                if (MoonSet) {
                    target.AddBuff(ModContent.BuffType<Moonraze>(), 300);
                }

                if (zeroSet) {
                    target.AddBuff(ModContent.BuffType<BrokenArmor>(), 1000);
                }

                if (perfectChaosMa) {
                    target.AddBuff(ModContent.BuffType<DiscordInferno>(), 300);
                }

                if (darkmatterSetMa) {
                    target.AddBuff(mod.BuffType("Electrified"), 500);
                }

                if (ChaosMa) {
                    string buffName = Main.rand.Next(2) == 0 ? "DragonFire" : "HydraToxin";
                    target.AddBuff(mod.BuffType(buffName), 180);
                }

                if (BlackLotusEmblem) {
                    target.AddBuff(mod.BuffType("Moonraze"), 180);
                }
            }

            if (proj.minion) {
                if (zeroSet1) {
                    target.AddBuff(ModContent.BuffType<BrokenArmor>(), 1000);
                }

                if (perfectChaosSu) {
                    target.AddBuff(ModContent.BuffType<DiscordInferno>(), 300);
                }

                if (impSet) {
                    target.AddBuff(BuffID.OnFire, 180);
                }

                if (darkmatterSetSu) {
                    target.AddBuff(mod.BuffType("Electrified"), 500);
                }
            }

            if (proj.thrown) {
                if (darkmatterSetTh) {
                    target.AddBuff(mod.BuffType("Electrified"), 500);
                }

                if (Alpha && Main.rand.Next(2) == 0 && !target.boss) {
                    target.AddBuff(BuffID.Wet, 500);
                }
            }

            if (ChampionMa) {
                if (Main.rand.Next(30) == 0) {
                    int i = Item.NewItem(target.Hitbox, mod.ItemType("CarrotBooster"), 1, false, 0, true);
                    Main.item[i].velocity = new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
                }
            }

            if (Baolei && (proj.melee || proj.magic)) {
                int buff = Main.dayTime ? BuffID.Daybreak : BuffID.OnFire;
                target.AddBuff(buff, 1000);
            }

            if (Naitokurosu && (proj.ranged || proj.minion)) {
                int buff = Main.dayTime ? BuffID.Venom : ModContent.BuffType<Moonraze>();
                target.AddBuff(buff, 1000);
            }

            if (Duality) {
                int buff = Main.dayTime ? BuffID.Daybreak : ModContent.BuffType<Moonraze>();
                target.AddBuff(buff, 1000);
            }

            if (clawsOfChaos) {
                player.ApplyDamageToNPC(target, 5, 0, 0, false);
            }

            if (DiscordShredder) {
                player.ApplyDamageToNPC(target, 30, 0, 0, false);
                target.AddBuff(ModContent.BuffType<DiscordInferno>(), 300);
            }

            if (StoneSoldier) {
                target.AddBuff(BuffID.Midas, 600);
            }
        }

        #endregion

        public override void OnRespawn(Player player) {
            base.OnRespawn(player);

            if (MegaMush && player.statLife < player.statLifeMax2) player.statLife = player.statLifeMax2;
        }

        public int[] Charges = null;
        public int[] Spheres = null;

        public float ShieldScale = 0;
        public float RingRotation = 0;

        public float TimeScale = 0;

        public override void PostItemCheck() {
            if (ItemLoader.GetItem(player.inventory[player.selectedItem].type) is UpdateDuringUseItem item) {
                item.UpdateItemUse(player);
            }
        }

        public override void PostUpdate() {
            if (!bossactive) {
                nohitplayer = true;
            }
            if (Ronin) {
                player.immune = true;
                player.immuneTime = 60;
            }
            if (olympianWings && player.dash < 1) {
                if (player.velocity.Y != 0) {
                    player.dash = 2;
                } else {
                    player.dash = 0;
                }
            }
            if (artifactJudgementCharge >= 250) {
                player.AddBuff(mod.BuffType("EyeOfJudgement"), 900);
                artifactJudgementCharge = 0;
            }
            if (artifactGuiltCharge >= 250) {
                player.AddBuff(mod.BuffType("EyeOfForsaken"), 900);
                artifactGuiltCharge = 0;
            }
            if (!Greed1 && !Greed2) {
                GreedyDamage = 0;
            }
            DarkmatterSet = darkmatterSetMe || darkmatterSetRa || darkmatterSetMa || darkmatterSetSu || darkmatterSetTh;

            if (NPC.AnyNPCs(ModContent.NPCType<AkumaTransition>())) {
                int n = BaseAI.GetNPC(player.Center, ModContent.NPCType<AkumaTransition>(), -1);
                NPC akuma = Main.npc[n];

                if (akuma.ai[0] >= 660) {
                    player.AddBuff(ModContent.BuffType<BlazingPain>(), 2);
                }
            } else if (NPC.AnyNPCs(ModContent.NPCType<AkumaA>())) {
                player.AddBuff(ModContent.BuffType<BlazingPain>(), 2);
            }

            if (BasePlayer.HasAccessory(player, ModContent.ItemType<Items.Vanity.HappySunSticker>(), true, true)) {
                Main.sunTexture = mod.GetTexture("Backgrounds/DemonSun");
                Main.sun3Texture = mod.GetTexture("Backgrounds/DemonSunEclipse");
            } else {
                Main.sunTexture = ModContent.GetTexture("Terraria/Sun"); ;
                Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
            }

            #region SagShieldDrawMethod

            if (SagCooldown > 0) {
                SagCooldown--;
            } else {
                SagCooldown = 0;
            }

            if (ShieldUp) {
                RingRotation += .05f;
                ShieldScale += .02f;
                if (ShieldScale >= 1f) {
                    ShieldScale = 1f;
                }
            } else {
                ShieldScale -= .02f;
                if (ShieldScale <= 0f) {
                    ShieldScale = 0f;
                }
            }

            if (ShieldScale > 0f || TimeScale > 0f) {
                RingRotation += .05f;
            }

            if (ShieldScale > 0) {
                RingRotation += .05f;
            }

            #endregion

            #region AsheFlameDrawMethod

            if (AsheCooldown > 0) {
                AsheCooldown--;
            } else {
                AsheCooldown = 0;
            }

            if (AsheFlame) {
                RingRotation += .05f;
                AsheFlameScale += .02f;
                if (AsheFlameScale >= 1f) {
                    AsheFlameScale = 1f;
                }
            } else {
                AsheFlameScale -= .02f;
                if (AsheFlameScale <= 0f) {
                    AsheFlameScale = 0f;
                }
            }

            if (AsheFlameScale > 0f) {
                RingRotation += .05f;
            }

            #endregion

            if (EntropyCooldown > 0) {
                EntropyCooldown--;
            }

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Equinox.DaybringerHead>()) || NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Equinox.NightcrawlerHead>())) {
                TimeScale = 0;
            }

            if (Orbiters) {
                Spheres = BaseAI.GetProjectiles(player.Center, mod.ProjectileType("FireOrbiter"), Main.myPlayer, 48);

                if (player.ownedProjectileCounts[mod.ProjectileType("FireOrbiter")] > 0) {
                    player.minionDamage += AAGlobalProjectile.CountProjectiles(ModContent.ProjectileType<Projectiles.AH.FireOrbiter>()) * .1f;

                    if (Main.netMode != 2 && Main.LocalPlayer.miscCounter % 3 == 0) {
                        for (int m = 0; m < Spheres.Length; m++) {
                            Projectile projectile = Main.projectile[Spheres[m]];

                            if (projectile != null && projectile.active) {
                                int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.AkumaDustLight>());

                                Main.dust[dustID].position += player.position - player.oldPosition;
                                Main.dust[dustID].velocity = (player.Center - projectile.Center) * 0.05f;
                                Main.dust[dustID].alpha = 100;
                                Main.dust[dustID].noGravity = true;
                            }
                        }
                    }
                }
            }

            AABiomesPlayer biomes = player.GetModPlayer<AABiomesPlayer>();
            if (AAWorld.ModContentGenerated || biomes.ZoneInferno || biomes.ZoneMire || biomes.ZoneVoid || biomes.Terrarium || biomes.ZoneMush) {
                AAWorld.ModContentGenerated = true;
                WorldgenReminder = true;
            }

            if (!WorldgenReminder) {
                if (Main.rand.Next(8) == 0) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo1"), new Color(180, 41, 32), false);
                    }
                } else if (Main.rand.Next(8) == 1) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo2"), new Color(45, 46, 70), false);
                    }
                } else if (Main.rand.Next(8) == 2) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo3"), new Color(255, 0, 0), false);
                    }
                } else if (Main.rand.Next(8) == 3) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo4"), new Color(102, 20, 48), false);
                    }
                } else if (Main.rand.Next(8) == 4) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo5"), new Color(72, 78, 117), false);
                    }
                } else if (Main.rand.Next(8) == 5) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo6"), new Color(128, 0, 0), false);
                    }
                } else if (Main.rand.Next(8) == 6) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo7"), new Color(216, 110, 40), false);
                    }
                } else if (Main.rand.Next(8) == 7) {
                    if (Main.netMode != 1) {
                        BaseUtility.Chat(Language.GetTextValue("Mods.AAMod.Common.WorldgenReminderInfo8"), new Color(43, 46, 61), false);
                    }
                }

                WorldgenReminder = true;
            }

            if (RStar) {
                Lighting.AddLight((int)(player.position.X + player.width / 2) / 16, (int)(player.position.Y + player.height / 2) / 16, 1f, 0.95f, 0.8f);
            }

            if (kindledSet || lantern) {
                Lighting.AddLight((int)(player.position.X + player.width / 2) / 16, (int)(player.position.Y + player.height / 2) / 16, AAColor.Lantern.R / 255, AAColor.Lantern.G / 255 * 0.95f, AAColor.Lantern.B / 255 * 0.8f);
            }

            if (NPC.AnyNPCs(ModContent.NPCType<Yamata>())) {
                player.AddBuff(ModContent.BuffType<YamataGravity>(), 10, true);
            }

            if (NPC.AnyNPCs(ModContent.NPCType<YamataA>())) {
                player.AddBuff(ModContent.BuffType<YamataAGravity>(), 10, true);
            }

            if (NPC.AnyNPCs(ModContent.NPCType<ZeroProtocol>())) {
                if (!Filters.Scene["MoonLordShake"].IsActive()) {
                    Filters.Scene.Activate("MoonLordShake", player.position, new object[0]);
                }

                Filters.Scene["MoonLordShake"].GetShader().UseIntensity(1f);
            }

            if (player.GetModPlayer<AAPlayer>().Assassin) {
                float RandomX = 50f;
                float RandomY = 25f;
                bool flag = player.itemAnimation > 0;
                if (flag && player.inventory[player.selectedItem].melee && Main.rand.Next(200) == 0 && player.whoAmI == Main.myPlayer) {
                    Vector2 SpeedVector = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter, true);
                    SpeedVector.Normalize();
                    if (SpeedVector.HasNaNs()) {
                        SpeedVector = Vector2.UnitX * player.direction;
                    }
                    SpeedVector *= 15f;
                    Vector2[] Spwanposition = new Vector2[3];
                    Spwanposition[0] = new Vector2(player.Center.X + player.direction * Main.rand.NextFloat(25f, RandomX), player.Center.Y - Main.rand.NextFloat(-RandomY, RandomY));
                    Spwanposition[1] = new Vector2(player.Center.X - player.direction * Main.rand.NextFloat(25f, RandomX), player.Center.Y - Main.rand.NextFloat(-RandomY, RandomY));
                    Spwanposition[2] = new Vector2(player.Center.X - player.direction * Main.rand.NextFloat(25f, RandomX), player.Center.Y - Main.rand.NextFloat(-RandomY, RandomY));
                    int i = 0;
                    while (i < 3) {
                        if (Main.netMode != 1) Projectile.NewProjectile(Spwanposition[i].X, Spwanposition[i].Y, SpeedVector.X, SpeedVector.Y, mod.ProjectileType("AssassinDagger"), (int)(player.inventory[player.selectedItem].damage * 1.3), 2f, player.whoAmI, 0f, 1f);
                        float round = 16f;
                        int k = 0;
                        while (k < round) {
                            Vector2 vector12 = Vector2.UnitX * 0f;
                            vector12 += -Vector2.UnitY.RotatedBy(k * (6.28318548f / round), default) * new Vector2(1f, 4f);
                            vector12 = vector12.RotatedBy(SpeedVector.ToRotation(), default);
                            int Dusti = Dust.NewDust(Spwanposition[i], 0, 0, mod.DustType("AcidDust"), 0f, 0f, 0, default, 1f);
                            Main.dust[Dusti].scale = 1.5f;
                            Main.dust[Dusti].noGravity = true;
                            Main.dust[Dusti].position = Spwanposition[i] + vector12;
                            Main.dust[Dusti].velocity = vector12.SafeNormalize(Vector2.UnitY) * 1f;
                            k++;
                        }
                        i++;
                    }
                }
            }

            if (BlackLotusEmblem && player.inventory[player.selectedItem].mana > 0 && player.statMana < (int)(player.inventory[player.selectedItem].mana * player.manaCost)) {
                BlackLotusQuickMana();
            }

            if (player.controlQuickHeal) {
                SpecialQuickHeal();
            }

            if (StripeManSet) {
                if (AAMod.ArmorAbilityKey.JustPressed) {
                    StripeCrasyLucky = !StripeCrasyLucky;
                }
            }

            if (StripeCrasyLucky || CrasyLucky) {
                if (StripeCrasyLucky) StripeCrasyLucky = true;
                Main.rand = new AAFakeRand();
                if (Main.raining) {
                    Main.rainTime = 300;
                    Main.maxRaining = .7f;
                }
            } else {
                StripeCrasyLucky = false;
                Main.rand = new UnifiedRandom();
            }

            if (CCBook || CCBookEX) {
                float slotscanuse = player.maxMinions - player.slotsMinions;
                if (slotscanuse > 1) {
                    bool RuneControl = player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.BunnyRune>()] > 1 || player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.DiscordRune>()] > 1 || player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.EnergyRune>()] > 1;
                    bool RuneControlEX = player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.TerraRune>()] > 1 || player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.ChaosRune>()] > 1 || player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.VoidRune>()] > 1;
                    if (RuneControl || RuneControlEX) {
                        player.ClearBuff(ModContent.BuffType<CCRune>());
                    }
                    if (player.FindBuffIndex(ModContent.BuffType<CCRune>()) == -1) {
                        player.AddBuff(ModContent.BuffType<CCRune>(), 3600, true);
                    }
                    if (CCBook) {
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.BunnyRune>()] < 1 && slotscanuse > 1f) {
                            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<Items.Dev.RuneBook.BunnyRune>(), (int)(1 * player.minionDamage), 0, player.whoAmI, 0f, 0f);
                        }
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.DiscordRune>()] < 1 && slotscanuse > 2f) {
                            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<Items.Dev.RuneBook.DiscordRune>(), (int)(50 * player.minionDamage), 4f, player.whoAmI, 0f, 0f);
                        }
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.EnergyRune>()] < 1 && slotscanuse > 3f) {
                            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<Items.Dev.RuneBook.EnergyRune>(), (int)(100 * player.minionDamage), 2f, player.whoAmI, 0f, 0f);
                        }
                    }
                    if (CCBookEX) {
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.TerraRune>()] < 1 && slotscanuse > 1f) {
                            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<Items.Dev.RuneBook.TerraRune>(), (int)(1 * player.minionDamage), 0, player.whoAmI, 0f, 0f);
                        }
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.ChaosRune>()] < 1 && slotscanuse > 2f) {
                            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<Items.Dev.RuneBook.ChaosRune>(), (int)(400 * player.minionDamage), 4f, player.whoAmI, 0f, 0f);
                        }
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Dev.RuneBook.VoidRune>()] < 1 && slotscanuse > 3f) {
                            Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<Items.Dev.RuneBook.VoidRune>(), (int)(800 * player.minionDamage), 2f, player.whoAmI, 0f, 0f);
                        }
                    }
                }
            }

            if (ChampionMe && AAMod.ArmorAbilityKey.JustPressed && !player.HasBuff(ModContent.BuffType<Items.Armor.Champion.RageCool>())) {
                int BuffLength = 240;
                if (player.statLife < (int)(player.statLifeMax2 * .75f)) {
                    BuffLength = 360;
                }
                if (player.statLife < (int)(player.statLifeMax2 * .5f)) {
                    BuffLength = 480;
                }
                if (player.statLife < (int)(player.statLifeMax2 * .25f)) {
                    BuffLength = 600;
                }
                player.AddBuff(ModContent.BuffType<Items.Armor.Champion.RageBuff>(), BuffLength);
                int RageCooldown = BuffLength * 4;
                player.AddBuff(ModContent.BuffType<Items.Armor.Champion.RageCool>(), RageCooldown);
            }

            if (player.HasBuff(ModContent.BuffType<Items.Armor.Champion.RageBuff>())) {
                player.armorEffectDrawShadowLokis = true;
            }

            if (ChampionRa && AAMod.ArmorAbilityKey.JustPressed && !player.HasBuff(mod.BuffType("DroneCool")) &&
                !AAGlobalProjectile.AnyProjectiles(mod.ProjectileType("RajahDrone"))) {
                Vector2 vector2;
                vector2.X = Main.mouseX + Main.screenPosition.X;
                vector2.Y = Main.mouseY + Main.screenPosition.Y;
                Projectile.NewProjectile(vector2.X, vector2.Y, 0, 0, mod.ProjectileType("RajahDrone"), (int)(100 * player.rangedDamage), 2, Main.myPlayer, 0f, 0f);
            }

            if (TerraSu) {
                if (AAMod.ArmorAbilityKey.JustPressed) {
                    CrystalMode++;
                    if (CrystalMode > 2) {
                        CrystalMode = 0;
                    }
                }
                if (CrystalMode == 2) {
                    player.lifeRegen += 12;
                    player.statDefense = (int)(player.statDefense * 1.2f);
                    player.allDamage /= 2;
                }
            }

            if (RoseCooldown > 0) {
                RoseCooldown--;
            }

            if (TerraMa && RoseCooldown <= 0) {
                if (AAMod.ArmorAbilityKey.JustPressed) {
                    RoseCooldown = 600;
                    float playerY = player.position.Y + player.height;

                    Projectile.NewProjectile(new Vector2(player.Center.X - 64, playerY), new Vector2(0, -10), ModContent.ProjectileType<Items.Armor.Terra.Projectiles.TerraRoseA>(), (int)(50 * player.magicDamage), 4, Main.myPlayer);
                    Projectile.NewProjectile(new Vector2(player.Center.X + 64, playerY), new Vector2(0, -10), ModContent.ProjectileType<Items.Armor.Terra.Projectiles.TerraRoseA>(), (int)(50 * player.magicDamage), 4, Main.myPlayer);
                }
            }
        }

        public void CarrotLevelup() {
            if (player.whoAmI == Main.myPlayer) {
                for (int i = 0; i < 22; i++) {
                    if (player.buffType[i] == mod.BuffType("CBoost1") ||
                        player.buffType[i] == mod.BuffType("CBoost2") ||
                        player.buffType[i] == mod.BuffType("CBoost3")) {
                        player.DelBuff(i);
                    }
                }
                CarrotBuff = (int)MathHelper.Clamp(CarrotBuff + 1, 0f, 3f);
                player.AddBuff(mod.BuffType("CBoost" + CarrotBuff), 480, true);
                return;
            }
        }

        public void SpecialQuickHeal() {
            if (player.noItems) {
                return;
            }
            Item item = new Item();
            for (int i = 0; i < 58; i++) {
                item = player.inventory[i];
                if (item.type == mod.ItemType("RoninPotion") && ItemLoader.CanUseItem(item, player)) {
                    break;
                }
            }
            if (item == null) {
                return;
            }
            if (player.potionDelay > 0 || player.statLife == player.statLifeMax2 && item.type != mod.ItemType("RoninPotion")) {
                return;
            }
            Main.PlaySound(item.UseSound, player.position);
            if (item.potion) {
                if (item.type == 227) {
                    player.potionDelay = player.restorationDelayTime;
                    player.AddBuff(21, player.potionDelay, true);
                } else {
                    player.potionDelay = player.potionDelayTime;
                    player.AddBuff(21, player.potionDelay, true);
                }
            }
            ItemLoader.UseItem(item, player);
            player.statLife += item.healLife;
            player.statMana += item.healMana;
            if (player.statLife > player.statLifeMax2) {
                player.statLife = player.statLifeMax2;
            }
            if (player.statMana > player.statManaMax2) {
                player.statMana = player.statManaMax2;
            }
            if (item.healLife > 0 && Main.myPlayer == player.whoAmI) {
                player.HealEffect(item.healLife, true);
            }
            if (item.healMana > 0) {
                player.AddBuff(94, Player.manaSickTime, true);
                if (Main.myPlayer == player.whoAmI) {
                    player.ManaEffect(item.healMana);
                }
            }
            if (ItemLoader.ConsumeItem(item, player)) {
                item.stack--;
            }
            if (item.stack <= 0) {
                item.TurnToAir();
            }
            Recipe.FindRecipes();
        }

        public void BlackLotusQuickMana() {
            if (player.noItems) {
                return;
            }
            if (player.statMana == player.statManaMax2) {
                return;
            }
            for (int i = 0; i < 58; i++) {
                if (player.inventory[i].stack > 0 && player.inventory[i].type > 0 && player.inventory[i].healMana > 0 && (player.potionDelay == 0 || !player.inventory[i].potion) && ItemLoader.CanUseItem(player.inventory[i], player)) {
                    Main.PlaySound(player.inventory[i].UseSound, player.position);
                    if (player.inventory[i].potion) {
                        if (player.inventory[i].type == 227) {
                            player.potionDelay = player.restorationDelayTime;
                            player.AddBuff(21, player.potionDelay, true);
                        } else {
                            player.potionDelay = player.potionDelayTime;
                            player.AddBuff(21, player.potionDelay, true);
                        }
                    }
                    ItemLoader.UseItem(player.inventory[i], player);
                    player.statLife += player.inventory[i].healLife;
                    player.statMana += player.inventory[i].healMana;
                    if (player.statLife > player.statLifeMax2) {
                        player.statLife = player.statLifeMax2;
                    }
                    if (player.statMana > player.statManaMax2) {
                        player.statMana = player.statManaMax2;
                    }
                    if (player.inventory[i].healLife > 0 && Main.myPlayer == player.whoAmI) {
                        player.HealEffect(player.inventory[i].healLife, true);
                    }
                    if (player.inventory[i].healMana > 0) {
                        player.AddBuff(94, 60, true);
                        if (Main.myPlayer == player.whoAmI) {
                            player.ManaEffect(player.inventory[i].healMana);
                        }
                    }
                    if (ItemLoader.ConsumeItem(player.inventory[i], player)) {
                        player.inventory[i].stack--;
                    }
                    if (player.inventory[i].stack <= 0) {
                        player.inventory[i].TurnToAir();
                    }
                    Recipe.FindRecipes();
                    return;
                }
            }
        }

        public override void PostUpdateBuffs() {
            if (player.mount.Active || player.mount.Cart) {
                player.dashDelay = 60;
                AADash = 0;
            }
        }

        public override void PostUpdateEquips() {
            if (player.mount.Active || player.mount.Cart) {
                player.dashDelay = 60;
                AADash = 0;
            }
        }

        public override void PostUpdateRunSpeeds() {
            float movespeedmax = 1f + MaxMovespeedboost;

            player.maxRunSpeed *= movespeedmax;

            if (player.pulley && AADash > 0) {
                AADashMovement();
            } else if (player.grappling[0] == -1 && !player.tongued) {
                AAHorizontalMovement();
                if (AADash > 0) {
                    AADashMovement();
                }
            }
        }

        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if (item.ranged && Assassin) {
                speedX *= 1.3f;
                speedY *= 1.3f;
                Vector2 SpeedVector = new Vector2(speedX, speedY);
                if (Main.rand.Next(10) == 0 && player.whoAmI == Main.myPlayer) {
                    float RandomX = 50f;
                    float RandomY = 25f;
                    Vector2[] Spwanposition = new Vector2[3];
                    Spwanposition[0] = new Vector2(player.Center.X + player.direction * Main.rand.NextFloat(25f, RandomX), player.Center.Y - Main.rand.NextFloat(-RandomY, RandomY));
                    Spwanposition[1] = new Vector2(player.Center.X - player.direction * Main.rand.NextFloat(25f, RandomX), player.Center.Y - Main.rand.NextFloat(-RandomY, RandomY));
                    Spwanposition[2] = new Vector2(player.Center.X - player.direction * Main.rand.NextFloat(25f, RandomX), player.Center.Y - Main.rand.NextFloat(-RandomY, RandomY));
                    for (int i = 0; i < 3; i++) {
                        Projectile.NewProjectile(Spwanposition[i].X, Spwanposition[i].Y, speedX, speedY, mod.ProjectileType("AssassinArrow"), (int)(item.damage * 1.3), 2f, player.whoAmI, 0f, 1f);
                        float round = 16f;
                        int k = 0;
                        while (k < round) {
                            Vector2 vector12 = Vector2.UnitX * 0f;
                            vector12 += -Vector2.UnitY.RotatedBy(k * (6.28318548f / round), default) * new Vector2(1f, 4f);
                            vector12 = vector12.RotatedBy(SpeedVector.ToRotation(), default);
                            int Dusti = Dust.NewDust(Spwanposition[i], 0, 0, mod.DustType("AcidDust"), 0f, 0f, 0, default, 1f);
                            Main.dust[Dusti].scale = 1.5f;
                            Main.dust[Dusti].noGravity = true;
                            Main.dust[Dusti].position = Spwanposition[i] + vector12;
                            Main.dust[Dusti].velocity = vector12.SafeNormalize(Vector2.UnitY) * 1f;
                            k++;
                        }
                    }
                }
            }
            return true;
        }

        public void AAHorizontalMovement() {
            float runSpeed = (player.accRunSpeed + player.maxRunSpeed) / 2f;
            if (player.controlLeft && player.velocity.X > -player.accRunSpeed && player.dashDelay >= 0) {
                if (player.velocity.X < -runSpeed && player.velocity.Y == 0f && !player.mount.Active) {
                    if (AADash == 1 && Main.rand.Next(50) == 0) {
                        int dust = Dust.NewDust(new Vector2(player.position.X - 4f, player.position.Y), player.width + 8, 4, ModContent.DustType<Feather>(), -player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 50, default, 1.5f);
                        Main.dust[dust].velocity.X = Main.dust[dust].velocity.X * 0.2f;
                        Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y * 0.2f;
                        Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
                    }
                }
            } else if (player.controlRight && player.velocity.X < player.accRunSpeed && player.dashDelay >= 0) {
                if (player.velocity.X > runSpeed && player.velocity.Y == 0f && !player.mount.Active) {
                    if (AADash == 1 && Main.rand.Next(50) == 0) {
                        int dust = Dust.NewDust(new Vector2(player.position.X - 4f, player.position.Y), player.width + 8, 4, ModContent.DustType<Feather>(), -player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 50, default, 1.5f);
                        Main.dust[dust].velocity.X = Main.dust[dust].velocity.X * 0.2f;
                        Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y * 0.2f;
                        Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
                    }
                }
            }
        }

        public void AADashMovement() {
            if (player.dashDelay > 0) {
                return;
            }
            if (player.dashDelay < 0) {
                float num7 = 12f;
                float num8 = 0.985f;
                float num9 = Math.Max(player.accRunSpeed, player.maxRunSpeed);
                float num10 = 0.94f;
                int num11 = 20;
                if (AADash == 1) {
                    for (int k = 0; k < 2; k++) {
                        int num12;
                        if (player.velocity.Y == 0f) {
                            num12 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 8, ModContent.DustType<Feather>(), 0f, 0f, 100, default, 1);
                        } else {
                            num12 = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height / 2 - 8f), player.width, 16, ModContent.DustType<Feather>(), 0f, 0f, 100, default, 1);
                        }
                        Main.dust[num12].velocity *= 0.1f;
                        Main.dust[num12].scale *= 1f + Main.rand.Next(20) * 0.01f;
                        Main.dust[num12].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
                    }
                }
                if (AADash > 0) {
                    player.vortexStealthActive = false;
                    if (player.velocity.X > num7 || player.velocity.X < -num7) {
                        player.velocity.X = player.velocity.X * num8;
                        return;
                    }
                    if (player.velocity.X > num9 || player.velocity.X < -num9) {
                        player.velocity.X = player.velocity.X * num10;
                        return;
                    }
                    player.dashDelay = num11;
                    if (player.velocity.X < 0f) {
                        player.velocity.X = -num9;
                        return;
                    }
                    if (player.velocity.X > 0f) {
                        player.velocity.X = num9;
                        return;
                    }
                }
            } else if (AADash > 0 && !player.mount.Active) {
                if (AADash == 1) {
                    int direction = 0;
                    bool DashAttempt = false;
                    if (AADashTime > 0) {
                        AADashTime--;
                    }
                    if (AADashTime < 0) {
                        AADashTime++;
                    }
                    if (player.controlRight && player.releaseRight && player.velocity.Y != 0) {
                        if (AADashTime > 0) {
                            direction = 1;
                            DashAttempt = true;
                            AADashTime = 0;
                        } else {
                            AADashTime = 15;
                        }
                    } else if (player.controlLeft && player.releaseLeft && player.velocity.Y != 0) {
                        if (AADashTime < 0) {
                            direction = -1;
                            DashAttempt = true;
                            AADashTime = 0;
                        } else {
                            AADashTime = -15;
                        }
                    }
                    if (DashAttempt) {
                        player.velocity.X = 14.5f * direction;
                        Point point = (player.Center + new Vector2(direction * player.width / 2 + 2, player.gravDir * -player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();
                        Point point2 = (player.Center + new Vector2(direction * player.width / 2 + 2, 0f)).ToTileCoordinates();
                        if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y)) {
                            player.velocity.X = player.velocity.X / 2f;
                        }
                        player.dashDelay = -1;
                        for (int num17 = 0; num17 < 2; num17++) {
                            int num18 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, ModContent.DustType<Feather>(), 0f, 0f, 100, default, 1);
                            Dust expr_CDB_cp_0 = Main.dust[num18];
                            expr_CDB_cp_0.position.X += Main.rand.Next(-5, 6);
                            Dust expr_D02_cp_0 = Main.dust[num18];
                            expr_D02_cp_0.position.Y += Main.rand.Next(-5, 6);
                            Main.dust[num18].velocity *= 0.2f;
                            Main.dust[num18].scale *= .1f + Main.rand.Next(20) * 0.01f;
                            Main.dust[num18].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
                        }
                        return;
                    }
                }
            }
        }

        #region Dev Armor

        public void DropDevArmor(int dropType) {
            //0 = Pre-HM
            //1 = HM
            //2 = Post-Plant
            //3 = PML
            //4 = PA
            string addonEX = dropType == 4 ? "EX" : ""; //only include EX if it's a dropType 3 (ie from ancients)

            bool spawnedDevItems = false; //this prevents it from not dropping anything if the chance lands on something it cannot drop yet (for prehm/hm) as by this point it's past the 10% chance and thus should drop.
            while (!spawnedDevItems) {
                int choice = Main.rand.Next(40);

                switch (choice) {
                    case 0:

                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Hallam.MagiciansHat>());

                        if (dropType >= 4) {
                            player.QuickSpawnItem(mod.ItemType("Prismeow" + addonEX));
                        }

                        spawnedDevItems = true;
                        break;

                    case 1:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Alphakip.AlphaBag>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("AmphibianLongsword" + addonEX));
                        }

                        if (dropType >= 4) {
                            player.QuickSpawnItem(mod.ItemType("AlphakipTerratool"));
                        }

                        spawnedDevItems = true;
                        break;

                    case 2:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Beg.BegBag>());

                        if (dropType >= 1) {
                            player.QuickSpawnItem(mod.ItemType("MonochromeApple"));
                        }

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("PoniumStaff" + addonEX));
                        }

                        spawnedDevItems = true;
                        break;

                    case 3:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Maskano.MaskBag>());

                        spawnedDevItems = true;
                        break;

                    case 4:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Charlie.CharlieBag>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("SoulSiphon"));
                        }
                        break;

                    case 5:
                        player.QuickSpawnItem(mod.ItemType("TailsHead"));
                        player.QuickSpawnItem(mod.ItemType("TailsBody"));
                        player.QuickSpawnItem(mod.ItemType("TailsLegs"));

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType(dropType == 4 ? "FreedomStar" : "MobianBuster"));
                        }

                        spawnedDevItems = true;
                        break;

                    case 6:
                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("SkrallStaff"));
                            spawnedDevItems = true;
                        }

                        break;

                    case 7:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Delly.DellyBag>());

                        break;

                    case 8:
                        player.QuickSpawnItem(mod.ItemType("FezLordsBag"));

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType(dropType == 4 ? "Chronos" : "TimeTeller"));
                        }

                        spawnedDevItems = true;
                        break;

                    case 9:
                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("TitanAxe" + addonEX));
                            spawnedDevItems = true;
                        }

                        break;

                    case 10:
                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("EnderStaff" + addonEX));
                            spawnedDevItems = true;
                        }

                        break;

                    case 11:

                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Eliza.LizBag>());

                        if (dropType >= 3) {
                            if (Main.rand.Next(2) == 0) {
                                player.QuickSpawnItem(mod.ItemType("CatsEyeRifle" + addonEX));
                            } else {
                                player.QuickSpawnItem(mod.ItemType(dropType == 4 ? "ArchwitchStaff" : "ArchwitchWand"));
                            }

                            if (dropType >= 4) {
                                player.QuickSpawnItem(mod.ItemType("LizTerratool"));
                            }
                        }

                        spawnedDevItems = true;
                        break;

                    case 12:

                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Aves.AvesBag>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("DuckstepGun" + addonEX));
                        }

                        spawnedDevItems = true;
                        break;

                    case 13:

                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Tied.OldMagiciansHat>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType(dropType == 4 ? "GentlemansLongblade" : "GentlemansRapier"));
                        }

                        spawnedDevItems = true;
                        break;

                    case 14:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Moon.MoonBag>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("Etheral" + addonEX));
                        }

                        spawnedDevItems = true;
                        break;

                    case 15:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Grox.GroviteSeaChest>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType(dropType == 4 ? "SoccStaff" : "SockStaff"));
                        }

                        if (dropType >= 4) {
                            player.QuickSpawnItem(mod.ItemType("GroviteTerratool"));
                        }

                        spawnedDevItems = true;
                        break;

                    case 16:

                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.CC.CCBox>());

                        if (dropType >= 2) {
                            player.QuickSpawnItem(mod.ItemType("CCRuneBookPage"));
                        }

                        spawnedDevItems = true;
                        break;

                    case 17:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Gibs.GibsBag>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(Main.rand.Next(2) == 0 ? mod.ItemType("Skullshot") : mod.ItemType("GibsFemur"));
                        }

                        spawnedDevItems = true;
                        break;

                    case 18:
                        player.QuickSpawnItem(mod.ItemType("ApawnEgg"));
                        spawnedDevItems = true;
                        break;

                    case 19:
                        player.QuickSpawnItem(mod.ItemType("CursedHood"));
                        player.QuickSpawnItem(mod.ItemType("CursedRobe"));
                        player.QuickSpawnItem(mod.ItemType("CursedPants"));

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("CursedSickle" + addonEX));
                        }

                        spawnedDevItems = true;
                        break;

                    case 20:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Mikpin.MikBag>());

                        spawnedDevItems = true;
                        break;

                    case 21:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Fargo.TopHat>());

                        if (dropType >= 3) {
                            if (Main.rand.Next(2) == 0) {
                                player.QuickSpawnItem(mod.ItemType("MagicAcorn" + addonEX));
                            } else {
                                player.QuickSpawnItem(mod.ItemType("Placeholder"));
                            }
                        }

                        spawnedDevItems = true;
                        break;

                    case 22:

                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Blazen.BlazenBag>());

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("ThunderLord" + addonEX));
                        }
                        spawnedDevItems = true;
                        break;

                    case 23:
                        player.QuickSpawnItem(ItemID.ReaperHood);
                        player.QuickSpawnItem(ItemID.ReaperRobe);

                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("GrimReaperScythe" + addonEX));
                        }
                        spawnedDevItems = true;
                        break;

                    case 24:
                        if (dropType >= 2) {
                            player.QuickSpawnItem(mod.ItemType("UmbralReaper"));
                        }
                        spawnedDevItems = true;
                        break;

                    case 25:
                        if (dropType >= 2) {
                            player.QuickSpawnItem(mod.ItemType("FuryForger" + addonEX));
                        }
                        spawnedDevItems = true;
                        break;

                    case 26:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Cerberus.InvokerBag>());
                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("InvokerStaff"));
                        }
                        spawnedDevItems = true;
                        break;

                    case 27:
                        if (dropType >= 2) {
                            player.QuickSpawnItem(mod.ItemType("GameRaider"));
                        }
                        spawnedDevItems = true;
                        break;
                    case 28:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Pluto.PlutoBag>());
                        break;
                    case 29:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.VoidEye.VoidBag>());
                        break;
                    case 30:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Anarchy.AnarchyBag>());
                        break;
                    case 31:
                        if (dropType >= 3) {
                            player.QuickSpawnItem(mod.ItemType("UmbreonSP" + addonEX));
                        }
                        break;
                    case 32:
                        player.QuickSpawnItem(ModContent.ItemType<Items.Vanity.Shox.ShoxBag>());
                        break;

                    default:
                        spawnedDevItems = false;
                        break;
                }
            }
        }

        public void PHMDevArmor() {
            DropDevArmor(0);
        }

        public void HMDevArmor() {
            DropDevArmor(1);
        }

        public void PPDevArmor() {
            DropDevArmor(2);
        }

        public void PMLDevArmor() {
            DropDevArmor(3);
        }

        public void SADevArmor() {
            DropDevArmor(4);
        }

        #endregion

        public override void PreUpdate() {
            if (VoidSubworld.IsInside()) {
                int leftTile = 41 * 16;
                int topTile = 41 * 16;
                int rightTile = (Main.maxTilesX - 42) * 16;
                int bottomTile = (Main.maxTilesY - 42) * 16;

                Vector2 relativePos = Main.screenPosition - player.Center;
                bool looped = false;
                if (player.BottomRight.X >= rightTile) {
                    player.position.X = leftTile;
                    looped = true;
                } else if (player.TopLeft.X <= leftTile) {
                    player.position.X = rightTile - player.width;
                    looped = true;
                }

                if (player.BottomRight.Y >= bottomTile) {
                    player.position.Y = topTile;
                    looped = true;
                } else if (player.TopLeft.Y <= topTile) {
                    player.position.Y = bottomTile - player.height;
                    looped = true;
                }

                if (looped) {
                    Main.screenPosition = player.Center + relativePos;
                }
            }

            /*if (player.Bottom.Y >= (Main.maxTilesY - 42) * 16) {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " fell out of the world."), Double.MaxValue, 0);
            }*/

            //groviteGlow[player.whoAmI] = false;
        }

        public override void GetWeaponKnockback(Item item, ref float knockback) {
            if (demonGauntlet) {
                if (item.melee) {
                    knockback += 2f;
                }
            }

            if (IsGoblin) {
                knockback += 5f;
            }
        }

        public override float UseTimeMultiplier(Item item) {
            float multiplier = 1f;

            if (item.damage > 0) {
                if (HydraPendant) {
                    multiplier *= 1.15f;
                }

                while (item.useTime / multiplier < 1) {
                    multiplier -= .1f;
                }

                while (item.useAnimation / multiplier < 2) {
                    multiplier -= .1f;
                }
            }

            return multiplier;
        }

        public override void ProcessTriggers(TriggersSet triggersSet) {
            if (SagShield) {
                if (AAMod.AccessoryAbilityKey.JustPressed && SagCooldown == 0) {
                    player.AddBuff(ModContent.BuffType<SagShield>(), 300);
                    SagCooldown = 5400;
                }
            }

            if (Witch) {
                if (AAMod.ArmorAbilityKey.JustPressed && AsheCooldown == 0) {
                    Main.PlaySound(29, (int)player.position.X, (int)player.position.Y, 104, 1f, 0f);
                    if (player.inventory[player.selectedItem].magic || player.inventory[player.selectedItem].summon) {
                        for (int i = 0; i < 8; i++) {
                            Vector2 shoot = new Vector2((float)Math.Sin(i * 0.25f * 3.1415926f), (float)Math.Cos(i * 0.25f * 3.1415926f));
                            shoot *= 8f;
                            int id = Projectile.NewProjectile(player.Center.X, player.Center.Y, shoot.X, shoot.Y, mod.ProjectileType("AsheFire"), player.inventory[player.selectedItem].damage, 5, Main.myPlayer, 0f, 1f);
                            Main.projectile[id].magic = true;
                            Main.projectile[id].hostile = false;
                            Main.projectile[id].friendly = true;
                        }
                    }
                    player.AddBuff(ModContent.BuffType<AsheFlame>(), 900);
                    AsheCooldown = 5400;
                }
            }

            if (entropySet) {
                if (AAMod.ArmorAbilityKey.JustPressed && EntropyCooldown <= 0) {
                    Vector2 newPos = default(Vector2);
                    newPos.X = (float)Main.mouseX + Main.screenPosition.X;
                    if (player.gravDir == 1f || RealityStone) {
                        newPos.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
                    } else {
                        newPos.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                    }

                    newPos.X -= player.width / 2;
                    if (newPos.X > 50f && newPos.X < (float)(Main.maxTilesX * 16 - 50) && newPos.Y > 50f && newPos.Y < (float)(Main.maxTilesY * 16 - 50)) {
                        int num265 = (int)(newPos.X / 16f);
                        int num266 = (int)(newPos.Y / 16f);
                        if ((Main.tile[num265, num266].wall != 87 || !((double)num266 > Main.worldSurface) || NPC.downedPlantBoss) && !Collision.SolidCollision(newPos, player.width, player.height)) {
                            player.Teleport(newPos, 1);
                            EntropyCooldown = 180;
                            NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, newPos.X, newPos.Y, 1);
                        }
                    }
                }
            }

            if (OldOneCharm) {
                if (AAMod.AccessoryAbilityKey.JustPressed && DD2Event.Ongoing && DD2Event.TimeLeftBetweenWaves > 0) {
                    DD2Event.TimeLeftBetweenWaves = 60;
                    if (Main.netMode != 0) {
                        AANet.SendNetMessage(AANet.DD2EventTime, (byte)DD2Event.TimeLeftBetweenWaves);
                    }
                }
            }

            if (BrokenCode) {
                if (AAMod.AccessoryAbilityKey.JustPressed && Main.myPlayer == player.whoAmI) {
                    if (!player.HasBuff(ModContent.BuffType<Glitched>())) {
                        // pause time
                        if (CodeCD <= 0) {
                            CodeCD = 60 * 20; // 20 seconds
                            Main.PlaySound(AAMod.instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/Glitch"));
                            player.AddBuff(ModContent.BuffType<Glitched>(), 160);
                        }
                    }
                }
            }

            if (CodeCD > 0) {
                if (!CodeOn) {
                    headOffset = new Vector2(Main.rand.NextFloat() * 80 - 40, Main.rand.NextFloat() * 80 - 40);
                    bodyOffset = new Vector2(Main.rand.NextFloat() * 80 - 40, Main.rand.NextFloat() * 80 - 40);
                    legOffset = new Vector2(Main.rand.NextFloat() * 80 - 40, Main.rand.NextFloat() * 80 - 40);
                    player.headPosition += headOffset;
                    player.bodyPosition += bodyOffset;
                    player.legPosition += legOffset;
                    CodeOn = true;
                }

                CodeCD--;
            } else {
                if (CodeOn) {
                    player.headPosition -= headOffset;
                    player.bodyPosition -= bodyOffset;
                    player.legPosition -= legOffset;
                    headOffset = Vector2.Zero;
                    bodyOffset = Vector2.Zero;
                    legOffset = Vector2.Zero;
                    CodeOn = false;
                }
            }

            if (ChaosRa2) {
                if (AAMod.ArmorAbilityKey.JustPressed && AbilityCD == 0) {
                    AbilityCD = 180;

                    int damage = 70;
                    float knockback = 1;

                    Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                    float speedX = Main.mouseX + Main.screenPosition.X - vector2.X;
                    float speedY = Main.mouseY + Main.screenPosition.Y - vector2.Y;

                    if (player.gravDir == -1f) {
                        speedY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
                    }

                    if (float.IsNaN(speedX) && float.IsNaN(speedY) || speedX == 0f && speedY == 0f) {
                        speedX = player.direction;
                        speedY = 0f;
                    }

                    vector2.X = Main.mouseX + Main.screenPosition.X;
                    vector2.Y = Main.mouseY + Main.screenPosition.Y;

                    Projectile.NewProjectile(player.Center.X, player.Center.Y, speedX, speedY, mod.ProjectileType("DragonShot"), damage, knockback, Main.myPlayer, 0f, 0f);
                }
            }

            if (AbilityCD != 0) {
                AbilityCD--;
            }
        }

        private static void LeaveDust(Player player) {
            for (int index = 0; index < 70; ++index)
                Main.dust[Dust.NewDust(player.position, player.width, player.height, 15, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
            Main.TeleportEffect(player.getRect(), 1);
            Main.TeleportEffect(player.getRect(), 3);
        }

        public bool ShinyCheck() {
            for (int i = 0; i < 58; i++) {
                Item item = player.inventory[i];
                if (item.type == mod.ItemType("ShinyCharm")) {
                    if (Main.rand.Next(2048) == 0) {
                        return true;
                    } else {
                        return false;
                    }
                }

            }
            if (Main.rand.Next(4096) == 0)
                return true;

            return false;
        }
        public int IZHoldTimer = 180;
        public bool InfZ = false;
        public int GetIZHealth = 2500000;
        public int EscapeLine = 180;
        public int RiftTimer;
        public int RiftDamage = 10;

        public int AARegenCount = 0;

        public override void UpdateLifeRegen() {
            if (SagShield) {
                if (player.lifeRegen < 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegen += 2;
            }

            if (TerraMe) {
                AARegenCount++;
                while (AARegenCount >= 100) {
                    AARegenCount -= 100;
                    if (player.statLife < player.statLifeMax2) {
                        player.statLife += 2;
                        for (int i = 0; i < 10; i++) {
                            int num6 = Dust.NewDust(player.position, player.width, player.height, 107, 0f, 0f, 175, default, 1.75f);
                            Main.dust[num6].noGravity = true;
                            Main.dust[num6].velocity *= 0.75f;
                            int num7 = Main.rand.Next(-40, 41);
                            int num8 = Main.rand.Next(-40, 41);
                            Dust expr_7EE_cp_0 = Main.dust[num6];
                            expr_7EE_cp_0.position.X = expr_7EE_cp_0.position.X + num7;
                            Dust expr_80A_cp_0 = Main.dust[num6];
                            expr_80A_cp_0.position.Y = expr_80A_cp_0.position.Y + num8;
                            Main.dust[num6].velocity.X = -num7 * 0.075f;
                            Main.dust[num6].velocity.Y = -num8 * 0.075f;
                        }
                    }
                }
            }
        }

        public override void UpdateBadLifeRegen() {
            if (Spear) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegen -= 2;
            }

            if (infinityOverload) {
                player.lifeRegen -= 60;
            }

            if (YamataGravity || YamataAGravity) {
                if (player.mount.CanFly) {
                    player.mount.Dismount(player);
                }

                player.wingTimeMax = 0;
                if (player.wingTime > player.wingTimeMax)
                    player.wingTimeMax = player.wingTimeMax;

                if (YamataAGravity) {
                    player.moveSpeed *= .58f;
                }
            }

            if (FFlames) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegen -= 40 * (player.statLife / player.statLifeMax2);
            }


            if (CursedHellfire) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegen -= 30;
            }

            if (shroomed && player.velocity.Y == 0) {
                player.velocity.X *= .8f;
            }

            if (Hunted) {
                if (player.rocketTimeMax > 30) {
                    player.wingTimeMax = 30;
                }

                if (player.accRunSpeed > 3f) {
                    player.accRunSpeed = 3f;
                }

                player.wingTimeMax /= 2;
                if (player.wingTimeMax <= 0) {
                    player.wingTimeMax = 0;
                }
            }

            if (terraBlaze) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegen -= 16;
            }

            if (dragonFire) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegen -= 8;

            }

            if (riftbent) {
                RiftTimer++;
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;

                if (RiftTimer >= 120) {
                    RiftDamage += 10;
                    RiftTimer = 0;
                }

                if (RiftDamage >= 80) {
                    RiftDamage = 80;
                }

                player.lifeRegen -= RiftDamage;
            } else {
                RiftDamage = 10;
                RiftTimer = 0;
            }

            if (hydraToxin) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                hydraToxinTime++;
                player.lifeRegen -= hydraToxinTime / 50 + 4;
            } else if (hydraToxinTime > 0) {
                hydraToxinTime = 0;
            }


            if (discordInferno) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;
                player.lifeRegen -= (int)player.velocity.Length() / 2 + 4;
                //player.allDamage *= 0.8f;
            }

            if (AkumaPain) {
                if (player.lifeRegen > 0) {
                    player.lifeRegen = 0;
                }

                player.lifeRegenTime = 0;

                if ((player.onFire || player.frostBurn || player.onFire2 || dragonFire || discordInferno) && player.lifeRegen < 0) {
                    player.lifeRegen *= 2;
                }
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
            if (bossactive) {
                nohitplayer = false;
            }
            if (ShieldUp) {
                return false;
            }
            if (Ronin) {
                return false;
            }
            if (AncientGoldSet) {
                long num = 0;
                for (int i = 0; i < 54; i++) {
                    if (player.inventory[i].type == 71) {
                        num += player.inventory[i].stack;
                    }
                    if (player.inventory[i].type == 72) {
                        num += player.inventory[i].stack * 100;
                    }
                    if (player.inventory[i].type == 73) {
                        num += player.inventory[i].stack * 10000;
                    }
                    if (player.inventory[i].type == 74) {
                        num += player.inventory[i].stack * 1000000;
                    }
                }
                if (num >= damage * 10000) {
                    for (int i = 0; i < 54; i++) {
                        if (player.inventory[i].type == 71) {
                            player.inventory[i].stack = 0;
                            player.inventory[i].TurnToAir();
                        }
                        if (player.inventory[i].type == 72) {
                            player.inventory[i].stack = 0;
                            player.inventory[i].TurnToAir();
                        }
                        if (player.inventory[i].type == 73) {
                            player.inventory[i].stack = 0;
                            player.inventory[i].TurnToAir();
                        }
                        if (player.inventory[i].type == 74) {
                            player.inventory[i].stack = 0;
                            player.inventory[i].TurnToAir();
                        }
                    }
                    damage = 0;
                    return false;
                }
            }
            return true;
        }

        public override void UpdateDead() {
            infinityOverload = false;
            discordInferno = false;
            dragonFire = false;
            hydraToxin = false;
            terraBlaze = false;
            Yanked = false;
            InfinityScorch = false;
            LockedOn = false;
            shroomed = false;
            riftbent = false;
            DestinedToDie = false;
            YamataGravity = false;
            YamataAGravity = false;
            Hunted = false;
            Spear = false;
            MaxMovespeedboost = 0;
            spellbookDamage = 1f;
        }

        public override void MeleeEffects(Item item, Rectangle hitbox) {
            if (demonGauntlet) {
                if (Main.rand.NextFloat() < 1f) {
                    int ThisDust = 170;
                    if (!WorldGen.crimson) {
                        ThisDust = 75;
                    }

                    Dust dust = Main.dust[Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ThisDust, 0f, 0f, 46)];
                    dust.noGravity = true;
                }
            }
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
            if (FFlames) {
                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("ForsakenDust"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default, 1.5f);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                    r *= 0.1f;
                    g *= 0.7f;
                    b *= 0.1f;
                }
            }

            if (infinityOverload) {
                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("InfinityOverloadB"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                r *= 0.1f;
                g *= 0.3f;
                b *= 0.7f;

                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("InfinityOverloadR"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                r *= 0.7f;
                g *= 0.2f;
                b *= 0.2f;

                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("InfinityOverloadG"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                r *= 0.1f;
                g *= 0.7f;
                b *= 0.1f;

                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("InfinityOverloadY"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                r *= 0.5f;
                g *= 0.5f;
                b *= 0.1f;

                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("InfinityOverloadP"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                r *= 0.6f;
                g *= 0.1f;
                b *= 0.6f;

                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("InfinityOverloadO"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                r *= 0.8f;
                g *= 0.5f;
                b *= 0.1f;

                fullBright = true;
            }

            if (terraBlaze) {
                if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 107, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                r *= 0.1f;
                g *= 0.7f;
                b *= 0.2f;

                fullBright = true;
            }

            if (CursedHellfire) {
                if (Main.rand.Next(4) == 0) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, 75, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100);

                    Main.dust[dust].scale = 3f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;

                    Main.playerDrawDust.Add(dust);
                }

                fullBright = true;
            }

            //Main.NewText(player.velocity.Length());
            if (discordInferno) {
                int particles = Math.Min((int)player.velocity.Length() * 2 + 1, 25);
                for (int i = 0; i < particles; i++) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width, player.height, ModContent.DustType<Dusts.Discord>(), 0f, -2.5f, 0);

                    Main.dust[dust].alpha = 100;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale += Main.rand.NextFloat();
                }
            }

            if (shroomed) {
                for (int i = 0; i < 2; i++) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width, player.height, ModContent.DustType<Dusts.ShroomDust>(), 0f, -2.5f, 0);

                    Main.dust[dust].alpha = 100;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale += Main.rand.NextFloat();
                }

                Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0f, 0.45f);
            }

            if (riftbent) {
                int Loops = RiftDamage / 10;
                for (int i = 0; i < Loops; i++) {
                    int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width, player.height, ModContent.DustType<Dusts.CthulhuAuraDust>(), 0f, -2.5f, 0);

                    Main.dust[dust].alpha = 100;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale += Main.rand.NextFloat();
                }

                Lighting.AddLight((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f), 0f, 0f, 0.45f);
            }
        }

        public override bool ConsumeAmmo(Item weapon, Item ammo) {
            if (Replicator) return false;

            if (ammo20percentdown && Main.rand.Next(5) == 0) {
                return false;
            }

            return base.ConsumeAmmo(weapon, ammo);
        }

        #region Highest Damage check

        public bool MeleeHighest(Player player) {
            return player.meleeDamage > player.rangedDamage &&
                player.meleeDamage > player.magicDamage &&
                player.meleeDamage > player.minionDamage &&
                player.meleeDamage > player.thrownDamage;
        }

        public bool RangedHighest(Player player) {
            return player.rangedDamage > player.meleeDamage &&
                player.rangedDamage > player.magicDamage &&
                player.rangedDamage > player.minionDamage &&
                player.rangedDamage > player.thrownDamage;
        }

        public bool MagicHighest(Player player) {
            return player.magicDamage > player.rangedDamage &&
                player.magicDamage > player.meleeDamage &&
                player.magicDamage > player.minionDamage &&
                player.magicDamage > player.thrownDamage;
        }

        public bool SummonHighest(Player player) {
            return player.minionDamage > player.rangedDamage &&
                player.minionDamage > player.magicDamage &&
                player.minionDamage > player.meleeDamage &&
                player.minionDamage > player.thrownDamage;
        }

        public bool ThrownHighest(Player player) {
            return player.thrownDamage > player.rangedDamage &&
                player.thrownDamage > player.magicDamage &&
                player.thrownDamage > player.minionDamage &&
                player.thrownDamage > player.meleeDamage;
        }

        #endregion

        public override void UpdateVanityAccessories() {
            for (int n = 10; n < 18 + player.extraAccessorySlots; n++) {
                Item item = player.armor[n];
                if (item.type == mod.ItemType("StripeManShirt")) {
                    player.accWatch = 3;
                    player.accDepthMeter = 1;
                    player.accCompass = 1;
                    player.accFishFinder = true;
                    player.accWeatherRadio = true;
                    player.accCalendar = true;
                    player.accThirdEye = true;
                    player.accJarOfSouls = true;
                    player.accCritterGuide = true;
                    player.accStopwatch = true;
                    player.accOreFinder = true;
                    player.accDreamCatcher = true;
                }
                if (item.type == mod.ItemType("Equinox")) {
                    player.hideWolf = false;
                    player.forceWerewolf = true;
                    if (player.wet && !player.lavaWet && (!player.mount.Active || player.mount.Type != 3) || !player.forceWerewolf) {
                        player.hideMerman = false;
                        player.forceMerman = true;
                    }
                }
            }
        }
    }
}