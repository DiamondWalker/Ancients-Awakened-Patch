using AAMod.NPCs.Bosses.Zero;
using AAMod.Util;
using AAMod.Worldgeneration.Dimension.Void;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AAMod.NPCs.Other.VoidRift
{
    public class VoidRift : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rift");
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 20000;
            npc.damage = 0;
            npc.defense = 20;
            npc.knockBackResist = 0f;
            npc.width = 264;
            npc.height = 297;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.alpha = 0;
            npc.dontTakeDamage = true;
            npc.boss = false;
            npc.npcSlots = 0;
            npc.ai[0] = 0.0f;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void AI()
        {
            npc.rotation -= 0.01f;
            npc.scale = 1.8f * npc.ai[0];
            //if (Main.BlackFadeIn > 0) Main.BlackFadeIn--;
            Main.NewText(Main.BlackFadeIn);
            if (npc.ai[0] < 1.0f) {
                npc.ai[0] = Math.Min(npc.ai[0] + 0.01f, 1.0f);
            } else {
                npc.ai[0] = 1.0f;
                foreach (Player player in Main.player) {
                    if (player != null && player.active && !player.dead) {
                        if (Vector2.Distance(player.Center, npc.Center) > 80) {
                            return;
                        }
                    }
                }
                Main.BlackFadeIn += 26;
                if (Main.BlackFadeIn > 255) {
                    if (VoidSubworld.IsInside()) {
                        VoidSubworld.Exit();
                    } else {
                        VoidSubworld.Enter();
                    }
                }
            }
        }

        /*public Color GetGlowAlpha()
        {
            return AAColor.ZeroShield;
        }*/


        public override bool PreDraw(SpriteBatch spritebatch, Color drawColor)
        {
            Texture2D Tex = Main.npcTexture[npc.type];
            Texture2D DimTex = mod.GetTexture("NPCs/Other/VoidRift/" + (VoidSubworld.IsInside() ? "Overworld" : "Void"));

            Color dimColor = Color.White;
            dimColor.A = (byte)(npc.ai[0] * 255);
            
            Color color = ColorUtils.COLOR_GLOWPULSE;
            color.A = 255;

            BaseDrawing.DrawTexture(spritebatch, DimTex, 0, npc.position - new Vector2(3, 4), npc.width, npc.height, npc.scale * 1.1f, 0, 0, 1, new Rectangle(0, 0, DimTex.Width, DimTex.Height), dimColor, true);
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, Tex.Width, Tex.Height), color, true);
            //BaseDrawing.DrawTexture(spritebatch, DimTex, 0, npc.Center - new Vector2(DimTex.Width, DimTex.Height) / 2, DimTex.Width, DimTex.Height, npc.scale, 0, 0, 1, new Rectangle(0, 0, DimTex.Width, DimTex.Height), Color.White, false);
            //BaseDrawing.DrawTexture(spritebatch, Tex, 0, npc.Center - new Vector2(Tex.Width, Tex.Height) / 2, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, Tex.Width, Tex.Height), color, false);

            return false;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage = 0;
            return false;
        }
    }

    public class RiftHandler : ModWorld {
        public static int RiftX = -1;
        public static int RiftY = -1;

        public override void Initialize() {
            RiftX = -1;
            RiftY = -1;
        }

        public override TagCompound Save() {
            var tag = new TagCompound();
            if (RiftX != -1) {
                tag.Add("RiftX", RiftX);
                tag.Add("RiftY", RiftY);
            }
            return tag;
        }

        public override void Load(TagCompound tag) {
            Reset(); //reset it so it doesn't fuck up between world loads	
            if (tag.ContainsKey("RiftX")) {
                RiftX = tag.GetInt("RiftX");
                RiftY = tag.GetInt("RiftY");
                if (AAWorld.downedEquinox && !VoidSubworld.IsInside())
                    NPC.NewNPC(RiftX, RiftY, mod.NPCType("VoidRift"));
            }
        }

        public override void PostUpdate() {
            if (Main.netMode != 1 && AAWorld.downedEquinox && !VoidSubworld.IsInside()) {
                SpawnDeactivatedZero();
            }
        }

        public void Reset() {
            RiftX = -1;
            RiftY = -1;
        }

        public void SpawnDeactivatedZero() {
            int VoidHeight = 140;

            Point spawnTilePos = new Point((Main.maxTilesX / 15 * 14) + (Main.maxTilesX / 15 / 2) - 100, VoidHeight);
            Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);
            bool anyRiftsExist = NPC.AnyNPCs(mod.NPCType("VoidRift"));
            if (!anyRiftsExist) {
                int whoAmI = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<VoidRift>());
                RiftX = (int)spawnPos.X;
                RiftY = (int)spawnPos.Y;
                if (Main.netMode == 2 && whoAmI != -1 && whoAmI < 200) {
                    NetMessage.SendData(MessageID.SyncNPC, number: whoAmI);
                }
            }
        }
    }
}

