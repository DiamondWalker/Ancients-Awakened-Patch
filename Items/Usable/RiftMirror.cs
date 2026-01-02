using AAMod.Globals.Players;
using AAMod.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Items.Usable
{
    public class RiftMirror : BaseAAItem, UpdateDuringUseItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rift Mirror");
            Tooltip.SetDefault(@"Gaze in the mirror to return home
Right click to return to your previous location");
        }    

		public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.MagicMirror);
        }

        public override bool AltFunctionUse(Player player) {
            if (player.GetModPlayer<AAPlayer>().RiftMirrorReturnPos != Vector2.Zero) return true;

            return base.AltFunctionUse(player);
        }

        public void UpdateItemUse(Player player) {
            // vanilla mirror code
            if (player.itemAnimation > 0) {
                if (Main.rand.Next(2) == 0) {
                    Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.1f);
                }

                if (player.itemTime == 0) {
                    player.itemTime = PlayerHooks.TotalUseTime(item.useTime, player, item);
                } else if (player.itemTime == PlayerHooks.TotalUseTime(item.useTime, player, item) / 2) {
                    for (int num332 = 0; num332 < 70; num332++) {
                        Dust.NewDust(player.position, player.width, player.height, 15, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                    }

                    player.grappling[0] = -1;
                    player.grapCount = 0;
                    for (int num333 = 0; num333 < 1000; num333++) {
                        if (Main.projectile[num333].active && Main.projectile[num333].owner == player.whoAmI && Main.projectile[num333].aiStyle == 7) {
                            Main.projectile[num333].Kill();
                        }
                    }

                    // custom code starts here
                    AAPlayer modPlayer = player.GetModPlayer<AAPlayer>();
                    if (player.altFunctionUse == 2 && modPlayer.RiftMirrorReturnPos != Vector2.Zero) { // right click (return)
                        player.Spawn();
                        player.position = modPlayer.RiftMirrorReturnPos;
                        modPlayer.RiftMirrorReturnPos = Vector2.Zero;
                    } else { // left click (home)
                        modPlayer.RiftMirrorReturnPos = player.position;
                        player.Spawn();
                    }
                    // custom code ends here

                    for (int num334 = 0; num334 < 70; num334++) {
                        Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                    }
                }
            }
        }
    }
}
