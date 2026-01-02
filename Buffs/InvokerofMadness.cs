using AAMod.Items.Dev.Invoker;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Buffs {
    public class InvokerofMadness : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("The Invoker of Madness");
			Description.SetDefault("The Crasy Invoker");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<InvokerPlayer>().InvokerMadness = true;
        }
	}
}