using AAMod.Globals.Players;
using AAMod.Items.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;

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
            player.GetModPlayer<AAPlayer>().RealityStone = true;
            player.gravControl = true;
            player.noFallDmg = true;

        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Reality Stone");
            Tooltip.SetDefault(@"Allows the holder to reverse gravity
Press Up to change gravity
Gravity change does not flip the screen
Negates fall damage");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 13));
        }

    }
}