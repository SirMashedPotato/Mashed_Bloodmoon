using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityFleckOnTargetExt : CompProperties_AbilityFleckOnTarget
    {
        public CompProperties_AbilityFleckOnTargetExt() => compClass = typeof(CompAbilityEffect_FleckOnTargetExt);

        public bool onlySelf = false;
    }
}
