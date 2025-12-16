using Microsoft.Xna.Framework;
using System;

namespace AAMod.Util
{

    public static class MathUtil
    {
        public static int Signum(int num)
        {
            if (num > 0) return 1;
            if (num < 0) return -1;
            return 0;
        }

        public static int Signum(float num)
        {
            if (num > 0) return 1;
            if (num < 0) return -1;
            return 0;
        }

        public static Vector2 VectorFromPolar(float magnitude, float angle) {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * magnitude;
        }

        public static float GetRelativeAngle(float angle1, float angle2) {
            angle1 = (float)(angle1 % (Math.PI * 2));
            if (angle1 < 0) angle1 += (float)(Math.PI * 2);
            angle2 = (float)(angle2 % (Math.PI * 2));
            if (angle2 < 0) angle2 += (float)(Math.PI * 2);

            float relativeAngle = angle1 - angle2;
            if (Math.Abs(relativeAngle) > Math.PI) {
                relativeAngle = -relativeAngle;
            }

            return relativeAngle;
        }

        public static Vector2 LimitVectorLength(Vector2 vector, float maxLength) {
            if (vector.Length() > maxLength) {
                vector.Normalize();
                vector *= maxLength;
            }
            return vector;
        }
    }
}
