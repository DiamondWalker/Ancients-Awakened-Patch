using Terraria.ModLoader;

namespace AAMod.Buffs {
    public class Glitched : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Glitched");
			Description.SetDefault("Your head is like 10 feet in front of you");
			//Main.persistentBuff[Type] = true;
			//Main.meleeBuff[Type] = true;
			canBeCleared = false;
			
		}
    }
}
