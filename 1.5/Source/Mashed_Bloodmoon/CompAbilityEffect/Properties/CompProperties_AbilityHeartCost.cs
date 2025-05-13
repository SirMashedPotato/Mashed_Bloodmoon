using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityHeartCost : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityHeartCost() => compClass = typeof(CompAbilityEffect_HeartCost);

        public int heartCost = 1;
    }
}