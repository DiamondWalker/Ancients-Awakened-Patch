using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Boss.Zero
{
    public class BrokenCode : BaseAAItem
    {
        
        public bool on = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Code");
            Tooltip.SetDefault(@"Pressing the accessory ability hotkey allows you to glitch with a 5 second cooldown
While cooldown is occurring, your speed is increased, you gain invincibility frames
While cooldown is occurring, your magic/summon weapons require no mana and have 20% increased damage
Teleportation has 15 second cooldown
'You don't look so good'
WARNING: May permanently displace appendages until game restart. This is a feature.");
            ItemID.Sets.ItemNoGravity[item.type] = true;
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