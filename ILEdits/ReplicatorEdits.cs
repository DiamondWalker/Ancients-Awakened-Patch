using AAMod.Globals.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AAMod.ILEdits {
    public class ReplicatorEdits {
        public static void ApplyEdits() {
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += NewProjectile;
        }

        private static int NewProjectile(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1) {
            int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);
            if (Main.player[Owner].GetModPlayer<AAPlayer>().Replicator) Main.projectile[index].noDropItem = true;
            return index;
        }
    }
}
