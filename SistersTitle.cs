using Terraria;
using Terraria.ModLoader;

namespace AAMod {
    public class SistersTitle : ModProjectile
	{
		public override string Texture => "AAMod/BlankTex";

		public override void SetDefaults()
		{
			base.projectile.width = 1;
			base.projectile.height = 1;
			base.projectile.penetrate = -1;
			base.projectile.hostile = false;
			base.projectile.friendly = false;
			base.projectile.tileCollide = false;
			base.projectile.ignoreWater = true;
			base.projectile.timeLeft = 300;
		}

		public override void AI()
		{
			Titles modPlayer = Main.player[base.projectile.owner].GetModPlayer<Titles>();
			modPlayer.text = true;
			modPlayer.BossID = (int)base.projectile.ai[0];
			base.projectile.velocity.X = 0f;
			base.projectile.velocity.Y = 0f;
			if (base.projectile.timeLeft <= 45)
			{
				if (modPlayer.alphaText < 255f)
				{
					modPlayer.alphaText += 10f;
					modPlayer.alphaText2 += 10f;
					modPlayer.alphaText3 += 10f;
					modPlayer.alphaText4 += 10f;
				}
				return;
			}
			if (base.projectile.timeLeft <= 240)
			{
				modPlayer.alphaText -= 5f;
			}
			if (base.projectile.timeLeft <= 200)
			{
				modPlayer.alphaText3 -= 5f;
			}
			if (base.projectile.timeLeft <= 160)
			{
				modPlayer.alphaText4 -= 5f;
			}
			if (modPlayer.alphaText2 > 0f)
			{
				modPlayer.alphaText2 -= 5f;
			}
		}
	}
}
