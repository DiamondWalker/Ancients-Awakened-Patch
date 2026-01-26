using AAMod.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.NPCs.Enemies.Void {
    public class SummoningPortal : ModNPC {
        public int Time { get => (int)npc.ai[0]; set => npc.ai[0] = value; }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Summoning Portal");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults() {
            npc.lifeMax = 10;
            npc.damage = 0;
            npc.width = 70;
            npc.height = 70;
            npc.aiStyle = -1;
            npc.alpha = 25;
            npc.dontCountMe = true;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true; // little trick to make the nulls render above it
            npc.scale = 1;
        }

        public override bool CheckActive() {
            return false;
        }
        private int summonTicks = 0;
        public override void AI() {
            AnimationHelper.UpdateAnimation(npc, new int[] { 0, 1, 2, 3 }, 12);

            npc.velocity = Vector2.Zero;
            npc.timeLeft = NPC.activeTime;

            npc.TargetClosest();
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            if (Main.netMode != 1 && npc.Distance(player.Center) < 800 && Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height)) {
                if (NPC.CountNPCS(ModContent.NPCType<Null>()) <= 10 && RandUtil.Chance(900)) {
                    int count = RandUtil.InclusiveRand(1, 4);
                    for (int i = 0; i < count; i++) {
                        int summoned = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Null>());
                        Main.npc[summoned].velocity = RandUtil.RandomPolar(Main.rand.NextFloat() * 5);
                    }
                }
            }
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit) {
            damage = 0;
            return false;
        }
    }
}
