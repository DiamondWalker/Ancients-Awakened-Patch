using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AAMod.Util {
    public static class AnimationHelper {
        public static void SetFrame(NPC npc, int frame) {
            SetFrame(npc, frame, npc.height);
        }

        public static void SetFrame(NPC npc, int frame, int frameHeight) {
            npc.frame.Y = frame * frameHeight;
            npc.frameCounter = 0;
        }

        public static void UpdateAnimation(NPC npc, int[] animation, int frames) {
            UpdateAnimation(npc, animation, frames, 1);
        }

        public static void UpdateAnimation(NPC npc, int[] animation, int frames, int frameSpeed) {
            UpdateAnimation(npc, animation, frames, frameSpeed, npc.height);
        }

        public static void UpdateAnimation(NPC npc, int[] animation, int frames, int frameSpeed, int frameHeight) {
            if (animation.Length == 1) {
                SetFrame(npc, animation[0], frameHeight);
            } else {
                int currentFrame = Array.IndexOf(animation, npc.frame.Y / frameHeight);
                if (currentFrame < 0) {
                    SetFrame(npc, animation[0], frameHeight);
                } else {
                    npc.frameCounter += frameSpeed;
                    if (npc.frameCounter >= frames) {
                        currentFrame++;
                        if (currentFrame < animation.Length) {
                            SetFrame(npc, animation[currentFrame], frameHeight);
                        } else {
                            SetFrame(npc, animation[0], frameHeight);
                        }
                    }
                }
            }
        }
    }
}
