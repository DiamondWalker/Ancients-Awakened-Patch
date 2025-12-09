using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAMod.Items.Usable {
    public class MegaMush : BaseAAItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mega Mushroom");
            Tooltip.SetDefault(@"Permanently unlocks the ability to respawn at max life");
        }

        public override void SetDefaults() {
            item.CloneDefaults(5);
            item.maxStack = 1;
            item.potion = false;
            item.healLife = 0;
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(0, 2, 0, 0);
        }

        // We use the CanUseItem hook to prevent a player from using this item while the boss is present in the world.
        public override bool CanUseItem(Player player) {
            return !player.GetModPlayer<AAPlayer>().MegaMush;
        }

        public override bool UseItem(Player player) {
            player.GetModPlayer<AAPlayer>().MegaMush = true;
            return true;
        }
    }
}
