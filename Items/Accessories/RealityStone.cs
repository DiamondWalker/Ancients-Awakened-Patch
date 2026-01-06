using AAMod.Globals.Players;
using AAMod.Items.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace AAMod.Items.Accessories
{
    public class RealityStone : BaseAAItem {
        public override void SetDefaults() {
            item.width = 38;
            item.height = 42;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            /*if (player.controlUp) {
                player.gravity = 0;
                player.velocity.Y *= 0.9f;
            }
            if (player.controlDown) {
                player.gravity = Math.Max(player.gravity, 0.4f);
                player.gravity *= 1.8f;
                player.maxFallSpeed *= 1.4f;
            }*/

            AAPlayer p = player.GetModPlayer<AAPlayer>();
            
            p.RealityStone = true;

            if (player.controlUp) {
                p.RealityGrav -= 0.09f;
            }
            if (player.controlDown) {
                p.RealityGrav += 0.09f;
            }
            p.RealityGrav = MathHelper.Clamp(p.RealityGrav, -1.0f, 1.0f);
            if (p.RealityGrav < 0.0f) {
                player.gravDir = -1;
            } else {
                player.gravDir = 1;
            }
            player.gravity = Math.Abs(p.RealityGrav) * 0.6f;
            //player.gravity = player.gravity * 1.8f * Math.Abs(p.RealityGrav);
            player.maxFallSpeed = Math.Abs(p.RealityGrav) * 15;
            player.gravControl2 = true; // this is the gravity globe one. It cannot change gravity in midair
            player.gravControl = false; // this is the gravitation potion one; they cause weird conflicts if both are active
            player.noFallDmg = true;

        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Reality Stone");
            Tooltip.SetDefault(@"Allows the holder to reverse gravity
Press Up to change gravity
Gravity change does flip screen
Increases wearer's fall speed");
        }

    }
}