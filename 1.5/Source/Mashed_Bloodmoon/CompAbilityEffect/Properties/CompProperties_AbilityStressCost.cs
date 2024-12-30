using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityStressCost : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityStressCost() => compClass = typeof(CompAbilityEffect_StressCost);

        public int stressCost = 1;
    }
}