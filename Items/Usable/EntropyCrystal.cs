using AAMod.Globals.Players;
using AAMod.Items.Base;
using AAMod.Worldgeneration.Dimension.Void;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AAMod.Items.Usable
{
    public class EntropyCrystal : BaseAAItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropy Crystal");
            Tooltip.SetDefault(@"Allows you to teleport to and from the Void");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 41));
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(50);
            item.width = 32;
            item.height = 32;
            item.rare = 1;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.UseSound = null;
            /*item.useAnimation = 45;
            item.useTime = 90;
            item.useStyle = 4;*/
            item.noUseGraphic = true;
        }

        public override bool UseItem(Player player)
        {
            player.GetModPlayer<RiftTeleportPlayer>().ForceTeleport = true;

            return true;
        }
    }
}