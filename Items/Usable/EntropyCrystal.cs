
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Terraria.DataStructures;
using AAMod.Worldgeneration.Dimension.Void;

namespace AAMod.Items.Usable
{
    //imported from my tAPI mod because I'm lazy
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
            item.width = 32;
            item.height = 32;
            item.rare = 1;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.useAnimation = 45;
            item.useTime = 90;
            item.useStyle = 4;
            item.noUseGraphic = true;
        }

        public override bool UseItem(Player player)
        {
            if (Main.netMode != 1)
            {
                if (!VoidSubworld.IsInside())
                {
                    VoidSubworld.Enter();
                }
                else
                {
                    VoidSubworld.Exit();
                }
            }

            return true;
        }
    }
}