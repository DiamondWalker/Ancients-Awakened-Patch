using AAMod.Globals.Players;
using AAMod.Items.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace AAMod.Items.Accessories
{
    public class VoidCore : BaseAAItem {
        public override void SetDefaults() {
            item.width = 38;
            item.height = 42;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 6;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.gravity = Math.Max(player.gravity, Player.defaultGravity);

        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Void Core");
            Tooltip.SetDefault(@"Provides artificial gravity");
        }
    }
}