using AAMod.NPCs.Other.VoidRift;
using AAMod.Worldgeneration.Dimension.Void;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.Globals.Players {
    public class RiftTeleportPlayer : ModPlayer {
        public bool ForceTeleport = false;
        public bool InRift = false;
        public int RiftTime = 0;

        public override void PostUpdate() {
            InRift = false;
            foreach (NPC npc in Main.npc) {
                if (npc.type == ModContent.NPCType<VoidRift>()) {
                    if (Vector2.Distance(player.Center, npc.Center) < 80) {
                        InRift = true;
                        break;
                    }
                }
            }

            if (InRift || ForceTeleport) {
                if (RiftTime < 300) {
                    RiftTime += 3;
                }
            } else {
                if (RiftTime > 0) {
                    RiftTime -= 3;
                }
            }

            if (RiftTime >= 300) {
                ForceTeleport = false;
                if (VoidSubworld.IsInside()) {
                    VoidSubworld.Exit();
                } else {
                    VoidSubworld.Enter();
                }
            }

            if (player == Main.LocalPlayer) {
                if (RiftTime > Main.BlackFadeIn) {
                    Main.BlackFadeIn = RiftTime;
                }
            }
        }
    }
}
