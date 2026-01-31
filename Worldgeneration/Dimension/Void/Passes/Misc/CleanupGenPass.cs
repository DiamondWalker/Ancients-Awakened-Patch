using AAMod.Worldgeneration.Dimension.Void.Passes.Islands;
using Terraria.Localization;
using Terraria.World.Generation;

namespace AAMod.Worldgeneration.Dimension.Void.Passes.Misc
{
    public class CleanupGenPass : GenPass
    {
        public CleanupGenPass() : base("Cleanup", 1f)
        {

        }

        public override void Apply(GenerationProgress progress)
        {
            progress.Message = Language.GetTextValue("Mods.AAMod.Common.AAVoidWorldBuildCleanup");

            IslandsGenPass.islands = null;
            // cleanup asteroids
        }
    }
}
