using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Items.Accessories {
    public class RealityStone : BaseAAItem {
        public override void SetDefaults() {
            item.width = 38;
            item.height = 42;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 6;
            item.accessory = true;
        }

        private static float grav = 0.0f;

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

            if (player.controlUp) {
                grav -= 0.09f;
            }
            if (player.controlDown) {
                grav += 0.09f;
            }
            grav = MathHelper.Clamp(grav, -1.0f, 1.0f);
            Main.NewText(grav);
            if (grav < 0.0f) {
                player.gravDir = -1;
            } else {
                player.gravDir = 1;
            }
            player.gravity = MathHelper.Clamp(player.gravity, -0.4f, 0.4f);
            player.gravity = player.gravity * 1.8f * Math.Abs(grav);
            player.maxFallSpeed = player.maxFallSpeed * 1.2f * Math.Abs(grav);
            player.gravControl2 = true;
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