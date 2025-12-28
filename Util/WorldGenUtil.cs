using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AAMod.Util {
    public class WorldGenUtil {
        public static bool TileAt(int x, int y) {
            return WorldGen.InWorld(x, y) && Main.tile[x, y] != null && Main.tile[x, y].active();
        }

        public static void DeleteTile(int x, int y) {
            if (!WorldGen.InWorld(x, y) || Main.tile[x, y] == null) return;
            Main.tile[x, y].type = 0;
            Main.tile[x, y].active(false);
        }
        public static void ClearCircle(int x, int y, int radius, Rectangle bounds = default) {
            for (int x2 = x - radius; x2 <= x + radius; x2++) {
                for (int y2 = y - radius; y2 <= y + radius; y2++) {
                    int distX = x2 - x;
                    int distY = y2 - y;
                    if ((bounds == default || bounds.Contains(x2, y2)) && Math.Sqrt(distX * distX + distY * distY) <= radius && TileAt(x2, y2)) {
                        DeleteTile(x2, y2);
                    }
                }
            }
        }
    }
}
