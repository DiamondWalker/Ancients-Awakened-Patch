using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AAMod.Util {
    public static class AnimationHelper {
        public static void UpdateAnimation(NPC npc, int[] animation, int frames) {
            UpdateAnimation(npc, animation, frames, npc.height);
        }

        public static void UpdateAnimation(NPC npc, int[] animation, int frames, int frameHeight) {
            UpdateAnimation(npc, animation, frames, 1, frameHeight);
        }

        public static void UpdateAnimation(NPC npc, int[] animation, int frames, int frameSpeed, int frameHeight) {
            if (animation.Length == 1) {
                npc.frame.Y = animation[0] * frameHeight;
                npc.frameCounter = 0;
            } else {
                int currentFrame = Array.IndexOf(animation, npc.frame.Y / frameHeight);
                if (currentFrame < 0) {
                    npc.frame.Y = animation[0] * frameHeight;
                    npc.frameCounter = 0;
                } else {
                    npc.frameCounter += frameSpeed;
                    if (npc.frameCounter >= frames) {
                        currentFrame++;
                        if (currentFrame < animation.Length) {
                            npc.frame.Y = animation[currentFrame] * frameHeight;
                            npc.frameCounter = 0;
                        } else {
                            npc.frame.Y = animation[0] * frameHeight;
                            npc.frameCounter = 0;
                        }
                    }
                }
            }
        }
    }
}
