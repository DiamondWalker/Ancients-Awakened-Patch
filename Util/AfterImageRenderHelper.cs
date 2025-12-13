using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AAMod.Util {
    public class AfterImageRenderHelper {
        /*private AfterImage[] afterImages;

        public AfterImageRenderHelper(int count) {
            afterImages = new AfterImage[count];
        }

        public void Update(Entity entity) {
            for (int i = afterImages.Length - 1; i >= 0; i--) {
                afterImages[i] = afterImages[i - 1];
            }
            afterImages[0] = new AfterImage(entity);
        }

        public void Render() {
            for (int i = afterImages.Length - 1; i >= 0; i--) {
                BaseDrawing.
            }
        }

        public void SetAfterImageCount(int count) {
            AfterImage[] newArray = new AfterImage[count];

            for (int i = 0; i < Math.Min(afterImages.Length, newArray.Length); i++) {
                newArray[i] = afterImages[i];
            }

            afterImages = newArray;
        }

        protected class AfterImage {
            public readonly Vector2 position;
            public readonly float width;
            public readonly float height;
            public readonly float scale;
            public readonly float rotation;

            public AfterImage(Entity entity) {
                position = entity.position;
                if (entity is NPC npc) {
                    width = npc.width;
                    height = npc.height;
                    scale = npc.scale;
                    rotation = npc.rotation;
                } else if (entity is Projectile proj) {
                    width = proj.width;
                    height = proj.height;
                    scale = proj.scale;
                    rotation = proj.rotation;
                } else {
                    throw new ArgumentException("This type of entity is not supported for after image rendering!");
                }
            }
        }*/
    }
}
