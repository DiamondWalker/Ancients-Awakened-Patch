using AAMod.Globals.Worlds;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace AAMod.NPCs.Bosses.Yamata.Awakened {
    [AutoloadBossHead]
    public class YamataAHead : ModNPC
    {
        private bool threeQuarterHealth = false;
        private bool HalfHealth = false;
        private bool tenthHealth = false;

        public override void SetStaticDefaults()
        {
			base.SetStaticDefaults();
            DisplayName.SetDefault("Yamata no Orochi");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * .8f);
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 480000;
            npc.damage = 100;
            npc.defense = 100;
            npc.width = 78;
            npc.height = 60;
            npc.npcSlots = 0;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.DeathSound = mod.GetLegacySoundSlot(SoundType.NPCKilled, "Sounds/Sounds/YamataRoar");
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Yamata2");
            npc.knockBackResist *= 0.05f;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            AAWorld.handleAncientBuff(npc, EnumAncients.Yamata);
        }

        public YamataA Body = null;

        public float[] internalAI = new float[4];
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.alpha == 0;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit) {
            if (!AAWorld.downedYamata) {
                if (npc.life <= (npc.lifeMax / 4 * 3) && threeQuarterHealth == false) {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA6"), new Color(146, 30, 68));
                    threeQuarterHealth = true;
                }
                if (npc.life <= npc.lifeMax / 2 && HalfHealth == false) {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA7"), new Color(146, 30, 68));
                    HalfHealth = true;
                }
                if (npc.life <= npc.lifeMax / 10 && tenthHealth == false) {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA8"), new Color(146, 30, 68));
                    tenthHealth = true;
                }
            }
            if (AAWorld.downedYamata) {
                if (npc.life <= (npc.lifeMax / 4 * 3) && threeQuarterHealth == false) {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA9"), new Color(146, 30, 68));
                    threeQuarterHealth = true;
                }
                if (npc.life <= npc.lifeMax / 2 && HalfHealth == false) {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA10"), new Color(146, 30, 68));
                    HalfHealth = true;
                }
                if (npc.life <= npc.lifeMax / 10 && tenthHealth == false) {
                    if (Main.netMode != 1) BaseUtility.Chat(Lang.BossChat("YamataA11"), new Color(146, 30, 68));
                    tenthHealth = true;
                }
            }

            return true;
        }

        public override void NPCLoot() {
            if (Body == null) {
                NPC npcBody = Main.npc[(int)npc.ai[0]];
                if (npcBody.type == ModContent.NPCType<YamataA>()) {
                    Body = (YamataA)npcBody.modNPC;
                }
            }
            if (Body != null) {
                Body.Tag = true;
            }
        }

        bool spawnHaruka = false;

        public override void AI()
        {
            if (Body == null)
            {
                NPC npcBody = Main.npc[(int)npc.ai[0]];
                if (npcBody.type == ModContent.NPCType<YamataA>())
                {
                    Body = (YamataA)npcBody.modNPC;
                }
            }
            if (Body == null)
                return;

            npc.alpha = Body.npc.alpha;


            if (!Body.npc.active)
            {
                if (Main.netMode != 1) //force a kill to prevent 'ghost hands'
                {
                    npc.life = 0;
                    npc.checkDead();
                    npc.netUpdate = true;
                }
                return;
            }

            npc.timeLeft = 100;
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            if (Yamata.TeleportMeBitch)
            {
                YamataA.TeleportMeBitch = false;
                npc.Center = Body.npc.Center;
                return;
            }

            Laugh();

            int roarSound = mod.GetSoundSlot(SoundType.Item, "Sounds/Sounds/YamataRoar");

            if (!player.active || player.dead || !Body.npc.active)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead || !Body.npc.active)
                {
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    return;
                }
            }

            npc.rotation = 0;
            Vector2 nextTarget = new Vector2(Body.npc.Center.X + npc.ai[1], Body.npc.Center.Y + npc.ai[2]);
            float dist = Vector2.Distance(nextTarget, npc.Center);
            if (dist < 100)
            {
                npc.velocity *= 0.9f;
                if (Math.Abs(npc.velocity.X) < 0.05f) npc.velocity.X = 0f;
                if (Math.Abs(npc.velocity.Y) < 0.05f) npc.velocity.Y = 0f;
            }
            else
            {
                npc.velocity = Vector2.Normalize(nextTarget - npc.Center);
                npc.velocity *= 10f;
            }
            //npc.position += Body.position - Body.oldPosition;

            switch (Body.currentAttack)
            {
                case 0: //while other heads are charging
                    if (internalAI[3] == 0)
                    {
                        internalAI[3] = 1;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<YamataHarukaProj>(), npc.damage / 4, 0f, Main.myPlayer, npc.target);


                        if (npc.life <= npc.lifeMax / 2 && !spawnHaruka)
                        {
                            spawnHaruka = true;
                        }
                    }
                    break;

                case 1: //while other heads are shooting waveray
                    break;

                case 2: //shoot shit
                    if (++internalAI[2] > 20)
                    {
                        internalAI[2] = 0;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 5f, mod.ProjectileType("YamataAVenom2"), npc.damage / 6, 0f, Main.myPlayer);
                    }
                    break;

                case 3: //breathe lingering flame
                    if (++internalAI[2] > 60)
                    {
                        internalAI[2] = 0;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 7f, mod.ProjectileType("YamataABomb"), npc.damage / 6, 0f, Main.myPlayer);
                    }
                    break;

                case 4: //shoot direct aim deathrays
                    if (Body.attackTime == npc.ai[3] * 60 - 30)
                    {
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center), mod.ProjectileType("YamataDeathraySmall"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
                    }
                    break;

                case 5: //shoot the shit again
                    goto case 2;

                case 6: //drop meteor that creates ripples across ground
                    if (++internalAI[2] > 90)
                    {
                        internalAI[2] = 0;
                        if (Main.netMode != 1)
                            for (int i = -1; i <= 1; i++)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center).RotatedBy(MathHelper.ToRadians(i * 5)) * 5f, mod.ProjectileType("YamataAVenom2"), npc.damage / 6, 0f, Main.myPlayer);
                    }break;

                case 7: //pause, let previous waves disperse
                    break;

                case 8: //breathe the lingering shit
                    goto case 3;

                case 9: //some mix of 2 attacks he already does, something homing + something directly aimed
                    if (--internalAI[2] < 0)
                    {
                        internalAI[2] = 120;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 7f, mod.ProjectileType("YamataABomb"), npc.damage / 6, 0f, Main.myPlayer);
                    }
                    break;

                case 10: //shoot the shit again
                    goto case 2;

                default:
                    goto case 0;
            }
            
            if (YamataA.TeleportMeBitch)
            {
                YamataA.TeleportMeBitch = false;
                npc.Center = Body.npc.Center;
                return;
            }
        }

        public int laughTimer = 0;
        bool Laughing = false;

        public override void FindFrame(int frameHeight) {
            npc.frame.Y = 0;
            if (Laughing) {
                npc.frame.Y = frameHeight;
                if ((laughTimer / 20) % 2 == 1) npc.frame.Y += frameHeight;
            }
        }

        public void Laugh()
        {
            if (laughTimer > 0 && !Laughing)
            {
                CombatText.NewText(npc.getRect(), new Color(146, 30, 68), "NYEH", true, true);
                Laughing = true;
            }
            else if (laughTimer <= 0)
            {
                Laughing = false;
            }
            if (Laughing)
            {
                laughTimer--;
                if (laughTimer % 20 == 0 && laughTimer != 120)
                {
                    CombatText.NewText(npc.getRect(), new Color(146, 30, 68), "HEH", true, true);
                }
            }
        }
    }
}
