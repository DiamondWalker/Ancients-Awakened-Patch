
using AAMod.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AAMod.NPCs.Enemies.Void
{
    public class Searcher : ModNPC
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Searcher");
        }

        public override void SetDefaults()
        {
            npc.width = 35;
            npc.height = 35;
            npc.value = BaseUtility.CalcValue(0, 0, 5, 50);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 2500;
            npc.defense = 30;
            npc.damage = 98;
            npc.HitSound = new LegacySoundStyle(3, 4, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0.0f;
            npc.noGravity = true;
            banner = npc.type;
			bannerItem = mod.ItemType("SearcherBanner");

        }

        float shootAI = 0;
        private int moveTime = 0;
        private float AttackAngle { get => npc.ai[0]; set => npc.ai[0] = value; }
        public override void AI() {
            // standard targetting code
            npc.TargetClosest(true);
            if (!npc.HasValidTarget) {
                return;
            }
            Player player = Main.player[npc.target];

            bool canSee = Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height);
            Vector2 fireAt = player.Center;

            npc.rotation = (float)Math.Atan2(fireAt.Y - npc.Center.Y, fireAt.X - npc.Center.X);
            if (npc.Center.X > player.Center.X) {
                npc.spriteDirection = 1;
                npc.rotation += (float)Math.PI;
            } else if (npc.Center.X < player.Center.X) {
                npc.spriteDirection = -1;
            }

            // move to the angle of attack
            Vector2 moveVec = GetCurrentTargetVec(player) - npc.Center;
            if (Vector2.Distance(npc.Center, player.Center) < 275) {
                // too close to player. Back off
                moveVec = -(player.Center - npc.Center);
            }
            moveVec.Normalize();
            npc.velocity += moveVec * 0.56f;
            npc.velocity = MathUtil.LimitVectorLength(npc.velocity, 16);

            // move and shoot code
            if (Main.netMode != 1) {
                shootAI++;
                // shoot 3 lasers
                if (shootAI >= 90 && shootAI <= 110 && shootAI % 10 == 0 && canSee) {
                    int projType = mod.ProjType("DeathLaser");
                    BaseAI.FireProjectile(fireAt, npc, projType, (int)(npc.damage * 0.25f), 0f, 4f);
                }

                // pick new location
                if (shootAI >= 140) {
                    int limit = 0;
                    do {
                        AttackAngle = Main.rand.NextFloat() * MathHelper.Pi * 2;
                        moveTime = Main.rand.Next(120, 361);
                        npc.netUpdate = true;
                        limit++;
                        if (limit >= 15) break;
                    } while (!HasGoodPosition(player));

                    shootAI = 0;
                }
            }
        }

        private bool HasGoodPosition(Player player) {
            Vector2 pos = GetCurrentTargetVec(player) - new Vector2(npc.width, npc.height) / 2;

            if (!Collision.CanHit(pos, npc.width, npc.height, player.position, player.width, player.height)) return false;
            if (!Collision.CanHit(npc.Center, npc.width, npc.height, pos, npc.width, npc.height)) return false;

            return true;
        }

        private Vector2 GetCurrentTargetVec(Player player) {
            return player.Center + MathUtil.VectorFromPolar(350, AttackAngle);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            bool isDead = npc.life <= 0;
            for (int m = 0; m < (isDead ? 25 : 5); m++)
            {
                int dustType = ModContent.DustType<Dusts.VoidDust>();
                Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, isDead ? 2f : 1.1f);
            }

            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SearcherGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SearcherGore2"), 1f);
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = mod.GetTexture("Glowmasks/Searcher_Glow");
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, Color.Red);
            return false;
        }

        /*public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Apocalyptite"));
        }*/
    }
}