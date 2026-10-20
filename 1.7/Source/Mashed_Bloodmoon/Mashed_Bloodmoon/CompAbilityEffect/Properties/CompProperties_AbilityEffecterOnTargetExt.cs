using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityEffecterOnTargetExt : CompProperties_AbilityEffecterOnTarget
    {
        public CompProperties_AbilityEffecterOnTargetExt() => compClass = typeof(CompAbilityEffect_EffecterOnTargetExt);

        public bool onlySelf = false;
        public bool onlyHostile = false;
    }
}
