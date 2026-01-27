using AAMod.Items.FishingItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AAMod.Globals.Players {
    public class AAFishingPlayer : ModPlayer {
        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk) {
            if (Main.rand.Next(100) < 10 + (player.cratePotion ? 10 : 0)) {
                if (liquidType == 0 && player.ZoneSnow) {
                    caughtType = mod.ItemType("IceCrate");
                }

                if (liquidType == 0 && player.ZoneDesert) {
                    caughtType = mod.ItemType("DesertCrate");
                }

                if ((liquidType == 0 || liquidType == 1) && player.GetModPlayer<AABiomesPlayer>().ZoneInferno) {
                    caughtType = mod.ItemType("InfernoCrate");
                }

                if (liquidType == 0 && player.GetModPlayer<AABiomesPlayer>().ZoneMire) {
                    caughtType = mod.ItemType("MireCrate");
                }

                if (liquidType == 0 && player.GetModPlayer<AABiomesPlayer>().ZoneVoid) {
                    caughtType = mod.ItemType("VoidCrate");
                }

                if (liquidType == 0 && player.GetModPlayer<AABiomesPlayer>().ZoneHoard) {
                    caughtType = ItemID.GoldenCrate;
                }

                if (liquidType == 1 && ItemID.Sets.CanFishInLava[fishingRod.type] && player.ZoneUnderworldHeight) {
                    caughtType = mod.ItemType("HellCrate");
                }
            }

            if (questFish == mod.ItemType("TriHeadedKoi") && player.GetModPlayer<AABiomesPlayer>().ZoneMire && Main.rand.NextBool()) {
                caughtType = mod.ItemType("TriHeadedKoi");
            }

            if (questFish == mod.ItemType("Fishmother") && player.GetModPlayer<AABiomesPlayer>().ZoneInferno && Main.rand.NextBool()) {
                caughtType = mod.ItemType("Fishmother");
            }

            if (questFish == mod.ItemType("GlitchFish") && player.GetModPlayer<AABiomesPlayer>().ZoneVoid && Main.rand.NextBool()) {
                caughtType = mod.ItemType("GlitchFish");
            }

            if (player.GetModPlayer<AABiomesPlayer>().ZoneInferno) {
                if (Main.rand.Next(50) == 0 && Main.hardMode) {
                    caughtType = mod.ItemType("ScorchShark");
                } else if (Main.rand.Next(49) == 0) {
                    caughtType = mod.ItemType("SharpeningLavaFish");
                }
            }

            if (player.GetModPlayer<AABiomesPlayer>().ZoneMire && Main.hardMode) {
                if (Main.rand.Next(50) == 0 && Main.hardMode) {
                    caughtType = mod.ItemType("SwimmingHydra");
                } else if (Main.rand.Next(49) == 0) {
                    caughtType = mod.ItemType("ToxinMonkfish");
                }
            }

            if (Main.rand.Next(4096) == 0 && liquidType == 0 && player.fishingSkill >= 100|| Main.rand.Next(2048) == 0 && player.accFishingLine && player.accTackleBox) {
                caughtType = ModContent.ItemType<ShinyCharmFish>();
            }
        }
    }
}
