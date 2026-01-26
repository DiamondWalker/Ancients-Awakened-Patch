using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AAMod.Util {
    public static class RandUtil {
        public static int InclusiveRand(int min, int max) {
            return Main.rand.Next(min, max + 1);
        }

        public static bool Chance(int chance) {
            return Main.rand.Next(chance) == 0;
        }

        public static Vector2 RandomPolar(float magnitude) {
            float angle = Main.rand.NextFloat() * (float)Math.PI * 2;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * magnitude;
        }
    }
}
