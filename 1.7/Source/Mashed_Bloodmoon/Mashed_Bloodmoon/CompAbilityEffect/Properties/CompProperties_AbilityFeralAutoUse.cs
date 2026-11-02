using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityFeralAutoUse : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityFeralAutoUse() => compClass = typeof(CompAbilityEffect_FeralAutoUse);

        public bool onlyInCombat = true;
        public bool mustBeHostile = false;
        public bool canTargetAnimals = true;
        public float minPain = 0f;
        public float minBleedRate = 0;
    }
}