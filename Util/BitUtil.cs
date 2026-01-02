namespace AAMod.Util {
    public class BitUtil {
        public static long CombineInts(int int1, int int2) {
            return ((long)int1 << 32) | (long)int2;
        }

        public static int GetLeftInt(long num) {
            return (int)((num >> 32) & -1); // -1 is all ones
        }

        public static int GetRightInt(long num) {
            return (int)(num & -1); // -1 is all ones
        }
    }
}
