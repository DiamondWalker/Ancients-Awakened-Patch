using Terraria.ModLoader;

namespace AAMod {
    public class Titles : ModPlayer
	{
		public bool text;

		public float alphaText = 255f;

		public float alphaText2 = 255f;

		public float alphaText3 = 255f;

		public float alphaText4 = 255f;

		public int BossID;

		public override void ResetEffects()
		{
			this.text = false;
		}

		public override void PreUpdate()
		{
			if (!AAGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<Title>()) && !AAGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<SistersTitle>()))
			{
				this.alphaText = 255f;
				this.alphaText2 = 255f;
			}
			if (!AAGlobalProjectile.AnyProjectiles(ModContent.ProjectileType<SistersTitle>()))
			{
				this.alphaText3 = 255f;
				this.alphaText4 = 255f;
			}
		}
	}
}
