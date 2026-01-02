using AAMod.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;


namespace AAMod.NPCs.Enemies.Void {
    public class Vortex : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex");
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 84;
            npc.height = 84;
            npc.aiStyle = -1;
            npc.damage = 120;
            npc.defense = 120;
            npc.lifeMax = 6000;
            npc.value = Item.sellPrice(0, 0, 50, 0);
            npc.HitSound = new LegacySoundStyle(3, 4, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0.12f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.netAlways = true;
            banner = npc.type;
			bannerItem = mod.ItemType("VortexBanner");
        }

        public float Rotation = 0;
        public float RotationVel;// { get => npc.ai[0]; set => npc.ai[0] = value; }

        /*public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Apocalyptite"));
        }*/

        public override void AI()
        {
            //BaseAI.AIElemental(npc, ref npc.ai, null, 1, false, false, 800f, 600f, 180, 3f);
            // standard targetting code
            npc.TargetClosest(true);
            if (!npc.HasValidTarget) {
                return;
            }
            Player player = Main.player[npc.target];

            Vector2 moveVec = player.Center - npc.Center;
            moveVec.Normalize();
            npc.velocity += moveVec * 0.2f;
            npc.velocity = MathUtil.LimitVectorLength(npc.velocity, 3.4f);

            if (Math.Abs(npc.Center.X - player.Center.X) < 150) {
                if (RotationVel > 0) {
                    RotationVel = Math.Min(RotationVel + .003f, .36f);
                } else if (RotationVel < 0) {
                    RotationVel = Math.Max(RotationVel - .003f, -.36f);
                }
            } else {
                if (npc.velocity.X > 0) {
                    RotationVel = Math.Min(RotationVel + .003f, .36f);
                } else if (npc.velocity.X < 0) {
                    RotationVel = Math.Max(RotationVel - .003f, -.36f);
                }
            }

            Rotation += RotationVel;

            float dist = Vector2.Distance(player.Center, npc.Center);
            Vector2 pullVec = npc.Center - player.Center;
            pullVec.Normalize();
            pullVec = pullVec * Math.Max(0, 900 - dist) / 1060 * Math.Abs(RotationVel) / 0.36f;
            player.velocity += pullVec;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D BladeTex = mod.GetTexture("NPCs/Enemies/Void/VortexBlades");
            Texture2D GlowTex = mod.GetTexture("Glowmasks/Vortex_Glow");
            Texture2D BladeGlowTex = mod.GetTexture("Glowmasks/VortexBlades_Glow");

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, npc.position, npc.width, npc.height, npc.scale, Rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, BladeGlowTex, 0, npc.position, npc.width, npc.height, npc.scale, Rotation, 0, 1, new Rectangle(0, 0, BladeGlowTex.Width, BladeGlowTex.Height), AAColor.ZeroShield, true);
            BaseDrawing.DrawTexture(spriteBatch, GlowTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, GlowTex.Width, GlowTex.Height), AAColor.ZeroShield, true);
            return false;
        }
    }
}