using AAMod.Globals.Players;
using AAMod.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace AAMod.Items.Armor.Entropy
{
    [AutoloadEquip(EquipType.Legs)]
	public class EntropyLeggings : BaseAAItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entropy Leggings");
			Tooltip.SetDefault(@"15% increased movement speed
13% increased damage
The power to destroy entire planets rests in this armor");

		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Glowmasks/" + GetType().Name + "_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = 300000;
            item.rare = 9;
			item.defense = 12;
            AARarity = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.15f;
            player.allDamage += 0.13f;
            player.GetModPlayer<AAPlayer>().MaxMovespeedboost += .15f;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = AAColor.Rarity12;
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ApocalyptitePlate", 13);
            recipe.AddIngredient(null, "VoidEnergy", 25);
            recipe.AddTile(null, "BinaryReassembler");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}