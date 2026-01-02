using AAMod.NPCs.Enemies.Sky;

namespace AAMod.NPCs.Bosses.Athena {
    public class SeraphA : Seraph
	{
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Seraph Guard");		
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 200;
            npc.damage = 90;
        }

        public override bool PreNPCLoot() => false;
    }
}