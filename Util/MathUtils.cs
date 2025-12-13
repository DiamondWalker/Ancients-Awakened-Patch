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
    }
}
