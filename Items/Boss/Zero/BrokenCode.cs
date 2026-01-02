using AAMod.Globals.Players;
using AAMod.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Items.Boss.Zero
{
    public class BrokenCode : BaseAAItem
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Code");
            Tooltip.SetDefault(@"Pressing the accessory ability hotkey allows you to glitch the game with a 20 second cooldown
Glitching the game causes all entities to freeze for 2.5 seconds and displaces your body parts for the duration of the cooldown
Pressing the accessory ability hotkey again during the freeze period will fix the freeze and telport the player to the cursor
'You don't look so good'");
            //ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        // TODO -- Velocity Y smaller, post NewItem?
        public override void SetDefaults()
        {
            item.width = 60;
            item.height = 52;
            item.maxStack = 1;
            item.value = Item.sellPrice(3, 0, 0, 0);
            item.expert = true; item.expertOnly = true;
            item.accessory = true;
            item.rare = 9; AARarity = 13;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = AAColor.Rarity13;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return AAColor.COLOR_WHITEFADE1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AAPlayer>().BrokenCode = true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Red.ToVector3() * 0.55f * Main.essScale);
        }
    }
}