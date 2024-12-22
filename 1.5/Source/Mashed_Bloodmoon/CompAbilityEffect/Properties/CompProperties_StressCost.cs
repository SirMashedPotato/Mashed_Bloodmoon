using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_StressCost : CompProperties_AbilityEffect
    {
        public CompProperties_StressCost() => compClass = typeof(CompAbilityEffect_StressCost);

        public int stressCost = 1;
    }
}