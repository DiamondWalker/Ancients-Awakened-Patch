using AAMod.Worldgeneration.Dimension.Void;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Items.Usable {
    public class VoidTester : BaseAAItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Void Tester");
            Tooltip.SetDefault(@"Teleports you into the void subworld.
Non-Consumable");
        }

        public override void SetDefaults() {
            item.width = 16;
            item.height = 16;
            item.rare = 8;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
        }

        public override bool UseItem(Player player) {
            VoidSubworld.Enter();
            return true;
        }
    }
}
