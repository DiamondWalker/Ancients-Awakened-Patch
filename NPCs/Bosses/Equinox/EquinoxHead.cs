using AAMod.Globals.Worlds;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AAMod.Dusts;

namespace AAMod.NPCs.Bosses.Equinox {
    public abstract class EquinoxHead : ModNPC {

        public abstract bool Empowered { get; }
        public abstract int[] SegmentTypes { get; }
        public abstract int AIUpdates { get; }
        public abstract int WormDustType { get; }
        public abstract int WormLength { get; }
        public abstract float EmpoweredMoveSpeed { get; }
        public abstract int EmpoweredDefense { get; }

        public bool IsHead { get => npc.type == mod.NPCType("DaybringerHead") || npc.type == mod.NPCType("NightcrawlerHead"); }
        public abstract void SpawnGore();

        private bool title;

        public override void SetDefaults() {
            npc.lifeMax = 100000;
            npc.damage = 125;
            npc.defense = 100;
            npc.value = Item.sellPrice(0, 10, 0, 0);
            for (int k = 0; k < npc.buffImmune.Length; k++) {
                npc.buffImmune[k] = true;
            }
            npc.knockBackResist = 0f;
            npc.width = 68;
            npc.height = 68;
            npc.boss = true;
            npc.aiStyle = -1;
            npc.timeLeft = 500;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.DeathSound = null;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Equinox");
            musicPriority = MusicPriority.BossHigh;
            bossBag = mod.ItemType("EquinoxBag");
        }

        public float[] internalAI = new float[8];
        public override void SendExtraAI(BinaryWriter writer) {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ) {
                if (IsHead) {
                    writer.Write(internalAI[0]);
                    writer.Write(internalAI[1]);
                    writer.Write(internalAI[2]);
                    writer.Write(internalAI[3]);
                    writer.Write(internalAI[4]);
                    writer.Write(internalAI[5]);
                    writer.Write(internalAI[6]);
                    writer.Write(internalAI[7]);

                    writer.Write(CloudCooldown);
                }

                writer.Write(initCustom);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                if (IsHead) {
                    internalAI[0] = reader.ReadFloat(); //DaybringerCounter
                    internalAI[1] = reader.ReadFloat(); //NightclawerCounter
                    internalAI[2] = reader.ReadFloat();
                    internalAI[3] = reader.ReadFloat();
                    internalAI[4] = reader.ReadFloat(); //DaybringerPosCheck
                    internalAI[5] = reader.ReadFloat(); //VelocitySave
                    internalAI[6] = reader.ReadFloat(); //VelocitySave
                    internalAI[7] = reader.ReadFloat();

                    CloudCooldown = reader.ReadInt();
                }

                initCustom = reader.ReadBoolean();
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
            scale = 1.5f;
            return null;
        }

        public override void BossHeadRotation(ref float rotation) {
            rotation = npc.rotation;
        }

        public override bool CheckActive() {
            npc.timeLeft--;
            return npc.timeLeft < 50;
        }

        public abstract void HandleDayNightCycle();

        public bool prevWormStronger = false;
        public bool initCustom = false;
        public int CloudCount = Main.expertMode ? 8 : 6;
        public int CloudCooldown = 400;

        public override bool PreAI() {
            if (!this.title && npc.type == mod.NPCType("DaybringerHead")) {
                AAMod.ShowTitle(base.npc, 17);
                this.title = true;
            }
            if (Main.netMode != 1 && !initCustom) {
                initCustom = true;
                internalAI[7] += npc.whoAmI % 7 * 12; //so it doesn't pew all at once
                npc.velocity.X += 0.1f;
                npc.velocity.Y -= 4f;
            }
            if (IsHead) {
                HandleDayNightCycle();
            }

            bool isDay = Main.dayTime;
            bool wormStronger = Empowered;

            float wormDistance = -26f;
            int aiCount = 2;
            float moveSpeedMax = 16f;
            npc.damage = 125;
            npc.defense = 100;

            if (wormStronger != prevWormStronger) {
                int dustType = WormDustType;
                for (int k = 0; k < 10; k++) {
                    int dustID = Dust.NewDust(npc.position, npc.width, npc.height, dustType, (int)(npc.velocity.X * 0.2f), (int)(npc.velocity.Y * 0.2f), 0, default, 1.5f);
                    Main.dust[dustID].noGravity = true;
                }
            }

            if (wormStronger) {
                if (Main.netMode == 0) {
                    npc.width = 136;
                    npc.height = 136;
                    wormDistance = -52f;
                }
                aiCount = AIUpdates;
                moveSpeedMax = EmpoweredMoveSpeed;
                npc.damage = 150;
                npc.defense = EmpoweredDefense;
            }

            for (int m = 0; m < aiCount; m++) {
                int Length = WormLength;
                int[] wormTypes = SegmentTypes;
                AAAI.AIWorm(npc, wormTypes, Length, wormDistance, moveSpeedMax, 0.07f, true, false, false, false, false, false);
            }

            if (IsHead) //prevents despawn and allows them to run away
            {
                bool foundTarget = TargetClosest();
                if (foundTarget) {
                    npc.timeLeft = 300;
                } else {
                    if (npc.timeLeft > 50) npc.timeLeft = 50;
                    npc.velocity.Y -= 0.2f;
                    if (npc.velocity.Y < -20f) npc.velocity.Y = -20f;
                    return false;
                }
            } else {
                npc.timeLeft = 300; //pieces should not despawn naturally, only despawn when the head does
            }

            Player target = Main.player[npc.target];

            if (IsHead && !DoWormAI(target)) return false;

            if (!IsHead) {
                npc.defense = Main.npc[npc.realLife].defense;
            }
            NormalAI(wormStronger, isDay);
            return false;
        }

        public abstract bool DoWormAI(Player target);

        public abstract void NormalAI(bool wormStronger, bool isDay);

        public int playerTooFarDist = 16000; //1000 tile radius, these worms move fast!		
        public bool TargetClosest() {
            int[] players = BaseAI.GetPlayers(npc.Center, Math.Min(20000f, playerTooFarDist * 3));
            float dist = 999999999f;
            int foundPlayer = -1;
            for (int m = 0; m < players.Length; m++) {
                Player p = Main.player[players[m]];
                if (Vector2.Distance(p.Center, npc.Center) < dist) {
                    dist = Vector2.Distance(p.Center, npc.Center);
                    foundPlayer = p.whoAmI;
                }
            }
            if (foundPlayer != -1) {
                BaseAI.SetTarget(npc, foundPlayer);
                return true;
            }
            return false;
        }

        public override void BossLoot(ref string name, ref int potionType) {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            npc.lifeMax = (int)(npc.lifeMax * 0.65f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.85f);
        }

        bool spawnedGore = false;
        public override void HitEffect(int hitDirection, double damage) {
            int dustType = WormDustType;
            for (int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, dustType, hitDirection, -1f, 0, default, 1.2f);
            }
            if (npc.life <= 0 || (npc.life - damage <= 0)) {
                Main.dayRate = 1;
                Main.fastForwardTime = false;
                if (!spawnedGore) {
                    spawnedGore = true;
                    SpawnGore();
                    for (int k = 0; k < 15; k++) {
                        Dust.NewDust(npc.position, npc.width, npc.height, dustType, hitDirection, -1f, 0, default, 1.5f);
                    }
                }
            }
        }

        public override void NPCLoot() {
            bool nightcrawler = npc.type == mod.NPCType("NightcrawlerHead") || npc.type == mod.NPCType("NightcrawlerBody") || npc.type == mod.NPCType("NightcrawlerTail");
            int otherWormAlive = nightcrawler ? mod.NPCType("DaybringerHead") : mod.NPCType("NightcrawlerHead");
            if (!nightcrawler) {
                AAWorld.downedDB = true;
                BaseAI.DropItem(npc, mod.ItemType("DBTrophy"), 1, 1, 15, true);
            } else {
                AAWorld.downedNC = true;
                BaseAI.DropItem(npc, mod.ItemType("NCTrophy"), 1, 1, 15, true);
            }
            if (NPC.CountNPCS(otherWormAlive) == 0) {
                AAWorld.downedEquinox = true;
            }
            string wormType = nightcrawler ? "Nightcrawler" : "Daybringer";
            if (Main.rand.Next(10) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(wormType + "Trophy"));
            }
            if (Main.expertMode) {
                npc.DropBossBags();
            } else {
                if (Main.rand.Next(7) == 0) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(wormType + "Mask"));
                }
                if (!nightcrawler) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Stardust"), Main.rand.Next(30, 75));
                } else {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DarkEnergy"), Main.rand.Next(30, 75));
                }
                if (AAWorld.RadiumOre) {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarIdol"));
                }
            }
        }

        public Color GetAuraAlpha() {
            Color c = Color.White * (Main.mouseTextColor / 255f);
            //c.A = 255;
            return c;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) {
            MakeSegmentsImmune(npc, player.whoAmI);
            ModifyCritArea(npc, ref crit);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            MakeSegmentsImmune(npc, projectile.owner);
            ModifyCritArea(npc, ref crit);
            if (projectile.penetrate != 1) {
                for (int i = 0; i < Main.maxNPCs; i++) {
                    if (Main.npc[i].active && (Main.npc[i].whoAmI == npc.realLife || (Main.npc[i].realLife >= 0 && Main.npc[i].realLife == npc.realLife))) {
                        Main.npc[i].immune[projectile.owner] = 10;
                    }
                }
                damage = (int)(damage * .44f);
            }
            if (!IsHead) {
                damage = (int)(damage * .76f);
            }
        }

        private void ModifyCritArea(NPC npc, ref bool crit) {
            if (npc.realLife >= 0) {
                if (npc.whoAmI == npc.realLife) {
                    crit = true;
                }
                if (npc.ai[0] == 0) {
                    crit = false;
                }
            }
        }

        public override void UpdateLifeRegen(ref int damage) {
            if (npc.realLife >= 0 && npc.whoAmI != npc.realLife) {
                damage = 0;
                npc.lifeRegen = 0;
            }
        }

        public void MakeSegmentsImmune(NPC npc, int id) {
            if (npc.realLife >= 0) {
                bool last = false;
                NPC parent = Main.npc[npc.realLife];
                parent.lifeRegen = npc.lifeRegen;
                int i = 0;
                while (parent.ai[0] > 0 || last) {
                    if (i++ > 200) { return; }
                    parent.immune[id] = npc.immune[id];
                    for (int j = 0; j < npc.buffType.Length; j++) {
                        if (npc.buffType[j] > 0 && npc.buffTime[j] > 0) {
                            parent.buffType[j] = npc.buffType[j];
                            parent.buffTime[j] = npc.buffTime[j];
                        }
                    }
                    if (last) { break; }
                    parent = Main.npc[(int)parent.ai[0]];
                    if (parent.ai[0] == 0) { last = true; }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor) {
            bool wormStronger = Empowered;
            Texture2D tex = Main.npcTexture[npc.type];
            npc.width = 68;
            npc.height = 68;
            if (wormStronger) {
                string texName = "NPCs/Bosses/Equinox/";
                if (npc.type == mod.NPCType("DaybringerHead")) { texName += "DaybringerHeadBig"; } else
                if (npc.type == mod.NPCType("DaybringerBody")) { texName += "DaybringerBodyBig"; } else
                if (npc.type == mod.NPCType("DaybringerTail")) { texName += "DaybringerTailBig"; } else
                if (npc.type == mod.NPCType("NightcrawlerHead")) { texName += "NightcrawlerHeadBig"; } else
                if (npc.type == mod.NPCType("NightcrawlerBody")) { texName += "NightcrawlerBodyBig"; } else
                if (npc.type == mod.NPCType("NightcrawlerTail")) { texName += "NightcrawlerTailBig"; }
                tex = mod.GetTexture(texName);

                int diff = Main.LocalPlayer.miscCounter % 50;
                float diffFloat = diff / 50f;
                float auraPercent = BaseUtility.MultiLerp(diffFloat, 0f, 1f, 0f); //did it this way so it's syncronized between all the segments
                //BaseDrawing.DrawAura(spritebatch, tex, 0, npc.position + new Vector2(0, npc.height / 2 + npc.gfxOffY), npc.width, npc.height, auraPercent, 2f, npc.scale, npc.rotation, npc.spriteDirection, Main.npcFrameCount[npc.type], npc.frame, 0, 0, GetAuraAlpha());
                BaseDrawing.DrawAura(spritebatch, tex, 0, npc, auraPercent, 2f, 0, npc.height / 2, GetAuraAlpha());
            }
            BaseDrawing.DrawTexture(spritebatch, tex, 0, npc, Color.White, true); //GetAuraAlpha());				
            return false;
        }
    }
}
