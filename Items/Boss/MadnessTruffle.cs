using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Boss
{
    public class MadnessTruffle : BaseAAItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Madness Truffle");
            Tooltip.SetDefault(@"+50 Health
+30 Mana
Increased jump speed and allows auto-jump
You are immune to fall damage
Increased jump height
You know what? Just don't put it anywhere near your mouth.");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.accessory = true;
            item.expert = true; item.expertOnly = true;
        }

        public override void UpdateEquip(Player player)
        {
            // hearty truffle
            player.statLifeMax2 += 50;

            // magic truffle
            player.statManaMax2 += 30;

            // toad leg
            player.autoJump = true;
            Player.jumpHeight += 5;
            player.jumpSpeedBoost += 1.5f;
            player.noFallDmg = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "HeartyTruffle", 1);
            recipe.AddIngredient(null, "MagicTruffle", 1);
            recipe.AddIngredient(null, "ToadLeg", 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}