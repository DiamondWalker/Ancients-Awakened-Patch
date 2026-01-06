using AAMod.Buffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.ILEdits {
    public class GlitchedEdits {
        public static void ApplyEdits() {
            On.Terraria.Player.Update += UpdatePlayer;
            On.Terraria.NPC.UpdateNPC += UpdateNPC;
            On.Terraria.Projectile.Update += UpdateProj;
            On.Terraria.Dust.UpdateDust += UpdateDust;
        }

        private static void UpdatePlayer(On.Terraria.Player.orig_Update orig, Player p, int i) {
            int buff = ModContent.BuffType<Glitched>();

            if (p.active && p.HasBuff(buff)) {
                // make sure the glitched buff is still counting down even though the others aren't
                int index = p.FindBuffIndex(buff);
                p.buffTime[index]--;

                // the teleport
                if (Main.myPlayer == p.whoAmI && AAMod.AccessoryAbilityKey.JustPressed) {
                    Vector2 vector32;
                    vector32.X = Main.mouseX + Main.screenPosition.X;
                    if (p.gravDir == 1f) {
                        vector32.Y = Main.mouseY + Main.screenPosition.Y - p.height;
                    } else {
                        vector32.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
                    }
                    vector32.X -= p.width / 2;
                    if (vector32.X > 50f && vector32.X < (Main.maxTilesX * 16) - 50 && vector32.Y > 50f && vector32.Y < (Main.maxTilesY * 16) - 50) {
                        int num246 = (int)(vector32.X / 16f);
                        int num247 = (int)(vector32.Y / 16f);
                        if ((Main.tile[num246, num247].wall != 87 || num247 <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector32, p.width, p.height)) {
                            p.Teleport(vector32, 1, 0);
                            NetMessage.SendData(65, -1, -1, null, 0, p.whoAmI, vector32.X, vector32.Y, 1, 0, 0);
                            Main.PlaySound(AAMod.instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/Glitch"));
                            p.ClearBuff(ModContent.BuffType<Glitched>());
                        }
                    }
                }

                return;
            }

            // if any other players have the buff, we still freeze
            foreach (Player player in Main.player) {
                if (player != null && player != p && player.active && player.HasBuff(buff)) {
                    return;
                }
            }

            orig(p, i);
        }

        private static void UpdateNPC(On.Terraria.NPC.orig_UpdateNPC orig, NPC npc, int i) {
            int buff = ModContent.BuffType<Glitched>();
            foreach (Player player in Main.player) {
                if (player != null && player.active && player.HasBuff(buff)) {
                    return;
                }
            }

            orig(npc, i);
        }

        private static void UpdateProj(On.Terraria.Projectile.orig_Update orig, Projectile proj, int i) {
            int buff = ModContent.BuffType<Glitched>();
            foreach (Player player in Main.player) {
                if (player != null && player.active && player.HasBuff(buff)) {
                    return;
                }
            }

            orig(proj, i);
        }

        private static void UpdateDust(On.Terraria.Dust.orig_UpdateDust orig) {
            int buff = ModContent.BuffType<Glitched>();
            foreach (Player player in Main.player) {
                if (player != null && player.active && player.HasBuff(buff)) {
                    return;
                }
            }

            orig();
        }
    }
}
