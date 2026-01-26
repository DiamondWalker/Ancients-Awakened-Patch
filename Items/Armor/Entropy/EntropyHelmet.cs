using AAMod.Globals.Players;
using AAMod.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace AAMod.Items.Armor.Entropy
{
    [AutoloadEquip(EquipType.Head)]
	public class EntropyHelmet : BaseAAItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entropy Helmet");
            Tooltip.SetDefault(@"12% increased damage
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
            item.width = 20;
            item.height = 22;
            item.value = 300000;
            item.defense = 14;
            item.rare = 9;
            AARarity = 12;
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = AAColor.Rarity12;
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.allDamage += 0.12f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("EntropyChestplate") && legs.type == mod.ItemType("EntropyLeggings");
		}

		public override void UpdateArmorSet(Player player)
		{
			
			player.setBonus = Language.GetTextValue("Mods.AAMod.Common.EntropyHelmetBonus");
            player.GetModPlayer<AAPlayer>().entropySet = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ApocalyptitePlate", 10);
            recipe.AddIngredient(null, "VoidEnergy", 20);
            recipe.AddTile(null, "BinaryReassembler");
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}