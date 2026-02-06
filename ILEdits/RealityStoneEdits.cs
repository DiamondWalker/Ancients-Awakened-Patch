using AAMod.Globals.Players;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AAMod.ILEdits {
    public class RealityStoneEdits {
        public static void ApplyEdits() {
            IL.Terraria.Player.Update += PlayerUpdate;
            On.Terraria.Player.SmartInteractLookup += SmartInteractLookup;
            On.Terraria.Player.SmartCursorLookup += SmartCursorLookup;
            On.Terraria.Player.ItemCheck += ItemCheck;
            On.Terraria.Player.QuickGrapple += QuickGrapple;
            On.Terraria.Main.DoDraw += DoDraw;
            On.Terraria.Main.DrawPlayer += DrawPlayer;
        }

        private static void PlayerUpdate(ILContext context) { // tile positions
            var c = new ILCursor(context);

            c.GotoNext(
                MoveType.After,
                i => i.Match(OpCodes.Conv_I4),
                i => i.MatchStsfld(typeof(Player), nameof(Player.tileTargetY)),
                i => i.Match(OpCodes.Ldarg_0),
                i => i.MatchLdfld(typeof(Player), nameof(Player.gravDir))
            );

            c.EmitDelegate<Func<float, float>>((og) => {
                return Main.LocalPlayer.GetModPlayer<AAPlayer>().RealityStone ? 1 : og;
            });
        }

        private static void SmartInteractLookup(On.Terraria.Player.orig_SmartInteractLookup orig, Player p) { // smart cursor
            if (p.whoAmI == Main.myPlayer && p.GetModPlayer<AAPlayer>().RealityStone) {
                float oldDir = p.gravDir;
                p.gravDir = 1;
                orig(p);
                p.gravDir = oldDir;
            } else {
                orig(p);
            }
        }

        private static void SmartCursorLookup(On.Terraria.Player.orig_SmartCursorLookup orig, Player p) { // smart cursor
            if (p.whoAmI == Main.myPlayer && p.GetModPlayer<AAPlayer>().RealityStone) {
                float oldDir = p.gravDir;
                p.gravDir = 1;
                orig(p);
                p.gravDir = oldDir;
            } else {
                orig(p);
            }
        }

        private static void ItemCheck(On.Terraria.Player.orig_ItemCheck orig, Player p, int i) { // invert mouse location
            if (p.whoAmI == Main.myPlayer && p.GetModPlayer<AAPlayer>().RealityStone && p.gravDir != 1) {
                int oldMouseY = Main.mouseY;
                Main.mouseY = Main.screenHeight - Main.mouseY;
                orig(p, i);
                Main.mouseY = oldMouseY;
            } else {
                orig(p, i);
            }
        }

        private static void QuickGrapple(On.Terraria.Player.orig_QuickGrapple orig, Player p) { // grapple location
            if (p.whoAmI == Main.myPlayer && p.GetModPlayer<AAPlayer>().RealityStone) {
                float oldDir = p.gravDir;
                p.gravDir = 1;
                orig(p);
                p.gravDir = oldDir;
            } else {
                orig(p);
            }
        }

        static float realityGravDir;
        private static void DrawPlayer(On.Terraria.Main.orig_DrawPlayer orig, Main main, Player p, Vector2 pos, float rot, Vector2 rotOrigin, float shadow) {
            if (p.whoAmI == Main.myPlayer && p.GetModPlayer<AAPlayer>().RealityStone) {
                float oldDir = p.gravDir;
                p.gravDir = realityGravDir;
                orig(main, p, pos, rot, rotOrigin, shadow);
                p.gravDir = oldDir;
            } else {
                orig(main, p, pos, rot, rotOrigin, shadow);
            }
        }

        private static void DoDraw(On.Terraria.Main.orig_DoDraw orig, Main main, GameTime time) {
            realityGravDir = Main.LocalPlayer.gravDir;
            if (AAMod.isFullyReady && Main.LocalPlayer.active && Main.LocalPlayer.GetModPlayer<AAPlayer>().RealityStone) {
                Main.LocalPlayer.gravDir = 1;
                orig(main, time);
                Main.LocalPlayer.gravDir = realityGravDir;
            } else {
                orig(main, time);
            }
        }
    }
}
