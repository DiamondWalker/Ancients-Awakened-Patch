using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AAMod.Items.Base {
    public interface UpdateDuringUseItem {
        void UpdateItemUse(Player player);
    }
}
