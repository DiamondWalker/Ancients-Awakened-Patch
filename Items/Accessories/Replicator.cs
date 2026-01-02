using AAMod.Globals.Players;
using AAMod.Items.Base;
using Terraria;

namespace AAMod.Items.Accessories
{
    public class Replicator : BaseAAItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AAPlayer>().Replicator = true;
		}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ammo Replicator");
            Tooltip.SetDefault("Provides unlimited ammo");
        }

    }
}