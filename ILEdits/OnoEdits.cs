using AAMod.Globals.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AAMod.ILEdits {
    public class OnoEdits {
        public static void ApplyEdits() {
            On.Terraria.Main.DrawPlayer_DrawAllLayers += PostDrawPlayer;
        }

        private static void PostDrawPlayer(On.Terraria.Main.orig_DrawPlayer_DrawAllLayers orig, Main main, Player drawPlayer, int projectileDrawPosition, int cHead) {
            orig(main, drawPlayer, projectileDrawPosition, cHead);
            if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].GetModPlayer<AAPlayer>().ono) BaseDrawing.DrawHitbox(Main.spriteBatch, drawPlayer.Hitbox, new Color(150, 0, 0, 50));
        }
    }
}
