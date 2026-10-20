using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_UseEffectUnlockClaw : CompProperties
    {
        public CompProperties_UseEffectUnlockClaw()
        {
            compClass = typeof(CompUseEffect_UnlockClaw);
        }

        public LycanthropeClawTypeDef clawDef;
    }
}
